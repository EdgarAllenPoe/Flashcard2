using FlashcardApp.Models;
using FlashcardApp.UI.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardApp.Services
{
    /// <summary>
    /// Service for managing and calculating statistics
    /// </summary>
    public class StatisticsService
    {
        private readonly DeckService _deckService;
        private readonly StudySessionService _studySessionService;

        public StatisticsService(DeckService deckService, StudySessionService studySessionService)
        {
            _deckService = deckService ?? throw new ArgumentNullException(nameof(deckService));
            _studySessionService = studySessionService ?? throw new ArgumentNullException(nameof(studySessionService));
        }

        /// <summary>
        /// Get overall statistics for all decks
        /// </summary>
        public Task<StatisticsData> GetOverallStatisticsAsync()
        {
            var decks = _deckService.LoadAllDecks();
            var statistics = new Dictionary<string, object>
            {
                ["TotalDecks"] = decks.Count,
                ["TotalFlashcards"] = decks.Sum(d => d.Flashcards.Count),
                ["TotalStudySessions"] = 25, // Mock data for now
                ["OverallSuccessRate"] = 85.5 // Mock data for now
            };

            return Task.FromResult(new StatisticsData
            {
                Statistics = statistics,
                Decks = decks
            });
        }

        /// <summary>
        /// Get statistics for a specific deck
        /// </summary>
        public Task<SessionResult> GetDeckStatisticsAsync(string deckId)
        {
            try
            {
                var deck = _deckService.LoadDeckById(deckId);
                if (deck == null)
                {
                    return Task.FromResult(new SessionResult
                    {
                        Success = false,
                        Message = "Deck not found",
                        SessionStatistics = null
                    });
                }

                // Mock statistics for now
                var sessionStats = new SessionStatistics
                {
                    TotalCards = deck.Flashcards.Count,
                    CardsStudied = deck.Flashcards.Count,
                    CorrectAnswers = (int)(deck.Flashcards.Count * 0.85),
                    IncorrectAnswers = (int)(deck.Flashcards.Count * 0.15)
                };

                return Task.FromResult(new SessionResult
                {
                    Success = true,
                    Message = "Deck statistics retrieved successfully",
                    SessionStatistics = sessionStats
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new SessionResult
                {
                    Success = false,
                    Message = $"Error retrieving deck statistics: {ex.Message}",
                    SessionStatistics = null
                });
            }
        }

        /// <summary>
        /// Get progress tracking data
        /// </summary>
        public Task<Dictionary<string, object>> GetProgressTrackingAsync()
        {
            var decks = _deckService.LoadAllDecks();
            var progressData = new Dictionary<string, object>
            {
                ["DecksCompleted"] = decks.Count(d => d.Flashcards.Count > 0),
                ["TotalDecks"] = decks.Count,
                ["CompletionRate"] = decks.Count > 0 ? Math.Round((double)decks.Count(d => d.Flashcards.Count > 0) / decks.Count * 100, 2) : 0.0
            };

            return Task.FromResult(progressData);
        }

        /// <summary>
        /// Get study time analytics
        /// </summary>
        public Task<Dictionary<string, object>> GetStudyTimeAnalyticsAsync()
        {
            // Mock data for now
            var analytics = new Dictionary<string, object>
            {
                ["TotalStudyTime"] = "2 hours 30 minutes",
                ["AverageSessionTime"] = "15 minutes",
                ["LongestSession"] = "45 minutes",
                ["StudyStreak"] = 7
            };

            return Task.FromResult(analytics);
        }
    }
}

