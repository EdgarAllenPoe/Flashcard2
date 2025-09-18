using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// WinUI 3 implementation of IUIRenderer for Windows Native applications
    /// </summary>
    public class WinUIRenderer : IUIRenderer
    {
        private readonly IUIOutput _output;
        private readonly WinUITheme _theme;
        private readonly StackPanel _mainPanel;

        public WinUIRenderer(IUIOutput output, WinUITheme theme, StackPanel mainPanel)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _mainPanel = mainPanel ?? throw new ArgumentNullException(nameof(mainPanel));
        }

        public async Task RenderWelcomeMessageAsync()
        {
            var welcomeText = "🎉 Welcome to Flashcard App - Windows Native Edition! 🎉\n\n";
            welcomeText += "This is the modern Windows implementation using WinUI 3.\n";
            welcomeText += "All your flashcard data and study progress are preserved.\n\n";
            
            _output.SetForegroundColor(_theme.GetColorForSection(SectionType.Welcome));
            _output.WriteLine(welcomeText);
            _output.ResetColor();
            
            await Task.CompletedTask;
        }

        public async Task RenderExitMessageAsync()
        {
            var exitText = "👋 Thank you for using Flashcard App!\n";
            exitText += "Your progress has been saved. See you next time!\n";
            
            _output.SetForegroundColor(_theme.GetColorForMessageType(MessageType.Info));
            _output.WriteLine(exitText);
            _output.ResetColor();
            
            await Task.CompletedTask;
        }

        public async Task RenderSectionHeaderAsync(string title, SectionType sectionType)
        {
            var icon = _theme.GetIconForSection(sectionType);
            var headerText = $"{icon} {title}\n";
            headerText += new string('=', title.Length + icon.Length + 1) + "\n\n";
            
            _output.SetForegroundColor(_theme.GetColorForSection(sectionType));
            _output.WriteLine(headerText);
            _output.ResetColor();
            
            await Task.CompletedTask;
        }

        public async Task RenderMessageAsync(MessageDefinition message)
        {
            var icon = _theme.GetIconForMessageType(message.Type);
            var messageText = $"{icon} {message.Text}\n\n";
            
            _output.SetForegroundColor(_theme.GetColorForMessageType(message.Type));
            _output.WriteLine(messageText);
            _output.ResetColor();
            
            // Message will remain visible until manually cleared
            
            await Task.CompletedTask;
        }

        public async Task RenderMenuAsync(MenuDefinition menu)
        {
            await RenderSectionHeaderAsync(menu.Title, menu.SectionType);
            
            foreach (var option in menu.Options)
            {
                var optionText = $"{option.Key}. {option.Title}";
                if (!string.IsNullOrEmpty(option.Description))
                {
                    optionText += $" - {option.Description}";
                }
                optionText += "\n";
                
                _output.SetForegroundColor(_theme.GetMenuOptionColor());
                _output.Write(optionText);
                _output.ResetColor();
            }
            
            _output.WriteLine("");
            await Task.CompletedTask;
        }

        public async Task RenderDeckListAsync(List<Deck> decks)
        {
            var deckList = decks.ToList();
            
            if (!deckList.Any())
            {
                await RenderMessageAsync(new MessageDefinition 
                { 
                    Type = MessageType.Info, 
                    Text = "No decks available. Create a new deck to get started!" 
                });
                return;
            }

            await RenderSectionHeaderAsync("Your Flashcard Decks", SectionType.DeckList);
            
            for (int i = 0; i < deckList.Count; i++)
            {
                var deck = deckList[i];
                var deckText = $"{i + 1}. {deck.Name}\n";
                deckText += $"   📊 {deck.ActiveCards} active cards\n";
                deckText += $"   📅 Last modified: {deck.LastModified:yyyy-MM-dd}\n";
                
                if (deck.Statistics != null)
                {
                    deckText += $"   🎯 Success rate: {deck.Statistics.OverallSuccessRate:F1}%\n";
                    deckText += $"   ⏱️ Study time: {deck.Statistics.TotalStudyTime:hh\\:mm}\n";
                }
                deckText += "\n";
                
                _output.WriteLine(deckText);
            }
            
            await Task.CompletedTask;
        }

        public async Task RenderDeckCardAsync(Deck deck, int cardNumber)
        {
            var cardText = $"📚 Deck: {deck.Name}\n";
            cardText += $"📄 Card {cardNumber} of {deck.ActiveCards}\n";
            cardText += $"📅 Last reviewed: {deck.LastModified:yyyy-MM-dd}\n\n";
            
            _output.WriteLine(cardText);
            await Task.CompletedTask;
        }

        public async Task RenderFlashcardAsync(Flashcard flashcard, int currentCardNumber, int totalCards, StudyMode studyMode)
        {
            var cardText = $"📚 Card {currentCardNumber} of {totalCards}\n";
            cardText += $"📦 Box: {flashcard.CurrentBox}\n";
            cardText += $"📅 Next review: {flashcard.NextReviewDate:yyyy-MM-dd}\n\n";
            
            cardText += $"❓ Question:\n{flashcard.Front}\n\n";
            
            if (studyMode == StudyMode.FrontToBack)
            {
                cardText += "Press any key to reveal the answer...\n";
            }
            else if (studyMode == StudyMode.BackToFront)
            {
                cardText += $"💡 Answer:\n{flashcard.Back}\n\n";
                cardText += "Press any key to reveal the question...\n";
            }
            else // Mixed mode
            {
                cardText += "Press any key to reveal the answer...\n";
            }
            
            _output.WriteLine(cardText);
            await Task.CompletedTask;
        }

        public async Task RenderInputPromptAsync(string prompt)
        {
            var promptText = $">>> {prompt}: ";
            _output.SetForegroundColor(_theme.GetInputPromptColor());
            _output.Write(promptText);
            _output.ResetColor();
            
            await Task.CompletedTask;
        }

        public async Task RenderStatisticsAsync(StatisticsData statistics)
        {
            await RenderSectionHeaderAsync("Study Statistics", SectionType.Statistics);
            
            var statsText = "📊 Overall Statistics:\n\n";
            
            foreach (var stat in statistics.Statistics)
            {
                statsText += $"   {stat.Key}: {stat.Value}\n";
            }
            
            statsText += "\n📚 Deck Statistics:\n\n";
            
            foreach (var deck in statistics.Decks)
            {
                statsText += $"   📖 {deck.Name}\n";
                if (deck.Statistics != null)
                {
                    statsText += $"      🎯 Success rate: {deck.Statistics.OverallSuccessRate:F1}%\n";
                    statsText += $"      ⏱️ Study time: {deck.Statistics.TotalStudyTime:hh\\:mm}\n";
                    statsText += $"      📅 Sessions: {deck.Statistics.TotalStudySessions}\n";
                }
                statsText += "\n";
            }
            
            _output.SetForegroundColor(_theme.GetStatisticsColor());
            _output.WriteLine(statsText);
            _output.ResetColor();
            
            await Task.CompletedTask;
        }

        public async Task RenderSessionResultsAsync(SessionResult results)
        {
            await RenderSectionHeaderAsync("Study Session Results", SectionType.SessionResults);
            
            var resultsText = "";
            
            if (results.Success)
            {
                resultsText += "🎉 Session completed successfully!\n\n";
                
                if (results.SessionStatistics != null)
                {
                    var stats = results.SessionStatistics;
                    resultsText += $"📊 Session Statistics:\n";
                    resultsText += $"   ✅ Correct answers: {stats.CorrectAnswers}\n";
                    resultsText += $"   ❌ Incorrect answers: {stats.IncorrectAnswers}\n";
                    resultsText += $"   📚 Total cards: {stats.TotalCards}\n";
                    resultsText += $"   ⏱️ Study time: {stats.TotalStudyTime:hh\\:mm\\:ss}\n";
                    resultsText += $"   🎯 Success rate: {stats.SuccessRate:F1}%\n\n";
                }
            }
            else
            {
                resultsText += $"❌ Session ended with an error:\n{results.Message}\n\n";
            }
            
            _output.SetForegroundColor(results.Success ? 
                _theme.GetColorForMessageType(MessageType.Success) : 
                _theme.GetColorForMessageType(MessageType.Error));
            _output.WriteLine(resultsText);
            _output.ResetColor();
            
            await Task.CompletedTask;
        }

        public async Task RenderPressAnyKeyAsync()
        {
            var pressKeyText = "Press any key to continue...\n";
            _output.SetForegroundColor(_theme.GetInputPromptColor());
            _output.Write(pressKeyText);
            _output.ResetColor();
            
            await Task.CompletedTask;
        }
    }
}
