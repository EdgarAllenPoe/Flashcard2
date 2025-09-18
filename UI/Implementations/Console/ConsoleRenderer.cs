using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlashcardApp.Models;
using FlashcardApp.UI.Abstractions;

namespace FlashcardApp.UI.Implementations.Console
{
    /// <summary>
    /// Console-specific implementation of IUIRenderer
    /// </summary>
    public class ConsoleRenderer : IUIRenderer
    {
        private readonly IUIOutput _output;
        private readonly IUITheme _theme;

        public ConsoleRenderer(IUIOutput output, IUITheme theme)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
        }

        public async Task RenderMenuAsync(MenuDefinition menu)
        {
            await Task.Run(() =>
            {
                // Render section header
                RenderSectionHeader(menu.Title, menu.SectionType);

                // Render menu options
                foreach (var option in menu.Options)
                {
                    RenderMenuOption(option);
                }

                _output.WriteLine("");
            });
        }

        public async Task RenderMessageAsync(MessageDefinition message)
        {
            await Task.Run(() =>
            {
                var color = _theme.GetColorForMessageType(message.Type);
                var icon = _theme.GetIconForMessageType(message.Type);

                _output.SetForegroundColor(color);
                _output.WriteLine($"    {icon} {message.Text}");
                _output.ResetColor();
            });
        }

        public async Task RenderStatisticsAsync(StatisticsData stats)
        {
            await Task.Run(() =>
            {
                var color = _theme.GetStatisticsColor();
                _output.SetForegroundColor(color);

                // Render overall statistics
                foreach (var stat in stats.Statistics)
                {
                    _output.WriteLine($"    {stat.Key.PadRight(20)}: {stat.Value}");
                }

                _output.WriteLine("");

                // Render deck statistics
                foreach (var deck in stats.Decks)
                {
                    RenderDeckStatistics(deck);
                }

                _output.ResetColor();
            });
        }

        public async Task RenderDeckListAsync(List<Deck> decks)
        {
            await Task.Run(() =>
            {
                RenderSectionHeader("ALL DECKS", SectionType.DeckList);

                if (decks.Count == 0)
                {
                    RenderMessage(new MessageDefinition { Text = "No decks found. Create your first deck to get started!", Type = MessageType.Info });
                    return;
                }

                for (int i = 0; i < decks.Count; i++)
                {
                    RenderDeckCard(decks[i], i + 1);
                }
            });
        }

        public async Task RenderSessionResultsAsync(SessionResult result)
        {
            await Task.Run(() =>
            {
                RenderSectionHeader("SESSION COMPLETED!", SectionType.SessionResults);

                if (result.SessionStatistics != null)
                {
                    var stats = result.SessionStatistics;
                    var color = _theme.GetStatisticsColor();
                    _output.SetForegroundColor(color);

                    RenderStatCard("", "Session Time", $"{stats.TotalStudyTime:mm\\:ss}");
                    RenderStatCard("", "Cards Studied", stats.CardsStudied.ToString());
                    RenderStatCard("", "Correct Answers", stats.CorrectAnswers.ToString());
                    RenderStatCard("", "Incorrect Answers", stats.IncorrectAnswers.ToString());
                    RenderStatCard("", "Success Rate", $"{stats.SuccessRate:F1}%");

                    _output.WriteLine("");
                    _output.ResetColor();

                    // Motivational message based on performance
                    if (stats.SuccessRate >= 80)
                    {
                        RenderMessage(new MessageDefinition { Text = "Excellent work! You're mastering this material!", Type = MessageType.Success });
                    }
                    else if (stats.SuccessRate >= 60)
                    {
                        RenderMessage(new MessageDefinition { Text = "Good progress! Keep practicing to improve further!", Type = MessageType.Info });
                    }
                    else
                    {
                        RenderMessage(new MessageDefinition { Text = "Keep studying! Every mistake is a learning opportunity!", Type = MessageType.Info });
                    }
                }
            });
        }

        public async Task RenderSectionHeaderAsync(string title, SectionType sectionType)
        {
            await Task.Run(() => RenderSectionHeader(title, sectionType));
        }

        public async Task RenderDeckCardAsync(Deck deck, int number)
        {
            await Task.Run(() => RenderDeckCard(deck, number));
        }

        public async Task RenderPressAnyKeyAsync()
        {
            await Task.Run(() =>
            {
                var color = _theme.GetMenuDescriptionColor();
                _output.SetForegroundColor(color);
                _output.WriteLine("");
                _output.WriteLine("    Press any key to continue...");
                _output.ResetColor();
            });
        }

        public async Task RenderInputPromptAsync(string prompt)
        {
            await Task.Run(() =>
            {
                var color = _theme.GetInputPromptColor();
                _output.SetForegroundColor(color);
                _output.Write($"    {prompt}: ");
                _output.ResetColor();
            });
        }

        public async Task RenderWelcomeMessageAsync()
        {
            await Task.Run(() =>
            {
                _output.Clear();

                var color = _theme.GetColorForSection(SectionType.Welcome);
                var icon = _theme.GetIconForSection(SectionType.Welcome);

                _output.SetForegroundColor(color);
                _output.WriteLine("");
                _output.WriteLine($"    {icon} FLASHCARD APP v2.0");
                _output.WriteLine("");
                _output.WriteLine("    Advanced Leitner Box Spaced Repetition");
                _output.WriteLine("");
                _output.WriteLine("    Beautiful • Modern • Effective • Smart");
                _output.WriteLine("");

                _output.ResetColor();

                _output.SetForegroundColor(UIColor.Green);
                _output.WriteLine("");
                _output.WriteLine("    Welcome to your personalized learning journey!");
                _output.WriteLine("    Master any subject with scientifically-proven spaced repetition");
                _output.WriteLine("    Enjoy a beautiful, intuitive interface designed for focus");

                _output.ResetColor();
                _output.WriteLine("");
            });
        }

        public async Task RenderExitMessageAsync()
        {
            await Task.Run(() =>
            {
                _output.Clear();

                var color = _theme.GetColorForSection(SectionType.Welcome);
                _output.SetForegroundColor(color);

                _output.WriteLine("");
                _output.WriteLine("    THANK YOU FOR LEARNING!");
                _output.WriteLine("");
                _output.WriteLine("    Remember: Every study session brings you closer to mastery!");
                _output.WriteLine("    Keep up the great work and never stop learning!");
                _output.WriteLine("");

                _output.ResetColor();
                _output.WriteLine("");
            });
        }

        private void RenderSectionHeader(string title, SectionType sectionType)
        {
            var color = _theme.GetColorForSection(sectionType);
            var icon = _theme.GetIconForSection(sectionType);

            _output.SetForegroundColor(color);
            _output.WriteLine("");
            _output.WriteLine($"    {icon} {title}");
            _output.WriteLine("");
            _output.ResetColor();
        }

        private void RenderMenuOption(MenuOption option)
        {
            var optionColor = _theme.GetMenuOptionColor();
            var descriptionColor = _theme.GetMenuDescriptionColor();

            _output.SetForegroundColor(optionColor);
            _output.Write($"    {option.Key}. ");

            _output.SetForegroundColor(UIColor.White);
            _output.Write(option.Title);

            if (!string.IsNullOrEmpty(option.Description))
            {
                _output.SetForegroundColor(descriptionColor);
                _output.WriteLine($" - {option.Description}");
            }
            else
            {
                _output.WriteLine("");
            }

            _output.ResetColor();
        }

        private void RenderDeckCard(Deck deck, int number)
        {
            var optionColor = _theme.GetMenuOptionColor();
            var descriptionColor = _theme.GetMenuDescriptionColor();

            _output.SetForegroundColor(optionColor);
            _output.WriteLine($"    Deck #{number}: {deck.Name}");

            _output.SetForegroundColor(UIColor.White);
            if (!string.IsNullOrEmpty(deck.Description))
            {
                _output.WriteLine($"    {deck.Description}");
            }

            _output.WriteLine($"    Cards: {deck.ActiveCards}/{deck.TotalCards} | Created: {deck.CreatedDate:yyyy-MM-dd}");
            _output.WriteLine($"    Last Modified: {deck.LastModified:yyyy-MM-dd HH:mm}");

            if (deck.Tags.Any())
            {
                _output.WriteLine($"    Tags: {string.Join(", ", deck.Tags)}");
            }

            _output.ResetColor();
            _output.WriteLine("");
        }

        private void RenderDeckStatistics(Deck deck)
        {
            var optionColor = _theme.GetMenuOptionColor();
            var descriptionColor = _theme.GetMenuDescriptionColor();

            _output.SetForegroundColor(optionColor);
            _output.WriteLine($"    {deck.Name}");

            _output.SetForegroundColor(UIColor.White);
            _output.WriteLine($"    Total Cards: {deck.TotalCards} | Active: {deck.ActiveCards}");
            _output.WriteLine($"    Study Sessions: {deck.Statistics.TotalStudySessions}");
            _output.WriteLine($"    Total Study Time: {deck.Statistics.TotalStudyTime:hh\\:mm\\:ss}");
            _output.WriteLine($"    Overall Success Rate: {deck.Statistics.OverallSuccessRate:F1}%");
            _output.WriteLine($"    Study Streak: {deck.Statistics.StudyStreak} days");

            _output.ResetColor();
        }

        private void RenderStatCard(string icon, string label, string value)
        {
            var color = _theme.GetStatisticsColor();
            _output.SetForegroundColor(color);
            _output.WriteLine($"    {icon} {label.PadRight(20)}: {value}");
            _output.ResetColor();
        }

        private void RenderMessage(MessageDefinition message)
        {
            var color = _theme.GetColorForMessageType(message.Type);
            var icon = _theme.GetIconForMessageType(message.Type);

            _output.SetForegroundColor(color);
            _output.WriteLine($"    {icon} {message.Text}");
            _output.ResetColor();
        }
    }
}
