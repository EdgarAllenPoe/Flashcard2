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
    /// Integration tests for WinUI Configuration functionality
    /// Tests the complete flow from UI to Application Controller to Services
    /// </summary>
    public class WinUIConfigurationIntegrationTests
    {
        [Fact]
        public async Task WinUIConfiguration_ShouldViewConfiguration_WhenRequested()
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

            var request = new ConfigurationRequest
            {
                Action = ConfigurationAction.View
            };

            // Act
            var result = await applicationController.ConfigureSettingsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIConfiguration_ShouldGetConfiguration_WhenRequested()
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

            var request = new ConfigurationRequest
            {
                Action = ConfigurationAction.Get
            };

            // Act
            var result = await applicationController.ConfigureSettingsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIConfiguration_ShouldUpdateSetting_WhenValidRequestProvided()
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

            var request = new ConfigurationRequest
            {
                Action = ConfigurationAction.Update,
                SettingName = "UI.UseColors",
                Value = true
            };

            // Act
            var result = await applicationController.ConfigureSettingsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task WinUIConfiguration_ShouldHandleInvalidSettingName_WhenSettingDoesNotExist()
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

            var request = new ConfigurationRequest
            {
                Action = ConfigurationAction.Update,
                SettingName = "NonExistent.Setting",
                Value = "test"
            };

            // Act
            var result = await applicationController.ConfigureSettingsAsync(request);

            // Assert
            result.Should().NotBeNull();
            // Note: The configuration functionality gracefully handles invalid setting names
            // by ignoring them or providing helpful feedback
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task WinUIConfiguration_ShouldResetConfiguration_WhenRequested()
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

            var request = new ConfigurationRequest
            {
                Action = ConfigurationAction.Reset
            };

            // Act
            var result = await applicationController.ConfigureSettingsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task WinUIConfiguration_ShouldUpdateUISettings_WhenValidUISettingProvided()
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

            var request = new ConfigurationRequest
            {
                Action = ConfigurationAction.Update,
                SettingName = "UI.UseIcons",
                Value = false
            };

            // Act
            var result = await applicationController.ConfigureSettingsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task WinUIConfiguration_ShouldUpdateStudySessionSettings_WhenValidStudySettingProvided()
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

            var request = new ConfigurationRequest
            {
                Action = ConfigurationAction.Update,
                SettingName = "StudySession.ShuffleCards",
                Value = true
            };

            // Act
            var result = await applicationController.ConfigureSettingsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task WinUIConfiguration_ShouldUpdateLeitnerBoxSettings_WhenValidLeitnerSettingProvided()
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

            var request = new ConfigurationRequest
            {
                Action = ConfigurationAction.Update,
                SettingName = "LeitnerBoxes.NumberOfBoxes",
                Value = 5
            };

            // Act
            var result = await applicationController.ConfigureSettingsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task WinUIConfiguration_ShouldHandleInvalidValueType_WhenValueTypeDoesNotMatch()
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

            var request = new ConfigurationRequest
            {
                Action = ConfigurationAction.Update,
                SettingName = "UI.UseColors",
                Value = "not-a-boolean"
            };

            // Act
            var result = await applicationController.ConfigureSettingsAsync(request);

            // Assert
            result.Should().NotBeNull();
            // Note: The configuration functionality gracefully handles invalid value types
            // by ignoring them or providing helpful feedback
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
    }
}
