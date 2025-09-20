using Newtonsoft.Json;
using FlashcardApp.Models;

namespace FlashcardApp.Services
{
    public class ConfigurationService
    {
        private readonly string _configFilePath;
        private AppConfiguration? _configuration;

        public ConfigurationService(string configFilePath = "config.json")
        {
            _configFilePath = configFilePath;
        }

        public AppConfiguration GetConfiguration()
        {
            if (_configuration == null)
            {
                LoadConfiguration();
            }
            return _configuration!;
        }

        public void LoadConfiguration()
        {
            try
            {
                if (File.Exists(_configFilePath))
                {
                    string json = File.ReadAllText(_configFilePath);
                    _configuration = JsonConvert.DeserializeObject<AppConfiguration>(json);
                }
                else
                {
                    _configuration = CreateDefaultConfiguration();
                    SaveConfiguration();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                _configuration = CreateDefaultConfiguration();
            }
        }

        public void SaveConfiguration()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_configuration, Formatting.Indented);
                File.WriteAllText(_configFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }

        public void UpdateConfiguration(Action<AppConfiguration> updateAction)
        {
            if (updateAction == null)
            {
                throw new ArgumentNullException(nameof(updateAction));
            }

            var config = GetConfiguration();
            updateAction(config);
            SaveConfiguration();
        }

        public AppConfiguration CreateDefaultConfiguration()
        {
            return new AppConfiguration
            {
                LeitnerBoxes = new LeitnerBoxConfiguration
                {
                    NumberOfBoxes = 5,
                    PromotionRules = new List<PromotionRule>
                    {
                        new PromotionRule { BoxNumber = 0, CorrectAnswersNeeded = 1 },
                        new PromotionRule { BoxNumber = 1, CorrectAnswersNeeded = 2 },
                        new PromotionRule { BoxNumber = 2, CorrectAnswersNeeded = 3 },
                        new PromotionRule { BoxNumber = 3, CorrectAnswersNeeded = 4 },
                        new PromotionRule { BoxNumber = 4, CorrectAnswersNeeded = 5 }
                    },
                    DemotionRules = new List<DemotionRule>
                    {
                        new DemotionRule { BoxNumber = 1, IncorrectAnswersNeeded = 1, DemoteToBox = 0 },
                        new DemotionRule { BoxNumber = 2, IncorrectAnswersNeeded = 1, DemoteToBox = 0 },
                        new DemotionRule { BoxNumber = 3, IncorrectAnswersNeeded = 1, DemoteToBox = 1 },
                        new DemotionRule { BoxNumber = 4, IncorrectAnswersNeeded = 1, DemoteToBox = 2 }
                    }
                },
                StudySession = new StudySessionConfiguration
                {
                    DefaultStudyMode = StudyMode.FrontToBack,
                    ShowStatistics = true,
                    AutoAdvance = false,
                    AutoAdvanceDelay = 3,
                    ShuffleCards = true,
                    ShowProgress = true,
                    KeyboardShortcuts = new KeyboardShortcutsConfiguration
                    {
                        CorrectAnswer = "1",
                        IncorrectAnswer = "2",
                        ShowAnswer = "space",
                        Quit = "q",
                        Skip = "s",
                        FlipCard = "f",
                        ShowStatistics = "t",
                        Help = "h"
                    }
                },
                FilePaths = new FilePathConfiguration
                {
                    DecksDirectory = "decks",
                    ConfigFileName = "config.json",
                    DeckFileExtension = ".json",
                    BackupDirectory = "backups",
                    ExportDirectory = "exports"
                },
                ReviewScheduling = new ReviewSchedulingConfiguration
                {
                    BoxIntervals = new List<BoxInterval>
                    {
                        new BoxInterval { BoxNumber = 0, IntervalDays = 1 },
                        new BoxInterval { BoxNumber = 1, IntervalDays = 3 },
                        new BoxInterval { BoxNumber = 2, IntervalDays = 7 },
                        new BoxInterval { BoxNumber = 3, IntervalDays = 14 },
                        new BoxInterval { BoxNumber = 4, IntervalDays = 30 }
                    },
                    NewCardInterval = 1,
                    MaxNewCardsPerDay = 20
                },
                DailyLimits = new DailyLimitsConfiguration
                {
                    MaxCardsPerDay = 100,
                    MinCardsPerDay = 5,
                    MaxStudyTimePerDay = TimeSpan.FromHours(2),
                    MinStudyTimePerDay = TimeSpan.FromMinutes(5)
                },
                UI = new UIConfiguration
                {
                    UseColors = true,
                    UseIcons = true,
                    ShowWelcomeMessage = true,
                    ClearScreenOnMenuChange = true,
                    ShowDetailedStatistics = true
                }
            };
        }

        public void EnsureDirectoriesExist()
        {
            var config = GetConfiguration();
            var directories = new[]
            {
                config.FilePaths.DecksDirectory,
                config.FilePaths.BackupDirectory,
                config.FilePaths.ExportDirectory
            };

            foreach (var directory in directories)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }

        /// <summary>
        /// Validates the configuration and returns true if valid
        /// </summary>
        public bool ValidateConfiguration(AppConfiguration? config)
        {
            if (config == null)
                return false;

            // Validate Leitner boxes
            if (config.LeitnerBoxes.NumberOfBoxes <= 0)
                return false;

            // Validate daily limits
            if (config.DailyLimits.MaxCardsPerDay < 0)
                return false;

            // Validate study session settings
            if (config.StudySession.AutoAdvanceDelay < 0)
                return false;

            // Validate file paths
            if (string.IsNullOrWhiteSpace(config.FilePaths.DecksDirectory))
                return false;

            // Validate box intervals
            if (config.ReviewScheduling.BoxIntervals.Any(interval => interval.IntervalDays <= 0))
                return false;

            // Validate promotion rules
            if (config.LeitnerBoxes.PromotionRules.Any(rule => rule.CorrectAnswersNeeded <= 0))
                return false;

            // Validate demotion rules
            if (config.LeitnerBoxes.DemotionRules.Any(rule => rule.IncorrectAnswersNeeded <= 0))
                return false;

            return true;
        }

        /// <summary>
        /// Gets validation errors for the configuration
        /// </summary>
        public List<string> GetValidationErrors(AppConfiguration? config)
        {
            var errors = new List<string>();

            if (config == null)
            {
                errors.Add("Configuration cannot be null");
                return errors;
            }

            // Validate Leitner boxes
            if (config.LeitnerBoxes.NumberOfBoxes <= 0)
                errors.Add("NumberOfBoxes must be greater than 0");

            // Validate daily limits
            if (config.DailyLimits.MaxCardsPerDay < 0)
                errors.Add("MaxCardsPerDay must be greater than or equal to 0");

            // Validate study session settings
            if (config.StudySession.AutoAdvanceDelay < 0)
                errors.Add("AutoAdvanceDelay must be greater than or equal to 0");

            // Validate file paths
            if (string.IsNullOrWhiteSpace(config.FilePaths.DecksDirectory))
                errors.Add("DecksDirectory cannot be empty");

            // Validate box intervals
            var invalidIntervals = config.ReviewScheduling.BoxIntervals.Where(interval => interval.IntervalDays <= 0).ToList();
            if (invalidIntervals.Any())
                errors.Add($"BoxIntervals must have IntervalDays greater than 0");

            // Validate promotion rules
            var invalidPromotionRules = config.LeitnerBoxes.PromotionRules.Where(rule => rule.CorrectAnswersNeeded <= 0).ToList();
            if (invalidPromotionRules.Any())
                errors.Add("PromotionRules must have CorrectAnswersNeeded greater than 0");

            // Validate demotion rules
            var invalidDemotionRules = config.LeitnerBoxes.DemotionRules.Where(rule => rule.IncorrectAnswersNeeded <= 0).ToList();
            if (invalidDemotionRules.Any())
                errors.Add("DemotionRules must have IncorrectAnswersNeeded greater than 0");

            return errors;
        }

        /// <summary>
        /// Validates and fixes configuration by setting invalid values to defaults
        /// </summary>
        public AppConfiguration ValidateAndFixConfiguration(AppConfiguration config)
        {
            if (config == null)
                return CreateDefaultConfiguration();

            // Fix Leitner boxes
            if (config.LeitnerBoxes.NumberOfBoxes <= 0)
                config.LeitnerBoxes.NumberOfBoxes = 5;

            // Fix daily limits
            if (config.DailyLimits.MaxCardsPerDay < 0)
                config.DailyLimits.MaxCardsPerDay = 100;

            // Fix study session settings
            if (config.StudySession.AutoAdvanceDelay < 0)
                config.StudySession.AutoAdvanceDelay = 3;

            // Fix file paths
            if (string.IsNullOrWhiteSpace(config.FilePaths.DecksDirectory))
                config.FilePaths.DecksDirectory = "decks";

            // Fix box intervals
            foreach (var interval in config.ReviewScheduling.BoxIntervals)
            {
                if (interval.IntervalDays <= 0)
                    interval.IntervalDays = 1;
            }

            // Fix promotion rules
            foreach (var rule in config.LeitnerBoxes.PromotionRules)
            {
                if (rule.CorrectAnswersNeeded <= 0)
                    rule.CorrectAnswersNeeded = 1;
            }

            // Fix demotion rules
            foreach (var rule in config.LeitnerBoxes.DemotionRules)
            {
                if (rule.IncorrectAnswersNeeded <= 0)
                    rule.IncorrectAnswersNeeded = 1;
            }

            return config;
        }

        /// <summary>
        /// Exports configuration to a file
        /// </summary>
        public void ExportConfiguration(string filePath, AppConfiguration? config = null, bool createBackup = false, string? backupPath = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            // Use current configuration if none provided
            if (config == null)
                config = GetConfiguration();

            try
            {
                // Create backup if requested
                if (createBackup && !string.IsNullOrWhiteSpace(backupPath))
                {
                    var backupConfig = GetConfiguration();
                    string backupJson = JsonConvert.SerializeObject(backupConfig, Formatting.Indented);
                    File.WriteAllText(backupPath, backupJson);
                }

                // Export configuration
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error exporting configuration: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Imports configuration from a file
        /// </summary>
        public AppConfiguration ImportConfiguration(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Configuration file not found: {filePath}");

            try
            {
                string json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                    throw new InvalidOperationException("Configuration file is empty");

                var importedConfig = JsonConvert.DeserializeObject<AppConfiguration>(json);

                if (importedConfig == null)
                    throw new InvalidOperationException("Failed to deserialize configuration");

                // Validate and fix the imported configuration
                importedConfig = ValidateAndFixConfiguration(importedConfig);

                return importedConfig;
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Invalid JSON in configuration file: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error importing configuration: {ex.Message}", ex);
            }
        }

    }
}
