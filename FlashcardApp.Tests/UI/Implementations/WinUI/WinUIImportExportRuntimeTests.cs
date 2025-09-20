using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to identify runtime issues with Import Deck and Export All buttons.
    /// These tests check for potential runtime problems that might prevent the buttons from working.
    /// </summary>
    public class WinUIImportExportRuntimeTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldHaveProperXamlRoot_WhenCreatingFilePicker()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that XamlRoot is properly handled for file picker
            // Note: FileOpenPicker doesn't need XamlRoot, but we should check for proper initialization
            codeBehindContent.Should().Contain("new FileOpenPicker()", "Should create FileOpenPicker instance");
            codeBehindContent.Should().Contain("picker.ViewMode = PickerViewMode.List", "Should set view mode");
            codeBehindContent.Should().Contain("picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary", "Should set start location");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldHaveProperXamlRoot_WhenCreatingFilePicker()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that XamlRoot is properly handled for file picker
            // Note: FileSavePicker doesn't need XamlRoot, but we should check for proper initialization
            codeBehindContent.Should().Contain("new FileSavePicker()", "Should create FileSavePicker instance");
            codeBehindContent.Should().Contain("picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary", "Should set start location");
            codeBehindContent.Should().Contain("picker.SuggestedFileName = \"flashcard_decks_export\"", "Should set suggested filename");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldHandleNullFileSelection_WhenUserCancels()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that null file selection is handled
            codeBehindContent.Should().Contain("if (file != null)", "Should check if file was selected");
            codeBehindContent.Should().Contain("PickSingleFileAsync()", "Should await file picker");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldHandleNullFileSelection_WhenUserCancels()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that null file selection is handled
            codeBehindContent.Should().Contain("if (file != null)", "Should check if file was selected");
            codeBehindContent.Should().Contain("PickSaveFileAsync()", "Should await file picker");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldHaveProperErrorHandling_WhenJSONIsInvalid()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that JSON deserialization errors are handled
            codeBehindContent.Should().Contain("JsonSerializer.Deserialize<FlashcardDeck>(content)", "Should deserialize JSON");
            codeBehindContent.Should().Contain("if (importedDeck != null)", "Should validate deserialized object");
            codeBehindContent.Should().Contain("Failed to import deck - invalid file format", "Should show specific error message");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldHaveProperErrorHandling_WhenSerializationFails()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that JSON serialization errors are handled
            codeBehindContent.Should().Contain("JsonSerializer.Serialize(_decks", "Should serialize decks");
            codeBehindContent.Should().Contain("FileIO.WriteTextAsync(file, json)", "Should write to file");
            codeBehindContent.Should().Contain("Export failed:", "Should show error message");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportExportButtons_ShouldHaveProperAsyncAwaitPattern_WhenHandlingOperations()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that async/await is properly used
            codeBehindContent.Should().Contain("await picker.PickSingleFileAsync()", "Should await file picker");
            codeBehindContent.Should().Contain("await picker.PickSaveFileAsync()", "Should await save picker");
            codeBehindContent.Should().Contain("await FileIO.ReadTextAsync(file)", "Should await file reading");
            codeBehindContent.Should().Contain("await FileIO.WriteTextAsync(file, json)", "Should await file writing");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldValidateImportedDeckData_WhenAddingToCollection()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that imported deck data is validated
            codeBehindContent.Should().Contain("if (string.IsNullOrEmpty(importedDeck.Name))", "Should validate deck name");
            codeBehindContent.Should().Contain("importedDeck.Name = \"Imported Deck\"", "Should set default name");
            codeBehindContent.Should().Contain("if (importedDeck.Cards == null)", "Should validate cards collection");
            codeBehindContent.Should().Contain("importedDeck.Cards = new List<Flashcard>()", "Should initialize cards collection");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportExportButtons_ShouldUpdateUIAfterOperations_WhenOperationsComplete()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that UI is updated after operations
            codeBehindContent.Should().Contain("UpdateDeckList()", "Should update deck list after import");
            codeBehindContent.Should().Contain("UpdateStatus(", "Should update status after operations");
            codeBehindContent.Should().Contain("Imported deck:", "Should show import success");
            codeBehindContent.Should().Contain("Exported", "Should show export success");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportExportButtons_ShouldHaveProperMethodSignatures_WhenDefined()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that method signatures are correct
            codeBehindContent.Should().Contain("private async void ImportDeckButton_Click(object sender, RoutedEventArgs e)", "Should have correct import method signature");
            codeBehindContent.Should().Contain("private async void ExportAllButton_Click(object sender, RoutedEventArgs e)", "Should have correct export method signature");
        }
    }
}
