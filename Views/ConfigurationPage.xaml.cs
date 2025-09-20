using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using FlashcardApp.Models;
using FlashcardApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace FlashcardApp.WinUI.Views
{
    /// <summary>
    /// Configuration page for managing application settings.
    /// Provides UI for editing configuration values with validation and import/export functionality.
    /// </summary>
    public partial class ConfigurationPage : Page, INotifyPropertyChanged
    {
        private readonly ConfigurationService _configurationService;
        private AppConfiguration _currentConfiguration;

        public ConfigurationPage()
        {
            this.InitializeComponent();
            this.DataContext = this; // Set DataContext for data binding
            _configurationService = new ConfigurationService();
            LoadConfiguration();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Loads configuration from the service and populates the UI
        /// </summary>
        private void LoadConfiguration()
        {
            try
            {
                _currentConfiguration = _configurationService.GetConfiguration();
                PopulateUI();
                UpdateStatus("Configuration loaded successfully");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// Populates the UI controls with current configuration values
        /// </summary>
        private void PopulateUI()
        {
            // Study Session Configuration
            StudyModeComboBox.SelectedIndex = (int)_currentConfiguration.StudySession.DefaultStudyMode;
            ShowStatisticsCheckBox.IsChecked = _currentConfiguration.StudySession.ShowStatistics;
            AutoAdvanceCheckBox.IsChecked = _currentConfiguration.StudySession.AutoAdvance;
            AutoAdvanceDelaySlider.Value = _currentConfiguration.StudySession.AutoAdvanceDelay;

            // Leitner Box Configuration
            NumberOfBoxesSlider.Value = _currentConfiguration.LeitnerBoxes.NumberOfBoxes;

            // File Paths Configuration
            DecksDirectoryTextBox.Text = _currentConfiguration.FilePaths.DecksDirectory;
            BackupDirectoryTextBox.Text = _currentConfiguration.FilePaths.BackupDirectory;
            ExportDirectoryTextBox.Text = _currentConfiguration.FilePaths.ExportDirectory;

            // Daily Limits Configuration
            MaxCardsPerDaySlider.Value = _currentConfiguration.DailyLimits.MaxCardsPerDay;
            MinCardsPerDaySlider.Value = _currentConfiguration.DailyLimits.MinCardsPerDay;

            // UI Settings Configuration
            UseColorsCheckBox.IsChecked = _currentConfiguration.UI.UseColors;
            UseIconsCheckBox.IsChecked = _currentConfiguration.UI.UseIcons;
            ShowWelcomeMessageCheckBox.IsChecked = _currentConfiguration.UI.ShowWelcomeMessage;
        }

        /// <summary>
        /// Updates the configuration from UI controls
        /// </summary>
        private void UpdateConfigurationFromUI()
        {
            // Study Session Configuration
            _currentConfiguration.StudySession.DefaultStudyMode = (StudyMode)StudyModeComboBox.SelectedIndex;
            _currentConfiguration.StudySession.ShowStatistics = ShowStatisticsCheckBox.IsChecked ?? false;
            _currentConfiguration.StudySession.AutoAdvance = AutoAdvanceCheckBox.IsChecked ?? false;
            _currentConfiguration.StudySession.AutoAdvanceDelay = (int)AutoAdvanceDelaySlider.Value;

            // Leitner Box Configuration
            _currentConfiguration.LeitnerBoxes.NumberOfBoxes = (int)NumberOfBoxesSlider.Value;

            // File Paths Configuration
            _currentConfiguration.FilePaths.DecksDirectory = DecksDirectoryTextBox.Text.Trim();
            _currentConfiguration.FilePaths.BackupDirectory = BackupDirectoryTextBox.Text.Trim();
            _currentConfiguration.FilePaths.ExportDirectory = ExportDirectoryTextBox.Text.Trim();

            // Daily Limits Configuration
            _currentConfiguration.DailyLimits.MaxCardsPerDay = (int)MaxCardsPerDaySlider.Value;
            _currentConfiguration.DailyLimits.MinCardsPerDay = (int)MinCardsPerDaySlider.Value;

            // UI Settings Configuration
            _currentConfiguration.UI.UseColors = UseColorsCheckBox.IsChecked ?? true;
            _currentConfiguration.UI.UseIcons = UseIconsCheckBox.IsChecked ?? true;
            _currentConfiguration.UI.ShowWelcomeMessage = ShowWelcomeMessageCheckBox.IsChecked ?? true;
        }

        /// <summary>
        /// Validates the current configuration
        /// </summary>
        private bool ValidateConfiguration()
        {
            UpdateConfigurationFromUI();
            return _configurationService.ValidateConfiguration(_currentConfiguration);
        }

        /// <summary>
        /// Shows validation errors to the user
        /// </summary>
        private void ShowValidationError(string message)
        {
            UpdateStatus($"Validation Error: {message}");
            // Could show a dialog here for better UX
        }

        /// <summary>
        /// Validates if a path is valid
        /// </summary>
        private bool IsValidPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            try
            {
                Path.GetFullPath(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates if a number is within valid range
        /// </summary>
        private bool IsValidNumber(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Updates the status text
        /// </summary>
        private void UpdateStatus(string message)
        {
            StatusText.Text = message;
        }

        /// <summary>
        /// Saves the current configuration
        /// </summary>
        private void SaveConfiguration()
        {
            try
            {
                if (ValidateConfiguration())
                {
                    _configurationService.UpdateConfiguration(config =>
                    {
                        // Copy the updated configuration
                        config.LeitnerBoxes = _currentConfiguration.LeitnerBoxes;
                        config.StudySession = _currentConfiguration.StudySession;
                        config.FilePaths = _currentConfiguration.FilePaths;
                        config.DailyLimits = _currentConfiguration.DailyLimits;
                        config.UI = _currentConfiguration.UI;
                    });
                    UpdateStatus("Configuration saved successfully");
                }
                else
                {
                    var errors = _configurationService.GetValidationErrors(_currentConfiguration);
                    ShowValidationError(string.Join(", ", errors));
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error saving configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// Resets configuration to defaults
        /// </summary>
        private void ResetToDefaults()
        {
            try
            {
                _currentConfiguration = _configurationService.CreateDefaultConfiguration();
                PopulateUI();
                UpdateStatus("Configuration reset to defaults");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error resetting configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// Imports configuration from a file
        /// </summary>
        private async void ImportConfiguration()
        {
            try
            {
                var fileOpenPicker = new FileOpenPicker();
                fileOpenPicker.FileTypeFilter.Add(".json");
                fileOpenPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

                // Initialize with window handle
                var window = App.MainWindow;
                if (window != null)
                {
                    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WinRT.Interop.InitializeWithWindow.Initialize(fileOpenPicker, hwnd);
                }

                var file = await fileOpenPicker.PickSingleFileAsync();
                if (file != null)
                {
                    var importedConfig = _configurationService.ImportConfiguration(file.Path);
                    _currentConfiguration = importedConfig;
                    PopulateUI();
                    UpdateStatus($"Configuration imported from {file.Name}");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error importing configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// Exports configuration to a file
        /// </summary>
        private async void ExportConfiguration()
        {
            try
            {
                var fileSavePicker = new FileSavePicker();
                fileSavePicker.FileTypeChoices.Add("JSON Configuration", new List<string> { ".json" });
                fileSavePicker.SuggestedFileName = "flashcard-config";
                fileSavePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

                // Initialize with window handle
                var window = App.MainWindow;
                if (window != null)
                {
                    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WinRT.Interop.InitializeWithWindow.Initialize(fileSavePicker, hwnd);
                }

                var file = await fileSavePicker.PickSaveFileAsync();
                if (file != null)
                {
                    UpdateConfigurationFromUI();
                    _configurationService.ExportConfiguration(file.Path, _currentConfiguration);
                    UpdateStatus($"Configuration exported to {file.Name}");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error exporting configuration: {ex.Message}");
            }
        }

        // Event Handlers

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveConfiguration();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetToDefaults();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            ImportConfiguration();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            ExportConfiguration();
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