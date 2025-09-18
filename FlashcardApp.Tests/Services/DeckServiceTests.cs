using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;

namespace FlashcardApp.Tests.Services
{
    public class DeckServiceTests : IDisposable
    {
        private readonly DeckService _service;
        private readonly string _testDecksDirectory;
        private readonly AppConfiguration _config;

        public DeckServiceTests()
        {
            _testDecksDirectory = Path.Combine(Path.GetTempPath(), $"test_decks_{Guid.NewGuid()}");
            Directory.CreateDirectory(_testDecksDirectory);
            
            var testConfigPath = Path.Combine(Path.GetTempPath(), $"test_config_{Guid.NewGuid()}.json");
            _config = new AppConfiguration
            {
                FilePaths = new FilePathConfiguration
                {
                    DecksDirectory = _testDecksDirectory,
                    DeckFileExtension = ".json",
                    BackupDirectory = _testDecksDirectory
                }
            };
            
            File.WriteAllText(testConfigPath, Newtonsoft.Json.JsonConvert.SerializeObject(_config));
            var configService = new ConfigurationService(testConfigPath);
            _service = new DeckService(configService);
        }

        public void Dispose()
        {
            if (Directory.Exists(_testDecksDirectory))
            {
                Directory.Delete(_testDecksDirectory, true);
            }
        }

        [Fact]
        public void LoadAllDecks_ShouldReturnEmptyListWhenDirectoryDoesNotExist()
        {
            // Act
            var result = _service.LoadAllDecks();

            // Assert
            result.Should().BeEmpty();
            Directory.Exists(_testDecksDirectory).Should().BeTrue(); // Should create directory
        }

        [Fact]
        public void LoadAllDecks_ShouldReturnEmptyListWhenNoFilesExist()
        {
            // Arrange
            Directory.CreateDirectory(_testDecksDirectory);

            // Act
            var result = _service.LoadAllDecks();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void LoadAllDecks_ShouldLoadValidDeckFiles()
        {
            // Arrange
            Directory.CreateDirectory(_testDecksDirectory);
            var deck = new Deck
            {
                Id = "test-deck-1",
                Name = "Test Deck",
                Description = "A test deck"
            };
            var deckJson = System.Text.Json.JsonSerializer.Serialize(deck, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(Path.Combine(_testDecksDirectory, "test-deck-1.json"), deckJson);

            // Act
            var result = _service.LoadAllDecks();

            // Assert
            result.Should().HaveCount(1);
            result[0].Id.Should().Be("test-deck-1");
            result[0].Name.Should().Be("Test Deck");
        }

        [Fact]
        public void SaveDeck_ShouldCreateFileWithCorrectName()
        {
            // Arrange
            var deck = new Deck
            {
                Id = "test-deck-1",
                Name = "Test Deck"
            };

            // Act
            var result = _service.SaveDeck(deck);

            // Assert
            result.Should().BeTrue();
            var expectedPath = Path.Combine(_testDecksDirectory, "test-deck-1.json");
            File.Exists(expectedPath).Should().BeTrue();
        }

        [Fact]
        public void SaveDeck_ShouldUpdateLastModified()
        {
            // Arrange
            var deck = new Deck
            {
                Id = "test-deck-1",
                Name = "Test Deck"
            };
            var originalModified = deck.LastModified;

            // Wait a small amount to ensure time difference
            Thread.Sleep(10);

            // Act
            _service.SaveDeck(deck);

            // Assert
            deck.LastModified.Should().BeAfter(originalModified);
        }

        [Fact]
        public void CreateNewDeck_ShouldCreateDeckWithUniqueId()
        {
            // Act
            var deck1 = _service.CreateNewDeck("Deck 1");
            var deck2 = _service.CreateNewDeck("Deck 2");

            // Assert
            deck1.Id.Should().NotBe(deck2.Id);
            deck1.Name.Should().Be("Deck 1");
            deck2.Name.Should().Be("Deck 2");
        }

        [Fact]
        public void CreateNewDeck_ShouldSetDefaultValues()
        {
            // Act
            var deck = _service.CreateNewDeck("Test Deck", "Test Description", new List<string> { "tag1", "tag2" });

            // Assert
            deck.Name.Should().Be("Test Deck");
            deck.Description.Should().Be("Test Description");
            deck.Tags.Should().BeEquivalentTo(new List<string> { "tag1", "tag2" });
            deck.Flashcards.Should().BeEmpty();
            deck.Statistics.Should().NotBeNull();
            deck.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        #region Backup Tests

        [Fact]
        public void BackupDeck_ShouldCreateBackupFile()
        {
            // Arrange
            var deck = _service.CreateNewDeck("Test Deck", "Test Description");
            var flashcard = new Flashcard { Front = "Test Question", Back = "Test Answer" };
            _service.AddFlashcardToDeck(deck, flashcard);

            // Act
            var result = _service.BackupDeck(deck);

            // Assert
            result.Should().BeTrue();
            
            // Check if backup file exists
            var backupFiles = Directory.GetFiles(_testDecksDirectory, "Test Deck_*.json");
            backupFiles.Should().HaveCount(1);
            
            // Verify backup content
            var backupContent = File.ReadAllText(backupFiles[0]);
            backupContent.Should().Contain("Test Deck");
            backupContent.Should().Contain("Test Question");
        }

        [Fact]
        public void BackupDeck_NullDeck_ShouldReturnFalse()
        {
            // Act
            var result = _service.BackupDeck(null!);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Search Tests

        [Fact]
        public void SearchDecks_ShouldFindMatchingDecks()
        {
            // Arrange
            var deck1 = _service.CreateNewDeck("Math Deck", "Mathematics flashcards");
            var deck2 = _service.CreateNewDeck("Science Deck", "Science flashcards");
            var deck3 = _service.CreateNewDeck("History Deck", "History flashcards");
            
            // Save decks to disk so SearchDecks can find them
            _service.SaveDeck(deck1);
            _service.SaveDeck(deck2);
            _service.SaveDeck(deck3);

            // Act
            var mathResults = _service.SearchDecks("Math");
            var scienceResults = _service.SearchDecks("Science");
            var allResults = _service.SearchDecks("Deck");

            // Assert
            mathResults.Should().HaveCount(1);
            mathResults.First().Name.Should().Be("Math Deck");
            
            scienceResults.Should().HaveCount(1);
            scienceResults.First().Name.Should().Be("Science Deck");
            
            allResults.Should().HaveCount(3);
        }

        [Fact]
        public void SearchDecks_ShouldBeCaseInsensitive()
        {
            // Arrange
            var deck = _service.CreateNewDeck("Math Deck", "Mathematics flashcards");
            _service.SaveDeck(deck);

            // Act
            var results1 = _service.SearchDecks("math");
            var results2 = _service.SearchDecks("MATH");
            var results3 = _service.SearchDecks("Math");

            // Assert
            results1.Should().HaveCount(1);
            results2.Should().HaveCount(1);
            results3.Should().HaveCount(1);
        }

        [Fact]
        public void SearchDecks_EmptySearchTerm_ShouldReturnAllDecks()
        {
            // Arrange
            var deck1 = _service.CreateNewDeck("Math Deck", "Mathematics flashcards");
            var deck2 = _service.CreateNewDeck("Science Deck", "Science flashcards");
            _service.SaveDeck(deck1);
            _service.SaveDeck(deck2);

            // Act
            var results = _service.SearchDecks("");

            // Assert
            results.Should().HaveCount(2);
        }

        [Fact]
        public void SearchFlashcards_ShouldFindMatchingFlashcards()
        {
            // Arrange
            var deck = _service.CreateNewDeck("Test Deck", "Test Description");
            var flashcard1 = new Flashcard { Front = "What is 2+2?", Back = "4" };
            var flashcard2 = new Flashcard { Front = "What is the capital of France?", Back = "Paris" };
            var flashcard3 = new Flashcard { Front = "What is photosynthesis?", Back = "Process by which plants make food" };
            
            _service.AddFlashcardToDeck(deck, flashcard1);
            _service.AddFlashcardToDeck(deck, flashcard2);
            _service.AddFlashcardToDeck(deck, flashcard3);

            // Act
            var mathResults = _service.SearchFlashcards(deck, "2+2");
            var capitalResults = _service.SearchFlashcards(deck, "capital");
            var whatResults = _service.SearchFlashcards(deck, "What");

            // Assert
            mathResults.Should().HaveCount(1);
            mathResults.First().Front.Should().Be("What is 2+2?");
            
            capitalResults.Should().HaveCount(1);
            capitalResults.First().Front.Should().Be("What is the capital of France?");
            
            whatResults.Should().HaveCount(3);
        }

        [Fact]
        public void SearchFlashcards_ShouldSearchBothFrontAndBack()
        {
            // Arrange
            var deck = _service.CreateNewDeck("Test Deck", "Test Description");
            var flashcard1 = new Flashcard { Front = "Question about Paris", Back = "Answer about France" };
            var flashcard2 = new Flashcard { Front = "Question about London", Back = "Answer about UK" };
            
            _service.AddFlashcardToDeck(deck, flashcard1);
            _service.AddFlashcardToDeck(deck, flashcard2);

            // Act
            var parisResults = _service.SearchFlashcards(deck, "Paris");
            var franceResults = _service.SearchFlashcards(deck, "France");

            // Assert
            parisResults.Should().HaveCount(1);
            franceResults.Should().HaveCount(1);
            parisResults.First().Should().Be(franceResults.First());
        }

        [Fact]
        public void SearchFlashcards_ShouldBeCaseInsensitive()
        {
            // Arrange
            var deck = _service.CreateNewDeck("Test Deck", "Test Description");
            var flashcard = new Flashcard { Front = "What is the capital of France?", Back = "Paris" };
            _service.AddFlashcardToDeck(deck, flashcard);

            // Act
            var results1 = _service.SearchFlashcards(deck, "france");
            var results2 = _service.SearchFlashcards(deck, "FRANCE");
            var results3 = _service.SearchFlashcards(deck, "France");

            // Assert
            results1.Should().HaveCount(1);
            results2.Should().HaveCount(1);
            results3.Should().HaveCount(1);
        }

        [Fact]
        public void SearchFlashcards_NullDeck_ShouldReturnEmptyList()
        {
            // Act
            var results = _service.SearchFlashcards(null!, "test");

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public void SearchFlashcards_EmptySearchTerm_ShouldReturnAllFlashcards()
        {
            // Arrange
            var deck = _service.CreateNewDeck("Test Deck", "Test Description");
            var flashcard1 = new Flashcard { Front = "Question 1", Back = "Answer 1" };
            var flashcard2 = new Flashcard { Front = "Question 2", Back = "Answer 2" };
            
            _service.AddFlashcardToDeck(deck, flashcard1);
            _service.AddFlashcardToDeck(deck, flashcard2);

            // Act
            var results = _service.SearchFlashcards(deck, "");

            // Assert
            results.Should().HaveCount(2);
        }

        #endregion
    }
}