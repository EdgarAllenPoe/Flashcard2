using FlashcardApp.Models;
using FlashcardApp.Services;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.UI.Implementations.Console;

namespace FlashcardApp.UI
{
    /// <summary>
    /// Demonstration class showing how the new UI abstraction architecture works
    /// This is a simplified example that shows the separation of concerns
    /// </summary>
    public class ConsoleUIDemo
    {
        private readonly ConsoleApplicationService _applicationService;
        private readonly IUserInteractionService _userInteraction;

        public ConsoleUIDemo(ConsoleApplicationService applicationService, IUserInteractionService userInteraction)
        {
            _applicationService = applicationService;
            _userInteraction = userInteraction;
        }

        public async Task RunDemoAsync()
        {
            // Set up the console
            await _userInteraction.SetTitleAsync("Flashcard App - Architecture Demo");
            await _userInteraction.SetCursorVisibleAsync(true);

            // Display welcome message using the abstraction
            await _userInteraction.DisplayWelcomeMessage();

            // Get configuration using the service layer
            var config = await _applicationService.GetConfigurationAsync();
            await _userInteraction.ShowMessageAsync($"Configuration loaded: {config.UI.UseColors} colors, {config.UI.UseIcons} icons");

            // Get all decks using the service layer
            var decks = await _applicationService.GetAllDecksAsync();
            await _userInteraction.ShowMessageAsync($"Found {decks.Count} decks");

            // Display deck list using the UI abstraction
            await _userInteraction.DisplayDeckListAsync(decks);

            // Show statistics using the service layer
            var statistics = await _applicationService.GetStatisticsAsync();
            if (statistics.Success)
            {
                // Convert StatisticsResult to StatisticsData for display
                var statisticsData = new StatisticsData
                {
                    Decks = statistics.Decks,
                    Statistics = new Dictionary<string, object>
                    {
                        ["Total Decks"] = statistics.OverallStatistics?.TotalDecks ?? 0,
                        ["Total Cards"] = statistics.OverallStatistics?.TotalCards ?? 0,
                        ["Total Study Time"] = statistics.OverallStatistics?.TotalStudyTime.ToString() ?? "0:00:00",
                        ["Average Success Rate"] = $"{statistics.OverallStatistics?.AverageSuccessRate:F1}%",
                        ["Total Study Sessions"] = statistics.OverallStatistics?.TotalStudySessions ?? 0
                    }
                };
                await _userInteraction.DisplayStatisticsAsync(new[] { statisticsData });
            }

            // Wait for user input
            await _userInteraction.WaitForAnyKeyAsync();

            // Display exit message
            await _userInteraction.DisplayExitMessage();
        }
    }
}
