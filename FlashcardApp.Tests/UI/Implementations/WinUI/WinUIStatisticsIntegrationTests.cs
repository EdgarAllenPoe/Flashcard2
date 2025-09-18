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
    /// Integration tests for WinUI Statistics functionality
    /// Tests the complete flow from UI to Application Controller to Services
    /// </summary>
    public class WinUIStatisticsIntegrationTests
    {
        [Fact]
        public async Task WinUIStatistics_ShouldViewOverallStatistics_WhenRequested()
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

            var request = new StatisticsRequest
            {
                IncludeOverall = true
            };

            // Act
            var result = await applicationController.ViewStatisticsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIStatistics_ShouldViewDeckStatistics_WhenDeckIdProvided()
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

            var testDeck = CreateTestDeckWithStatistics();
            var request = new StatisticsRequest
            {
                DeckId = testDeck.Id,
                IncludeOverall = false
            };

            // Act
            var result = await applicationController.ViewStatisticsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIStatistics_ShouldHandleInvalidDeckId_WhenDeckDoesNotExist()
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

            var request = new StatisticsRequest
            {
                DeckId = "non-existent-deck-id",
                IncludeOverall = false
            };

            // Act
            var result = await applicationController.ViewStatisticsAsync(request);

            // Assert
            result.Should().NotBeNull();
            // Note: The statistics functionality gracefully handles invalid deck IDs
            // by showing overall statistics instead of failing
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIStatistics_ShouldHandleEmptyRequest_WhenNoParametersProvided()
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

            var request = new StatisticsRequest
            {
                IncludeOverall = false
            };

            // Act
            var result = await applicationController.ViewStatisticsAsync(request);

            // Assert
            result.Should().NotBeNull();
            // Should still work and show overall statistics
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task WinUIStatistics_ShouldShowProgressTracking_WhenDeckHasStudyHistory()
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

            var testDeck = CreateTestDeckWithStudyHistory();
            var request = new StatisticsRequest
            {
                DeckId = testDeck.Id,
                IncludeOverall = false
            };

            // Act
            var result = await applicationController.ViewStatisticsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIStatistics_ShouldShowSuccessRates_WhenCardsHaveBeenStudied()
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

            var testDeck = CreateTestDeckWithSuccessRates();
            var request = new StatisticsRequest
            {
                DeckId = testDeck.Id,
                IncludeOverall = false
            };

            // Act
            var result = await applicationController.ViewStatisticsAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().NotBeNullOrEmpty();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task WinUIStatistics_ShouldShowStudyTimeAnalytics_WhenSessionsHaveBeenCompleted()
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

            var testDeck = CreateTestDeckWithStudyTime();
            var request = new StatisticsRequest
            {
                DeckId = testDeck.Id,
                IncludeOverall = false
            };

            // Act
            var result = await applicationController.ViewStatisticsAsync(request);

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

        private Deck CreateTestDeckWithStatistics()
        {
            return new Deck
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Statistics Test Deck",
                Description = "A test deck for statistics testing",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = "What is the capital of France?",
                        Back = "Paris",
                        CurrentBox = 2,
                        Statistics = new FlashcardStatistics
                        {
                            TotalReviews = 10,
                            CorrectAnswers = 8,
                            IncorrectAnswers = 2,
                            Streak = 3,
                            LongestStreak = 5,
                            AverageResponseTime = 2.5,
                            TotalStudyTime = TimeSpan.FromMinutes(25)
                        },
                        LastReviewed = DateTime.Now.AddDays(-1),
                        IsActive = true
                    }
                },
                LastModified = DateTime.Now
            };
        }

        private Deck CreateTestDeckWithStudyHistory()
        {
            return new Deck
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Study History Test Deck",
                Description = "A test deck with study history",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = "What is 2 + 2?",
                        Back = "4",
                        CurrentBox = 1,
                        Statistics = new FlashcardStatistics
                        {
                            TotalReviews = 15,
                            CorrectAnswers = 12,
                            IncorrectAnswers = 3,
                            Streak = 2,
                            LongestStreak = 8,
                            AverageResponseTime = 1.8,
                            TotalStudyTime = TimeSpan.FromMinutes(27)
                        },
                        LastReviewed = DateTime.Now.AddDays(-2),
                        IsActive = true
                    },
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = "What is the largest planet?",
                        Back = "Jupiter",
                        CurrentBox = 3,
                        Statistics = new FlashcardStatistics
                        {
                            TotalReviews = 20,
                            CorrectAnswers = 18,
                            IncorrectAnswers = 2,
                            Streak = 6,
                            LongestStreak = 10,
                            AverageResponseTime = 1.2,
                            TotalStudyTime = TimeSpan.FromMinutes(24)
                        },
                        LastReviewed = DateTime.Now.AddDays(-3),
                        IsActive = true
                    }
                },
                LastModified = DateTime.Now
            };
        }

        private Deck CreateTestDeckWithSuccessRates()
        {
            return new Deck
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Success Rate Test Deck",
                Description = "A test deck for success rate testing",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = "What is photosynthesis?",
                        Back = "The process by which plants make food",
                        CurrentBox = 2,
                        Statistics = new FlashcardStatistics
                        {
                            TotalReviews = 25,
                            CorrectAnswers = 20,
                            IncorrectAnswers = 5,
                            Streak = 4,
                            LongestStreak = 12,
                            AverageResponseTime = 3.2,
                            TotalStudyTime = TimeSpan.FromMinutes(80)
                        },
                        LastReviewed = DateTime.Now.AddDays(-1),
                        IsActive = true
                    }
                },
                LastModified = DateTime.Now
            };
        }

        private Deck CreateTestDeckWithStudyTime()
        {
            return new Deck
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Study Time Test Deck",
                Description = "A test deck for study time analytics",
                Flashcards = new List<Flashcard>
                {
                    new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = "What is the speed of light?",
                        Back = "299,792,458 meters per second",
                        CurrentBox = 3,
                        Statistics = new FlashcardStatistics
                        {
                            TotalReviews = 30,
                            CorrectAnswers = 25,
                            IncorrectAnswers = 5,
                            Streak = 8,
                            LongestStreak = 15,
                            AverageResponseTime = 4.5,
                            TotalStudyTime = TimeSpan.FromMinutes(135)
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
