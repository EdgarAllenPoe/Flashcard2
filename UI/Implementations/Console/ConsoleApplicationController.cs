using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashcardApp.Models;
using FlashcardApp.Services;
using FlashcardApp.UI.Abstractions;

namespace FlashcardApp.UI.Implementations.Console
{
    /// <summary>
    /// Console-specific implementation of IApplicationController
    /// </summary>
    public class ConsoleApplicationController : IApplicationController
    {
        private readonly ConfigurationService _configService;
        private readonly DeckService _deckService;
        private readonly StudySessionService _studySessionService;
        private readonly LeitnerBoxService _leitnerBoxService;

        public ConsoleApplicationController(
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

        public async Task<ApplicationResult> StartStudySessionAsync(StudySessionRequest request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (request.SelectedDeck == null)
                    {
                        return new ApplicationResult
                        {
                            Success = false,
                            Message = "No deck selected for study session.",
                            ErrorCode = "NO_DECK_SELECTED"
                        };
                    }

                    var result = _studySessionService.StartStudySession(
                        request.SelectedDeck,
                        request.StudyMode,
                        request.MaxCards);

                    return new ApplicationResult
                    {
                        Success = result.Success,
                        Message = result.Message,
                        Data = result
                    };
                }
                catch (Exception ex)
                {
                    return new ApplicationResult
                    {
                        Success = false,
                        Message = $"Error starting study session: {ex.Message}",
                        ErrorCode = "STUDY_SESSION_ERROR"
                    };
                }
            });
        }

        public async Task<ApplicationResult> ManageDecksAsync(DeckManagementRequest request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    switch (request.Action)
                    {
                        case DeckManagementAction.Create:
                            return CreateDeck(request);
                        case DeckManagementAction.Edit:
                            return EditDeck(request);
                        case DeckManagementAction.Delete:
                            return DeleteDeck(request);
                        case DeckManagementAction.View:
                            return ViewDeck(request);
                        case DeckManagementAction.Import:
                            return ImportDeck(request);
                        case DeckManagementAction.Export:
                            return ExportDeck(request);
                        case DeckManagementAction.List:
                            return ListDecks();
                        default:
                            return new ApplicationResult
                            {
                                Success = false,
                                Message = "Unknown deck management action.",
                                ErrorCode = "UNKNOWN_ACTION"
                            };
                    }
                }
                catch (Exception ex)
                {
                    return new ApplicationResult
                    {
                        Success = false,
                        Message = $"Error managing deck: {ex.Message}",
                        ErrorCode = "DECK_MANAGEMENT_ERROR"
                    };
                }
            });
        }

        public async Task<ApplicationResult> ViewStatisticsAsync(StatisticsRequest request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var decks = _deckService.LoadAllDecks();
                    var statistics = new StatisticsData();

                    if (request.IncludeOverall)
                    {
                        // Calculate overall statistics
                        var totalDecks = decks.Count;
                        var totalCards = decks.Sum(d => d.TotalCards);
                        var totalStudyTime = TimeSpan.FromTicks(decks.Sum(d => d.Statistics.TotalStudyTime.Ticks));
                        var averageSuccessRate = decks.Any() ? decks.Average(d => d.Statistics.OverallSuccessRate) : 0;

                        statistics.Statistics = new Dictionary<string, object>
                        {
                            ["TotalDecks"] = totalDecks,
                            ["TotalCards"] = totalCards,
                            ["TotalStudyTime"] = totalStudyTime.ToString(@"hh\:mm\:ss"),
                            ["AverageSuccessRate"] = averageSuccessRate.ToString("F1")
                        };
                    }

                    if (!string.IsNullOrEmpty(request.DeckId))
                    {
                        var specificDeck = decks.FirstOrDefault(d => d.Id == request.DeckId);
                        if (specificDeck != null)
                        {
                            statistics.Decks = new List<Deck> { specificDeck };
                        }
                    }
                    else
                    {
                        statistics.Decks = decks;
                    }

                    return new ApplicationResult
                    {
                        Success = true,
                        Message = "Statistics retrieved successfully.",
                        Data = statistics
                    };
                }
                catch (Exception ex)
                {
                    return new ApplicationResult
                    {
                        Success = false,
                        Message = $"Error retrieving statistics: {ex.Message}",
                        ErrorCode = "STATISTICS_ERROR"
                    };
                }
            });
        }

        public async Task<ApplicationResult> ConfigureSettingsAsync(ConfigurationRequest request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    switch (request.Action)
                    {
                        case ConfigurationAction.View:
                            return new ApplicationResult
                            {
                                Success = true,
                                Message = "Configuration retrieved successfully.",
                                Data = _configService.GetConfiguration()
                            };
                        case ConfigurationAction.Update:
                            return UpdateConfiguration(request);
                        case ConfigurationAction.Reset:
                            return ResetConfiguration();
                        case ConfigurationAction.Get:
                            return new ApplicationResult
                            {
                                Success = true,
                                Message = "Configuration retrieved successfully.",
                                Data = _configService.GetConfiguration()
                            };
                        default:
                            return new ApplicationResult
                            {
                                Success = false,
                                Message = "Unknown configuration action.",
                                ErrorCode = "UNKNOWN_CONFIG_ACTION"
                            };
                    }
                }
                catch (Exception ex)
                {
                    return new ApplicationResult
                    {
                        Success = false,
                        Message = $"Error configuring settings: {ex.Message}",
                        ErrorCode = "CONFIGURATION_ERROR"
                    };
                }
            });
        }

        public async Task<ApplicationResult> ShowHelpAsync(HelpRequest request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var helpContent = GetHelpContent(request.Topic);
                    return new ApplicationResult
                    {
                        Success = true,
                        Message = "Help content retrieved successfully.",
                        Data = helpContent
                    };
                }
                catch (Exception ex)
                {
                    return new ApplicationResult
                    {
                        Success = false,
                        Message = $"Error retrieving help: {ex.Message}",
                        ErrorCode = "HELP_ERROR"
                    };
                }
            });
        }

        public async Task<List<Deck>> GetAllDecksAsync()
        {
            return await Task.Run(() => _deckService.LoadAllDecks());
        }

        public async Task<AppConfiguration> GetConfigurationAsync()
        {
            return await Task.Run(() => _configService.GetConfiguration());
        }

        private ApplicationResult CreateDeck(DeckManagementRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DeckName))
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "Deck name is required.",
                    ErrorCode = "MISSING_DECK_NAME"
                };
            }

            // Check for duplicate names and offer to use a unique name
            if (_deckService.DeckNameExists(request.DeckName))
            {
                var uniqueName = _deckService.GetUniqueDeckName(request.DeckName);
                return new ApplicationResult
                {
                    Success = false,
                    Message = $"A deck named '{request.DeckName}' already exists. Suggested unique name: '{uniqueName}'",
                    ErrorCode = "DUPLICATE_DECK_NAME",
                    Data = uniqueName
                };
            }

            var deck = _deckService.CreateNewDeck(request.DeckName, request.Description ?? "", request.Tags ?? new List<string>());
            
            if (_deckService.SaveDeck(deck))
            {
                return new ApplicationResult
                {
                    Success = true,
                    Message = $"Deck '{request.DeckName}' created successfully!",
                    Data = deck
                };
            }
            else
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "Failed to create deck.",
                    ErrorCode = "SAVE_DECK_FAILED"
                };
            }
        }

        private ApplicationResult EditDeck(DeckManagementRequest request)
        {
            if (request.SelectedDeck == null)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "No deck selected for editing.",
                    ErrorCode = "NO_DECK_SELECTED"
                };
            }

            // Update deck properties if provided
            if (!string.IsNullOrWhiteSpace(request.DeckName))
            {
                request.SelectedDeck.Name = request.DeckName;
            }
            if (request.Description != null)
            {
                request.SelectedDeck.Description = request.Description;
            }
            if (request.Tags != null)
            {
                request.SelectedDeck.Tags = request.Tags;
            }

            request.SelectedDeck.LastModified = DateTime.Now;

            if (_deckService.SaveDeck(request.SelectedDeck))
            {
                return new ApplicationResult
                {
                    Success = true,
                    Message = "Deck updated successfully!",
                    Data = request.SelectedDeck
                };
            }
            else
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "Failed to update deck.",
                    ErrorCode = "UPDATE_DECK_FAILED"
                };
            }
        }

        private ApplicationResult DeleteDeck(DeckManagementRequest request)
        {
            if (request.SelectedDeck == null)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "No deck selected for deletion.",
                    ErrorCode = "NO_DECK_SELECTED"
                };
            }

            if (_deckService.DeleteDeck(request.SelectedDeck.Id))
            {
                return new ApplicationResult
                {
                    Success = true,
                    Message = $"Deck '{request.SelectedDeck.Name}' deleted successfully!",
                    Data = request.SelectedDeck
                };
            }
            else
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "Failed to delete deck.",
                    ErrorCode = "DELETE_DECK_FAILED"
                };
            }
        }

        private ApplicationResult ViewDeck(DeckManagementRequest request)
        {
            if (request.SelectedDeck == null)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "No deck selected for viewing.",
                    ErrorCode = "NO_DECK_SELECTED"
                };
            }

            return new ApplicationResult
            {
                Success = true,
                Message = "Deck retrieved successfully.",
                Data = request.SelectedDeck
            };
        }

        private ApplicationResult ImportDeck(DeckManagementRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FilePath))
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "File path is required for import.",
                    ErrorCode = "MISSING_FILE_PATH"
                };
            }

            try
            {
                var importedDeck = _deckService.ImportDeck(request.FilePath);
                if (importedDeck != null)
                {
                    if (_deckService.SaveDeck(importedDeck))
                    {
                        return new ApplicationResult
                        {
                            Success = true,
                            Message = $"Deck '{importedDeck.Name}' imported successfully!",
                            Data = importedDeck
                        };
                    }
                    else
                    {
                        return new ApplicationResult
                        {
                            Success = false,
                            Message = "Failed to save imported deck.",
                            ErrorCode = "SAVE_IMPORTED_DECK_FAILED"
                        };
                    }
                }
                else
                {
                    return new ApplicationResult
                    {
                        Success = false,
                        Message = "Failed to import deck. Please check the file format.",
                        ErrorCode = "IMPORT_DECK_FAILED"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = $"Error importing deck: {ex.Message}",
                    ErrorCode = "IMPORT_DECK_ERROR"
                };
            }
        }

        private ApplicationResult ExportDeck(DeckManagementRequest request)
        {
            if (request.SelectedDeck == null)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "No deck selected for export.",
                    ErrorCode = "NO_DECK_SELECTED"
                };
            }

            if (string.IsNullOrWhiteSpace(request.FilePath))
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = "File path is required for export.",
                    ErrorCode = "MISSING_FILE_PATH"
                };
            }

            try
            {
                if (_deckService.ExportDeck(request.SelectedDeck, request.FilePath))
                {
                    return new ApplicationResult
                    {
                        Success = true,
                        Message = $"Deck '{request.SelectedDeck.Name}' exported successfully!",
                        Data = request.FilePath
                    };
                }
                else
                {
                    return new ApplicationResult
                    {
                        Success = false,
                        Message = "Failed to export deck.",
                        ErrorCode = "EXPORT_DECK_FAILED"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = $"Error exporting deck: {ex.Message}",
                    ErrorCode = "EXPORT_DECK_ERROR"
                };
            }
        }

        private ApplicationResult ListDecks()
        {
            var decks = _deckService.LoadAllDecks();
            return new ApplicationResult
            {
                Success = true,
                Message = "Decks retrieved successfully.",
                Data = decks
            };
        }

        private ApplicationResult UpdateConfiguration(ConfigurationRequest request)
        {
            try
            {
                _configService.UpdateConfiguration(config =>
                {
                    // This is a simplified implementation
                    // In a real scenario, you'd have more sophisticated configuration updating logic
                    if (!string.IsNullOrWhiteSpace(request.SettingName) && request.Value != null)
                    {
                        // Handle specific setting updates based on setting name
                        // This would need to be expanded based on the actual configuration structure
                    }
                });

                return new ApplicationResult
                {
                    Success = true,
                    Message = "Configuration updated successfully.",
                    Data = _configService.GetConfiguration()
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = $"Error updating configuration: {ex.Message}",
                    ErrorCode = "UPDATE_CONFIG_FAILED"
                };
            }
        }

        private ApplicationResult ResetConfiguration()
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

                return new ApplicationResult
                {
                    Success = true,
                    Message = "Configuration has been reset to defaults!",
                    Data = _configService.GetConfiguration()
                };
            }
            catch (Exception ex)
            {
                return new ApplicationResult
                {
                    Success = false,
                    Message = $"Failed to reset configuration: {ex.Message}",
                    ErrorCode = "RESET_CONFIG_FAILED"
                };
            }
        }

        private string GetHelpContent(string? topic)
        {
            var helpContent = @"
HELP & GUIDE

This application uses the scientifically-proven Leitner Box system for
spaced repetition learning, helping you retain information long-term.

How the Leitner Box System Works:
• Cards start in Box 0 (review daily)
• Correct answers move cards to higher boxes
• Incorrect answers move cards back to lower boxes
• Higher boxes have longer review intervals
• This optimizes your study time and retention

Study Session Controls:
• Any Key - Show answer (space, enter, letters, numbers)
• 1 - Mark as correct (move to higher box)
• 2 - Mark as incorrect (move to lower box)
• S - Skip card (no box change)
• Q or ESC - Quit session (saves progress)
• H - Show this help

Pro Tips:
• Study regularly for best results
• Don't worry about mistakes - they're part of learning!
• Use the statistics to track your progress
• Create focused decks for different subjects
";

            if (!string.IsNullOrWhiteSpace(topic))
            {
                // Add topic-specific help content here
                helpContent += $"\n\nTopic: {topic}\n";
                // Add specific help for the topic
            }

            return helpContent;
        }
    }
}
