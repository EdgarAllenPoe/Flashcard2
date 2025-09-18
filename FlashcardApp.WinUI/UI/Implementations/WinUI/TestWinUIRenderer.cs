using FlashcardApp.UI.Abstractions;
using FlashcardApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// Test-friendly WinUI implementation of IUIRenderer for unit testing
    /// This version can be instantiated without UI controls
    /// </summary>
    public class TestWinUIRenderer : IUIRenderer
    {
        private readonly IUIOutput _output;
        private readonly IUITheme _theme;
        private readonly List<string> _renderedContent = new List<string>();

        public TestWinUIRenderer(IUIOutput output, IUITheme theme)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
        }

        public IReadOnlyList<string> RenderedContent => _renderedContent.AsReadOnly();

        public Task RenderMenuAsync(MenuDefinition menu)
        {
            var content = $"MENU: {menu.Title}";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            
            foreach (var option in menu.Options)
            {
                var optionContent = $"  {option.Key}. {option.Title} - {option.Description}";
                _renderedContent.Add(optionContent);
                _output.WriteLine(optionContent);
            }
            
            return Task.CompletedTask;
        }

        public Task RenderMessageAsync(MessageDefinition message)
        {
            var icon = _theme.GetIconForMessageType(message.Type);
            var content = $"{icon} {message.Text}";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            return Task.CompletedTask;
        }

        public Task RenderStatisticsAsync(StatisticsData stats)
        {
            var content = "STATISTICS:";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            
            foreach (var stat in stats.Statistics)
            {
                var statContent = $"  {stat.Key}: {stat.Value}";
                _renderedContent.Add(statContent);
                _output.WriteLine(statContent);
            }
            
            return Task.CompletedTask;
        }

        public Task RenderDeckListAsync(List<Deck> decks)
        {
            var content = "DECK LIST:";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            
            for (int i = 0; i < decks.Count; i++)
            {
                var deck = decks[i];
                var deckContent = $"  {i + 1}. {deck.Name} ({deck.ActiveCards} cards)";
                _renderedContent.Add(deckContent);
                _output.WriteLine(deckContent);
            }
            
            return Task.CompletedTask;
        }

        public Task RenderSessionResultsAsync(SessionResult result)
        {
            var content = $"SESSION RESULTS: {result.Message}";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            
            if (result.SessionStatistics != null)
            {
                var stats = result.SessionStatistics;
                var statsContent = $"  Cards Studied: {stats.CardsStudied}, Correct: {stats.CorrectAnswers}, Incorrect: {stats.IncorrectAnswers}";
                _renderedContent.Add(statsContent);
                _output.WriteLine(statsContent);
            }
            
            return Task.CompletedTask;
        }

        public Task RenderSectionHeaderAsync(string title, SectionType sectionType)
        {
            var icon = _theme.GetIconForSection(sectionType);
            var content = $"{icon} {title}";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            return Task.CompletedTask;
        }

        public Task RenderDeckCardAsync(Deck deck, int number)
        {
            var content = $"DECK {number}: {deck.Name}";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            return Task.CompletedTask;
        }

        public Task RenderPressAnyKeyAsync()
        {
            var content = "Press any key to continue...";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            return Task.CompletedTask;
        }

        public Task RenderInputPromptAsync(string prompt)
        {
            var content = $"{prompt} ";
            _renderedContent.Add(content);
            _output.Write(content);
            return Task.CompletedTask;
        }

        public Task RenderWelcomeMessageAsync()
        {
            var content = "Welcome to Flashcard App!";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            return Task.CompletedTask;
        }

        public Task RenderExitMessageAsync()
        {
            var content = "Thank you for using Flashcard App!";
            _renderedContent.Add(content);
            _output.WriteLine(content);
            return Task.CompletedTask;
        }
    }
}
