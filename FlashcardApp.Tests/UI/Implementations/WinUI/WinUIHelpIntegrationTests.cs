using FlashcardApp.Models;
using FlashcardApp.Services;
using FlashcardApp.UI.Abstractions;
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
    /// Integration tests for WinUI Help & Guide functionality
    /// Tests the complete flow from UI to Application Controller to Services
    /// </summary>
    public class WinUIHelpIntegrationTests
    {
        [Fact]
        public async Task WinUIHelp_ShouldShowMainHelp_WhenRequested()
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

            var request = new HelpRequest
            {
                Topic = "main"
            };

            // Act
            var result = await applicationController.ShowHelpAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIHelp_ShouldShowStudySessionHelp_WhenStudySessionTopicRequested()
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

            var request = new HelpRequest
            {
                Topic = "study-session"
            };

            // Act
            var result = await applicationController.ShowHelpAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIHelp_ShouldShowDeckManagementHelp_WhenDeckManagementTopicRequested()
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

            var request = new HelpRequest
            {
                Topic = "deck-management"
            };

            // Act
            var result = await applicationController.ShowHelpAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIHelp_ShouldShowStatisticsHelp_WhenStatisticsTopicRequested()
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

            var request = new HelpRequest
            {
                Topic = "statistics"
            };

            // Act
            var result = await applicationController.ShowHelpAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIHelp_ShouldShowConfigurationHelp_WhenConfigurationTopicRequested()
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

            var request = new HelpRequest
            {
                Topic = "configuration"
            };

            // Act
            var result = await applicationController.ShowHelpAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIHelp_ShouldShowLeitnerBoxHelp_WhenLeitnerBoxTopicRequested()
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

            var request = new HelpRequest
            {
                Topic = "leitner-box"
            };

            // Act
            var result = await applicationController.ShowHelpAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIHelp_ShouldShowKeyboardShortcutsHelp_WhenShortcutsTopicRequested()
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

            var request = new HelpRequest
            {
                Topic = "keyboard-shortcuts"
            };

            // Act
            var result = await applicationController.ShowHelpAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIHelp_ShouldHandleEmptyTopic_WhenNoTopicProvided()
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

            var request = new HelpRequest
            {
                Topic = null
            };

            // Act
            var result = await applicationController.ShowHelpAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIHelp_ShouldHandleUnknownTopic_WhenInvalidTopicProvided()
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

            var request = new HelpRequest
            {
                Topic = "unknown-topic"
            };

            // Act
            var result = await applicationController.ShowHelpAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
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
    }
}
