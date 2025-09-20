using Xunit;
using FlashcardApp.Tests;
using FluentAssertions;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// TDD tests for DeckManagement page button functionality and wiring.
    /// </summary>
    public class WinUIDeckManagementButtonTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveAllButtonsWiredUp_WhenPageIsCreated()
        {
            // Arrange & Act
            var xamlContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml");
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that all buttons have Click handlers
            xamlContent.Should().Contain("Click=\"BackButton_Click\"");
            xamlContent.Should().Contain("Click=\"CreateDeckButton_Click\"");
            xamlContent.Should().Contain("Click=\"EditDeckButton_Click\"");
            xamlContent.Should().Contain("Click=\"DeleteDeckButton_Click\"");
            xamlContent.Should().Contain("Click=\"AddCardButton_Click\"");
            xamlContent.Should().Contain("Click=\"StudyDeckButton_Click\"");
            xamlContent.Should().Contain("Click=\"ImportDeckButton_Click\"");
            xamlContent.Should().Contain("Click=\"ExportAllButton_Click\"");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveAllClickHandlersImplemented_WhenCodeBehindIsCreated()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that all click handlers are implemented
            codeBehindContent.Should().Contain("private void BackButton_Click");
            codeBehindContent.Should().Contain("private void CreateDeckButton_Click");
            codeBehindContent.Should().Contain("private async void EditDeckButton_Click");
            codeBehindContent.Should().Contain("private async void DeleteDeckButton_Click");
            codeBehindContent.Should().Contain("private async void AddCardButton_Click");
            codeBehindContent.Should().Contain("private void StudyDeckButton_Click");
            codeBehindContent.Should().Contain("private async void ImportDeckButton_Click");
            codeBehindContent.Should().Contain("private async void ExportAllButton_Click");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperButtonStates_WhenNoDeckIsSelected()
        {
            // Arrange & Act
            var xamlContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml");

            // Assert - Check that buttons are properly disabled when no deck is selected
            xamlContent.Should().Contain("IsEnabled=\"False\"");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveCreateDeckFunctionality_WhenCreateButtonIsClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that CreateDeckButton_Click creates a new deck
            codeBehindContent.Should().Contain("new FlashcardDeck");
            codeBehindContent.Should().Contain("_decks.Add(newDeck)");
            codeBehindContent.Should().Contain("UpdateDeckList()");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveDeleteDeckFunctionality_WhenDeleteButtonIsClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that DeleteDeckButton_Click removes the selected deck
            codeBehindContent.Should().Contain("_decks.Remove(_selectedDeck)");
            codeBehindContent.Should().Contain("_selectedDeck = null");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveAddCardFunctionality_WhenAddCardButtonIsClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that AddCardButton_Click adds a new card to selected deck
            codeBehindContent.Should().Contain("ShowCardEditDialog");
            codeBehindContent.Should().Contain("_selectedDeck.Cards.Add(newCard)");
            codeBehindContent.Should().Contain("UpdateDeckDetails()");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveShowCardEditDialogMethod_WhenAddingOrEditingCards()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that ShowCardEditDialog method exists and is properly implemented
            codeBehindContent.Should().Contain("private async Task<Flashcard?> ShowCardEditDialog");
            codeBehindContent.Should().Contain("ContentDialog");
            codeBehindContent.Should().Contain("Add New Card");
            codeBehindContent.Should().Contain("Edit Card");
            codeBehindContent.Should().Contain("Question");
            codeBehindContent.Should().Contain("Answer");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveCardButtonClickHandler_WhenCardsAreDisplayed()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that cards are clickable for editing
            codeBehindContent.Should().Contain("CardButton_Click");
            codeBehindContent.Should().Contain("cardButton.Click += CardButton_Click");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperAsyncMethods_WhenUsingDialogs()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that async methods are properly declared
            codeBehindContent.Should().Contain("private async void EditDeckButton_Click");
            codeBehindContent.Should().Contain("private async void DeleteDeckButton_Click");
            codeBehindContent.Should().Contain("private async void AddCardButton_Click");
            codeBehindContent.Should().Contain("private async void ImportDeckButton_Click");
            codeBehindContent.Should().Contain("private async void ExportAllButton_Click");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveRequiredUsingStatements_WhenUsingAsyncAndStorage()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that required using statements are present
            codeBehindContent.Should().Contain("using System.Threading.Tasks;");
            codeBehindContent.Should().Contain("using Windows.Storage;");
            codeBehindContent.Should().Contain("using Windows.Storage.Pickers;");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveEditDeckFunctionality_WhenEditButtonIsClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that EditDeckButton_Click has proper implementation
            codeBehindContent.Should().Contain("Edit Deck");
            codeBehindContent.Should().Contain("_selectedDeck.Name");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveStudyDeckFunctionality_WhenStudyButtonIsClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that StudyDeckButton_Click has proper implementation
            codeBehindContent.Should().Contain("Starting study session");
            codeBehindContent.Should().Contain("_selectedDeck.Name");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveImportExportFunctionality_WhenImportExportButtonsAreClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that Import/Export buttons have proper implementations
            codeBehindContent.Should().Contain("ImportDeckButton_Click");
            codeBehindContent.Should().Contain("ExportAllButton_Click");
            codeBehindContent.Should().Contain("_decks.Count");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldUpdateButtonStates_WhenDeckIsSelected()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that button states are updated when deck is selected
            codeBehindContent.Should().Contain("DeckButton_Click");
            codeBehindContent.Should().Contain("_selectedDeck = deck");
            codeBehindContent.Should().Contain("UpdateDeckDetails()");
            codeBehindContent.Should().Contain("UpdateButtonStates()");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveUpdateButtonStatesMethod_WhenManagingButtonStates()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that UpdateButtonStates method exists and manages button states
            codeBehindContent.Should().Contain("private void UpdateButtonStates()");
            codeBehindContent.Should().Contain("EditDeckButton.IsEnabled");
            codeBehindContent.Should().Contain("DeleteDeckButton.IsEnabled");
            codeBehindContent.Should().Contain("AddCardButton.IsEnabled");
            codeBehindContent.Should().Contain("StudyDeckButton.IsEnabled");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveProperNavigation_WhenBackButtonIsClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that BackButton_Click navigates back to main page
            codeBehindContent.Should().Contain("this.Frame.Navigate(typeof(MainPage))");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DeckManagementPage_ShouldHaveStatusUpdates_WhenButtonsAreClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that all button clicks update status
            codeBehindContent.Should().Contain("UpdateStatus(");
            codeBehindContent.Should().Contain("StatusText.Text = message");
        }
    }
}

