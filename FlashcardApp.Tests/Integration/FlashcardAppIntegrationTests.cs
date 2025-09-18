using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;

namespace FlashcardApp.Tests.Integration
{
    public class FlashcardAppIntegrationTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly ConfigurationService _configService;
        private readonly DeckService _deckService;
        private readonly LeitnerBoxService _leitnerBoxService;
        private readonly StudySessionService _studySessionService;

        public FlashcardAppIntegrationTests()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), $"flashcard_test_{Guid.NewGuid()}");
            Directory.CreateDirectory(_testDirectory);
            
            var configPath = Path.Combine(_testDirectory, "config.json");
            _configService = new ConfigurationService(configPath);
            _deckService = new DeckService(_configService);
            _leitnerBoxService = new LeitnerBoxService(_configService);
            _studySessionService = new StudySessionService(_configService, _leitnerBoxService, _deckService);
            
            _configService.EnsureDirectoriesExist();
        }

        public void Dispose()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }

        [Fact]
        public void CompleteWorkflow_CreateDeck_AddCards_ShouldWork()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Integration Test Deck", "A deck for integration testing");
            
            var flashcard1 = new Flashcard
            {
                Front = "What is the capital of France?",
                Back = "Paris",
                Tags = new List<string> { "geography", "capitals" }
            };
            
            var flashcard2 = new Flashcard
            {
                Front = "What is 2 + 2?",
                Back = "4",
                Tags = new List<string> { "math", "arithmetic" }
            };

            // Act - Add flashcards to deck
            _deckService.AddFlashcardToDeck(deck, flashcard1);
            _deckService.AddFlashcardToDeck(deck, flashcard2);

            // Assert - Deck should be saved and loadable
            var loadedDeck = _deckService.LoadDeckById(deck.Id);
            loadedDeck.Should().NotBeNull();
            loadedDeck!.Flashcards.Should().HaveCount(2);
            loadedDeck.Flashcards.Should().Contain(f => f.Front == "What is the capital of France?");
            loadedDeck.Flashcards.Should().Contain(f => f.Front == "What is 2 + 2?");

            // Note: Study session testing is covered by StudySessionServiceTests
            // Interactive study sessions are not suitable for automated testing
        }

        [Fact]
        public void LeitnerBoxSystem_ShouldPromoteAndDemoteCardsCorrectly()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Leitner Test Deck");
            var flashcard = new Flashcard
            {
                Front = "Test Question",
                Back = "Test Answer",
                CurrentBox = 0
            };
            _deckService.AddFlashcardToDeck(deck, flashcard);

            // Act - Process correct answers to promote
            _leitnerBoxService.ProcessCorrectAnswer(flashcard);
            _deckService.SaveDeck(deck);

            // Assert - Card should be promoted to box 1
            var loadedDeck = _deckService.LoadDeckById(deck.Id);
            var loadedCard = loadedDeck!.Flashcards.First();
            loadedCard.CurrentBox.Should().Be(1);
            loadedCard.Statistics.CorrectAnswers.Should().Be(1);

            // Act - Process incorrect answer to demote
            _leitnerBoxService.ProcessIncorrectAnswer(loadedCard);
            _deckService.SaveDeck(loadedDeck);

            // Assert - Card should be demoted back to box 0
            var finalDeck = _deckService.LoadDeckById(deck.Id);
            var finalCard = finalDeck!.Flashcards.First();
            finalCard.CurrentBox.Should().Be(0);
            finalCard.Statistics.IncorrectAnswers.Should().Be(1);
        }

        [Fact]
        public void ReviewScheduling_ShouldSetCorrectNextReviewDates()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Review Test Deck");
            var flashcard = new Flashcard
            {
                Front = "Test Question",
                Back = "Test Answer",
                CurrentBox = 0
            };
            _deckService.AddFlashcardToDeck(deck, flashcard);

            // Act - Update next review date
            _leitnerBoxService.UpdateNextReviewDate(flashcard);
            _deckService.SaveDeck(deck);

            // Assert - Next review date should be set correctly
            var loadedDeck = _deckService.LoadDeckById(deck.Id);
            var loadedCard = loadedDeck!.Flashcards.First();
            loadedCard.NextReviewDate.Should().NotBeNull();
            
            var expectedDate = DateTime.Now.AddDays(1); // Box 0 has 1-day interval
            loadedCard.NextReviewDate.Should().BeCloseTo(expectedDate, TimeSpan.FromMinutes(1));
        }

        [Fact]
        public void Configuration_ShouldPersistAndLoadCorrectly()
        {
            // Arrange
            var originalConfig = _configService.GetConfiguration();
            var originalBoxes = originalConfig.LeitnerBoxes.NumberOfBoxes;

            // Act - Update configuration
            _configService.UpdateConfiguration(config =>
            {
                config.LeitnerBoxes.NumberOfBoxes = 7;
                config.DailyLimits.MaxCardsPerDay = 50;
            });

            // Create new service instance to test persistence
            var newConfigPath = Path.Combine(_testDirectory, "config.json");
            var newConfigService = new ConfigurationService(newConfigPath);
            var loadedConfig = newConfigService.GetConfiguration();

            // Assert - Configuration should be persisted
            loadedConfig.LeitnerBoxes.NumberOfBoxes.Should().Be(7);
            loadedConfig.DailyLimits.MaxCardsPerDay.Should().Be(50);
        }

        [Fact]
        public void MultipleDecks_ShouldBeManagedIndependently()
        {
            // Arrange
            var deck1 = _deckService.CreateNewDeck("Deck 1");
            var deck2 = _deckService.CreateNewDeck("Deck 2");
            
            var card1 = new Flashcard { Front = "Question 1", Back = "Answer 1" };
            var card2 = new Flashcard { Front = "Question 2", Back = "Answer 2" };

            // Act
            _deckService.AddFlashcardToDeck(deck1, card1);
            _deckService.AddFlashcardToDeck(deck2, card2);

            // Assert - Decks should be independent
            var loadedDeck1 = _deckService.LoadDeckById(deck1.Id);
            var loadedDeck2 = _deckService.LoadDeckById(deck2.Id);

            loadedDeck1!.Flashcards.Should().HaveCount(1);
            loadedDeck1.Flashcards.Should().Contain(c => c.Front == "Question 1");
            loadedDeck1.Flashcards.Should().NotContain(c => c.Front == "Question 2");

            loadedDeck2!.Flashcards.Should().HaveCount(1);
            loadedDeck2.Flashcards.Should().Contain(c => c.Front == "Question 2");
            loadedDeck2.Flashcards.Should().NotContain(c => c.Front == "Question 1");
        }
    }
}