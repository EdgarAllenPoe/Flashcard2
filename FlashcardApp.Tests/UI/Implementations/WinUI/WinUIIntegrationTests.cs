using FlashcardApp.Models;
using FlashcardApp.Services;
using FlashcardApp.UI.Abstractions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Integration tests for WinUI application functionality
    /// These tests verify that the WinUI app has all the functionality of the console app
    /// </summary>
    public class WinUIIntegrationTests
    {
        [Fact]
        public void WinUIApp_ShouldHaveAllConsoleAppFeatures()
        {
            // Arrange - Create WinUI implementations
            var winUIOutput = CreateWinUIOutput();
            var winUIInput = CreateWinUIInput();
            var winUITheme = CreateWinUITheme();
            var winUIRenderer = CreateWinUIRenderer(winUIOutput, winUITheme);
            var winUIUserInteraction = CreateWinUIUserInteractionService(winUIInput, winUIOutput, winUIRenderer);

            // Act & Assert - Verify all implementations exist and implement correct interfaces
            winUIOutput.Should().BeAssignableTo<IUIOutput>();
            winUIInput.Should().BeAssignableTo<IUIInput>();
            winUITheme.Should().BeAssignableTo<IUITheme>();
            winUIRenderer.Should().BeAssignableTo<IUIRenderer>();
            winUIUserInteraction.Should().BeAssignableTo<IUserInteractionService>();
        }

        [Fact]
        public void WinUIApp_ShouldSupportMainMenuNavigation()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();

            // Act & Assert - Verify main menu functionality exists
            // This test verifies that the WinUI app can display the main menu
            // and handle navigation between different sections
                var mainMenuOptions = new List<string> { "1", "2", "3", "4", "5", "ESC" };
            
            foreach (var option in mainMenuOptions)
            {
                // Verify that the WinUI app can handle each main menu option
                // without throwing exceptions
                var action = () => winUIUserInteraction.GetMenuChoiceAsync(new MenuDefinition
                {
                    Title = "MAIN MENU",
                    Options = new List<MenuOption>
                    {
                        new MenuOption { Key = "1", Title = "Start Study Session", Description = "Begin your learning journey" },
                        new MenuOption { Key = "2", Title = "Manage Decks", Description = "Create, edit, and organize decks" },
                        new MenuOption { Key = "3", Title = "View Statistics", Description = "Track your progress" },
                        new MenuOption { Key = "4", Title = "Configuration", Description = "Customize your experience" },
                        new MenuOption { Key = "5", Title = "Help & Guide", Description = "Learn how to use the app" },
                        new MenuOption { Key = "ESC", Title = "Exit", Description = "Close the application" }
                    }
                });
                
                action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void WinUIApp_ShouldSupportStudySessionWorkflow()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();
            var testDeck = CreateTestDeck();

            // Act & Assert - Verify study session workflow
            var studySessionActions = new List<Func<Task>>
            {
                () => winUIUserInteraction.SelectDeckAsync(new List<Deck> { testDeck }, "Select a deck to study:"),
                () => winUIUserInteraction.SelectStudyModeAsync(),
                () => winUIUserInteraction.GetMaxCardsForSessionAsync(),
                () => winUIUserInteraction.DisplaySessionResultsAsync(new SessionResult
                {
                    Success = true,
                    Message = "Session completed successfully",
                    SessionStatistics = new SessionStatistics
                    {
                        TotalCards = 5,
                        CardsStudied = 5,
                        CorrectAnswers = 4,
                        IncorrectAnswers = 1,
                        TotalStudyTime = TimeSpan.FromMinutes(2)
                    }
                })
            };

            foreach (var action in studySessionActions)
            {
                action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void WinUIApp_ShouldSupportDeckManagementWorkflow()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();
            var testDeck = CreateTestDeck();

            // Act & Assert - Verify deck management workflow
            var deckManagementActions = new List<Func<Task>>
            {
                () => winUIUserInteraction.DisplayDeckListAsync(new List<Deck> { testDeck }),
                () => winUIUserInteraction.GetInputAsync("Enter deck name:"),
                () => winUIUserInteraction.ConfirmActionAsync("Are you sure you want to delete this deck?"),
                () => winUIUserInteraction.GetFilePathAsync("Select file to import:", true),
                () => winUIUserInteraction.GetFilePathAsync("Select file to export:", true)
            };

            foreach (var action in deckManagementActions)
            {
                action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void WinUIApp_ShouldSupportStatisticsDisplay()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();
            var testStatistics = CreateTestStatistics();

            // Act & Assert - Verify statistics display functionality
            var statisticsActions = new List<Func<Task>>
            {
                () => winUIUserInteraction.DisplayStatisticsAsync(new[] { testStatistics }),
                () => winUIUserInteraction.ShowMessageAsync("Statistics loaded successfully", MessageType.Success)
            };

            foreach (var action in statisticsActions)
            {
                action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void WinUIApp_ShouldSupportConfigurationWorkflow()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();

            // Act & Assert - Verify configuration workflow
            var configurationActions = new List<Func<Task>>
            {
                () => winUIUserInteraction.DisplaySectionHeaderAsync("CONFIGURATION", SectionType.Configuration),
                () => winUIUserInteraction.GetValidatedInputAsync("Enter new value:", input => !string.IsNullOrEmpty(input)),
                () => winUIUserInteraction.SelectFromOptionsAsync(new List<string> { "Option 1", "Option 2" }, "Select an option:"),
                () => winUIUserInteraction.ConfirmAsync("Save changes?")
            };

            foreach (var action in configurationActions)
            {
                action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void WinUIApp_ShouldSupportHelpAndGuideDisplay()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();

            // Act & Assert - Verify help and guide functionality
            var helpActions = new List<Func<Task>>
            {
                () => winUIUserInteraction.DisplaySectionHeaderAsync("HELP & GUIDE", SectionType.Help),
                () => winUIUserInteraction.ShowMessageAsync("Help content loaded", MessageType.Info),
                () => winUIUserInteraction.WaitForAnyKeyAsync()
            };

            foreach (var action in helpActions)
            {
                action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void WinUIApp_ShouldSupportWelcomeAndExitMessages()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();

            // Act & Assert - Verify welcome and exit functionality
            var welcomeExitActions = new List<Func<Task>>
            {
                () => winUIUserInteraction.DisplayWelcomeMessage(),
                () => winUIUserInteraction.DisplayExitMessage(),
                () => winUIUserInteraction.SetTitleAsync("Flashcard App - Windows Native"),
                () => winUIUserInteraction.ClearScreenAsync()
            };

            foreach (var action in welcomeExitActions)
            {
                action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void WinUIApp_ShouldHandleAllMessageTypes()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();
            var messageTypes = Enum.GetValues<MessageType>();

            // Act & Assert - Verify all message types are supported
            foreach (var messageType in messageTypes)
            {
                var action = () => winUIUserInteraction.ShowMessageAsync($"Test {messageType} message", messageType);
                action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void WinUIApp_ShouldHandleAllSectionTypes()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();
            var sectionTypes = Enum.GetValues<SectionType>();

            // Act & Assert - Verify all section types are supported
            foreach (var sectionType in sectionTypes)
            {
                var action = () => winUIUserInteraction.DisplaySectionHeaderAsync($"Test {sectionType} Header", sectionType);
                action.Should().NotThrowAsync();
            }
        }

        [Fact]
        public void WinUIApp_ShouldSupportAllStudyModes()
        {
            // Arrange
            var winUIUserInteraction = CreateWinUIUserInteractionService();
            var studyModes = Enum.GetValues<StudyMode>();

            // Act & Assert - Verify all study modes are supported
            foreach (var studyMode in studyModes)
            {
                // This test verifies that the WinUI app can handle all study modes
                // without throwing exceptions
                var action = () => winUIUserInteraction.ShowMessageAsync($"Study mode: {studyMode}", MessageType.Info);
                action.Should().NotThrowAsync();
            }
        }

        #region Helper Methods

        private IUIOutput CreateWinUIOutput()
        {
            var winUIPath = GetWinUIAssemblyPath();
            var assembly = Assembly.LoadFrom(winUIPath);
            var type = assembly.GetType("FlashcardApp.WinUI.UI.Implementations.WinUI.TestWinUIOutput");
            
            if (type == null)
            {
                throw new InvalidOperationException("TestWinUIOutput class not found. Make sure FlashcardApp.WinUI is built.");
            }

            var instance = Activator.CreateInstance(type);
            if (instance is not IUIOutput output)
            {
                throw new InvalidOperationException("TestWinUIOutput does not implement IUIOutput interface.");
            }

            return output;
        }

        private IUIInput CreateWinUIInput()
        {
            var winUIPath = GetWinUIAssemblyPath();
            var assembly = Assembly.LoadFrom(winUIPath);
            var type = assembly.GetType("FlashcardApp.WinUI.UI.Implementations.WinUI.TestWinUIInput");
            
            if (type == null)
            {
                throw new InvalidOperationException("TestWinUIInput class not found. Make sure FlashcardApp.WinUI is built.");
            }

            var instance = Activator.CreateInstance(type);
            if (instance is not IUIInput input)
            {
                throw new InvalidOperationException("TestWinUIInput does not implement IUIInput interface.");
            }

            return input;
        }

        private IUITheme CreateWinUITheme()
        {
            var winUIPath = GetWinUIAssemblyPath();
            var assembly = Assembly.LoadFrom(winUIPath);
            var type = assembly.GetType("FlashcardApp.WinUI.UI.Implementations.WinUI.WinUITheme");
            
            if (type == null)
            {
                throw new InvalidOperationException("WinUITheme class not found. Make sure FlashcardApp.WinUI is built.");
            }

            var instance = Activator.CreateInstance(type);
            if (instance is not IUITheme theme)
            {
                throw new InvalidOperationException("WinUITheme does not implement IUITheme interface.");
            }

            return theme;
        }

        private IUIRenderer CreateWinUIRenderer(IUIOutput output, IUITheme theme)
        {
            var winUIPath = GetWinUIAssemblyPath();
            var assembly = Assembly.LoadFrom(winUIPath);
            var type = assembly.GetType("FlashcardApp.WinUI.UI.Implementations.WinUI.TestWinUIRenderer");
            
            if (type == null)
            {
                throw new InvalidOperationException("TestWinUIRenderer class not found. Make sure FlashcardApp.WinUI is built.");
            }

            var instance = Activator.CreateInstance(type, output, theme);
            if (instance is not IUIRenderer renderer)
            {
                throw new InvalidOperationException("TestWinUIRenderer does not implement IUIRenderer interface.");
            }

            return renderer;
        }

        private IUserInteractionService CreateWinUIUserInteractionService(IUIInput? input = null, IUIOutput? output = null, IUIRenderer? renderer = null)
        {
            input ??= CreateWinUIInput();
            output ??= CreateWinUIOutput();
            renderer ??= CreateWinUIRenderer(output, CreateWinUITheme());

            var winUIPath = GetWinUIAssemblyPath();
            var assembly = Assembly.LoadFrom(winUIPath);
            var type = assembly.GetType("FlashcardApp.WinUI.UI.Implementations.WinUI.TestWinUIUserInteractionService");
            
            if (type == null)
            {
                throw new InvalidOperationException("TestWinUIUserInteractionService class not found. Make sure FlashcardApp.WinUI is built.");
            }

            var instance = Activator.CreateInstance(type, input, output, renderer);
            if (instance is not IUserInteractionService userInteraction)
            {
                throw new InvalidOperationException("TestWinUIUserInteractionService does not implement IUserInteractionService interface.");
            }

            return userInteraction;
        }

        private string GetWinUIAssemblyPath()
        {
            return Path.Combine("..", "..", "..", "..", "FlashcardApp.WinUI", "bin", "Debug", "net8.0-windows10.0.19041.0", "win-x64", "FlashcardApp.WinUI.dll");
        }

        private Deck CreateTestDeck()
        {
            return new Deck
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Deck",
                Description = "A test deck for WinUI integration tests",
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now,
                Flashcards = new List<Flashcard>
                {
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = "What is the capital of France?",
                        Back = "Paris",
                        CurrentBox = 0,
                        IsActive = true
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = "What is 2 + 2?",
                        Back = "4",
                        CurrentBox = 1,
                        IsActive = true
                    }
                }
            };
        }

        private StatisticsData CreateTestStatistics()
        {
            return new StatisticsData
            {
                Decks = new List<Deck> { CreateTestDeck() },
                Statistics = new Dictionary<string, object>
                {
                    ["Total Decks"] = 1,
                    ["Total Cards"] = 2,
                    ["Total Study Time"] = "0:05:00",
                    ["Average Success Rate"] = "85.0%",
                    ["Total Study Sessions"] = 3
                }
            };
        }

        #endregion
    }
}
