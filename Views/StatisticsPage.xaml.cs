using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.WinUI.Views
{
    /// <summary>
    /// Statistics and progress tracking page for the flashcard application.
    /// Displays study statistics, progress charts, deck performance, and study goals.
    /// </summary>
    public partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            this.InitializeComponent();
            InitializeStatistics();
        }

        private void InitializeStatistics()
        {
            // Initialize with sample statistics data
            UpdateOverviewCards();
            UpdateDeckPerformance();
            UpdateStudyGoals();
            UpdateStatus("Statistics loaded successfully");
        }

        private void UpdateOverviewCards()
        {
            // Sample data - in a real app, this would come from a statistics service
            TotalStudyTimeText.Text = "2h 30m";
            CardsStudiedText.Text = "156";
            AccuracyText.Text = "87%";
            StreakText.Text = "7 days";
        }

        private void UpdateDeckPerformance()
        {
            // Sample deck performance data
            // In a real app, this would be populated from actual study data
            var deckPerformances = new[]
            {
                new { Name = "Spanish Vocabulary", CardsStudied = 45, Accuracy = 89, Rating = "⭐⭐⭐⭐⭐" },
                new { Name = "Math Formulas", CardsStudied = 23, Accuracy = 78, Rating = "⭐⭐⭐⭐" },
                new { Name = "History Facts", CardsStudied = 67, Accuracy = 92, Rating = "⭐⭐⭐⭐⭐" }
            };

            DeckPerformancePanel.Children.Clear();

            foreach (var deck in deckPerformances)
            {
                var deckBorder = new Border
                {
                    Background = new SolidColorBrush(Microsoft.UI.Colors.LightGray),
                    Padding = new Thickness(15),
                    CornerRadius = new CornerRadius(6),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var stackPanel = new StackPanel();

                var nameText = new TextBlock
                {
                    Text = deck.Name,
                    FontSize = 14,
                    FontWeight = Microsoft.UI.Text.FontWeights.SemiBold
                };
                stackPanel.Children.Add(nameText);

                var statsText = new TextBlock
                {
                    Text = $"{deck.CardsStudied} cards studied • {deck.Accuracy}% accuracy",
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gray)
                };
                stackPanel.Children.Add(statsText);

                Grid.SetColumn(stackPanel, 0);
                grid.Children.Add(stackPanel);

                var ratingText = new TextBlock
                {
                    Text = deck.Rating,
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(ratingText, 1);
                grid.Children.Add(ratingText);

                deckBorder.Child = grid;
                DeckPerformancePanel.Children.Add(deckBorder);
            }
        }

        private void UpdateStudyGoals()
        {
            // Sample study goals data
            DailyGoalProgress.Value = 75; // 15/20 cards
            WeeklyGoalProgress.Value = 68; // 68/100 cards
        }

        private void UpdateStatus(string message)
        {
            StatusText.Text = message;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to main page
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
