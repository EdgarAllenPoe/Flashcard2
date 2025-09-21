using FlashcardApp.UI.Abstractions;
using FlashcardApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// Test-friendly WinUI implementation of IUIRenderer for unit testing
    /// This version can be instantiated without WinUI dependencies
    /// </summary>
    public class TestWinUIRenderer : IUIRenderer
    {
        private readonly TestWinUIOutput _output;
        private readonly IUITheme _theme;

        public TestWinUIRenderer(TestWinUIOutput output, IUITheme theme)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
        }

        public Task RenderMenuAsync(MenuDefinition menu)
        {
            _output.WriteLine($"=== {menu.Title} ===");
            foreach (var option in menu.Options)
            {
                _output.WriteLine($"{option.Key}. {option.Title} - {option.Description}");
            }
            return Task.CompletedTask;
        }

        public Task RenderMessageAsync(MessageDefinition message)
        {
            var icon = _theme.GetIconForMessageType(message.Type);
            var color = _theme.GetColorForMessageType(message.Type);

            _output.SetForegroundColor(color);
            _output.WriteLine($"{icon} {message.Text}");
            _output.ResetColor();

            return Task.CompletedTask;
        }

        public Task RenderStatisticsAsync(StatisticsData stats)
        {
            _output.WriteLine("=== Statistics ===");
            foreach (var stat in stats.Statistics)
            {
                _output.WriteLine($"{stat.Key}: {stat.Value}");
            }
            return Task.CompletedTask;
        }

        public async Task RenderDeckListAsync(List<Deck> decks)
        {
            _output.WriteLine("=== Deck List ===");
            for (int i = 0; i < decks.Count; i++)
            {
                await RenderDeckCardAsync(decks[i], i + 1);
            }
        }

        public Task RenderSessionResultsAsync(SessionResult result)
        {
            _output.WriteLine("=== Session Results ===");
            _output.WriteLine($"Success: {result.Success}");
            _output.WriteLine($"Message: {result.Message}");
            if (result.SessionStatistics != null)
            {
                _output.WriteLine($"Cards Studied: {result.SessionStatistics.CardsStudied}");
                _output.WriteLine($"Correct Answers: {result.SessionStatistics.CorrectAnswers}");
                _output.WriteLine($"Incorrect Answers: {result.SessionStatistics.IncorrectAnswers}");
            }
            return Task.CompletedTask;
        }

        public Task RenderSectionHeaderAsync(string title, SectionType sectionType)
        {
            var icon = _theme.GetIconForSection(sectionType);
            var color = _theme.GetColorForSection(sectionType);

            _output.SetForegroundColor(color);
            _output.WriteLine($"\n{icon} {title}");
            _output.WriteLine(new string('=', title.Length + icon.Length + 2));
            _output.ResetColor();

            return Task.CompletedTask;
        }

        public Task RenderDeckCardAsync(Deck deck, int number)
        {
            _output.WriteLine($"{number}. {deck.Name}");
            _output.WriteLine($"   Description: {deck.Description}");
            _output.WriteLine($"   Cards: {deck.TotalCards}");
            _output.WriteLine($"   Created: {deck.CreatedDate:yyyy-MM-dd}");
            return Task.CompletedTask;
        }

        public Task RenderPressAnyKeyAsync()
        {
            _output.WriteLine("\nPress any key to continue...");
            return Task.CompletedTask;
        }

        public Task RenderInputPromptAsync(string prompt)
        {
            _output.SetForegroundColor(_theme.GetInputPromptColor());
            _output.Write($"{prompt}: ");
            _output.ResetColor();
            return Task.CompletedTask;
        }

        public Task RenderWelcomeMessageAsync()
        {
            _output.WriteLine("Welcome to Flashcard App!");
            _output.WriteLine("Your intelligent study companion using the Leitner Box system.");
            return Task.CompletedTask;
        }

        public Task RenderExitMessageAsync()
        {
            _output.WriteLine("Thank you for using Flashcard App!");
            _output.WriteLine("Keep studying and improving!");
            return Task.CompletedTask;
        }
    }
}
