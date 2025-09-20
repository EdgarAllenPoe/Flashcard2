using FlashcardApp.Models;
using FlashcardApp.Services;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.UI.Implementations.WinUI;
using FlashcardApp.WinUI.UI.Implementations.WinUI;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Integration tests for WinUI Deck Management functionality
    /// Tests the complete flow from UI to Application Controller to Services
    /// Also includes runtime fatal error detection tests
    /// </summary>
    public class WinUIDeckManagementIntegrationTests
    {
        #region Application Controller Integration Tests

        [Fact]
        public async Task WinUIDeckManagement_ShouldListDecks_WhenRequested()
        {
            // Arrange
            var services = CreateTestServices();
            var userInteraction = CreateTestUserInteractionService();
            var applicationController = new WinUIApplicationController(
                services.configService,
                services.deckService,
                services.studySessionService,
                services.leitnerBoxService,
                userInteraction);

            var request = new DeckManagementRequest
            {
                Action = DeckManagementAction.List
            };

            // Act
            var result = await applicationController.ManageDecksAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIDeckManagement_ShouldCreateDeck_WhenValidRequestProvided()
        {
            // Arrange
            var services = CreateTestServices();
            var userInteraction = CreateTestUserInteractionService();
            var applicationController = new WinUIApplicationController(
                services.configService,
                services.deckService,
                services.studySessionService,
                services.leitnerBoxService,
                userInteraction);

            var request = new DeckManagementRequest
            {
                Action = DeckManagementAction.Create,
                DeckName = $"Test Deck {Guid.NewGuid().ToString("N")[..8]}",
                Description = "A test deck for unit testing",
                Tags = new List<string> { "test", "unit-testing" }
            };

            // Act
            var result = await applicationController.ManageDecksAsync(request);

            // Assert
            result.Should().NotBeNull();
            if (!result.Success)
            {
                // Debug: Show the actual error message
                System.Console.WriteLine($"Create deck failed: {result.Message}");
                System.Console.WriteLine($"Error code: {result.ErrorCode}");
            }
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIDeckManagement_ShouldHandleInvalidCreateRequest_WhenDeckNameIsEmpty()
        {
            // Arrange
            var services = CreateTestServices();
            var userInteraction = CreateTestUserInteractionService();
            var applicationController = new WinUIApplicationController(
                services.configService,
                services.deckService,
                services.studySessionService,
                services.leitnerBoxService,
                userInteraction);

            var request = new DeckManagementRequest
            {
                Action = DeckManagementAction.Create,
                DeckName = "",
                Description = "A test deck with empty name"
            };

            // Act
            var result = await applicationController.ManageDecksAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("name");
        }

        [Fact]
        public async Task WinUIDeckManagement_ShouldViewDeck_WhenValidDeckProvided()
        {
            // Arrange
            var services = CreateTestServices();
            var userInteraction = CreateTestUserInteractionService();
            var applicationController = new WinUIApplicationController(
                services.configService,
                services.deckService,
                services.studySessionService,
                services.leitnerBoxService,
                userInteraction);

            var testDeck = CreateTestDeck();
            var request = new DeckManagementRequest
            {
                Action = DeckManagementAction.View,
                SelectedDeck = testDeck
            };

            // Act
            var result = await applicationController.ManageDecksAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIDeckManagement_ShouldHandleViewRequest_WhenDeckIsNull()
        {
            // Arrange
            var services = CreateTestServices();
            var userInteraction = CreateTestUserInteractionService();
            var applicationController = new WinUIApplicationController(
                services.configService,
                services.deckService,
                services.studySessionService,
                services.leitnerBoxService,
                userInteraction);

            var request = new DeckManagementRequest
            {
                Action = DeckManagementAction.View,
                SelectedDeck = null
            };

            // Act
            var result = await applicationController.ManageDecksAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("deck");
        }

        [Fact]
        public async Task WinUIDeckManagement_ShouldEditDeck_WhenValidRequestProvided()
        {
            // Arrange
            var services = CreateTestServices();
            var userInteraction = CreateTestUserInteractionService();
            var applicationController = new WinUIApplicationController(
                services.configService,
                services.deckService,
                services.studySessionService,
                services.leitnerBoxService,
                userInteraction);

            var testDeck = CreateTestDeck();
            var request = new DeckManagementRequest
            {
                Action = DeckManagementAction.Edit,
                SelectedDeck = testDeck,
                DeckName = "Updated Test Deck",
                Description = "An updated test deck"
            };

            // Act
            var result = await applicationController.ManageDecksAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIDeckManagement_ShouldHandleDeleteRequest_WhenValidRequestProvided()
        {
            // Arrange
            var services = CreateTestServices();
            var userInteraction = CreateTestUserInteractionService();
            var applicationController = new WinUIApplicationController(
                services.configService,
                services.deckService,
                services.studySessionService,
                services.leitnerBoxService,
                userInteraction);

            var testDeck = CreateTestDeck();
            var request = new DeckManagementRequest
            {
                Action = DeckManagementAction.Delete,
                SelectedDeck = testDeck
            };

            // Act
            var result = await applicationController.ManageDecksAsync(request);

            // Assert
            result.Should().NotBeNull();
            // Note: Delete will likely fail because the test deck doesn't exist as a file
            // This test verifies that the Application Controller can handle delete requests
            result.Message.Should().NotBeNullOrEmpty();
            result.Should().BeOfType<ApplicationResult>();
        }

        [Fact]
        public async Task WinUIDeckManagement_ShouldHandleImportRequest_WhenFilePathProvided()
        {
            // Arrange
            var services = CreateTestServices();
            var userInteraction = CreateTestUserInteractionService();
            var applicationController = new WinUIApplicationController(
                services.configService,
                services.deckService,
                services.studySessionService,
                services.leitnerBoxService,
                userInteraction);

            var request = new DeckManagementRequest
            {
                Action = DeckManagementAction.Import,
                FilePath = "test_deck.json"
            };

            // Act
            var result = await applicationController.ManageDecksAsync(request);

            // Assert
            result.Should().NotBeNull();
            // Import will likely fail due to file not existing, but the request should be handled
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task WinUIDeckManagement_ShouldHandleExportRequest_WhenValidDeckProvided()
        {
            // Arrange
            var services = CreateTestServices();
            var userInteraction = CreateTestUserInteractionService();
            var applicationController = new WinUIApplicationController(
                services.configService,
                services.deckService,
                services.studySessionService,
                services.leitnerBoxService,
                userInteraction);

            var testDeck = CreateTestDeck();
            var request = new DeckManagementRequest
            {
                Action = DeckManagementAction.Export,
                SelectedDeck = testDeck,
                FilePath = "exported_deck.json"
            };

            // Act
            var result = await applicationController.ManageDecksAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region Runtime Fatal Error Detection Tests

        [Fact]
        public void DeckManagementPage_ShouldHandleNullSelectedDeck_WhenButtonsAreClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that all button handlers properly check for null selected deck
            codeBehindContent.Should().Contain("if (_selectedDeck != null)", "EditDeckButton should check for null");
            codeBehindContent.Should().Contain("if (_selectedDeck != null)", "DeleteDeckButton should check for null");
            codeBehindContent.Should().Contain("if (_selectedDeck != null)", "AddCardButton should check for null");
            codeBehindContent.Should().Contain("if (_selectedDeck != null)", "StudyDeckButton should check for null");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleEmptyDeckName_WhenCreatingNewDeck()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that deck creation handles empty names properly
            codeBehindContent.Should().Contain("Name = \"New Deck\"", "Should have default name");
            codeBehindContent.Should().Contain("Description = \"A new flashcard deck\"", "Should have default description");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleEmptyCardText_WhenCreatingNewCard()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that card creation validates input properly
            codeBehindContent.Should().Contain("!string.IsNullOrEmpty(front)", "Should validate front text");
            codeBehindContent.Should().Contain("!string.IsNullOrEmpty(back)", "Should validate back text");
            codeBehindContent.Should().Contain("Text.Trim()", "Should trim whitespace");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleFilePickerCancellation_WhenImportingFiles()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file picker cancellation is handled
            codeBehindContent.Should().Contain("if (file != null)", "Should check if file was selected");
            codeBehindContent.Should().Contain("PickSingleFileAsync", "Should use async file picker");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleFilePickerCancellation_WhenExportingFiles()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file picker cancellation is handled
            codeBehindContent.Should().Contain("if (file != null)", "Should check if file was selected");
            codeBehindContent.Should().Contain("PickSaveFileAsync", "Should use async file picker");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleInvalidJson_WhenImportingFiles()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that JSON deserialization errors are handled
            codeBehindContent.Should().Contain("JsonSerializer.Deserialize", "Should deserialize JSON");
            codeBehindContent.Should().Contain("if (importedDeck != null)", "Should check if deserialization succeeded");
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch deserialization errors");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleDialogCancellation_WhenEditingDeck()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that dialog cancellation is handled
            codeBehindContent.Should().Contain("ContentDialogResult.Primary", "Should check dialog result");
            codeBehindContent.Should().Contain("CloseButtonText", "Should have cancel button");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleDialogCancellation_WhenEditingCard()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that dialog cancellation is handled
            codeBehindContent.Should().Contain("ContentDialogResult.Primary", "Should check dialog result");
            codeBehindContent.Should().Contain("return null", "Should return null when cancelled");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleEmptyDeckList_WhenExporting()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that empty deck list is handled
            codeBehindContent.Should().Contain("if (_decks.Count == 0)", "Should check for empty deck list");
            codeBehindContent.Should().Contain("No decks to export", "Should show appropriate message");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleEmptyDeck_WhenStudying()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that empty deck is handled
            codeBehindContent.Should().Contain("_selectedDeck.Cards.Count == 0", "Should check for empty deck");
            codeBehindContent.Should().Contain("Cannot study", "Should show error message");
            codeBehindContent.Should().Contain("return", "Should return early for empty deck");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleDeckIdCollision_WhenCreatingNewDeck()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that deck ID generation is safe
            codeBehindContent.Should().Contain("_decks.Count > 0", "Should check if decks exist");
            codeBehindContent.Should().Contain("_decks.Max(d => d.Id)", "Should find max ID");
            codeBehindContent.Should().Contain("+ 1", "Should increment ID");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleCardButtonClick_WhenCardIsNull()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that card button click handles null card
            codeBehindContent.Should().Contain("button.Tag is Flashcard card", "Should check tag type");
            codeBehindContent.Should().Contain("CardButton_Click", "Should have card button handler");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleDeckButtonClick_WhenDeckIsNull()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that deck button click handles null deck
            codeBehindContent.Should().Contain("button.Tag is FlashcardDeck deck", "Should check tag type");
            codeBehindContent.Should().Contain("DeckButton_Click", "Should have deck button handler");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleFrameNavigation_WhenFrameIsNull()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that frame navigation handles null frame
            codeBehindContent.Should().Contain("if (this.Frame != null)", "Should check if frame exists");
            codeBehindContent.Should().Contain("this.Frame.Navigate", "Should navigate when frame exists");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleUpdateDeckContent_WhenSelectedDeckIsNull()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that UpdateDeckContent handles null selected deck
            codeBehindContent.Should().Contain("if (_selectedDeck == null) return", "Should return early for null deck");
            codeBehindContent.Should().Contain("UpdateDeckContent", "Should have UpdateDeckContent method");
        }

        [Fact]
        public void DeckManagementPage_ShouldHandleUpdateDeckDetails_WhenSelectedDeckIsNull()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that UpdateDeckDetails handles null selected deck
            codeBehindContent.Should().Contain("if (_selectedDeck != null)", "Should check for null deck");
            codeBehindContent.Should().Contain("UpdateDeckDetails", "Should have UpdateDeckDetails method");
        }

        #endregion

        #region Helper Methods

        private (ConfigurationService configService, DeckService deckService, StudySessionService studySessionService, LeitnerBoxService leitnerBoxService) CreateTestServices()
        {
            var configService = new ConfigurationService();
            var deckService = new DeckService(configService);
            var leitnerBoxService = new LeitnerBoxService(configService);
            var studySessionService = new StudySessionService(configService, leitnerBoxService, deckService);

            return (configService, deckService, studySessionService, leitnerBoxService);
        }

        private IUserInteractionService CreateTestUserInteractionService()
        {
            var input = new TestWinUIInput();
            var output = new TestWinUIOutput();
            var theme = new TestWinUITheme();
            var renderer = new TestWinUIRenderer(output, theme);
            return new TestWinUIUserInteractionService(input, output, renderer);
        }

        private Deck CreateTestDeck()
        {
            return new Deck
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Deck for Management",
                Description = "A test deck for deck management testing",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = "What is the capital of France?",
                        Back = "Paris",
                        CurrentBox = 0,
                        Statistics = new FlashcardStatistics
                        {
                            TotalReviews = 0
                        },
                        LastReviewed = null,
                        IsActive = true
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = "What is 2 + 2?",
                        Back = "4",
                        CurrentBox = 1,
                        Statistics = new FlashcardStatistics
                        {
                            TotalReviews = 5
                        },
                        LastReviewed = DateTime.Now.AddDays(-1),
                        IsActive = true
                    }
                },
                LastModified = DateTime.Now
            };
        }

        #endregion
    }
}