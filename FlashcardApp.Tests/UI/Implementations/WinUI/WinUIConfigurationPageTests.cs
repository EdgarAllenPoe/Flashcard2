using FluentAssertions;
using Xunit;
using System.IO;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for the Configuration page UI implementation
    /// </summary>
    public class WinUIConfigurationPageTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldExist_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPagePath = Path.Combine("..", "..", "..", "..", "Views", "ConfigurationPage.xaml");
            var configurationPageExists = File.Exists(configurationPagePath);

            // Assert
            configurationPageExists.Should().BeTrue("Configuration page should exist for settings management");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveProperXamlStructure_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert
            configurationPageXamlContent.Should().Contain("x:Class=\"FlashcardApp.WinUI.Views.ConfigurationPage\"", "Should have correct class declaration");
            configurationPageXamlContent.Should().Contain("Grid.RowDefinitions", "Should have grid row definitions for layout");
            configurationPageXamlContent.Should().Contain("ScrollViewer", "Should have scrollable content area");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveConfigurationSections_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert
            // Should have sections for different configuration categories
            configurationPageXamlContent.Should().Contain("Study Session", "Should have study session configuration section");
            configurationPageXamlContent.Should().Contain("Leitner Box", "Should have Leitner box configuration section");
            configurationPageXamlContent.Should().Contain("File Paths", "Should have file paths configuration section");
            configurationPageXamlContent.Should().Contain("Daily Limits", "Should have daily limits configuration section");
            configurationPageXamlContent.Should().Contain("UI Settings", "Should have UI settings configuration section");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveActionButtons_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert
            // Should have action buttons for configuration management
            configurationPageXamlContent.Should().Contain("SaveButton", "Should have save configuration button");
            configurationPageXamlContent.Should().Contain("ResetButton", "Should have reset to defaults button");
            configurationPageXamlContent.Should().Contain("ImportButton", "Should have import configuration button");
            configurationPageXamlContent.Should().Contain("ExportButton", "Should have export configuration button");
            configurationPageXamlContent.Should().Contain("BackButton", "Should have back navigation button");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveInputControls_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert
            // Should have various input controls for configuration
            configurationPageXamlContent.Should().Contain("TextBox", "Should have text input controls");
            configurationPageXamlContent.Should().Contain("CheckBox", "Should have checkbox controls for boolean settings");
            configurationPageXamlContent.Should().Contain("ComboBox", "Should have dropdown controls for selection");
            configurationPageXamlContent.Should().Contain("Slider", "Should have slider controls for numeric values");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveCodeBehindStructure_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert
            // Should have proper code-behind structure
            configurationPageCodeBehindContent.Should().Contain("class ConfigurationPage", "Should have ConfigurationPage class");
            configurationPageCodeBehindContent.Should().Contain("ConfigurationService", "Should use ConfigurationService");
            configurationPageCodeBehindContent.Should().Contain("LoadConfiguration", "Should have method to load configuration");
            configurationPageCodeBehindContent.Should().Contain("SaveConfiguration", "Should have method to save configuration");
            configurationPageCodeBehindContent.Should().Contain("ResetToDefaults", "Should have method to reset to defaults");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveValidationMethods_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert
            // Should have validation methods
            configurationPageCodeBehindContent.Should().Contain("ValidateConfiguration", "Should have configuration validation method");
            configurationPageCodeBehindContent.Should().Contain("ShowValidationError", "Should have method to show validation errors");
            configurationPageCodeBehindContent.Should().Contain("IsValidPath", "Should have path validation method");
            configurationPageCodeBehindContent.Should().Contain("IsValidNumber", "Should have number validation method");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveImportExportMethods_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert
            // Should have import/export functionality
            configurationPageCodeBehindContent.Should().Contain("ImportConfiguration", "Should have import configuration method");
            configurationPageCodeBehindContent.Should().Contain("ExportConfiguration", "Should have export configuration method");
            configurationPageCodeBehindContent.Should().Contain("FileOpenPicker", "Should use FileOpenPicker for import");
            configurationPageCodeBehindContent.Should().Contain("FileSavePicker", "Should use FileSavePicker for export");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveEventHandlers_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert
            // Should have event handlers for user interactions
            configurationPageCodeBehindContent.Should().Contain("SaveButton_Click", "Should have save button click handler");
            configurationPageCodeBehindContent.Should().Contain("ResetButton_Click", "Should have reset button click handler");
            configurationPageCodeBehindContent.Should().Contain("ImportButton_Click", "Should have import button click handler");
            configurationPageCodeBehindContent.Should().Contain("ExportButton_Click", "Should have export button click handler");
            configurationPageCodeBehindContent.Should().Contain("BackButton_Click", "Should have back button click handler");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDataBinding_WhenConfigurationFeatureIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert
            // Should have data binding setup
            configurationPageCodeBehindContent.Should().Contain("DataContext", "Should set DataContext for data binding");
            configurationPageCodeBehindContent.Should().Contain("PropertyChanged", "Should implement property change notifications");
            configurationPageCodeBehindContent.Should().Contain("OnPropertyChanged", "Should have property change notification method");
        }
    }
}
