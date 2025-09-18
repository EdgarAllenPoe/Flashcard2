using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.Services;
using FlashcardApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace FlashcardApp.WinUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            
            // Setup event handlers
            SetupEventHandlers();
            
            // Start the application
            _ = Task.Run(async () => await StartApplicationAsync());
        }

        private void SetupEventHandlers()
        {
            // Handle window closing
            this.Closed += MainWindow_Closed;
            
            // Handle input text box events
            InputTextBox.KeyDown += InputTextBox_KeyDown;
        }

        private async void InputTextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                await ProcessUserInputAsync();
            }
        }

        private async Task ProcessUserInputAsync()
        {
            var input = InputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(input))
                return;

            // Clear input
            InputTextBox.Text = string.Empty;
            
            // Add the input to the output
            OutputTextBlock.Text += $"\n> {input}";
            
            // Process the input
            await ProcessCommandAsync(input);
        }

        private async Task ProcessCommandAsync(string input)
        {
            switch (input.ToUpper())
            {
                case "1":
                    await ShowStudySessionInfo();
                    break;
                case "2":
                    await ShowDeckManagementInfo();
                    break;
                case "3":
                    await ShowStatisticsInfo();
                    break;
                case "4":
                    await ShowConfigurationInfo();
                    break;
                case "5":
                    await ShowHelpInfo();
                    break;
                case "ESC":
                case "EXIT":
                    await ShowExitMessage();
                    break;
                default:
                    OutputTextBlock.Text += $"\nInvalid choice: {input}. Please try again.";
                    break;
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await ProcessUserInputAsync();
        }

        private async Task StartApplicationAsync()
        {
            try
            {
                // Show welcome message
                OutputTextBlock.Text = "🧠 Welcome to Flashcard App - Windows Native Version!\n\n";
                OutputTextBlock.Text += "This application helps you study using the Leitner Box system with spaced repetition.\n\n";
                
                // Show main menu
                await ShowMainMenuAsync();
            }
            catch (Exception ex)
            {
                OutputTextBlock.Text += $"\nError starting application: {ex.Message}";
            }
        }

        private async Task ShowMainMenuAsync()
        {
            OutputTextBlock.Text += "MAIN MENU\n";
            OutputTextBlock.Text += "==========\n\n";
            OutputTextBlock.Text += "1. Start Study Session - Begin your learning journey\n";
            OutputTextBlock.Text += "2. Manage Decks - Create, edit, and organize decks\n";
            OutputTextBlock.Text += "3. View Statistics - Track your progress\n";
            OutputTextBlock.Text += "4. Configuration - Customize app settings\n";
            OutputTextBlock.Text += "5. Help & Guide - Show help information\n";
            OutputTextBlock.Text += "ESC. Exit - Close the application\n\n";
            OutputTextBlock.Text += "Enter your choice: ";
        }

        private async Task ShowStudySessionInfo()
        {
            OutputTextBlock.Text += "\n📚 Study Session Feature\n";
            OutputTextBlock.Text += "=======================\n";
            OutputTextBlock.Text += "This feature will allow you to:\n";
            OutputTextBlock.Text += "- Select a deck to study\n";
            OutputTextBlock.Text += "- Choose study mode (Front to Back, Back to Front, Random, Review)\n";
            OutputTextBlock.Text += "- Set maximum number of cards\n";
            OutputTextBlock.Text += "- Track your progress with the Leitner Box system\n\n";
            OutputTextBlock.Text += "Feature implementation in progress...\n\n";
        }

        private async Task ShowDeckManagementInfo()
        {
            OutputTextBlock.Text += "\n🗂️ Deck Management Feature\n";
            OutputTextBlock.Text += "==========================\n";
            OutputTextBlock.Text += "This feature will allow you to:\n";
            OutputTextBlock.Text += "- Create new decks\n";
            OutputTextBlock.Text += "- Edit existing decks\n";
            OutputTextBlock.Text += "- Delete decks\n";
            OutputTextBlock.Text += "- Import/Export decks (JSON, CSV, XLSX)\n";
            OutputTextBlock.Text += "- View all your decks\n\n";
            OutputTextBlock.Text += "Feature implementation in progress...\n\n";
        }

        private async Task ShowStatisticsInfo()
        {
            OutputTextBlock.Text += "\n📊 Statistics Feature\n";
            OutputTextBlock.Text += "=====================\n";
            OutputTextBlock.Text += "This feature will show you:\n";
            OutputTextBlock.Text += "- Overall study statistics\n";
            OutputTextBlock.Text += "- Per-deck statistics\n";
            OutputTextBlock.Text += "- Progress tracking\n";
            OutputTextBlock.Text += "- Success rates\n";
            OutputTextBlock.Text += "- Study time analytics\n\n";
            OutputTextBlock.Text += "Feature implementation in progress...\n\n";
        }

        private async Task ShowConfigurationInfo()
        {
            OutputTextBlock.Text += "\n⚙️ Configuration Feature\n";
            OutputTextBlock.Text += "========================\n";
            OutputTextBlock.Text += "This feature will allow you to:\n";
            OutputTextBlock.Text += "- Customize UI settings\n";
            OutputTextBlock.Text += "- Set default study preferences\n";
            OutputTextBlock.Text += "- Configure file paths\n";
            OutputTextBlock.Text += "- Manage application settings\n\n";
            OutputTextBlock.Text += "Feature implementation in progress...\n\n";
        }

        private async Task ShowHelpInfo()
        {
            OutputTextBlock.Text += "\n❓ Help & Guide\n";
            OutputTextBlock.Text += "===============\n";
            OutputTextBlock.Text += "Flashcard App Help & Guide\n\n";
            OutputTextBlock.Text += "This application helps you study using the Leitner Box system with spaced repetition.\n\n";
            OutputTextBlock.Text += "Main Features:\n";
            OutputTextBlock.Text += "- Study Sessions: Practice with your flashcards\n";
            OutputTextBlock.Text += "- Deck Management: Create, edit, and organize your decks\n";
            OutputTextBlock.Text += "- Statistics: Track your learning progress\n";
            OutputTextBlock.Text += "- Configuration: Customize app settings\n\n";
            OutputTextBlock.Text += "Study Modes:\n";
            OutputTextBlock.Text += "- Front to Back: Study cards from front to back\n";
            OutputTextBlock.Text += "- Back to Front: Study cards from back to front\n";
            OutputTextBlock.Text += "- Random: Study cards in random order\n";
            OutputTextBlock.Text += "- Review: Review all cards\n\n";
            OutputTextBlock.Text += "Navigation:\n";
            OutputTextBlock.Text += "- Use the number keys to select menu options\n";
            OutputTextBlock.Text += "- Press ESC to go back or exit\n";
            OutputTextBlock.Text += "- Use the input field to enter text\n\n";
        }

        private async Task ShowExitMessage()
        {
            OutputTextBlock.Text += "\n🚪 Thank you for using Flashcard App!\n";
            OutputTextBlock.Text += "Good luck with your studies!\n\n";
        }

        private async void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            try
            {
                // Cleanup if needed
            }
            catch
            {
                // Ignore errors during shutdown
            }
        }
    }
}
