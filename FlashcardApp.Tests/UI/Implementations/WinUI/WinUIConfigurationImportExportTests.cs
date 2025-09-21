using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for Import/Export functionality in Configuration page
    /// </summary>
    public class WinUIConfigurationImportExportTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveImportExportMethods_WhenImportExportIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Import/Export methods
            configurationPageCodeBehindContent.Should().Contain("private async Task ImportConfigurationAsync()", "Should have async ImportConfigurationAsync method");
            configurationPageCodeBehindContent.Should().Contain("private async Task ExportConfigurationAsync()", "Should have async ExportConfigurationAsync method");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePickerImports_WhenImportExportIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - File picker imports
            configurationPageCodeBehindContent.Should().Contain("using Windows.Storage.Pickers;", "Should import Windows.Storage.Pickers for file pickers");
            configurationPageCodeBehindContent.Should().Contain("using Windows.Storage;", "Should import Windows.Storage for file operations");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePickerLogic_WhenImportExportIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - File picker logic
            configurationPageCodeBehindContent.Should().Contain("FileOpenPicker", "Should use FileOpenPicker for import");
            configurationPageCodeBehindContent.Should().Contain("FileSavePicker", "Should use FileSavePicker for export");
            configurationPageCodeBehindContent.Should().Contain(".json", "Should filter for JSON files");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveAsyncEventHandlers_WhenImportExportIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Async event handlers
            configurationPageCodeBehindContent.Should().Contain("private async void ImportButton_Click(object sender, RoutedEventArgs e)", "Should have async ImportButton_Click handler");
            configurationPageCodeBehindContent.Should().Contain("private async void ExportButton_Click(object sender, RoutedEventArgs e)", "Should have async ExportButton_Click handler");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveErrorHandling_WhenImportExportIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Error handling
            configurationPageCodeBehindContent.Should().Contain("try", "Should have try blocks in import/export methods");
            configurationPageCodeBehindContent.Should().Contain("catch (Exception ex)", "Should have catch blocks for exception handling");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Error importing configuration\")", "Should show error status for import failures");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Error exporting configuration\")", "Should show error status for export failures");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveSuccessMessages_WhenImportExportIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Success messages
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Configuration imported successfully\")", "Should show success message for import");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Configuration exported successfully\")", "Should show success message for export");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFileOperations_WhenImportExportIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - File operations
            configurationPageCodeBehindContent.Should().Contain("await FileIO.ReadTextAsync", "Should read file content for import");
            configurationPageCodeBehindContent.Should().Contain("await FileIO.WriteTextAsync", "Should write file content for export");
            configurationPageCodeBehindContent.Should().Contain("JsonConvert.SerializeObject", "Should serialize configuration to JSON");
            configurationPageCodeBehindContent.Should().Contain("JsonConvert.DeserializeObject", "Should deserialize configuration from JSON");
        }
    }
}
