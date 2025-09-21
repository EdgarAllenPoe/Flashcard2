using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for validation functionality in Configuration page
    /// </summary>
    public class WinUIConfigurationValidationTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveValidationMethod_WhenValidationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Validation method
            configurationPageCodeBehindContent.Should().Contain("ValidateConfiguration", "Should have ValidateConfiguration method");
            configurationPageCodeBehindContent.Should().Contain("_configurationService.ValidateConfiguration", "Should call ConfigurationService.ValidateConfiguration");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldValidateBeforeSave_WhenValidationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Validation before save
            configurationPageCodeBehindContent.Should().Contain("if (ValidateConfiguration())", "Should validate before saving");
            configurationPageCodeBehindContent.Should().Contain("_configurationService.SaveConfiguration()", "Should save only if validation passes");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldShowValidationErrors_WhenValidationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Validation error handling
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus", "Should update status for validation results");
            configurationPageCodeBehindContent.Should().Contain("validation", "Should handle validation messages");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveResetToDefaults_WhenValidationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Reset to defaults functionality
            configurationPageCodeBehindContent.Should().Contain("ResetToDefaults", "Should have ResetToDefaults method");
            configurationPageCodeBehindContent.Should().Contain("CreateDefaultConfiguration", "Should create default configuration");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveImportExportMethods_WhenValidationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Import/Export methods
            configurationPageCodeBehindContent.Should().Contain("ImportConfiguration", "Should have ImportConfiguration method");
            configurationPageCodeBehindContent.Should().Contain("ExportConfiguration", "Should have ExportConfiguration method");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveImportExportButtons_WhenValidationIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Import/Export buttons
            configurationPageXamlContent.Should().Contain("x:Name=\"ImportButton\"", "Should have Import button");
            configurationPageXamlContent.Should().Contain("x:Name=\"ExportButton\"", "Should have Export button");
            configurationPageXamlContent.Should().Contain("Click=\"ImportButton_Click\"", "Should have Import button click handler");
            configurationPageXamlContent.Should().Contain("Click=\"ExportButton_Click\"", "Should have Export button click handler");
        }
    }
}
