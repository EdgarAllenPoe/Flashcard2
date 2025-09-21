using FlashcardApp.Models;
using FlashcardApp.UI.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// Test-friendly WinUI implementation of IUserInteractionService for unit testing
    /// This version can be instantiated without WinUI dependencies
    /// </summary>
    public class TestWinUIUserInteractionService : IUserInteractionService
    {
        private readonly TestWinUIInput _input;
        private readonly TestWinUIOutput _output;
        private readonly TestWinUIRenderer _renderer;

        public TestWinUIUserInteractionService(TestWinUIInput input, TestWinUIOutput output, TestWinUIRenderer renderer)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public async Task<Deck?> SelectDeckAsync(List<Deck> decks, string prompt)
        {
            await _renderer.RenderInputPromptAsync(prompt);
            await _renderer.RenderDeckListAsync(decks);

            var choice = await _input.GetUserChoiceAsync();
            if (int.TryParse(choice, out int index) && index > 0 && index <= decks.Count)
            {
                return decks[index - 1];
            }
            return null;
        }

        public async Task<StudyMode> SelectStudyModeAsync()
        {
            var menu = new MenuDefinition
            {
                Title = "Select Study Mode",
                Options = new List<MenuOption>
                {
                    new MenuOption { Key = "1", Title = "Front to Back", Description = "Study cards from front to back" },
                    new MenuOption { Key = "2", Title = "Back to Front", Description = "Study cards from back to front" },
                    new MenuOption { Key = "3", Title = "Random", Description = "Study cards in random order" }
                }
            };

            await _renderer.RenderMenuAsync(menu);
            var choice = await _input.GetUserChoiceAsync();

            return choice switch
            {
                "1" => StudyMode.FrontToBack,
                "2" => StudyMode.BackToFront,
                "3" => StudyMode.Mixed,
                _ => StudyMode.FrontToBack
            };
        }

        public async Task<int> GetMaxCardsForSessionAsync()
        {
            await _renderer.RenderInputPromptAsync("Enter maximum number of cards for this session");
            var input = await _input.GetUserChoiceAsync();
            return int.TryParse(input, out int maxCards) ? maxCards : 10;
        }

        public async Task<bool> ConfirmActionAsync(string message)
        {
            await _renderer.RenderInputPromptAsync($"{message} (y/n)");
            var choice = await _input.GetUserChoiceAsync();
            return choice?.ToLower() == "y" || choice?.ToLower() == "yes";
        }

        public async Task<string> GetInputAsync(string prompt)
        {
            await _renderer.RenderInputPromptAsync(prompt);
            return await _input.GetUserChoiceAsync();
        }

        public async Task<string> GetValidatedInputAsync(string prompt, Func<string, bool> validator)
        {
            string input;
            do
            {
                await _renderer.RenderInputPromptAsync(prompt);
                input = await _input.GetUserChoiceAsync();
            } while (!validator(input));

            return input;
        }

        public async Task<string?> SelectFromOptionsAsync(List<string> options, string prompt)
        {
            await _renderer.RenderInputPromptAsync(prompt);
            for (int i = 0; i < options.Count; i++)
            {
                _output.WriteLine($"{i + 1}. {options[i]}");
            }

            var choice = await _input.GetUserChoiceAsync();
            if (int.TryParse(choice, out int index) && index > 0 && index <= options.Count)
            {
                return options[index - 1];
            }
            return null;
        }

        public async Task<string?> GetFilePathAsync(string prompt, bool allowBrowse = true)
        {
            await _renderer.RenderInputPromptAsync(prompt);
            return await _input.GetUserChoiceAsync();
        }

        public async Task WaitForAnyKeyAsync()
        {
            await _renderer.RenderPressAnyKeyAsync();
            await _input.WaitForAnyKeyAsync();
        }

        public async Task ShowMessageAsync(string message, MessageType messageType = MessageType.Info)
        {
            var messageDef = new MessageDefinition { Text = message, Type = messageType };
            await _renderer.RenderMessageAsync(messageDef);
        }

        public async Task DisplayWelcomeMessage()
        {
            await _renderer.RenderWelcomeMessageAsync();
        }

        public async Task DisplayExitMessage()
        {
            await _renderer.RenderExitMessageAsync();
        }

        public async Task DisplaySectionHeaderAsync(string title, SectionType sectionType)
        {
            await _renderer.RenderSectionHeaderAsync(title, sectionType);
        }

        public async Task<string> GetMenuChoiceAsync(MenuDefinition menu)
        {
            await _renderer.RenderMenuAsync(menu);
            return await _input.GetUserChoiceAsync();
        }

        public async Task DisplayDeckListAsync(IEnumerable<Deck> decks)
        {
            await _renderer.RenderDeckListAsync(decks.ToList());
        }

        public async Task DisplaySessionResultsAsync(SessionResult results)
        {
            await _renderer.RenderSessionResultsAsync(results);
        }

        public async Task DisplayStatisticsAsync(IEnumerable<StatisticsData> statistics)
        {
            foreach (var stat in statistics)
            {
                await _renderer.RenderStatisticsAsync(stat);
            }
        }

        public async Task<string> GetImmediateChoiceAsync()
        {
            return await _input.GetImmediateChoiceAsync();
        }

        public async Task ClearScreenAsync()
        {
            _output.Clear();
            await Task.CompletedTask;
        }

        public async Task SetTitleAsync(string title)
        {
            _output.SetTitle(title);
            await Task.CompletedTask;
        }

        public async Task SetCursorVisibleAsync(bool visible)
        {
            _output.SetCursorVisible(visible);
            await Task.CompletedTask;
        }

        public async Task<bool> ConfirmAsync(string message)
        {
            return await ConfirmActionAsync(message);
        }
    }
}
