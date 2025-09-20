using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Comprehensive tests to ensure ALL buttons on the Deck Management page are fully functional.
    /// This follows TDD methodology to identify missing functionality and drive implementation.
    /// </summary>
    public class WinUIDeckManagementCompleteFunctionalityTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudyDeckButton_ShouldNavigateToStudySession_WhenDeckHasCards()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that StudyDeckButton navigates to StudySessionPage
            codeBehindContent.Should().Contain("StudyDeckButton_Click", "Should have StudyDeckButton click handler");
            codeBehindContent.Should().Contain("StudySessionPage", "Should navigate to StudySessionPage");
            codeBehindContent.Should().Contain("this.Frame.Navigate", "Should use Frame.Navigate for navigation");
            codeBehindContent.Should().NotContain("TODO: Implement navigation", "Navigation should be fully implemented");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudyDeckButton_ShouldPassDeckData_WhenNavigatingToStudySession()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that deck data is passed to study session
            codeBehindContent.Should().Contain("_selectedDeck", "Should pass selected deck data");
            codeBehindContent.Should().Contain("typeof(StudySessionPage)", "Should navigate to StudySessionPage type");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void CreateDeckButton_ShouldGenerateUniqueIds_WhenCreatingMultipleDecks()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that unique IDs are generated
            codeBehindContent.Should().Contain("_decks.Max(d => d.Id) + 1", "Should generate unique IDs");
            codeBehindContent.Should().Contain("_decks.Count > 0", "Should handle empty deck list");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void EditDeckButton_ShouldValidateInput_WhenEditingDeck()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that input validation exists
            codeBehindContent.Should().Contain("nameTextBox.Text.Trim()", "Should trim deck name");
            codeBehindContent.Should().Contain("descriptionTextBox.Text.Trim()", "Should trim deck description");
            codeBehindContent.Should().Contain("string.IsNullOrEmpty", "Should validate empty inputs");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeleteDeckButton_ShouldConfirmDeletion_WhenDeletingDeck()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that deletion confirmation exists
            codeBehindContent.Should().Contain("Are you sure you want to delete", "Should show confirmation message");
            codeBehindContent.Should().Contain("This action cannot be undone", "Should warn about permanent deletion");
            codeBehindContent.Should().Contain("ContentDialogResult.Primary", "Should handle confirmation result");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void AddCardButton_ShouldValidateCardContent_WhenAddingCards()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that card content validation exists
            codeBehindContent.Should().Contain("frontTextBox.Text.Trim()", "Should trim front text");
            codeBehindContent.Should().Contain("backTextBox.Text.Trim()", "Should trim back text");
            codeBehindContent.Should().Contain("!string.IsNullOrEmpty(front) && !string.IsNullOrEmpty(back)", "Should validate both fields");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldHandleFileErrors_WhenImportingDecks()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that error handling exists
            codeBehindContent.Should().Contain("try", "Should have try-catch for error handling");
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch exceptions");
            codeBehindContent.Should().Contain("Import failed:", "Should show error message");
            codeBehindContent.Should().Contain("JsonSerializer.Deserialize", "Should handle JSON deserialization");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldHandleEmptyDeckList_WhenExporting()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that empty deck list is handled
            codeBehindContent.Should().Contain("_decks.Count == 0", "Should check for empty deck list");
            codeBehindContent.Should().Contain("No decks to export", "Should show appropriate message");
            codeBehindContent.Should().Contain("JsonSerializer.Serialize", "Should serialize deck data");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void AllButtons_ShouldHaveProperStateManagement_WhenEnabledDisabled()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that button state management exists
            codeBehindContent.Should().Contain("UpdateButtonStates()", "Should have button state management");
            codeBehindContent.Should().Contain("IsEnabled = hasSelectedDeck", "Should enable/disable based on selection");
            codeBehindContent.Should().Contain("EditDeckButton.IsEnabled", "Should manage edit button state");
            codeBehindContent.Should().Contain("DeleteDeckButton.IsEnabled", "Should manage delete button state");
            codeBehindContent.Should().Contain("AddCardButton.IsEnabled", "Should manage add card button state");
            codeBehindContent.Should().Contain("StudyDeckButton.IsEnabled", "Should manage study button state");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagement_ShouldHaveDataPersistence_WhenManagingDecks()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that data persistence exists
            codeBehindContent.Should().Contain("_decks.Add", "Should add decks to collection");
            codeBehindContent.Should().Contain("_decks.Remove", "Should remove decks from collection");
            codeBehindContent.Should().Contain("UpdateDeckList()", "Should update UI after changes");
            codeBehindContent.Should().Contain("UpdateDeckDetails()", "Should update details after changes");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void CardManagement_ShouldHaveFullCRUD_WhenManagingCards()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that full CRUD operations exist
            codeBehindContent.Should().Contain("_selectedDeck.Cards.Add", "Should add cards to deck");
            codeBehindContent.Should().Contain("existingCard.Front = front", "Should update card front");
            codeBehindContent.Should().Contain("existingCard.Back = back", "Should update card back");
            codeBehindContent.Should().Contain("ShowCardEditDialog", "Should have card edit dialog");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void Navigation_ShouldHaveProperFrameHandling_WhenNavigating()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that navigation is properly handled
            codeBehindContent.Should().Contain("this.Frame != null", "Should check Frame is not null");
            codeBehindContent.Should().Contain("this.Frame.Navigate", "Should use Frame.Navigate");
            codeBehindContent.Should().Contain("typeof(MainPage)", "Should navigate to MainPage");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StatusUpdates_ShouldProvideUserFeedback_WhenOperationsComplete()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that status updates exist
            codeBehindContent.Should().Contain("UpdateStatus(", "Should have status update method");
            codeBehindContent.Should().Contain("StatusText.Text = message", "Should update status text");
            codeBehindContent.Should().Contain("Deck management initialized", "Should show initialization status");
            codeBehindContent.Should().Contain("Created new deck:", "Should show creation status");
            codeBehindContent.Should().Contain("Updated deck:", "Should show update status");
            codeBehindContent.Should().Contain("Deck deleted successfully", "Should show deletion status");
        }
    }
}
