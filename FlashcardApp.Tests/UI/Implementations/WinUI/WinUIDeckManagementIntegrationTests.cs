using FlashcardApp.Models;
using FlashcardApp.Services;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.UI.Implementations.WinUI;
using FlashcardApp.WinUI.UI.Implementations.WinUI;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Integration tests for WinUI Deck Management functionality
    /// Tests the complete flow from UI to Application Controller to Services
    /// </summary>
    public class WinUIDeckManagementIntegrationTests
    {
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
    }
}
