using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for File Paths configuration section in Configuration page
    /// </summary>
    public class WinUIConfigurationFilePathsTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePathsSection_WhenFilePathsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - File Paths section
            configurationPageXamlContent.Should().Contain("File Paths", "Should have File Paths section");
            configurationPageXamlContent.Should().Contain("x:Name=\"DeckStoragePathTextBox\"", "Should have Deck Storage Path text box");
            configurationPageXamlContent.Should().Contain("Header=\"Deck storage location\"", "Should have proper header for deck storage path");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDeckStorageSettings_WhenFilePathsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Deck Storage settings
            configurationPageXamlContent.Should().Contain("x:Name=\"BackupDirectoryTextBox\"", "Should have Backup Directory text box");
            configurationPageXamlContent.Should().Contain("Header=\"Backup directory\"", "Should have proper header for backup directory");
            configurationPageXamlContent.Should().Contain("x:Name=\"ExportPathTextBox\"", "Should have Export Path text box");
            configurationPageXamlContent.Should().Contain("Header=\"Default export path\"", "Should have proper header for export path");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveImportExportPaths_WhenFilePathsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Import/Export paths
            configurationPageXamlContent.Should().Contain("x:Name=\"ImportPathTextBox\"", "Should have Import Path text box");
            configurationPageXamlContent.Should().Contain("Header=\"Default import path\"", "Should have proper header for import path");
            configurationPageXamlContent.Should().Contain("x:Name=\"LogFilePathTextBox\"", "Should have Log File Path text box");
            configurationPageXamlContent.Should().Contain("Header=\"Log file location\"", "Should have proper header for log file path");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHavePathValidation_WhenFilePathsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Path validation
            configurationPageXamlContent.Should().Contain("PlaceholderText=\"C:\\Users\\Username\\Documents\\Flashcards\"", "Should have placeholder text for deck storage");
            configurationPageXamlContent.Should().Contain("PlaceholderText=\"C:\\Users\\Username\\Documents\\Flashcards\\Backups\"", "Should have placeholder text for backup directory");
            configurationPageXamlContent.Should().Contain("PlaceholderText=\"C:\\Users\\Username\\Documents\\Flashcards\\Exports\"", "Should have placeholder text for export path");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePathsDataBinding_WhenFilePathsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Data binding for File Paths
            configurationPageCodeBehindContent.Should().Contain("FilePaths.DecksDirectory", "Should bind to FilePaths.DecksDirectory");
            configurationPageCodeBehindContent.Should().Contain("FilePaths.BackupDirectory", "Should bind to FilePaths.BackupDirectory");
            configurationPageCodeBehindContent.Should().Contain("FilePaths.ExportDirectory", "Should bind to FilePaths.ExportDirectory");
            configurationPageCodeBehindContent.Should().Contain("ImportPathTextBox.Text = \"imports\"", "Should set default import path");
            configurationPageCodeBehindContent.Should().Contain("LogFilePathTextBox.Text = \"logs\"", "Should set default log file path");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePathsValidation_WhenFilePathsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - File Paths validation
            configurationPageCodeBehindContent.Should().Contain("DeckStoragePathTextBox", "Should reference DeckStoragePathTextBox in code-behind");
            configurationPageCodeBehindContent.Should().Contain("BackupDirectoryTextBox", "Should reference BackupDirectoryTextBox in code-behind");
            configurationPageCodeBehindContent.Should().Contain("ExportPathTextBox", "Should reference ExportPathTextBox in code-behind");
            configurationPageCodeBehindContent.Should().Contain("ImportPathTextBox", "Should reference ImportPathTextBox in code-behind");
            configurationPageCodeBehindContent.Should().Contain("LogFilePathTextBox", "Should reference LogFilePathTextBox in code-behind");
        }
    }
}
