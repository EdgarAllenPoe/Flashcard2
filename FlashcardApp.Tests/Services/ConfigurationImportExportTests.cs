using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;
using FlashcardApp.Tests;
using Newtonsoft.Json;

namespace FlashcardApp.Tests.Services
{
    /// <summary>
    /// Tests for configuration import/export functionality
    /// </summary>
    public class ConfigurationImportExportTests : IDisposable
    {
        private readonly string _testConfigPath;
        private readonly string _testExportPath;
        private readonly ConfigurationService _service;

        public ConfigurationImportExportTests()
        {
            _testConfigPath = Path.Combine(Path.GetTempPath(), $"test_config_import_export_{Guid.NewGuid()}.json");
            _testExportPath = Path.Combine(Path.GetTempPath(), $"test_export_{Guid.NewGuid()}.json");
            _service = new ConfigurationService(_testConfigPath);
        }

        public void Dispose()
        {
            if (File.Exists(_testConfigPath))
            {
                File.Delete(_testConfigPath);
            }
            if (File.Exists(_testExportPath))
            {
                File.Delete(_testExportPath);
            }
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportConfiguration_ShouldCreateValidJsonFile_WhenConfigurationIsExported()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.LeitnerBoxes.NumberOfBoxes = 7;
            config.DailyLimits.MaxCardsPerDay = 50;

            // Act
            _service.ExportConfiguration(_testExportPath);

            // Assert
            File.Exists(_testExportPath).Should().BeTrue("Export file should be created");
            var exportedContent = File.ReadAllText(_testExportPath);
            exportedContent.Should().Contain("\"numberOfBoxes\": 7", "Should contain exported configuration values");
            exportedContent.Should().Contain("\"maxCardsPerDay\": 50", "Should contain exported configuration values");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportConfiguration_ShouldLoadConfiguration_WhenValidJsonFileIsImported()
        {
            // Arrange
            var originalConfig = _service.GetConfiguration();
            originalConfig.LeitnerBoxes.NumberOfBoxes = 8;
            originalConfig.DailyLimits.MaxCardsPerDay = 75;
            _service.SaveConfiguration();

            // Create export file
            _service.ExportConfiguration(_testExportPath);

            // Reset configuration
            var newService = new ConfigurationService(_testConfigPath);
            var resetConfig = newService.GetConfiguration();
            resetConfig.LeitnerBoxes.NumberOfBoxes = 5; // Default value
            resetConfig.DailyLimits.MaxCardsPerDay = 100; // Default value

            // Act
            var importedConfig = newService.ImportConfiguration(_testExportPath);

            // Assert
            importedConfig.Should().NotBeNull("Imported configuration should not be null");
            importedConfig.LeitnerBoxes.NumberOfBoxes.Should().Be(8, "Should import correct number of boxes");
            importedConfig.DailyLimits.MaxCardsPerDay.Should().Be(75, "Should import correct max cards per day");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportConfiguration_ShouldThrowException_WhenFileDoesNotExist()
        {
            // Arrange
            var nonExistentPath = Path.Combine(Path.GetTempPath(), "non_existent_config.json");

            // Act & Assert
            var action = () => _service.ImportConfiguration(nonExistentPath);
            action.Should().Throw<FileNotFoundException>("Should throw exception for non-existent file");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportConfiguration_ShouldThrowException_WhenFileContainsInvalidJson()
        {
            // Arrange
            var invalidJsonPath = Path.Combine(Path.GetTempPath(), $"invalid_json_{Guid.NewGuid()}.json");
            File.WriteAllText(invalidJsonPath, "{ invalid json content }");

            try
            {
                // Act & Assert
                var action = () => _service.ImportConfiguration(invalidJsonPath);
                action.Should().Throw<JsonException>("Should throw exception for invalid JSON");
            }
            finally
            {
                if (File.Exists(invalidJsonPath))
                {
                    File.Delete(invalidJsonPath);
                }
            }
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportConfiguration_ShouldValidateImportedConfiguration_WhenConfigurationIsImported()
        {
            // Arrange
            var invalidConfig = new AppConfiguration
            {
                LeitnerBoxes = new LeitnerBoxConfiguration { NumberOfBoxes = 0 }, // Invalid
                DailyLimits = new DailyLimitsConfiguration { MaxCardsPerDay = -1 } // Invalid
            };

            var invalidJson = JsonConvert.SerializeObject(invalidConfig, Formatting.Indented);
            File.WriteAllText(_testExportPath, invalidJson);

            // Act
            var importedConfig = _service.ImportConfiguration(_testExportPath);

            // Assert
            importedConfig.Should().NotBeNull("Should return configuration even if invalid");
            // The service should fix invalid values during import
            importedConfig.LeitnerBoxes.NumberOfBoxes.Should().BeGreaterThan(0, "Should fix invalid number of boxes");
            importedConfig.DailyLimits.MaxCardsPerDay.Should().BeGreaterThanOrEqualTo(0, "Should fix invalid max cards per day");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportConfiguration_ShouldIncludeAllConfigurationSections_WhenConfigurationIsExported()
        {
            // Arrange
            var config = _service.GetConfiguration();

            // Act
            _service.ExportConfiguration(_testExportPath);

            // Assert
            var exportedContent = File.ReadAllText(_testExportPath);
            exportedContent.Should().Contain("\"leitnerBoxes\"", "Should include Leitner boxes configuration");
            exportedContent.Should().Contain("\"studySession\"", "Should include study session configuration");
            exportedContent.Should().Contain("\"filePaths\"", "Should include file paths configuration");
            exportedContent.Should().Contain("\"reviewScheduling\"", "Should include review scheduling configuration");
            exportedContent.Should().Contain("\"dailyLimits\"", "Should include daily limits configuration");
            exportedContent.Should().Contain("\"ui\"", "Should include UI configuration");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportConfiguration_ShouldPreserveAllConfigurationSections_WhenConfigurationIsImported()
        {
            // Arrange
            var originalConfig = _service.GetConfiguration();
            originalConfig.StudySession.ShowStatistics = false;
            originalConfig.UI.UseColors = false;
            originalConfig.ReviewScheduling.MaxNewCardsPerDay = 15;

            _service.ExportConfiguration(_testExportPath);

            // Reset to defaults
            var newService = new ConfigurationService(_testConfigPath);

            // Act
            var importedConfig = newService.ImportConfiguration(_testExportPath);

            // Assert
            importedConfig.StudySession.ShowStatistics.Should().BeFalse("Should preserve study session settings");
            importedConfig.UI.UseColors.Should().BeFalse("Should preserve UI settings");
            importedConfig.ReviewScheduling.MaxNewCardsPerDay.Should().Be(15, "Should preserve review scheduling settings");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportConfiguration_ShouldCreateBackup_WhenBackupIsRequested()
        {
            // Arrange
            var config = _service.GetConfiguration();
            var backupPath = Path.Combine(Path.GetTempPath(), $"backup_{Guid.NewGuid()}.json");

            try
            {
                // Act
                _service.ExportConfiguration(_testExportPath, createBackup: true, backupPath: backupPath);

                // Assert
                File.Exists(_testExportPath).Should().BeTrue("Export file should be created");
                File.Exists(backupPath).Should().BeTrue("Backup file should be created");
            }
            finally
            {
                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }
            }
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportConfiguration_ShouldReturnNull_WhenFileIsEmpty()
        {
            // Arrange
            File.WriteAllText(_testExportPath, "");

            // Act & Assert
            var action = () => _service.ImportConfiguration(_testExportPath);
            action.Should().Throw<InvalidOperationException>("Should throw exception for empty file");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportConfiguration_ShouldUseCurrentConfiguration_WhenConfigurationIsNull()
        {
            // Act
            _service.ExportConfiguration(_testExportPath, null);

            // Assert
            File.Exists(_testExportPath).Should().BeTrue("Export file should be created with current configuration");
            var exportedContent = File.ReadAllText(_testExportPath);
            exportedContent.Should().Contain("\"leitnerBoxes\"", "Should contain current configuration");
        }
    }
}
