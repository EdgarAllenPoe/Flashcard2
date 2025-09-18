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
                    DeckFileExtension = ".json"
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
    }
}