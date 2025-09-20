using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlashcardApp.Models;
using FlashcardApp.UI.Abstractions;

namespace FlashcardApp.Services
{
    /// <summary>
    /// Service that handles business logic for console application operations
    /// </summary>
    public class ConsoleApplicationService
    {
        private readonly ConfigurationService _configService;
        private readonly DeckService _deckService;
        private readonly StudySessionService _studySessionService;
        private readonly LeitnerBoxService _leitnerBoxService;

        public ConsoleApplicationService(
            ConfigurationService configService,
            DeckService deckService,
            StudySessionService studySessionService,
            LeitnerBoxService leitnerBoxService)
        {
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
            _deckService = deckService ?? throw new ArgumentNullException(nameof(deckService));
            _studySessionService = studySessionService ?? throw new ArgumentNullException(nameof(studySessionService));
            _leitnerBoxService = leitnerBoxService ?? throw new ArgumentNullException(nameof(leitnerBoxService));
        }

        #region Study Session Operations

        public async Task<StudySessionResult> StartStudySessionAsync(Deck selectedDeck, StudyMode studyMode, int maxCards)
        {
            return await Task.Run(() => _studySessionService.StartStudySession(selectedDeck, studyMode, maxCards));
        }

        public async Task<List<Deck>> GetAllDecksAsync()
        {
            return await Task.Run(() => _deckService.LoadAllDecks());
        }

        public async Task<Deck?> SelectDeckAsync(List<Deck> decks, string prompt)
        {
            return await Task.Run(() =>
            {
                if (!decks.Any())
                {
                    return (Deck?)null;
                }

                // This will be handled by the UI layer
                return (Deck?)null; // Placeholder - actual selection logic will be in UI
            });
        }

        public async Task<StudyMode> SelectStudyModeAsync()
        {
            return await Task.Run(() => StudyMode.FrontToBack); // Default, actual selection in UI
        }

        public async Task<int> GetMaxCardsForSessionAsync()
        {
            return await Task.Run(() => 100); // Default, actual input in UI
        }

        #endregion

        #region Deck Management Operations

        public async Task<DeckCreationResult> CreateNewDeckAsync(string name, string description, List<string> tags)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return new DeckCreationResult
                    {
                        Success = false,
                        Message = "Deck name cannot be empty.",
                        ErrorCode = "EMPTY_NAME"
                    };
                }

                // Check for duplicate names
                if (_deckService.DeckNameExists(name))
                {
                    var uniqueName = _deckService.GetUniqueDeckName(name);
                    return new DeckCreationResult
                    {
                        Success = false,
                        Message = $"A deck named '{name}' already exists. Suggested unique name: '{uniqueName}'",
                        ErrorCode = "DUPLICATE_NAME",
                        SuggestedName = uniqueName
                    };
                }

                var deck = _deckService.CreateNewDeck(name, description, tags);

                if (_deckService.SaveDeck(deck))
                {
                    return new DeckCreationResult
                    {
                        Success = true,
                        Message = $"Deck '{name}' created successfully!",
                        Deck = deck
                    };
                }
                else
                {
                    return new DeckCreationResult
                    {
                        Success = false,
                        Message = "Failed to create deck.",
                        ErrorCode = "SAVE_FAILED"
                    };
                }
            });
        }

        public async Task<DeckUpdateResult> UpdateDeckAsync(Deck deck, string? newName = null, string? newDescription = null, List<string>? newTags = null)
        {
            return await Task.Run(() =>
            {
                bool hasChanges = false;

                if (!string.IsNullOrWhiteSpace(newName) && newName != deck.Name)
                {
                    deck.Name = newName;
                    hasChanges = true;
                }

                if (newDescription != null && newDescription != deck.Description)
                {
                    deck.Description = newDescription;
                    hasChanges = true;
                }

                if (newTags != null)
                {
                    deck.Tags = newTags;
                    hasChanges = true;
                }

                if (hasChanges)
                {
                    deck.LastModified = DateTime.Now;

                    if (_deckService.SaveDeck(deck))
                    {
                        return new DeckUpdateResult
                        {
                            Success = true,
                            Message = "Deck updated successfully!",
                            Deck = deck
                        };
                    }
                    else
                    {
                        return new DeckUpdateResult
                        {
                            Success = false,
                            Message = "Failed to update deck.",
                            ErrorCode = "SAVE_FAILED"
                        };
                    }
                }

                return new DeckUpdateResult
                {
                    Success = true,
                    Message = "No changes made.",
                    Deck = deck
                };
            });
        }

        public async Task<DeckDeletionResult> DeleteDeckAsync(Deck deck)
        {
            return await Task.Run(() =>
            {
                if (_deckService.DeleteDeck(deck.Id))
                {
                    return new DeckDeletionResult
                    {
                        Success = true,
                        Message = $"Deck '{deck.Name}' deleted successfully!",
                        DeletedDeck = deck
                    };
                }
                else
                {
                    return new DeckDeletionResult
                    {
                        Success = false,
                        Message = "Failed to delete deck.",
                        ErrorCode = "DELETE_FAILED"
                    };
                }
            });
        }

        public async Task<DeckImportResult> ImportDeckAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                if (!File.Exists(filePath))
                {
                    return new DeckImportResult
                    {
                        Success = false,
                        Message = $"File not found: {filePath}",
                        ErrorCode = "FILE_NOT_FOUND"
                    };
                }

                try
                {
                    var importedDeck = _deckService.ImportDeck(filePath);
                    if (importedDeck != null)
                    {
                        if (_deckService.SaveDeck(importedDeck))
                        {
                            return new DeckImportResult
                            {
                                Success = true,
                                Message = $"Deck '{importedDeck.Name}' imported successfully!",
                                ImportedDeck = importedDeck,
                                CardsImported = importedDeck.TotalCards,
                                FileFormat = Path.GetExtension(filePath).ToUpper()
                            };
                        }
                        else
                        {
                            return new DeckImportResult
                            {
                                Success = false,
                                Message = "Failed to save imported deck.",
                                ErrorCode = "SAVE_FAILED"
                            };
                        }
                    }
                    else
                    {
                        return new DeckImportResult
                        {
                            Success = false,
                            Message = "Failed to import deck. Please check the file format.",
                            ErrorCode = "IMPORT_FAILED"
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new DeckImportResult
                    {
                        Success = false,
                        Message = $"Error importing deck: {ex.Message}",
                        ErrorCode = "IMPORT_ERROR"
                    };
                }
            });
        }

        public async Task<DeckExportResult> ExportDeckAsync(Deck deck, string filePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (_deckService.ExportDeck(deck, filePath))
                    {
                        return new DeckExportResult
                        {
                            Success = true,
                            Message = $"Deck '{deck.Name}' exported successfully!",
                            ExportedDeck = deck,
                            FilePath = filePath,
                            FileFormat = Path.GetExtension(filePath).ToUpper()
                        };
                    }
                    else
                    {
                        return new DeckExportResult
                        {
                            Success = false,
                            Message = "Failed to export deck.",
                            ErrorCode = "EXPORT_FAILED"
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new DeckExportResult
                    {
                        Success = false,
                        Message = $"Error exporting deck: {ex.Message}",
                        ErrorCode = "EXPORT_ERROR"
                    };
                }
            });
        }

        #endregion

        #region Statistics Operations

        public async Task<StatisticsResult> GetStatisticsAsync()
        {
            return await Task.Run(() =>
            {
                var decks = _deckService.LoadAllDecks();

                if (!decks.Any())
                {
                    return new StatisticsResult
                    {
                        Success = false,
                        Message = "No decks available.",
                        ErrorCode = "NO_DECKS"
                    };
                }

                var overallStats = CalculateOverallStatistics(decks);

                return new StatisticsResult
                {
                    Success = true,
                    Message = "Statistics retrieved successfully.",
                    Decks = decks,
                    OverallStatistics = overallStats
                };
            });
        }

        private OverallStatistics CalculateOverallStatistics(List<Deck> decks)
        {
            var totalDecks = decks.Count;
            var totalCards = decks.Sum(d => d.TotalCards);
            var totalStudyTime = TimeSpan.FromTicks(decks.Sum(d => d.Statistics.TotalStudyTime.Ticks));
            var averageSuccessRate = decks.Any() ? decks.Average(d => d.Statistics.OverallSuccessRate) : 0;
            var totalStudySessions = decks.Sum(d => d.Statistics.TotalStudySessions);

            return new OverallStatistics
            {
                TotalDecks = totalDecks,
                TotalCards = totalCards,
                TotalStudyTime = totalStudyTime,
                AverageSuccessRate = averageSuccessRate,
                TotalStudySessions = totalStudySessions
            };
        }

        #endregion

        #region Configuration Operations

        public async Task<AppConfiguration> GetConfigurationAsync()
        {
            return await Task.Run(() => _configService.GetConfiguration());
        }

        public async Task<ConfigurationUpdateResult> UpdateConfigurationAsync(Action<AppConfiguration> updateAction)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _configService.UpdateConfiguration(updateAction);
                    return new ConfigurationUpdateResult
                    {
                        Success = true,
                        Message = "Configuration updated successfully.",
                        Configuration = _configService.GetConfiguration()
                    };
                }
                catch (Exception ex)
                {
                    return new ConfigurationUpdateResult
                    {
                        Success = false,
                        Message = $"Error updating configuration: {ex.Message}",
                        ErrorCode = "UPDATE_FAILED"
                    };
                }
            });
        }

        public async Task<ConfigurationResetResult> ResetConfigurationAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var defaultConfig = new AppConfiguration();
                    _configService.UpdateConfiguration(config =>
                    {
                        config.LeitnerBoxes = defaultConfig.LeitnerBoxes;
                        config.StudySession = defaultConfig.StudySession;
                        config.DailyLimits = defaultConfig.DailyLimits;
                        config.UI = defaultConfig.UI;
                        config.FilePaths = defaultConfig.FilePaths;
                        config.ReviewScheduling = defaultConfig.ReviewScheduling;
                    });

                    return new ConfigurationResetResult
                    {
                        Success = true,
                        Message = "Configuration has been reset to defaults!",
                        Configuration = _configService.GetConfiguration()
                    };
                }
                catch (Exception ex)
                {
                    return new ConfigurationResetResult
                    {
                        Success = false,
                        Message = $"Failed to reset configuration: {ex.Message}",
                        ErrorCode = "RESET_FAILED"
                    };
                }
            });
        }

        #endregion

        #region File Operations

        public async Task<List<string>> GetAvailableImportFilesAsync(string directory)
        {
            return await Task.Run(() =>
            {
                var supportedExtensions = new[] { "*.csv", "*.xlsx", "*.json" };
                var files = new List<string>();

                foreach (var extension in supportedExtensions)
                {
                    var foundFiles = Directory.GetFiles(directory, extension);
                    if (extension == "*.json")
                    {
                        // Filter out config.json
                        foundFiles = foundFiles.Where(f => !f.EndsWith("config.json")).ToArray();
                    }
                    files.AddRange(foundFiles);
                }

                return files;
            });
        }

        public async Task<string> GenerateDefaultExportPathAsync(Deck deck, string format = "json")
        {
            return await Task.Run(() =>
            {
                var currentDir = Directory.GetCurrentDirectory();
                var safeFileName = deck.Name.Replace(" ", "_").Replace(":", "").Replace("/", "").Replace("\\", "");
                return Path.Combine(currentDir, $"{safeFileName}_export.{format}");
            });
        }

        #endregion
    }

    #region Result Classes

    public class DeckCreationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string? ErrorCode { get; set; }
        public string? SuggestedName { get; set; }
        public Deck? Deck { get; set; }
    }

    public class DeckUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string? ErrorCode { get; set; }
        public Deck? Deck { get; set; }
    }

    public class DeckDeletionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string? ErrorCode { get; set; }
        public Deck? DeletedDeck { get; set; }
    }

    public class DeckImportResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string? ErrorCode { get; set; }
        public Deck? ImportedDeck { get; set; }
        public int CardsImported { get; set; }
        public string? FileFormat { get; set; }
    }

    public class DeckExportResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string? ErrorCode { get; set; }
        public Deck? ExportedDeck { get; set; }
        public string? FilePath { get; set; }
        public string? FileFormat { get; set; }
    }

    public class StatisticsResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string? ErrorCode { get; set; }
        public List<Deck> Decks { get; set; } = new();
        public OverallStatistics? OverallStatistics { get; set; }
    }

    public class OverallStatistics
    {
        public int TotalDecks { get; set; }
        public int TotalCards { get; set; }
        public TimeSpan TotalStudyTime { get; set; }
        public double AverageSuccessRate { get; set; }
        public int TotalStudySessions { get; set; }
    }

    public class ConfigurationUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string? ErrorCode { get; set; }
        public AppConfiguration? Configuration { get; set; }
    }

    public class ConfigurationResetResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string? ErrorCode { get; set; }
        public AppConfiguration? Configuration { get; set; }
    }

    #endregion
}
