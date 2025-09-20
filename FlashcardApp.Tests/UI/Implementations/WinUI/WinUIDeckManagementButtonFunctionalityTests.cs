using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to verify that each button on the Deck Management page does what it's supposed to do.
    /// This follows TDD methodology to ensure all button functionality works correctly.
    /// </summary>
    public class WinUIDeckManagementButtonFunctionalityTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void BackButton_ShouldNavigateToMainPage_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that BackButton navigates to MainPage
            codeBehindContent.Should().Contain("BackButton_Click", "Should have BackButton click handler");
            codeBehindContent.Should().Contain("this.Frame.Navigate(typeof(MainPage))", "Should navigate to MainPage");
            codeBehindContent.Should().Contain("if (this.Frame != null)", "Should check if Frame exists before navigation");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void CreateDeckButton_ShouldCreateNewDeck_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that CreateDeckButton creates a new deck
            codeBehindContent.Should().Contain("CreateDeckButton_Click", "Should have CreateDeckButton click handler");
            codeBehindContent.Should().Contain("new FlashcardDeck", "Should create new FlashcardDeck instance");
            codeBehindContent.Should().Contain("Name = \"New Deck\"", "Should set default deck name");
            codeBehindContent.Should().Contain("Description = \"A new flashcard deck\"", "Should set default description");
            codeBehindContent.Should().Contain("_decks.Add(newDeck)", "Should add new deck to collection");
            codeBehindContent.Should().Contain("UpdateDeckList()", "Should update deck list display");
            codeBehindContent.Should().Contain("UpdateButtonStates()", "Should update button states");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void EditDeckButton_ShouldOpenEditDialog_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that EditDeckButton opens edit dialog
            codeBehindContent.Should().Contain("EditDeckButton_Click", "Should have EditDeckButton click handler");
            codeBehindContent.Should().Contain("if (_selectedDeck != null)", "Should check if deck is selected");
            codeBehindContent.Should().Contain("Title = \"Edit Deck\"", "Should have correct dialog title");
            codeBehindContent.Should().Contain("PrimaryButtonText = \"Save\"", "Should have Save button");
            codeBehindContent.Should().Contain("CloseButtonText = \"Cancel\"", "Should have Cancel button");
            codeBehindContent.Should().Contain("Header = \"Deck Name\"", "Should have name input field");
            codeBehindContent.Should().Contain("Header = \"Description\"", "Should have description input field");
            codeBehindContent.Should().Contain("_selectedDeck.Name = newName", "Should update deck name");
            codeBehindContent.Should().Contain("_selectedDeck.Description = newDescription", "Should update deck description");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeleteDeckButton_ShouldShowConfirmationDialog_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that DeleteDeckButton shows confirmation dialog
            codeBehindContent.Should().Contain("DeleteDeckButton_Click", "Should have DeleteDeckButton click handler");
            codeBehindContent.Should().Contain("if (_selectedDeck != null)", "Should check if deck is selected");
            codeBehindContent.Should().Contain("Title = \"Delete Deck\"", "Should have correct dialog title");
            codeBehindContent.Should().Contain("Are you sure you want to delete", "Should show confirmation message");
            codeBehindContent.Should().Contain("PrimaryButtonText = \"Delete\"", "Should have Delete button");
            codeBehindContent.Should().Contain("CloseButtonText = \"Cancel\"", "Should have Cancel button");
            codeBehindContent.Should().Contain("DefaultButton = ContentDialogButton.Close", "Should default to Cancel");
            codeBehindContent.Should().Contain("_decks.Remove(_selectedDeck)", "Should remove deck from collection");
            codeBehindContent.Should().Contain("_selectedDeck = null", "Should clear selected deck");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void AddCardButton_ShouldOpenCardEditDialog_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that AddCardButton opens card edit dialog
            codeBehindContent.Should().Contain("AddCardButton_Click", "Should have AddCardButton click handler");
            codeBehindContent.Should().Contain("if (_selectedDeck != null)", "Should check if deck is selected");
            codeBehindContent.Should().Contain("ShowCardEditDialog(null)", "Should call ShowCardEditDialog with null for new card");
            codeBehindContent.Should().Contain("_selectedDeck.Cards.Add(newCard)", "Should add new card to deck");
            codeBehindContent.Should().Contain("UpdateDeckDetails()", "Should update deck details display");
            codeBehindContent.Should().Contain("UpdateDeckList()", "Should update deck list display");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudyDeckButton_ShouldValidateDeckAndShowMessage_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that StudyDeckButton validates deck and shows message
            codeBehindContent.Should().Contain("StudyDeckButton_Click", "Should have StudyDeckButton click handler");
            codeBehindContent.Should().Contain("if (_selectedDeck != null)", "Should check if deck is selected");
            codeBehindContent.Should().Contain("_selectedDeck.Cards.Count == 0", "Should check if deck is empty");
            codeBehindContent.Should().Contain("Cannot study", "Should show error for empty deck");
            codeBehindContent.Should().Contain("Starting study session for", "Should show study session message");
            codeBehindContent.Should().Contain("this.Frame.Navigate(typeof(StudySessionPage)", "Should navigate to StudySessionPage");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldOpenFilePicker_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that ImportDeckButton opens file picker
            codeBehindContent.Should().Contain("ImportDeckButton_Click", "Should have ImportDeckButton click handler");
            codeBehindContent.Should().Contain("FileOpenPicker", "Should use FileOpenPicker");
            codeBehindContent.Should().Contain("PickerViewMode.List", "Should set list view mode");
            codeBehindContent.Should().Contain("PickerLocationId.DocumentsLibrary", "Should start in Documents library");
            codeBehindContent.Should().Contain(".json", "Should filter for JSON files");
            codeBehindContent.Should().Contain("PickSingleFileAsync", "Should pick single file");
            codeBehindContent.Should().Contain("JsonSerializer.Deserialize<FlashcardDeck>", "Should deserialize JSON");
            codeBehindContent.Should().Contain("_decks.Add(importedDeck)", "Should add imported deck");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldOpenFileSavePicker_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that ExportAllButton opens file save picker
            codeBehindContent.Should().Contain("ExportAllButton_Click", "Should have ExportAllButton click handler");
            codeBehindContent.Should().Contain("if (_decks.Count == 0)", "Should check if there are decks to export");
            codeBehindContent.Should().Contain("No decks to export", "Should show message for empty deck list");
            codeBehindContent.Should().Contain("FileSavePicker", "Should use FileSavePicker");
            codeBehindContent.Should().Contain("PickerLocationId.DocumentsLibrary", "Should start in Documents library");
            codeBehindContent.Should().Contain("FileTypeChoices.Add", "Should add file type choices");
            codeBehindContent.Should().Contain("SuggestedFileName = \"flashcard_decks_export\"", "Should suggest filename");
            codeBehindContent.Should().Contain("JsonSerializer.Serialize", "Should serialize to JSON");
            codeBehindContent.Should().Contain("WriteIndented = true", "Should format JSON output");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckButtons_ShouldSelectDeck_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that deck buttons select the deck
            codeBehindContent.Should().Contain("DeckButton_Click", "Should have DeckButton click handler");
            codeBehindContent.Should().Contain("button.Tag is FlashcardDeck deck", "Should check button tag type");
            codeBehindContent.Should().Contain("_selectedDeck = deck", "Should set selected deck");
            codeBehindContent.Should().Contain("UpdateDeckDetails()", "Should update deck details");
            codeBehindContent.Should().Contain("UpdateButtonStates()", "Should update button states");
            codeBehindContent.Should().Contain("deckButton.Click += DeckButton_Click", "Should wire up click event");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void CardButtons_ShouldOpenCardEditDialog_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that card buttons open edit dialog
            codeBehindContent.Should().Contain("CardButton_Click", "Should have CardButton click handler");
            codeBehindContent.Should().Contain("button.Tag is Flashcard card", "Should check button tag type");
            codeBehindContent.Should().Contain("ShowCardEditDialog(card)", "Should call ShowCardEditDialog with existing card");
            codeBehindContent.Should().Contain("cardButton.Click += CardButton_Click", "Should wire up click event");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ShowCardEditDialog_ShouldHandleNewAndExistingCards_WhenCalled()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that ShowCardEditDialog handles both new and existing cards
            codeBehindContent.Should().Contain("ShowCardEditDialog", "Should have ShowCardEditDialog method");
            codeBehindContent.Should().Contain("async Task<Flashcard?>", "Should have proper async return type");
            codeBehindContent.Should().Contain("existingCard == null ? \"Add New Card\" : \"Edit Card\"", "Should set correct title");
            codeBehindContent.Should().Contain("Header = \"Question\"", "Should have question input field");
            codeBehindContent.Should().Contain("Header = \"Answer\"", "Should have answer input field");
            codeBehindContent.Should().Contain("!string.IsNullOrEmpty(front) && !string.IsNullOrEmpty(back)", "Should validate input");
            codeBehindContent.Should().Contain("existingCard.Front = front", "Should update existing card front");
            codeBehindContent.Should().Contain("existingCard.Back = back", "Should update existing card back");
            codeBehindContent.Should().Contain("new Flashcard { Front = front, Back = back }", "Should create new card");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void AllButtons_ShouldHaveProperErrorHandling_WhenClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that all buttons have proper error handling
            codeBehindContent.Should().Contain("try", "Should have try-catch blocks");
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch exceptions");
            codeBehindContent.Should().Contain("UpdateStatus", "Should update status on errors");
            codeBehindContent.Should().Contain("Import failed", "Should show import error message");
            codeBehindContent.Should().Contain("Export failed", "Should show export error message");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void AllDialogs_ShouldHaveProperXamlRoot_WhenCreated()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that all dialogs have XamlRoot set
            codeBehindContent.Should().Contain("XamlRoot = this.XamlRoot", "Should set XamlRoot for all dialogs");
            codeBehindContent.Should().Contain("ContentDialog", "Should use ContentDialog");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ButtonStates_ShouldUpdateCorrectly_WhenDeckIsSelected()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that button states update correctly
            codeBehindContent.Should().Contain("UpdateButtonStates", "Should have UpdateButtonStates method");
            codeBehindContent.Should().Contain("bool hasSelectedDeck = _selectedDeck != null", "Should check if deck is selected");
            codeBehindContent.Should().Contain("EditDeckButton.IsEnabled = hasSelectedDeck", "Should enable/disable Edit button");
            codeBehindContent.Should().Contain("DeleteDeckButton.IsEnabled = hasSelectedDeck", "Should enable/disable Delete button");
            codeBehindContent.Should().Contain("AddCardButton.IsEnabled = hasSelectedDeck", "Should enable/disable Add Card button");
            codeBehindContent.Should().Contain("StudyDeckButton.IsEnabled = hasSelectedDeck", "Should enable/disable Study button");
        }
    }
}
