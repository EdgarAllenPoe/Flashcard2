using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FlashcardApp.Services;
using FlashcardApp.Models;
using Windows.Storage.Pickers;
using Windows.Storage;
using Newtonsoft.Json;
using WinRT.Interop;

namespace FlashcardApp.WinUI.Views
{
    /// <summary>
    /// Minimal Configuration page for testing
    /// </summary>
    public partial class ConfigurationPage : Page, INotifyPropertyChanged
    {
        private ConfigurationService _configurationService;
        private AppConfiguration _currentConfiguration;
        private System.Threading.Timer _saveTimer;

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        // Backing fields for data binding
        private int _numberOfBoxes;
        private string _reviewIntervals;
        private int _maxCardsPerDay;
        private int _maxStudyTimeMinutes;
        private int _breakIntervalMinutes;
        private bool _autoAdvance;
        private int _autoAdvanceDelay;
        private bool _showStatistics;
        private bool _showProgress;
        private bool _shuffleCards;
        private string _deckStoragePath;
        private string _backupDirectory;
        private string _exportPath;
        private string _importPath;
        private string _logFilePath;
        private string _studyMode;

        public ConfigurationPage()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Constructor started");
                this.InitializeComponent();
                this.DataContext = this; // Set DataContext for data binding
                _configurationService = new ConfigurationService();
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Constructor completed successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error in constructor: {ex.Message}");
                throw;
            }
        }

        // INotifyPropertyChanged implementation
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Bound properties for data binding
        public int NumberOfBoxes
        {
            get => _numberOfBoxes;
            set
            {
                if (_numberOfBoxes != value)
                {
                    _numberOfBoxes = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ReviewIntervals
        {
            get => _reviewIntervals;
            set
            {
                if (_reviewIntervals != value)
                {
                    _reviewIntervals = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxCardsPerDay
        {
            get => _maxCardsPerDay;
            set
            {
                if (_maxCardsPerDay != value)
                {
                    _maxCardsPerDay = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxStudyTimeMinutes
        {
            get => _maxStudyTimeMinutes;
            set
            {
                if (_maxStudyTimeMinutes != value)
                {
                    _maxStudyTimeMinutes = value;
                    OnPropertyChanged();
                }
            }
        }

        public int BreakIntervalMinutes
        {
            get => _breakIntervalMinutes;
            set
            {
                if (_breakIntervalMinutes != value)
                {
                    _breakIntervalMinutes = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool AutoAdvance
        {
            get => _autoAdvance;
            set
            {
                if (_autoAdvance != value)
                {
                    _autoAdvance = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AutoAdvanceDelay
        {
            get => _autoAdvanceDelay;
            set
            {
                if (_autoAdvanceDelay != value)
                {
                    _autoAdvanceDelay = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowStatistics
        {
            get => _showStatistics;
            set
            {
                if (_showStatistics != value)
                {
                    _showStatistics = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowProgress
        {
            get => _showProgress;
            set
            {
                if (_showProgress != value)
                {
                    _showProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShuffleCards
        {
            get => _shuffleCards;
            set
            {
                if (_shuffleCards != value)
                {
                    _shuffleCards = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DeckStoragePath
        {
            get => _deckStoragePath;
            set
            {
                if (_deckStoragePath != value)
                {
                    _deckStoragePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BackupDirectory
        {
            get => _backupDirectory;
            set
            {
                if (_backupDirectory != value)
                {
                    _backupDirectory = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ExportPath
        {
            get => _exportPath;
            set
            {
                if (_exportPath != value)
                {
                    _exportPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ImportPath
        {
            get => _importPath;
            set
            {
                if (_importPath != value)
                {
                    _importPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LogFilePath
        {
            get => _logFilePath;
            set
            {
                if (_logFilePath != value)
                {
                    _logFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StudyMode
        {
            get => _studyMode;
            set
            {
                if (_studyMode != value)
                {
                    _studyMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: BackButton_Click");
            // Navigate back to main page
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: SaveButton_Click");
            if (ValidateConfiguration())
        {
            try
            {
                    _configurationService.SaveConfiguration();
                    UpdateStatus("Configuration saved successfully");
            }
            catch (Exception ex)
            {
                    System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error saving configuration: {ex.Message}");
                    UpdateStatus("Error saving configuration");
                }
            }
            else
            {
                UpdateStatus("Configuration validation failed");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ResetButton_Click");
            ResetToDefaults();
            UpdateStatus("Configuration reset to defaults");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: Page_Loaded");
            LoadConfiguration();
            LoadConfigurationIntoProperties();
            SetupEventHandlers();
        }

        private void LoadConfigurationIntoProperties()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Loading configuration into properties");
                
                // Load Leitner Box properties
                NumberOfBoxes = _currentConfiguration.LeitnerBoxes.NumberOfBoxes;
                var intervals = _currentConfiguration.ReviewScheduling.BoxIntervals
                    .OrderBy(bi => bi.BoxNumber)
                    .Select(bi => bi.IntervalDays.ToString());
                ReviewIntervals = string.Join(", ", intervals);

                // Load Daily Limits properties
                MaxCardsPerDay = _currentConfiguration.DailyLimits.MaxCardsPerDay;
                MaxStudyTimeMinutes = (int)_currentConfiguration.DailyLimits.MaxStudyTimePerDay.TotalMinutes;
                BreakIntervalMinutes = 5; // Default value

                // Load Advanced Study Session properties
                AutoAdvance = _currentConfiguration.StudySession.AutoAdvance;
                AutoAdvanceDelay = _currentConfiguration.StudySession.AutoAdvanceDelay;
                ShowStatistics = _currentConfiguration.StudySession.ShowStatistics;
                ShowProgress = _currentConfiguration.StudySession.ShowProgress;
                ShuffleCards = _currentConfiguration.StudySession.ShuffleCards;

                // Load File Paths properties
                DeckStoragePath = _currentConfiguration.FilePaths.DecksDirectory;
                BackupDirectory = _currentConfiguration.FilePaths.BackupDirectory;
                ExportPath = _currentConfiguration.FilePaths.ExportDirectory;
                ImportPath = "imports"; // Default value
                LogFilePath = "logs"; // Default value

                // Load Study Mode property
                StudyMode = _currentConfiguration.StudySession.DefaultStudyMode.ToString();

                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Configuration loaded into properties successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error loading configuration into properties: {ex.Message}");
            }
        }

        private void SavePropertiesToConfiguration()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Saving properties to configuration");
                
                // Save Leitner Box properties
                _currentConfiguration.LeitnerBoxes.NumberOfBoxes = NumberOfBoxes;
                
                // Parse review intervals from text
                if (!string.IsNullOrEmpty(ReviewIntervals))
                {
                    var intervals = ReviewIntervals.Split(',')
                        .Select(s => s.Trim())
                        .Where(s => int.TryParse(s, out _))
                        .Select((s, index) => new { BoxNumber = index, IntervalDays = int.Parse(s) })
                        .ToList();
                    
                    _currentConfiguration.ReviewScheduling.BoxIntervals = intervals
                        .Select(i => new Models.BoxInterval { BoxNumber = i.BoxNumber, IntervalDays = i.IntervalDays })
                        .ToList();
                }

                // Save Daily Limits properties
                _currentConfiguration.DailyLimits.MaxCardsPerDay = MaxCardsPerDay;
                _currentConfiguration.DailyLimits.MaxStudyTimePerDay = TimeSpan.FromMinutes(MaxStudyTimeMinutes);

                // Save Advanced Study Session properties
                _currentConfiguration.StudySession.AutoAdvance = AutoAdvance;
                _currentConfiguration.StudySession.AutoAdvanceDelay = AutoAdvanceDelay;
                _currentConfiguration.StudySession.ShowStatistics = ShowStatistics;
                _currentConfiguration.StudySession.ShowProgress = ShowProgress;
                _currentConfiguration.StudySession.ShuffleCards = ShuffleCards;

                // Save File Paths properties
                _currentConfiguration.FilePaths.DecksDirectory = DeckStoragePath;
                _currentConfiguration.FilePaths.BackupDirectory = BackupDirectory;
                _currentConfiguration.FilePaths.ExportDirectory = ExportPath;

                // Save Study Mode property
                if (Enum.TryParse<StudyMode>(StudyMode, out var studyMode))
                {
                    _currentConfiguration.StudySession.DefaultStudyMode = studyMode;
                }

                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Properties saved to configuration successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error saving properties to configuration: {ex.Message}");
            }
        }

        private void SetupEventHandlers()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Setting up event handlers");
                
                // Slider value changed events
                if (NumberOfBoxesSlider != null)
                {
                    NumberOfBoxesSlider.ValueChanged += NumberOfBoxesSlider_ValueChanged;
                }
                
                if (MaxCardsPerDaySlider != null)
                {
                    MaxCardsPerDaySlider.ValueChanged += MaxCardsPerDaySlider_ValueChanged;
                }
                
                if (MaxStudyTimeSlider != null)
                {
                    MaxStudyTimeSlider.ValueChanged += MaxStudyTimeSlider_ValueChanged;
                }
                
                if (AutoAdvanceDelaySlider != null)
                {
                    AutoAdvanceDelaySlider.ValueChanged += AutoAdvanceDelaySlider_ValueChanged;
                }
                
                // CheckBox events
                if (AutoAdvanceCheckBox != null)
                {
                    AutoAdvanceCheckBox.Checked += AutoAdvanceCheckBox_Checked;
                    AutoAdvanceCheckBox.Unchecked += AutoAdvanceCheckBox_Unchecked;
                }
                
                if (ShowStatisticsCheckBox != null)
                {
                    ShowStatisticsCheckBox.Checked += ShowStatisticsCheckBox_Checked;
                    ShowStatisticsCheckBox.Unchecked += ShowStatisticsCheckBox_Unchecked;
                }
                
                if (ShowProgressCheckBox != null)
                {
                    ShowProgressCheckBox.Checked += ShowProgressCheckBox_Checked;
                    ShowProgressCheckBox.Unchecked += ShowProgressCheckBox_Unchecked;
                }
                
                if (ShuffleCardsCheckBox != null)
                {
                    ShuffleCardsCheckBox.Checked += ShuffleCardsCheckBox_Checked;
                    ShuffleCardsCheckBox.Unchecked += ShuffleCardsCheckBox_Unchecked;
                }
                
                // TextBox text changed events
                if (ReviewIntervalTextBox != null)
                {
                    ReviewIntervalTextBox.TextChanged += ReviewIntervalTextBox_TextChanged;
                }
                
                if (DeckStoragePathTextBox != null)
                {
                    DeckStoragePathTextBox.TextChanged += DeckStoragePathTextBox_TextChanged;
                }
                
                if (BackupDirectoryTextBox != null)
                {
                    BackupDirectoryTextBox.TextChanged += BackupDirectoryTextBox_TextChanged;
                }
                
                if (ExportPathTextBox != null)
                {
                    ExportPathTextBox.TextChanged += ExportPathTextBox_TextChanged;
                }
                
                if (ImportPathTextBox != null)
                {
                    ImportPathTextBox.TextChanged += ImportPathTextBox_TextChanged;
                }
                
                if (LogFilePathTextBox != null)
                {
                    LogFilePathTextBox.TextChanged += LogFilePathTextBox_TextChanged;
                }
                
                // ComboBox selection changed events
                if (StudyModeComboBox != null)
                {
                    StudyModeComboBox.SelectionChanged += StudyModeComboBox_SelectionChanged;
                }
                
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Event handlers set up successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error setting up event handlers: {ex.Message}");
            }
        }

        private void LoadConfiguration()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Loading configuration");
                _currentConfiguration = _configurationService.GetConfiguration();
                
                // Populate Leitner Box controls
                if (NumberOfBoxesSlider != null)
                {
            NumberOfBoxesSlider.Value = _currentConfiguration.LeitnerBoxes.NumberOfBoxes;
                }
                
                if (ReviewIntervalTextBox != null)
                {
                    var intervals = _currentConfiguration.ReviewScheduling.BoxIntervals
                        .OrderBy(bi => bi.BoxNumber)
                        .Select(bi => bi.IntervalDays.ToString());
                    ReviewIntervalTextBox.Text = string.Join(", ", intervals);
                }

                // Populate Daily Limits controls
                if (MaxCardsPerDaySlider != null)
                {
            MaxCardsPerDaySlider.Value = _currentConfiguration.DailyLimits.MaxCardsPerDay;
                }

                if (MaxStudyTimeSlider != null)
                {
                    MaxStudyTimeSlider.Value = (int)_currentConfiguration.DailyLimits.MaxStudyTimePerDay.TotalMinutes;
                }

                if (BreakIntervalSlider != null)
                {
                    // Use a default break interval since it's not in the configuration
                    BreakIntervalSlider.Value = 5;
                }

                // Populate Advanced Study Session controls
                if (AutoAdvanceCheckBox != null)
                {
                    AutoAdvanceCheckBox.IsChecked = _currentConfiguration.StudySession.AutoAdvance;
                }

                if (AutoAdvanceDelaySlider != null)
                {
                    AutoAdvanceDelaySlider.Value = _currentConfiguration.StudySession.AutoAdvanceDelay;
                }

                if (ShowStatisticsCheckBox != null)
                {
                    ShowStatisticsCheckBox.IsChecked = _currentConfiguration.StudySession.ShowStatistics;
                }

                if (ShowProgressCheckBox != null)
                {
                    ShowProgressCheckBox.IsChecked = _currentConfiguration.StudySession.ShowProgress;
                }

                if (ShuffleCardsCheckBox != null)
                {
                    ShuffleCardsCheckBox.IsChecked = _currentConfiguration.StudySession.ShuffleCards;
                }

                // Populate File Paths controls
                if (DeckStoragePathTextBox != null)
                {
                    DeckStoragePathTextBox.Text = _currentConfiguration.FilePaths.DecksDirectory;
                }

                if (BackupDirectoryTextBox != null)
                {
                    BackupDirectoryTextBox.Text = _currentConfiguration.FilePaths.BackupDirectory;
                }

                if (ExportPathTextBox != null)
                {
                    ExportPathTextBox.Text = _currentConfiguration.FilePaths.ExportDirectory;
                }

                if (ImportPathTextBox != null)
                {
                    // Use a default import path since it's not in the configuration
                    ImportPathTextBox.Text = "imports";
                }

                if (LogFilePathTextBox != null)
                {
                    // Use a default log file path since it's not in the configuration
                    LogFilePathTextBox.Text = "logs";
                }
                
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Configuration loaded successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error loading configuration: {ex.Message}");
                // Create default configuration if loading fails
                _currentConfiguration = _configurationService.CreateDefaultConfiguration();
            }
        }

        private bool ValidateConfiguration()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Validating configuration");
                return _configurationService.ValidateConfiguration(_currentConfiguration);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error validating configuration: {ex.Message}");
                return false;
            }
        }

        private void ResetToDefaults()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Resetting to defaults");
                _currentConfiguration = _configurationService.CreateDefaultConfiguration();
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Configuration reset to defaults");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error resetting configuration: {ex.Message}");
            }
        }

        private async Task ImportConfigurationAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Importing configuration");
                
                var picker = new FileOpenPicker();
                picker.ViewMode = PickerViewMode.List;
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeFilter.Add(".json");
                
                // Initialize with window handle for WinUI 3
                var window = App.MainWindow;
                if (window != null)
                {
                    var hwnd = WindowNative.GetWindowHandle(window);
                    InitializeWithWindow.Initialize(picker, hwnd);
                }
                
                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    var jsonContent = await FileIO.ReadTextAsync(file);
                    var importedConfig = JsonConvert.DeserializeObject<AppConfiguration>(jsonContent);
                    
                    if (importedConfig != null)
                    {
                    _currentConfiguration = importedConfig;
                        LoadConfiguration(); // Refresh UI with imported data
                        UpdateStatus("Configuration imported successfully");
                    }
                    else
                    {
                        UpdateStatus("Error: Invalid configuration file format");
                    }
                }
                else
                {
                    UpdateStatus("Import cancelled");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error importing configuration: {ex.Message}");
                UpdateStatus("Error importing configuration");
            }
        }

        private async Task ExportConfigurationAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Exporting configuration");
                
                var picker = new FileSavePicker();
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeChoices.Add("JSON Configuration", new[] { ".json" });
                picker.SuggestedFileName = "flashcard-config";
                
                // Initialize with window handle for WinUI 3
                var window = App.MainWindow;
                if (window != null)
                {
                    var hwnd = WindowNative.GetWindowHandle(window);
                    InitializeWithWindow.Initialize(picker, hwnd);
                }
                
                var file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    var jsonContent = JsonConvert.SerializeObject(_currentConfiguration, Formatting.Indented);
                    await FileIO.WriteTextAsync(file, jsonContent);
                    UpdateStatus("Configuration exported successfully");
                }
                else
                {
                    UpdateStatus("Export cancelled");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error exporting configuration: {ex.Message}");
                UpdateStatus("Error exporting configuration");
            }
        }

        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ImportButton_Click");
            await ImportConfigurationAsync();
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ExportButton_Click");
            await ExportConfigurationAsync();
        }

        // Slider value changed event handlers
        private void NumberOfBoxesSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: NumberOfBoxesSlider value changed");
            DebouncedSave();
        }

        private void MaxCardsPerDaySlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: MaxCardsPerDaySlider value changed");
            DebouncedSave();
        }

        private void MaxStudyTimeSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: MaxStudyTimeSlider value changed");
            DebouncedSave();
        }

        private void AutoAdvanceDelaySlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: AutoAdvanceDelaySlider value changed");
            DebouncedSave();
        }

        // CheckBox event handlers
        private void AutoAdvanceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: AutoAdvanceCheckBox checked");
            DebouncedSave();
        }

        private void AutoAdvanceCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: AutoAdvanceCheckBox unchecked");
            DebouncedSave();
        }

        private void ShowStatisticsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ShowStatisticsCheckBox checked");
            DebouncedSave();
        }

        private void ShowStatisticsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ShowStatisticsCheckBox unchecked");
            DebouncedSave();
        }

        private void ShowProgressCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ShowProgressCheckBox checked");
            DebouncedSave();
        }

        private void ShowProgressCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ShowProgressCheckBox unchecked");
            DebouncedSave();
        }

        private void ShuffleCardsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ShuffleCardsCheckBox checked");
            DebouncedSave();
        }

        private void ShuffleCardsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ShuffleCardsCheckBox unchecked");
            DebouncedSave();
        }

        // TextBox text changed event handlers
        private void ReviewIntervalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ReviewIntervalTextBox text changed");
            DebouncedSave();
        }

        private void DeckStoragePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: DeckStoragePathTextBox text changed");
            DebouncedSave();
        }

        private void BackupDirectoryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: BackupDirectoryTextBox text changed");
            DebouncedSave();
        }

        private void ExportPathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ExportPathTextBox text changed");
            DebouncedSave();
        }

        private void ImportPathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ImportPathTextBox text changed");
            DebouncedSave();
        }

        private void LogFilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: LogFilePathTextBox text changed");
            DebouncedSave();
        }

        // ComboBox selection changed event handler
        private void StudyModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: StudyModeComboBox selection changed");
            DebouncedSave();
        }

        private void DebouncedSave()
        {
            try
            {
                // Dispose existing timer
                _saveTimer?.Dispose();
                
                // Create new timer with 1 second delay
                _saveTimer = new System.Threading.Timer(_ => 
                {
                    // Use Dispatcher to ensure UI thread execution
                    DispatcherQueue.TryEnqueue(() => SaveConfigurationOnChange());
                }, null, 1000, System.Threading.Timeout.Infinite);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error in debounced save: {ex.Message}");
            }
        }

        private void SaveUIToConfiguration()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Saving UI values to configuration");
                
                // Save Leitner Box settings
                if (NumberOfBoxesSlider != null)
                {
                    _currentConfiguration.LeitnerBoxes.NumberOfBoxes = (int)NumberOfBoxesSlider.Value;
                }
                
                if (ReviewIntervalTextBox != null)
                {
                    // Parse review intervals from text
                    var intervalText = ReviewIntervalTextBox.Text;
                    if (!string.IsNullOrEmpty(intervalText))
                    {
                        var intervals = intervalText.Split(',')
                            .Select(s => s.Trim())
                            .Where(s => int.TryParse(s, out _))
                            .Select((s, index) => new { BoxNumber = index, IntervalDays = int.Parse(s) })
                            .ToList();
                        
                        _currentConfiguration.ReviewScheduling.BoxIntervals = intervals
                            .Select(i => new Models.BoxInterval { BoxNumber = i.BoxNumber, IntervalDays = i.IntervalDays })
                            .ToList();
                    }
                }

                // Save Daily Limits settings
                if (MaxCardsPerDaySlider != null)
                {
                    _currentConfiguration.DailyLimits.MaxCardsPerDay = (int)MaxCardsPerDaySlider.Value;
                }
                
                if (MaxStudyTimeSlider != null)
                {
                    _currentConfiguration.DailyLimits.MaxStudyTimePerDay = TimeSpan.FromMinutes(MaxStudyTimeSlider.Value);
                }

                // Save Advanced Study Session settings
                if (AutoAdvanceCheckBox != null)
                {
                    _currentConfiguration.StudySession.AutoAdvance = AutoAdvanceCheckBox.IsChecked == true;
                }
                
                if (AutoAdvanceDelaySlider != null)
                {
                    _currentConfiguration.StudySession.AutoAdvanceDelay = (int)AutoAdvanceDelaySlider.Value;
                }
                
                if (ShowStatisticsCheckBox != null)
                {
                    _currentConfiguration.StudySession.ShowStatistics = ShowStatisticsCheckBox.IsChecked == true;
                }
                
                if (ShowProgressCheckBox != null)
                {
                    _currentConfiguration.StudySession.ShowProgress = ShowProgressCheckBox.IsChecked == true;
                }
                
                if (ShuffleCardsCheckBox != null)
                {
                    _currentConfiguration.StudySession.ShuffleCards = ShuffleCardsCheckBox.IsChecked == true;
                }

                // Save File Paths settings
                if (DeckStoragePathTextBox != null)
                {
                    _currentConfiguration.FilePaths.DecksDirectory = DeckStoragePathTextBox.Text;
                }
                
                if (BackupDirectoryTextBox != null)
                {
                    _currentConfiguration.FilePaths.BackupDirectory = BackupDirectoryTextBox.Text;
                }
                
                if (ExportPathTextBox != null)
                {
                    _currentConfiguration.FilePaths.ExportDirectory = ExportPathTextBox.Text;
                }
                
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: UI values saved to configuration successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error saving UI to configuration: {ex.Message}");
            }
        }

        private void SaveConfigurationOnChange()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Auto-saving configuration");
                SavePropertiesToConfiguration();
                _configurationService.SaveConfiguration();
                UpdateStatus("Configuration auto-saved");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error auto-saving configuration: {ex.Message}");
                UpdateStatus("Error auto-saving configuration");
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("ConfigurationPage: Page_Unloaded");
                _saveTimer?.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error in Page_Unloaded: {ex.Message}");
            }
        }

        // TODO: Implement UI Settings functionality
        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ThemeComboBox selection changed - TODO: Implement theme switching");
            UpdateStatus("Theme selection changed - TODO: Implement theme switching");
        }

        private void FontSizeSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: FontSizeSlider value changed - TODO: Implement font size changes");
            UpdateStatus("Font size changed - TODO: Implement font size changes");
        }

        private void EnableAnimationsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableAnimationsCheckBox checked - TODO: Implement animation toggle");
            UpdateStatus("Animations enabled - TODO: Implement animation toggle");
        }

        private void EnableAnimationsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableAnimationsCheckBox unchecked - TODO: Implement animation toggle");
            UpdateStatus("Animations disabled - TODO: Implement animation toggle");
        }

        private void AnimationSpeedSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: AnimationSpeedSlider value changed - TODO: Implement animation speed changes");
            UpdateStatus("Animation speed changed - TODO: Implement animation speed changes");
        }

        // TODO: Implement Accessibility functionality
        private void EnableKeyboardNavigationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableKeyboardNavigationCheckBox checked - TODO: Implement keyboard navigation");
            UpdateStatus("Keyboard navigation enabled - TODO: Implement keyboard navigation");
        }

        private void EnableKeyboardNavigationCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableKeyboardNavigationCheckBox unchecked - TODO: Implement keyboard navigation");
            UpdateStatus("Keyboard navigation disabled - TODO: Implement keyboard navigation");
        }

        private void TabOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: TabOrderComboBox selection changed - TODO: Implement tab order changes");
            UpdateStatus("Tab order changed - TODO: Implement tab order changes");
        }

        private void EnableScreenReaderCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableScreenReaderCheckBox checked - TODO: Implement screen reader support");
            UpdateStatus("Screen reader support enabled - TODO: Implement screen reader support");
        }

        private void EnableScreenReaderCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableScreenReaderCheckBox unchecked - TODO: Implement screen reader support");
            UpdateStatus("Screen reader support disabled - TODO: Implement screen reader support");
        }

        private void AnnounceChangesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: AnnounceChangesCheckBox checked - TODO: Implement change announcements");
            UpdateStatus("Change announcements enabled - TODO: Implement change announcements");
        }

        private void AnnounceChangesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: AnnounceChangesCheckBox unchecked - TODO: Implement change announcements");
            UpdateStatus("Change announcements disabled - TODO: Implement change announcements");
        }

        // TODO: Implement Responsive Layout functionality
        private void SmallBreakpointTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: SmallBreakpointTextBox text changed - TODO: Implement responsive breakpoints");
            UpdateStatus("Small breakpoint changed - TODO: Implement responsive breakpoints");
        }

        private void MediumBreakpointTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: MediumBreakpointTextBox text changed - TODO: Implement responsive breakpoints");
            UpdateStatus("Medium breakpoint changed - TODO: Implement responsive breakpoints");
        }

        private void LargeBreakpointTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: LargeBreakpointTextBox text changed - TODO: Implement responsive breakpoints");
            UpdateStatus("Large breakpoint changed - TODO: Implement responsive breakpoints");
        }

        private void LayoutModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: LayoutModeComboBox selection changed - TODO: Implement layout mode changes");
            UpdateStatus("Layout mode changed - TODO: Implement layout mode changes");
        }

        // TODO: Implement Configuration Backup functionality
        private void EnableAutoBackupCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableAutoBackupCheckBox checked - TODO: Implement auto backup");
            UpdateStatus("Auto backup enabled - TODO: Implement auto backup");
        }

        private void EnableAutoBackupCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableAutoBackupCheckBox unchecked - TODO: Implement auto backup");
            UpdateStatus("Auto backup disabled - TODO: Implement auto backup");
        }

        private void BackupIntervalSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: BackupIntervalSlider value changed - TODO: Implement backup interval changes");
            UpdateStatus("Backup interval changed - TODO: Implement backup interval changes");
        }

        private void MaxBackupsSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: MaxBackupsSlider value changed - TODO: Implement max backups changes");
            UpdateStatus("Max backups changed - TODO: Implement max backups changes");
        }

        private void CreateBackupButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: CreateBackupButton_Click - TODO: Implement backup creation");
            UpdateStatus("Create backup clicked - TODO: Implement backup creation");
        }

        private void RestoreBackupButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: RestoreBackupButton_Click - TODO: Implement backup restoration");
            UpdateStatus("Restore backup clicked - TODO: Implement backup restoration");
        }

        private void ManageBackupsButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ManageBackupsButton_Click - TODO: Implement backup management");
            UpdateStatus("Manage backups clicked - TODO: Implement backup management");
        }

        // TODO: Implement Advanced Validation functionality
        private void EnableCrossFieldValidationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableCrossFieldValidationCheckBox checked - TODO: Implement cross-field validation");
            UpdateStatus("Cross-field validation enabled - TODO: Implement cross-field validation");
        }

        private void EnableCrossFieldValidationCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: EnableCrossFieldValidationCheckBox unchecked - TODO: Implement cross-field validation");
            UpdateStatus("Cross-field validation disabled - TODO: Implement cross-field validation");
        }

        private void ShowValidationErrorsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ShowValidationErrorsCheckBox checked - TODO: Implement detailed validation errors");
            UpdateStatus("Detailed validation errors enabled - TODO: Implement detailed validation errors");
        }

        private void ShowValidationErrorsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ShowValidationErrorsCheckBox unchecked - TODO: Implement detailed validation errors");
            UpdateStatus("Detailed validation errors disabled - TODO: Implement detailed validation errors");
        }

        private void ValidateOnChangeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ValidateOnChangeCheckBox checked - TODO: Implement validation on change");
            UpdateStatus("Validation on change enabled - TODO: Implement validation on change");
        }

        private void ValidateOnChangeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ValidateOnChangeCheckBox unchecked - TODO: Implement validation on change");
            UpdateStatus("Validation on change disabled - TODO: Implement validation on change");
        }

        // Placeholder methods for future implementation
        private void ApplyUISettings()
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ApplyUISettings - TODO: Implement UI settings application");
            UpdateStatus("Applying UI settings - TODO: Implement UI settings application");
        }

        private void ApplyAccessibilitySettings()
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ApplyAccessibilitySettings - TODO: Implement accessibility settings application");
            UpdateStatus("Applying accessibility settings - TODO: Implement accessibility settings application");
        }

        private void ApplyResponsiveLayoutSettings()
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ApplyResponsiveLayoutSettings - TODO: Implement responsive layout settings application");
            UpdateStatus("Applying responsive layout settings - TODO: Implement responsive layout settings application");
        }

        private void CreateConfigurationBackup()
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: CreateConfigurationBackup - TODO: Implement configuration backup creation");
            UpdateStatus("Creating configuration backup - TODO: Implement configuration backup creation");
        }

        private void RestoreConfigurationBackup()
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: RestoreConfigurationBackup - TODO: Implement configuration backup restoration");
            UpdateStatus("Restoring configuration backup - TODO: Implement configuration backup restoration");
        }

        private void ApplyAdvancedValidationSettings()
        {
            System.Diagnostics.Debug.WriteLine("ConfigurationPage: ApplyAdvancedValidationSettings - TODO: Implement advanced validation settings application");
            UpdateStatus("Applying advanced validation settings - TODO: Implement advanced validation settings application");
        }

        private void UpdateStatus(string message)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Updating status to: {message}");
                if (StatusText != null)
                {
                    StatusText.Text = message;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ConfigurationPage: Error updating status: {ex.Message}");
            }
        }
    }
}
