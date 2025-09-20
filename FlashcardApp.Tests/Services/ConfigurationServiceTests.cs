using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;
using FlashcardApp.Tests;

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

        [Fact, Trait("Category", TestCategories.Fast)]
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

        [Fact, Trait("Category", TestCategories.Fast)]
        public void GetConfiguration_ShouldReturnSameInstance()
        {
            // Act
            var config1 = _service.GetConfiguration();
            var config2 = _service.GetConfiguration();

            // Assert
            config1.Should().BeSameAs(config2);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
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

        [Fact, Trait("Category", TestCategories.Fast)]
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

        [Fact, Trait("Category", TestCategories.Fast)]
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

        [Fact, Trait("Category", TestCategories.Fast)]
        public void UpdateConfiguration_WithNullAction_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => _service.UpdateConfiguration(null!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void SaveConfiguration_ShouldCreateConfigFile()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.LeitnerBoxes.NumberOfBoxes = 8;

            // Act
            _service.SaveConfiguration();

            // Assert
            File.Exists(_testConfigPath).Should().BeTrue();
            var savedContent = File.ReadAllText(_testConfigPath);
            savedContent.Should().Contain("\"numberOfBoxes\": 8");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void LoadConfiguration_ShouldLoadFromFile()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.LeitnerBoxes.NumberOfBoxes = 6;
            _service.SaveConfiguration();

            // Create a new service instance
            var newService = new ConfigurationService(_testConfigPath);

            // Act
            newService.LoadConfiguration();
            var loadedConfig = newService.GetConfiguration();

            // Assert
            loadedConfig.LeitnerBoxes.NumberOfBoxes.Should().Be(6);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void EnsureDirectoriesExist_ShouldCreateMultipleDirectories()
        {
            // Arrange
            var config = _service.GetConfiguration();
            var testDecksDir = Path.Combine(Path.GetTempPath(), $"test_decks_{Guid.NewGuid()}");
            var testBackupDir = Path.Combine(Path.GetTempPath(), $"test_backups_{Guid.NewGuid()}");
            var testExportDir = Path.Combine(Path.GetTempPath(), $"test_exports_{Guid.NewGuid()}");

            config.FilePaths.DecksDirectory = testDecksDir;
            config.FilePaths.BackupDirectory = testBackupDir;
            config.FilePaths.ExportDirectory = testExportDir;

            try
            {
                // Act
                _service.EnsureDirectoriesExist();

                // Assert
                Directory.Exists(testDecksDir).Should().BeTrue();
                Directory.Exists(testBackupDir).Should().BeTrue();
                Directory.Exists(testExportDir).Should().BeTrue();
            }
            finally
            {
                if (Directory.Exists(testDecksDir)) Directory.Delete(testDecksDir);
                if (Directory.Exists(testBackupDir)) Directory.Delete(testBackupDir);
                if (Directory.Exists(testExportDir)) Directory.Delete(testExportDir);
            }
        }
    }
}
