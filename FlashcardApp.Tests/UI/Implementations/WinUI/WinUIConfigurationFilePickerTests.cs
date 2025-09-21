using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for file picker functionality in Configuration page
    /// </summary>
    public class WinUIConfigurationFilePickerTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePickerInitialization_WhenFilePickersAreWorking()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - File picker initialization
            configurationPageCodeBehindContent.Should().Contain("var picker = new FileOpenPicker();", "Should create FileOpenPicker for import");
            configurationPageCodeBehindContent.Should().Contain("var picker = new FileSavePicker();", "Should create FileSavePicker for export");
            configurationPageCodeBehindContent.Should().Contain("InitializeWithWindow", "Should initialize file pickers with window handle");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePickerConfiguration_WhenFilePickersAreWorking()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - File picker configuration
            configurationPageCodeBehindContent.Should().Contain("picker.ViewMode = PickerViewMode.List;", "Should set ViewMode for import picker");
            configurationPageCodeBehindContent.Should().Contain("picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;", "Should set start location for both pickers");
            configurationPageCodeBehindContent.Should().Contain("picker.FileTypeFilter.Add(\".json\");", "Should add JSON filter for import");
            configurationPageCodeBehindContent.Should().Contain("picker.FileTypeChoices.Add(\"JSON Configuration\", new[] { \".json\" });", "Should add JSON file type for export");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePickerAsyncCalls_WhenFilePickersAreWorking()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Async file picker calls
            configurationPageCodeBehindContent.Should().Contain("await picker.PickSingleFileAsync();", "Should call PickSingleFileAsync for import");
            configurationPageCodeBehindContent.Should().Contain("await picker.PickSaveFileAsync();", "Should call PickSaveFileAsync for export");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePickerNullChecks_WhenFilePickersAreWorking()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Null checks for file picker results
            configurationPageCodeBehindContent.Should().Contain("if (file != null)", "Should check if file is not null after picker");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Import cancelled\")", "Should handle import cancellation");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Export cancelled\")", "Should handle export cancellation");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePickerErrorHandling_WhenFilePickersAreWorking()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Error handling for file picker operations
            configurationPageCodeBehindContent.Should().Contain("catch (Exception ex)", "Should have exception handling in import/export methods");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Error importing configuration\")", "Should show error for import failures");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Error exporting configuration\")", "Should show error for export failures");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePickerSuccessHandling_WhenFilePickersAreWorking()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Success handling for file picker operations
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Configuration imported successfully\")", "Should show success for import");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Configuration exported successfully\")", "Should show success for export");
            configurationPageCodeBehindContent.Should().Contain("LoadConfiguration(); // Refresh UI with imported data", "Should refresh UI after import");
        }
    }
}
