using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Input;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Windows.System;

namespace FlashcardApp.WinUI.Views
{
    /// <summary>
    /// Interactive study session page with flashcard display and scoring.
    /// Supports keyboard shortcuts:
    /// Space - Reveal Answer
    /// 1 - Correct
    /// 2 - Incorrect
    /// Escape - Back
    /// </summary>
    public partial class StudySessionPage : Page
    {
        // Keyboard shortcut constants
        private const VirtualKey REVEAL_ANSWER_KEY = VirtualKey.Space;
        private const VirtualKey CORRECT_KEY = VirtualKey.Number1;
        private const VirtualKey INCORRECT_KEY = VirtualKey.Number2;
        private const VirtualKey BACK_KEY = VirtualKey.Escape;

        private int _currentCardIndex = 0;
        private int _correctCount = 0;
        private int _incorrectCount = 0;
        private bool _answerRevealed = false;
        private readonly List<StudyCard> _studyCards = new();
        private FlashcardDeck? _studyDeck = null;

        public StudySessionPage()
        {
            this.InitializeComponent();
            this.Focus(FocusState.Programmatic); // Ensure keyboard focus for shortcuts
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // Get the deck data passed from navigation
            if (e.Parameter is FlashcardDeck deck)
            {
                _studyDeck = deck;
                InitializeStudySession();
            }
            else
            {
                // Fallback to sample data if no deck provided
                InitializeStudySession();
            }
        }

        private void InitializeStudySession()
        {
            _studyCards.Clear();
            
            if (_studyDeck != null && _studyDeck.Cards.Count > 0)
            {
                // Use the selected deck's cards
                foreach (var card in _studyDeck.Cards)
                {
                    _studyCards.Add(new StudyCard { Front = card.Front, Back = card.Back });
                }
            }
            else
            {
                // Fallback to sample study cards
                _studyCards.AddRange(new[]
                {
                    new StudyCard { Front = "What is the capital of France?", Back = "Paris" },
                    new StudyCard { Front = "What is 2 + 2?", Back = "4" },
                    new StudyCard { Front = "What is the largest planet in our solar system?", Back = "Jupiter" },
                    new StudyCard { Front = "What is the chemical symbol for gold?", Back = "Au" },
                    new StudyCard { Front = "What year did World War II end?", Back = "1945" }
                });
            }

            DisplayCurrentCard();
            UpdateProgress();
            UpdateStats();
        }

        private void DisplayCurrentCard()
        {
            if (_currentCardIndex < _studyCards.Count)
            {
                var currentCard = _studyCards[_currentCardIndex];
                CardContent.Text = currentCard.Front;
                AnswerContent.Text = currentCard.Back;
                CardTypeIndicator.Text = "Front";
                
                // Reset answer visibility
                AnswerBorder.Visibility = Visibility.Collapsed;
                _answerRevealed = false;
                
                // Reset button visibility
                RevealAnswerButton.Visibility = Visibility.Visible;
                CorrectButton.Visibility = Visibility.Collapsed;
                IncorrectButton.Visibility = Visibility.Collapsed;
                
                SessionStatusText.Text = "Study the question above";
            }
        }

        private void UpdateProgress()
        {
            if (_studyCards.Count > 0)
            {
                var progress = (double)(_currentCardIndex + 1) / _studyCards.Count * 100;
                SessionProgressBar.Value = progress;
                ProgressText.Text = $"Card {_currentCardIndex + 1} of {_studyCards.Count}";
            }
        }

        private void UpdateStats()
        {
            SessionStatsText.Text = $"Correct: {_correctCount} | Incorrect: {_incorrectCount}";
        }

        private void RevealAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_answerRevealed)
            {
                AnswerBorder.Visibility = Visibility.Visible;
                CardTypeIndicator.Text = "Answer";
                _answerRevealed = true;
                
                // Show scoring buttons
                RevealAnswerButton.Visibility = Visibility.Collapsed;
                CorrectButton.Visibility = Visibility.Visible;
                IncorrectButton.Visibility = Visibility.Visible;
                
                SessionStatusText.Text = "How did you do?";
            }
        }

        private void CorrectButton_Click(object sender, RoutedEventArgs e)
        {
            _correctCount++;
            ProcessAnswer(true);
        }

        private void IncorrectButton_Click(object sender, RoutedEventArgs e)
        {
            _incorrectCount++;
            ProcessAnswer(false);
        }

        private void ProcessAnswer(bool isCorrect)
        {
            // Update stats
            UpdateStats();
            
            // Move to next card or end session
            _currentCardIndex++;
            
            if (_currentCardIndex < _studyCards.Count)
            {
                DisplayCurrentCard();
                UpdateProgress();
                SessionStatusText.Text = isCorrect ? "Great job! Next card..." : "Keep trying! Next card...";
            }
            else
            {
                // End of session
                EndStudySession();
            }
        }

        private void EndStudySession()
        {
            var successRate = _studyCards.Count > 0 ? (double)_correctCount / _studyCards.Count * 100 : 0;
            
            CardContent.Text = $"🎉 Study Session Complete!\n\n" +
                              $"Cards Studied: {_studyCards.Count}\n" +
                              $"Correct: {_correctCount}\n" +
                              $"Incorrect: {_incorrectCount}\n" +
                              $"Success Rate: {successRate:F1}%";
            
            AnswerBorder.Visibility = Visibility.Collapsed;
            RevealAnswerButton.Visibility = Visibility.Collapsed;
            CorrectButton.Visibility = Visibility.Collapsed;
            IncorrectButton.Visibility = Visibility.Collapsed;
            
            SessionProgressBar.Value = 100;
            ProgressText.Text = "Complete";
            SessionStatusText.Text = "Study session finished";
        }

        /// <summary>
        /// Handles keyboard shortcuts with higher priority to prevent default navigation.
        /// Space - Reveal Answer, 1 - Correct, 2 - Incorrect, Escape - Back
        /// </summary>
        private void OnPreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Debug.WriteLine($"Study Session preview key pressed: {e.Key}");

            // Handle space bar with highest priority to prevent navigation
            if (e.Key == VirtualKey.Space)
            {
                if (RevealAnswerButton.Visibility == Visibility.Visible && !_answerRevealed)
                {
                    Debug.WriteLine("Reveal Answer shortcut triggered (Preview)");
                    RevealAnswerButton_Click(sender, new RoutedEventArgs());
                }
                // Always mark space bar as handled to prevent default navigation
                e.Handled = true;
                return;
            }

            // Handle other shortcuts
            switch (e.Key)
            {
                case VirtualKey.Number1:
                    if (CorrectButton.Visibility == Visibility.Visible && _answerRevealed)
                    {
                        Debug.WriteLine("Correct shortcut triggered (Preview)");
                        CorrectButton_Click(sender, new RoutedEventArgs());
                        e.Handled = true;
                    }
                    break;

                case VirtualKey.Number2:
                    if (IncorrectButton.Visibility == Visibility.Visible && _answerRevealed)
                    {
                        Debug.WriteLine("Incorrect shortcut triggered (Preview)");
                        IncorrectButton_Click(sender, new RoutedEventArgs());
                        e.Handled = true;
                    }
                    break;

                case VirtualKey.Escape:
                    Debug.WriteLine("Back shortcut triggered (Preview)");
                    BackButton_Click(sender, new RoutedEventArgs());
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Handles keyboard shortcuts for study session actions.
        /// Space - Reveal Answer, 1 - Correct, 2 - Incorrect, Escape - Back
        /// </summary>
        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Simple key handling without modifier checks for now

            Debug.WriteLine($"Study Session keyboard shortcut pressed: {e.Key}");

            switch (e.Key)
            {
                case VirtualKey.Space:
                    // Always handle space bar to prevent default navigation behavior
                    if (RevealAnswerButton.Visibility == Visibility.Visible && !_answerRevealed)
                    {
                        Debug.WriteLine("Reveal Answer shortcut triggered");
                        RevealAnswerButton_Click(sender, new RoutedEventArgs());
                    }
                    // Always mark space bar as handled to prevent navigation
                    e.Handled = true;
                    break;

                case VirtualKey.Number1:
                    if (CorrectButton.Visibility == Visibility.Visible && _answerRevealed)
                    {
                        Debug.WriteLine("Correct shortcut triggered");
                        CorrectButton_Click(sender, new RoutedEventArgs());
                        e.Handled = true;
                    }
                    break;

                case VirtualKey.Number2:
                    if (IncorrectButton.Visibility == Visibility.Visible && _answerRevealed)
                    {
                        Debug.WriteLine("Incorrect shortcut triggered");
                        IncorrectButton_Click(sender, new RoutedEventArgs());
                        e.Handled = true;
                    }
                    break;

                case VirtualKey.Escape:
                    Debug.WriteLine("Back shortcut triggered");
                    BackButton_Click(sender, new RoutedEventArgs());
                    e.Handled = true;
                    break;

                default:
                    // Unhandled key - do nothing
                    break;
            }

            // Set automation properties for accessibility
            if (e.Handled)
            {
                Microsoft.UI.Xaml.Automation.AutomationProperties.SetLiveSetting(this, Microsoft.UI.Xaml.Automation.Peers.AutomationLiveSetting.Assertive);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to main page
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private class StudyCard
        {
            public string Front { get; set; } = string.Empty;
            public string Back { get; set; } = string.Empty;
        }
    }
}

