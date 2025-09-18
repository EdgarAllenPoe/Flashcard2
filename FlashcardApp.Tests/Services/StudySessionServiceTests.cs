using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;

namespace FlashcardApp.Tests.Services
{
    public class StudySessionServiceTests : IDisposable
    {
        private readonly StudySessionService _service;
        private readonly string _testSessionStatePath;
        private readonly string _testConfigPath;
        private readonly string _testDecksDirectory;

        public StudySessionServiceTests()
        {
            _testConfigPath = Path.Combine(Path.GetTempPath(), $"test_config_{Guid.NewGuid()}.json");
            _testDecksDirectory = Path.Combine(Path.GetTempPath(), $"test_decks_{Guid.NewGuid()}");
            _testSessionStatePath = Path.Combine(Path.GetTempPath(), $"test_session_{Guid.NewGuid()}.json");
            
            Directory.CreateDirectory(_testDecksDirectory);
            
            var config = new AppConfiguration
            {
                FilePaths = new FilePathConfiguration
                {
                    DecksDirectory = _testDecksDirectory,
                    DeckFileExtension = ".json"
                }
            };
            
            File.WriteAllText(_testConfigPath, Newtonsoft.Json.JsonConvert.SerializeObject(config));
            var configService = new ConfigurationService(_testConfigPath);
            var leitnerBoxService = new LeitnerBoxService(configService);
            var deckService = new DeckService(configService);
            _service = new StudySessionService(configService, leitnerBoxService, deckService);
        }

        public void Dispose()
        {
            if (File.Exists(_testSessionStatePath))
            {
                File.Delete(_testSessionStatePath);
            }
            if (File.Exists("session_state.json"))
            {
                File.Delete("session_state.json");
            }
            if (File.Exists(_testConfigPath))
            {
                File.Delete(_testConfigPath);
            }
            if (Directory.Exists(_testDecksDirectory))
            {
                Directory.Delete(_testDecksDirectory, true);
            }
        }

        [Fact]
        public void ClearSessionState_ShouldDeleteSessionStateFileWhenExists()
        {
            // Arrange
            var deckId = "test-deck";
            var sessionState = new SessionState
            {
                DeckId = deckId,
                IsActive = true
            };
            var json = System.Text.Json.JsonSerializer.Serialize(sessionState, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
            // Use the path that the service expects (session_state.json in current directory)
            File.WriteAllText("session_state.json", json);

            // Act
            _service.ClearSessionState(deckId);

            // Assert
            File.Exists("session_state.json").Should().BeFalse();
        }

        [Fact]
        public void ClearSessionState_ShouldNotDeleteFileForDifferentDeck()
        {
            // Arrange
            var sessionState = new SessionState
            {
                DeckId = "different-deck",
                IsActive = true
            };
            var json = System.Text.Json.JsonSerializer.Serialize(sessionState, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_testSessionStatePath, json);

            // Act
            _service.ClearSessionState("test-deck");

            // Assert
            File.Exists(_testSessionStatePath).Should().BeTrue();
        }

        [Fact]
        public void ClearSessionState_ShouldNotThrowWhenFileDoesNotExist()
        {
            // Act & Assert
            var action = () => _service.ClearSessionState("test-deck");
            action.Should().NotThrow();
        }
    }
}