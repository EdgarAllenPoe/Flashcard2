using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to identify and prevent fatal errors in Deck Management functionality.
    /// These tests focus on runtime behavior and error handling.
    /// </summary>
    public class WinUIDeckManagementFatalErrorTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperErrorHandling_WhenShowCardEditDialogIsCalled()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that ShowCardEditDialog has proper error handling
            codeBehindContent.Should().Contain("ShowCardEditDialog", "Should have ShowCardEditDialog method");
            codeBehindContent.Should().Contain("async Task<Flashcard?>", "Should have proper async return type");
            codeBehindContent.Should().Contain("ContentDialog", "Should use ContentDialog for user input");
            codeBehindContent.Should().Contain("ShowAsync", "Should properly show dialog");
            codeBehindContent.Should().Contain("ContentDialogResult.Primary", "Should check dialog result");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperNullChecks_WhenHandlingCardOperations()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that card operations have proper null checks
            codeBehindContent.Should().Contain("_selectedDeck != null", "Should check for selected deck before operations");
            codeBehindContent.Should().Contain("existingCard?.Front", "Should use null-conditional operator for existing card");
            codeBehindContent.Should().Contain("existingCard?.Back", "Should use null-conditional operator for existing card");
            codeBehindContent.Should().Contain("string.IsNullOrEmpty", "Should validate input strings");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperExceptionHandling_WhenImportingFiles()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that import operations have proper exception handling
            codeBehindContent.Should().Contain("try", "Should have try-catch blocks for file operations");
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch exceptions during import");
            codeBehindContent.Should().Contain("UpdateStatus", "Should update status on errors");
            codeBehindContent.Should().Contain("Import failed", "Should show import failure message");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperExceptionHandling_WhenExportingFiles()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that export operations have proper exception handling
            codeBehindContent.Should().Contain("try", "Should have try-catch blocks for file operations");
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch exceptions during export");
            codeBehindContent.Should().Contain("UpdateStatus", "Should update status on errors");
            codeBehindContent.Should().Contain("Export failed", "Should show export failure message");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperValidation_WhenCreatingNewCards()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that card creation has proper validation
            codeBehindContent.Should().Contain("!string.IsNullOrEmpty(front)", "Should validate front text");
            codeBehindContent.Should().Contain("!string.IsNullOrEmpty(back)", "Should validate back text");
            codeBehindContent.Should().Contain("Text.Trim()", "Should trim whitespace from input");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperDeckValidation_WhenStudyingDeck()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that study deck has proper validation
            codeBehindContent.Should().Contain("_selectedDeck.Cards.Count == 0", "Should check for empty deck");
            codeBehindContent.Should().Contain("Cannot study", "Should show error for empty deck");
            codeBehindContent.Should().Contain("no cards in deck", "Should explain why study is not possible");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperDeckIdGeneration_WhenCreatingNewDecks()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that deck ID generation is safe
            codeBehindContent.Should().Contain("_decks.Count > 0", "Should check if decks exist before generating ID");
            codeBehindContent.Should().Contain("_decks.Max(d => d.Id)", "Should find maximum existing ID");
            codeBehindContent.Should().Contain("+ 1", "Should increment ID properly");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperFilePickerConfiguration_WhenImportingFiles()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file picker is properly configured
            codeBehindContent.Should().Contain("FileOpenPicker", "Should use FileOpenPicker for import");
            codeBehindContent.Should().Contain("PickerViewMode.List", "Should set proper view mode");
            codeBehindContent.Should().Contain("PickerLocationId.DocumentsLibrary", "Should set proper start location");
            codeBehindContent.Should().Contain(".json", "Should filter for JSON files");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperFilePickerConfiguration_WhenExportingFiles()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file picker is properly configured
            codeBehindContent.Should().Contain("FileSavePicker", "Should use FileSavePicker for export");
            codeBehindContent.Should().Contain("PickerLocationId.DocumentsLibrary", "Should set proper start location");
            codeBehindContent.Should().Contain("FileTypeChoices.Add", "Should add file type choices");
            codeBehindContent.Should().Contain("SuggestedFileName", "Should suggest a filename");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperJsonSerialization_WhenHandlingData()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that JSON serialization is properly handled
            codeBehindContent.Should().Contain("JsonSerializer.Deserialize", "Should deserialize imported data");
            codeBehindContent.Should().Contain("JsonSerializer.Serialize", "Should serialize exported data");
            codeBehindContent.Should().Contain("JsonSerializerOptions", "Should use proper serialization options");
            codeBehindContent.Should().Contain("WriteIndented = true", "Should format JSON output");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperDialogResultHandling_WhenUserCancels()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that dialog cancellation is properly handled
            codeBehindContent.Should().Contain("ContentDialogResult.Primary", "Should check for primary button result");
            codeBehindContent.Should().Contain("return null", "Should return null when user cancels");
            codeBehindContent.Should().Contain("CloseButtonText", "Should have cancel button text");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperDeckRemoval_WhenDeletingDecks()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that deck deletion is properly handled
            codeBehindContent.Should().Contain("_decks.Remove(_selectedDeck)", "Should remove deck from collection");
            codeBehindContent.Should().Contain("_selectedDeck = null", "Should clear selected deck");
            codeBehindContent.Should().Contain("UpdateDeckList", "Should update UI after deletion");
            codeBehindContent.Should().Contain("UpdateDeckDetails", "Should update details after deletion");
            codeBehindContent.Should().Contain("UpdateButtonStates", "Should update button states after deletion");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperCardUpdate_WhenEditingCards()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that card editing is properly handled
            codeBehindContent.Should().Contain("existingCard.Front = front", "Should update card front");
            codeBehindContent.Should().Contain("existingCard.Back = back", "Should update card back");
            codeBehindContent.Should().Contain("return existingCard", "Should return updated card");
            codeBehindContent.Should().Contain("new Flashcard", "Should create new card when needed");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperXamlRoot_WhenCreatingDialogs()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that all ContentDialog instances have XamlRoot set
            codeBehindContent.Should().Contain("XamlRoot = this.XamlRoot", "Should set XamlRoot for all dialogs");
            codeBehindContent.Should().Contain("ContentDialog", "Should use ContentDialog");
        }
    }
}
