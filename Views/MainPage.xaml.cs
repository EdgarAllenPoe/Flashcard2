using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;

namespace FlashcardApp.WinUI.Views
{
    /// <summary>
    /// A modern WinUI page with simplified layout for testing.
    /// </summary>
    public partial class MainPage : Page
    {
        private bool _isDarkTheme = false;
        private readonly Dictionary<string, string> _contentCache = new();

        public MainPage()
        {
            this.InitializeComponent();
            InitializeContentCache();
            InitializeLayout();
            UpdateThemeButton();
        }

        private void InitializeContentCache()
        {
            _contentCache["welcome"] = "Welcome to FlashcardApp! 🧠\n\nThis is a modern WinUI application with:\n• Header Bar with app title\n• Navigation sidebar with buttons\n• Dynamic Content Area\n• Status Bar\n\nClick a navigation button to get started.";
            _contentCache["study"] = "📚 Study Sessions\n\nStart a new study session or continue where you left off.\n\nFeatures:\n• Multiple study modes\n• Progress tracking\n• Performance analytics";
            _contentCache["decks"] = "🗂️ Deck Management\n\nCreate, edit, and organize your flashcard decks.\n\nFeatures:\n• Create new decks\n• Import/Export decks\n• Organize cards";
            _contentCache["stats"] = "📊 Statistics\n\nView your learning progress and performance metrics.\n\nFeatures:\n• Study time tracking\n• Accuracy statistics\n• Progress charts";
            _contentCache["config"] = "⚙️ Configuration\n\nCustomize your FlashcardApp experience.\n\nFeatures:\n• Theme settings\n• Study preferences\n• File paths";
            _contentCache["help"] = "❓ Help\n\nGet help and learn how to use FlashcardApp.\n\nFeatures:\n• User guide\n• Keyboard shortcuts\n• Troubleshooting";
        }

        private void InitializeLayout()
        {
            // Initialize the layout using cached content
            OutputTextBlock.Text = _contentCache["welcome"];
            StatusText.Text = "Ready";
        }

        private void StudySessions_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to study session page
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(StudySessionPage));
            }
            else
            {
                // Fallback to content display if navigation is not available
                OutputTextBlock.Text = _contentCache["study"];
                StatusText.Text = "Study Sessions selected";
            }
        }

        private void DeckManagement_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to deck management page
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(DeckManagementPage));
            }
            else
            {
                // Fallback to content display if navigation is not available
                OutputTextBlock.Text = _contentCache["decks"];
                StatusText.Text = "Deck Management selected";
            }
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            // Display statistics content (StatisticsPage removed for stability)
            OutputTextBlock.Text = _contentCache["stats"];
            StatusText.Text = "Statistics selected";
        }

        private void Configuration_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to configuration page
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ConfigurationPage));
            }
            else
            {
                // Fallback to content display if navigation is not available
                OutputTextBlock.Text = _contentCache["config"];
                StatusText.Text = "Configuration selected";
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            // Display help content (HelpPage removed for stability)
            OutputTextBlock.Text = _contentCache["help"];
            StatusText.Text = "Help selected";
        }

        private void ThemeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle between light and dark theme
            _isDarkTheme = !_isDarkTheme;

            // Apply theme to the root element efficiently
            if (this.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = _isDarkTheme ? ElementTheme.Dark : ElementTheme.Light;
            }

            UpdateThemeButton();
            StatusText.Text = _isDarkTheme ? "Dark theme applied" : "Light theme applied";
        }

        private void UpdateThemeButton()
        {
            if (ThemeToggleButton != null)
            {
                // Cache the tooltip strings to avoid repeated string allocations
                var (content, tooltip) = _isDarkTheme ? ("☀️", "Switch to Light Theme") : ("🌙", "Switch to Dark Theme");
                ThemeToggleButton.Content = content;
                ToolTipService.SetToolTip(ThemeToggleButton, tooltip);
            }
        }
    }
}
