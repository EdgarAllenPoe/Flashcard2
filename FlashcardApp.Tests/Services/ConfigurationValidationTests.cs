using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.Services
{
    /// <summary>
    /// Tests for configuration validation and error handling
    /// </summary>
    public class ConfigurationValidationTests : IDisposable
    {
        private readonly string _testConfigPath;
        private readonly ConfigurationService _service;

        public ConfigurationValidationTests()
        {
            _testConfigPath = Path.Combine(Path.GetTempPath(), $"test_config_validation_{Guid.NewGuid()}.json");
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
        public void ValidateConfiguration_ShouldReturnTrue_ForValidConfiguration()
        {
            // Arrange
            var config = _service.GetConfiguration();

            // Act
            var isValid = _service.ValidateConfiguration(config);

            // Assert
            isValid.Should().BeTrue("Default configuration should be valid");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateConfiguration_ShouldReturnFalse_ForInvalidNumberOfBoxes()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.LeitnerBoxes.NumberOfBoxes = 0; // Invalid: must be > 0

            // Act
            var isValid = _service.ValidateConfiguration(config);

            // Assert
            isValid.Should().BeFalse("Configuration with 0 boxes should be invalid");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateConfiguration_ShouldReturnFalse_ForNegativeMaxCardsPerDay()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.DailyLimits.MaxCardsPerDay = -1; // Invalid: must be >= 0

            // Act
            var isValid = _service.ValidateConfiguration(config);

            // Assert
            isValid.Should().BeFalse("Configuration with negative max cards should be invalid");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateConfiguration_ShouldReturnFalse_ForInvalidAutoAdvanceDelay()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.StudySession.AutoAdvanceDelay = -1; // Invalid: must be >= 0

            // Act
            var isValid = _service.ValidateConfiguration(config);

            // Assert
            isValid.Should().BeFalse("Configuration with negative auto advance delay should be invalid");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateConfiguration_ShouldReturnFalse_ForEmptyDecksDirectory()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.FilePaths.DecksDirectory = ""; // Invalid: must not be empty

            // Act
            var isValid = _service.ValidateConfiguration(config);

            // Assert
            isValid.Should().BeFalse("Configuration with empty decks directory should be invalid");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateConfiguration_ShouldReturnFalse_ForInvalidBoxIntervals()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.ReviewScheduling.BoxIntervals.Clear();
            config.ReviewScheduling.BoxIntervals.Add(new BoxInterval { BoxNumber = 0, IntervalDays = -1 }); // Invalid: must be > 0

            // Act
            var isValid = _service.ValidateConfiguration(config);

            // Assert
            isValid.Should().BeFalse("Configuration with negative interval days should be invalid");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateConfiguration_ShouldReturnFalse_ForInvalidPromotionRules()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.LeitnerBoxes.PromotionRules.Clear();
            config.LeitnerBoxes.PromotionRules.Add(new PromotionRule { BoxNumber = 0, CorrectAnswersNeeded = 0 }); // Invalid: must be > 0

            // Act
            var isValid = _service.ValidateConfiguration(config);

            // Assert
            isValid.Should().BeFalse("Configuration with 0 correct answers needed should be invalid");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateConfiguration_ShouldReturnFalse_ForInvalidDemotionRules()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.LeitnerBoxes.DemotionRules.Clear();
            config.LeitnerBoxes.DemotionRules.Add(new DemotionRule { BoxNumber = 1, IncorrectAnswersNeeded = 0, DemoteToBox = 0 }); // Invalid: must be > 0

            // Act
            var isValid = _service.ValidateConfiguration(config);

            // Assert
            isValid.Should().BeFalse("Configuration with 0 incorrect answers needed should be invalid");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void GetValidationErrors_ShouldReturnEmptyList_ForValidConfiguration()
        {
            // Arrange
            var config = _service.GetConfiguration();

            // Act
            var errors = _service.GetValidationErrors(config);

            // Assert
            errors.Should().BeEmpty("Valid configuration should have no validation errors");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void GetValidationErrors_ShouldReturnErrors_ForInvalidConfiguration()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.LeitnerBoxes.NumberOfBoxes = 0;
            config.DailyLimits.MaxCardsPerDay = -1;

            // Act
            var errors = _service.GetValidationErrors(config);

            // Assert
            errors.Should().NotBeEmpty("Invalid configuration should have validation errors");
            errors.Should().Contain(error => error.Contains("NumberOfBoxes"), "Should contain error for invalid number of boxes");
            errors.Should().Contain(error => error.Contains("MaxCardsPerDay"), "Should contain error for invalid max cards per day");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateAndFixConfiguration_ShouldFixInvalidValues_WhenConfigurationIsInvalid()
        {
            // Arrange
            var config = _service.GetConfiguration();
            config.LeitnerBoxes.NumberOfBoxes = 0;
            config.DailyLimits.MaxCardsPerDay = -1;

            // Act
            var fixedConfig = _service.ValidateAndFixConfiguration(config);

            // Assert
            fixedConfig.LeitnerBoxes.NumberOfBoxes.Should().BeGreaterThan(0, "Should fix invalid number of boxes");
            fixedConfig.DailyLimits.MaxCardsPerDay.Should().BeGreaterThanOrEqualTo(0, "Should fix invalid max cards per day");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateAndFixConfiguration_ShouldNotModifyValidConfiguration()
        {
            // Arrange
            var originalConfig = _service.GetConfiguration();
            var originalNumberOfBoxes = originalConfig.LeitnerBoxes.NumberOfBoxes;
            var originalMaxCards = originalConfig.DailyLimits.MaxCardsPerDay;

            // Act
            var fixedConfig = _service.ValidateAndFixConfiguration(originalConfig);

            // Assert
            fixedConfig.LeitnerBoxes.NumberOfBoxes.Should().Be(originalNumberOfBoxes, "Should not modify valid number of boxes");
            fixedConfig.DailyLimits.MaxCardsPerDay.Should().Be(originalMaxCards, "Should not modify valid max cards per day");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ValidateConfiguration_ShouldReturnFalse_ForNullConfiguration()
        {
            // Act
            var isValid = _service.ValidateConfiguration(null!);

            // Assert
            isValid.Should().BeFalse("Null configuration should be invalid");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void GetValidationErrors_ShouldReturnError_ForNullConfiguration()
        {
            // Act
            var errors = _service.GetValidationErrors(null!);

            // Assert
            errors.Should().NotBeEmpty("Null configuration should have validation errors");
            errors.Should().Contain("Configuration cannot be null", "Should contain null configuration error");
        }
    }
}
