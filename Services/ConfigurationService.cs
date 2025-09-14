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
            updateAction(_configuration!);
            SaveConfiguration();
        }

        private AppConfiguration CreateDefaultConfiguration()
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
    }
}
