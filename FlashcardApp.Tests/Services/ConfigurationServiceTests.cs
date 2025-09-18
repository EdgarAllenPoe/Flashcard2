using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;

namespace FlashcardApp.Tests.Services
{
    public class ConfigurationServiceTests : IDisposable
    {
        private readonly string _testConfigPath;
        private readonly ConfigurationService _service;

        public ConfigurationServiceTests()
        {
            _testConfigPath = Path.Combine(Path.GetTempPath(), $"test_config_{Guid.NewGuid()}.json");
            _service = new ConfigurationService(_testConfigPath);
        }

        public void Dispose()
        {
            if (File.Exists(_testConfigPath))
            {
                File.Delete(_testConfigPath);
            }
        }

        [Fact]
        public void Constructor_ShouldCreateDefaultConfiguration()
        {
            // Act
            var config = _service.GetConfiguration();

            // Assert
            config.Should().NotBeNull();
            config.LeitnerBoxes.Should().NotBeNull();
            config.StudySession.Should().NotBeNull();
            config.FilePaths.Should().NotBeNull();
            config.ReviewScheduling.Should().NotBeNull();
            config.DailyLimits.Should().NotBeNull();
            config.UI.Should().NotBeNull();
        }

        [Fact]
        public void GetConfiguration_ShouldReturnSameInstance()
        {
            // Act
            var config1 = _service.GetConfiguration();
            var config2 = _service.GetConfiguration();

            // Assert
            config1.Should().BeSameAs(config2);
        }

        [Fact]
        public void UpdateConfiguration_ShouldModifyConfiguration()
        {
            // Arrange
            var originalConfig = _service.GetConfiguration();
            var newNumberOfBoxes = 7;

            // Act
            _service.UpdateConfiguration(config =>
            {
                config.LeitnerBoxes.NumberOfBoxes = newNumberOfBoxes;
            });

            // Assert
            var updatedConfig = _service.GetConfiguration();
            updatedConfig.LeitnerBoxes.NumberOfBoxes.Should().Be(newNumberOfBoxes);
            updatedConfig.Should().BeSameAs(originalConfig);
        }

        [Fact]
        public void UpdateConfiguration_ShouldPersistChanges()
        {
            // Arrange
            var newNumberOfBoxes = 7;
            var newMaxCards = 50;

            // Act
            _service.UpdateConfiguration(config =>
            {
                config.LeitnerBoxes.NumberOfBoxes = newNumberOfBoxes;
                config.DailyLimits.MaxCardsPerDay = newMaxCards;
            });

            // Create a new service instance to test persistence
            var newService = new ConfigurationService(_testConfigPath);
            var loadedConfig = newService.GetConfiguration();

            // Assert
            loadedConfig.LeitnerBoxes.NumberOfBoxes.Should().Be(newNumberOfBoxes);
            loadedConfig.DailyLimits.MaxCardsPerDay.Should().Be(newMaxCards);
        }

        [Fact]
        public void EnsureDirectoriesExist_ShouldCreateDirectories()
        {
            // Arrange
            var config = _service.GetConfiguration();
            var testDir = Path.Combine(Path.GetTempPath(), $"test_decks_{Guid.NewGuid()}");
            config.FilePaths.DecksDirectory = testDir;

            try
            {
                // Act
                _service.EnsureDirectoriesExist();

                // Assert
                Directory.Exists(testDir).Should().BeTrue();
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir);
                }
            }
        }

        [Fact]
        public void UpdateConfiguration_WithNullAction_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => _service.UpdateConfiguration(null!);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}