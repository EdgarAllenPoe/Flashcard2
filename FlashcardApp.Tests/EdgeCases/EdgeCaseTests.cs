using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;

namespace FlashcardApp.Tests.EdgeCases
{
    public class EdgeCaseTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly ConfigurationService _configService;
        private readonly DeckService _deckService;
        private readonly LeitnerBoxService _leitnerBoxService;
        private readonly StudySessionService _studySessionService;

        public EdgeCaseTests()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), $"edge_case_test_{Guid.NewGuid()}");
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
        public void EmptyDeck_ShouldHandleGracefully()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Empty Deck");

            // Act
            var studyResult = _studySessionService.StartStudySession(deck, StudyMode.FrontToBack, 10);

            // Assert
            studyResult.Success.Should().BeFalse();
            studyResult.Message.Should().Be("No cards available for study at this time.");
        }

        [Fact]
        public void DeckWithOnlyInactiveCards_ShouldHandleGracefully()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Inactive Cards Deck");
            var flashcard = new Flashcard 
            { 
                Front = "Test Question", 
                Back = "Test Answer", 
                IsActive = false 
            };
            _deckService.AddFlashcardToDeck(deck, flashcard);

            // Act
            var studyResult = _studySessionService.StartStudySession(deck, StudyMode.FrontToBack, 10);

            // Assert
            studyResult.Success.Should().BeFalse();
            studyResult.Message.Should().Be("No cards available for study at this time.");
        }

        [Fact]
        public void VeryLongDeckName_ShouldBeHandled()
        {
            // Arrange
            var veryLongName = new string('A', 1000);

            // Act
            var deck = _deckService.CreateNewDeck(veryLongName);

            // Assert
            deck.Name.Should().Be(veryLongName);
            _deckService.SaveDeck(deck).Should().BeTrue();
        }

        [Fact]
        public void VeryLongFlashcardContent_ShouldBeHandled()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Long Content Deck");
            var veryLongContent = new string('X', 10000);
            var flashcard = new Flashcard 
            { 
                Front = veryLongContent, 
                Back = veryLongContent 
            };

            // Act
            _deckService.AddFlashcardToDeck(deck, flashcard);

            // Assert
            var loadedDeck = _deckService.LoadDeckById(deck.Id);
            loadedDeck!.Flashcards.Should().HaveCount(1);
            loadedDeck.Flashcards[0].Front.Should().Be(veryLongContent);
            loadedDeck.Flashcards[0].Back.Should().Be(veryLongContent);
        }

        [Fact]
        public void SpecialCharactersInContent_ShouldBeHandled()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Special Characters Deck");
            var specialContent = "Special chars: !@#$%^&*()_+-=[]{}|;':\",./<>?`~";
            var flashcard = new Flashcard 
            { 
                Front = specialContent, 
                Back = specialContent 
            };

            // Act
            _deckService.AddFlashcardToDeck(deck, flashcard);

            // Assert
            var loadedDeck = _deckService.LoadDeckById(deck.Id);
            loadedDeck!.Flashcards.Should().HaveCount(1);
            loadedDeck.Flashcards[0].Front.Should().Be(specialContent);
            loadedDeck.Flashcards[0].Back.Should().Be(specialContent);
        }

        [Fact]
        public void UnicodeCharacters_ShouldBeHandled()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Unicode Deck");
            var unicodeContent = "Unicode: 你好世界 🌍 émojis 🎉";
            var flashcard = new Flashcard 
            { 
                Front = unicodeContent, 
                Back = unicodeContent 
            };

            // Act
            _deckService.AddFlashcardToDeck(deck, flashcard);

            // Assert
            var loadedDeck = _deckService.LoadDeckById(deck.Id);
            loadedDeck!.Flashcards.Should().HaveCount(1);
            loadedDeck.Flashcards[0].Front.Should().Be(unicodeContent);
            loadedDeck.Flashcards[0].Back.Should().Be(unicodeContent);
        }

        [Fact]
        public void StudySessionWithZeroMaxCards_ShouldBeHandled()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Zero Max Cards Study Deck");
            var flashcard = new Flashcard 
            { 
                Front = "Test Question", 
                Back = "Test Answer" 
            };
            _deckService.AddFlashcardToDeck(deck, flashcard);

            // Act
            var studyResult = _studySessionService.StartStudySession(deck, StudyMode.FrontToBack, 0);

            // Assert
            studyResult.Success.Should().BeFalse();
            studyResult.Message.Should().Be("No cards available for study at this time.");
        }

        [Fact]
        public void StudySessionWithNegativeMaxCards_ShouldBeHandled()
        {
            // Arrange
            var deck = _deckService.CreateNewDeck("Negative Max Cards Study Deck");
            var flashcard = new Flashcard 
            { 
                Front = "Test Question", 
                Back = "Test Answer" 
            };
            _deckService.AddFlashcardToDeck(deck, flashcard);

            // Act
            var studyResult = _studySessionService.StartStudySession(deck, StudyMode.FrontToBack, -1);

            // Assert
            studyResult.Success.Should().BeFalse();
            studyResult.Message.Should().Be("No cards available for study at this time.");
        }
    }
}