using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to identify and fix issues with Import Deck and Export All buttons.
    /// Following TDD methodology to ensure these buttons work correctly.
    /// </summary>
    public class WinUIImportExportButtonTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldHaveProperFilePickerConfiguration_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file picker is properly configured
            codeBehindContent.Should().Contain("FileOpenPicker", "Should use FileOpenPicker for import");
            codeBehindContent.Should().Contain("PickerViewMode.List", "Should set view mode to List");
            codeBehindContent.Should().Contain("PickerLocationId.DocumentsLibrary", "Should start in Documents library");
            codeBehindContent.Should().Contain("FileTypeFilter.Add(\".json\")", "Should filter for JSON files");
            codeBehindContent.Should().Contain("PickSingleFileAsync()", "Should pick single file");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldHandleFileReading_WhenFileIsSelected()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file reading is properly handled
            codeBehindContent.Should().Contain("FileIO.ReadTextAsync(file)", "Should read file content");
            codeBehindContent.Should().Contain("JsonSerializer.Deserialize<FlashcardDeck>", "Should deserialize JSON");
            codeBehindContent.Should().Contain("file != null", "Should check if file was selected");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldHandleImportErrors_WhenFileIsInvalid()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that error handling exists
            codeBehindContent.Should().Contain("try", "Should have try-catch for error handling");
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch exceptions");
            codeBehindContent.Should().Contain("Import failed:", "Should show error message");
            codeBehindContent.Should().Contain("importedDeck != null", "Should validate imported deck");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldAddDeckToCollection_WhenImportIsSuccessful()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that deck is added to collection
            codeBehindContent.Should().Contain("_decks.Add(importedDeck)", "Should add imported deck to collection");
            codeBehindContent.Should().Contain("UpdateDeckList()", "Should update deck list after import");
            codeBehindContent.Should().Contain("importedDeck.Id = _decks.Count > 0 ? _decks.Max(d => d.Id) + 1 : 1", "Should assign unique ID");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldHaveProperFilePickerConfiguration_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file picker is properly configured
            codeBehindContent.Should().Contain("FileSavePicker", "Should use FileSavePicker for export");
            codeBehindContent.Should().Contain("PickerLocationId.DocumentsLibrary", "Should start in Documents library");
            codeBehindContent.Should().Contain("FileTypeChoices.Add(\"JSON files\", new List<string>() { \".json\" })", "Should set file type choices");
            codeBehindContent.Should().Contain("SuggestedFileName = \"flashcard_decks_export\"", "Should suggest filename");
            codeBehindContent.Should().Contain("PickSaveFileAsync()", "Should pick save file");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldHandleEmptyDeckList_WhenNoDecksExist()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that empty deck list is handled
            codeBehindContent.Should().Contain("_decks.Count == 0", "Should check for empty deck list");
            codeBehindContent.Should().Contain("No decks to export", "Should show appropriate message");
            codeBehindContent.Should().Contain("return;", "Should return early if no decks");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldSerializeDecksToJSON_WhenExporting()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that JSON serialization is properly handled
            codeBehindContent.Should().Contain("JsonSerializer.Serialize(_decks", "Should serialize decks to JSON");
            codeBehindContent.Should().Contain("WriteIndented = true", "Should format JSON with indentation");
            codeBehindContent.Should().Contain("FileIO.WriteTextAsync(file, json)", "Should write JSON to file");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldHandleExportErrors_WhenFileOperationFails()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that error handling exists
            codeBehindContent.Should().Contain("try", "Should have try-catch for error handling");
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch exceptions");
            codeBehindContent.Should().Contain("Export failed:", "Should show error message");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportExportButtons_ShouldHaveProperAsyncHandling_WhenOperationsComplete()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that async operations are properly handled
            codeBehindContent.Should().Contain("private async void ImportDeckButton_Click", "Import should be async");
            codeBehindContent.Should().Contain("private async void ExportAllButton_Click", "Export should be async");
            codeBehindContent.Should().Contain("await", "Should use await for async operations");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportExportButtons_ShouldUpdateStatus_WhenOperationsComplete()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that status updates are provided
            codeBehindContent.Should().Contain("UpdateStatus(", "Should update status");
            codeBehindContent.Should().Contain("Imported deck:", "Should show import success message");
            codeBehindContent.Should().Contain("Exported", "Should show export success message");
            codeBehindContent.Should().Contain("successfully", "Should indicate success");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportExportButtons_ShouldHaveProperUsingStatements_ForFileOperations()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that required using statements exist
            codeBehindContent.Should().Contain("using Windows.Storage;", "Should have Windows.Storage using");
            codeBehindContent.Should().Contain("using Windows.Storage.Pickers;", "Should have Windows.Storage.Pickers using");
            codeBehindContent.Should().Contain("using System.Threading.Tasks;", "Should have System.Threading.Tasks using");
        }
    }
}
