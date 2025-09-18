using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashcardApp.Models;
using FlashcardApp.UI.Abstractions;

namespace FlashcardApp.UI.Implementations.Console
{
    /// <summary>
    /// Console-specific implementation of IUserInteractionService
    /// </summary>
    public class ConsoleUserInteractionService : IUserInteractionService
    {
        private readonly IUIInput _input;
        private readonly IUIOutput _output;
        private readonly IUITheme _theme;
        private readonly IUIRenderer _renderer;

        public ConsoleUserInteractionService(IUIInput input, IUIOutput output, IUITheme theme, IUIRenderer renderer)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public async Task<Deck?> SelectDeckAsync(List<Deck> decks, string prompt)
        {
            return await Task.Run(async () =>
            {
                while (true)
                {
                    _output.Clear();
                    await _renderer.RenderSectionHeaderAsync("SELECT DECK", SectionType.DeckList);
                    _output.WriteLine("");

                    for (int i = 0; i < decks.Count; i++)
                    {
                        var deck = decks[i];
                        _output.WriteLine($"    {i + 1}. {deck.Name} ({deck.ActiveCards} active cards)");
                    }

                    _output.WriteLine("");
                    _output.WriteLine("    ESC. Cancel");
                    _output.WriteLine("");
                    await _renderer.RenderInputPromptAsync("Enter your choice");

                    var keyInfo = await _input.ReadKeyAsync(true);
                    if (keyInfo.Key == UIKey.Escape)
                    {
                        return null; // ESC pressed - return null to indicate cancellation
                    }

                    _output.Write(keyInfo.KeyChar.ToString());
                    var remainingInput = await _input.ReadLineAsync();
                    var input = keyInfo.KeyChar + remainingInput;

                    if (int.TryParse(input, out int choice) && choice > 0 && choice <= decks.Count)
                    {
                        return decks[choice - 1];
                    }

                    // Invalid selection - show error and loop again
                    await ShowMessageAsync("Invalid selection. Please try again.", MessageType.Error);
                    await WaitForAnyKeyAsync();
                }
            });
        }

        public async Task<StudyMode> SelectStudyModeAsync()
        {
            return await Task.Run(async () =>
            {
                _output.Clear();
                await _renderer.RenderSectionHeaderAsync("SELECT STUDY MODE", SectionType.StudySession);
                _output.WriteLine("");

                _output.WriteLine("    1. Front to Back (default) - Show question first, then answer");
                _output.WriteLine("    2. Back to Front - Show answer first, then question");
                _output.WriteLine("    3. Mixed - Randomly switch between modes");
                _output.WriteLine("");
                await _renderer.RenderInputPromptAsync("Enter your choice (default: FrontToBack)");

                var input = await _input.GetImmediateChoiceAsync();
                if (input == "ESC")
                {
                    return StudyMode.FrontToBack; // Return default on ESC
                }

                return input switch
                {
                    "2" => StudyMode.BackToFront,
                    "3" => StudyMode.Mixed,
                    _ => StudyMode.FrontToBack
                };
            });
        }

        public async Task<int> GetMaxCardsForSessionAsync()
        {
            return await Task.Run(async () =>
            {
                await _renderer.RenderInputPromptAsync("Maximum cards for this session (default: 100)");

                // Handle input properly for numeric input with Enter support
                var keyInfo = await _input.ReadKeyAsync(true);
                if (keyInfo.Key == UIKey.Escape)
                {
                    return 100; // Return default on ESC
                }

                // If Enter was pressed immediately, use default
                if (keyInfo.Key == UIKey.Enter)
                {
                    return 100;
                }

                // Otherwise, read the full input line
                _output.Write(keyInfo.KeyChar.ToString());
                var remainingInput = await _input.ReadLineAsync();
                var input = keyInfo.KeyChar + remainingInput;

                if (int.TryParse(input, out int maxCards) && maxCards > 0)
                {
                    return Math.Min(maxCards, 1000); // Cap at reasonable limit
                }

                return 100;
            });
        }

        public async Task<bool> ConfirmActionAsync(string message)
        {
            return await Task.Run(async () =>
            {
                await _renderer.RenderInputPromptAsync($"{message} (y/N)");
                var confirmation = await _input.ReadLineAsync();
                return confirmation?.ToLower() == "y" || confirmation?.ToLower() == "yes";
            });
        }

        public async Task<string> GetInputAsync(string prompt)
        {
            return await Task.Run(async () =>
            {
                await _renderer.RenderInputPromptAsync(prompt);
                return await _input.ReadLineAsync();
            });
        }

        public async Task<string> GetValidatedInputAsync(string prompt, Func<string, bool> validator)
        {
            return await Task.Run(async () =>
            {
                while (true)
                {
                    await _renderer.RenderInputPromptAsync(prompt);
                    var input = await _input.ReadLineAsync();

                    if (validator(input))
                    {
                        return input;
                    }

                    await ShowMessageAsync("Invalid input. Please try again.", MessageType.Error);
                }
            });
        }

        public async Task<string?> SelectFromOptionsAsync(List<string> options, string prompt)
        {
            return await Task.Run(async () =>
            {
                _output.WriteLine("");
                for (int i = 0; i < options.Count; i++)
                {
                    _output.WriteLine($"    {i + 1}. {options[i]}");
                }
                _output.WriteLine("");
                await _renderer.RenderInputPromptAsync(prompt);

                var input = await _input.GetImmediateChoiceAsync();
                if (input == "ESC")
                {
                    return null;
                }

                if (int.TryParse(input, out int choice) && choice > 0 && choice <= options.Count)
                {
                    return options[choice - 1];
                }

                await ShowMessageAsync("Invalid selection.", MessageType.Error);
                return null;
            });
        }

        public async Task<string?> GetFilePathAsync(string prompt, bool allowBrowse = true)
        {
            return await Task.Run(async () =>
            {
                await _renderer.RenderInputPromptAsync(prompt);
                var input = await _input.GetUserChoiceAsync();

                if (input == "ESC")
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(input) && allowBrowse)
                {
                    // Show files in current directory
                    var currentDir = Directory.GetCurrentDirectory();
                    var supportedExtensions = new[] { "*.csv", "*.xlsx", "*.json" };
                    var files = new List<string>();

                    foreach (var extension in supportedExtensions)
                    {
                        var foundFiles = Directory.GetFiles(currentDir, extension);
                        if (extension == "*.json")
                        {
                            // Filter out config.json
                            foundFiles = foundFiles.Where(f => !f.EndsWith("config.json")).ToArray();
                        }
                        files.AddRange(foundFiles);
                    }

                    if (!files.Any())
                    {
                        await ShowMessageAsync("No supported files found in current directory.", MessageType.Error);
                        return null;
                    }

                    _output.WriteLine("");
                    _output.WriteLine("Available files:");
                    for (int i = 0; i < files.Count; i++)
                    {
                        var fileName = Path.GetFileName(files[i]);
                        var extension = Path.GetExtension(files[i]).ToUpper();
                        _output.WriteLine($"  {i + 1}. {fileName} ({extension})");
                    }
                    _output.WriteLine("");
                    await _renderer.RenderInputPromptAsync("Enter the number of the file to import (or ESC to cancel)");

                    var keyInfo = await _input.ReadKeyAsync(true);
                    if (keyInfo.Key == UIKey.Escape)
                    {
                        return null;
                    }

                    var choice = keyInfo.KeyChar.ToString();
                    if (int.TryParse(choice, out int fileIndex) && fileIndex > 0 && fileIndex <= files.Count)
                    {
                        return files[fileIndex - 1];
                    }

                    await ShowMessageAsync("Invalid file selection.", MessageType.Error);
                    return null;
                }

                return input;
            });
        }

        public async Task WaitForAnyKeyAsync()
        {
            await _renderer.RenderPressAnyKeyAsync();
            await _input.WaitForAnyKeyAsync();
        }

        public async Task ShowMessageAsync(string message, MessageType messageType = MessageType.Info)
        {
            await _renderer.RenderMessageAsync(new MessageDefinition { Text = message, Type = messageType });
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
            await _renderer.RenderStatisticsAsync(statistics.FirstOrDefault() ?? new StatisticsData());
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
