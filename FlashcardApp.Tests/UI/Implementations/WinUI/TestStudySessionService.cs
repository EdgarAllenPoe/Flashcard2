using FlashcardApp.Models;
using FlashcardApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Test-friendly version of StudySessionService that bypasses console operations
    /// This prevents integration tests from hanging on user input
    /// </summary>
    public class TestStudySessionService : StudySessionService
    {
        public TestStudySessionService(ConfigurationService configService, LeitnerBoxService leitnerBoxService, DeckService deckService)
            : base(configService, leitnerBoxService, deckService)
        {
        }

        public new StudySessionResult StartStudySession(Deck deck, StudyMode studyMode, int maxCards = 50)
        {
            // For testing, we'll simulate a study session without console interaction
            // This completely bypasses the base class method to avoid console operations
            var cardsToStudy = GetCardsForStudySession(deck, studyMode, maxCards);

            if (!cardsToStudy.Any())
            {
                return new StudySessionResult
                {
                    Success = false,
                    Message = "No cards available for study at this time.",
                    SessionStatistics = new StudySessionStatistics()
                };
            }

            // Simulate studying a few cards without console interaction
            var studiedCards = new List<Flashcard>();
            var sessionStartTime = DateTime.Now;
            var correctAnswers = 0;
            var incorrectAnswers = 0;

            // Study up to 3 cards for testing (to keep tests fast)
            var cardsToStudyCount = Math.Min(3, cardsToStudy.Count);

            for (int i = 0; i < cardsToStudyCount; i++)
            {
                var card = cardsToStudy[i];
                studiedCards.Add(card);

                // Simulate user response (70% correct for testing)
                var isCorrect = i % 3 != 0; // 2 out of 3 correct

                if (isCorrect)
                {
                    correctAnswers++;
                    // Move card to next box
                    card.CurrentBox = Math.Min(card.CurrentBox + 1, 4);
                }
                else
                {
                    incorrectAnswers++;
                    // Move card to previous box (but not below 0)
                    card.CurrentBox = Math.Max(card.CurrentBox - 1, 0);
                }

                card.LastReviewed = DateTime.Now;
                card.Statistics.TotalReviews++;
            }

            var sessionStats = new StudySessionStatistics
            {
                TotalCards = studiedCards.Count,
                TotalReviews = studiedCards.Count,
                CorrectAnswers = correctAnswers,
                IncorrectAnswers = incorrectAnswers,
                SuccessRate = studiedCards.Count > 0 ? (double)correctAnswers / studiedCards.Count * 100 : 0,
                SessionTime = TimeSpan.FromSeconds(studiedCards.Count * 2), // Simulate 2 seconds per card
                TotalStudyTime = TimeSpan.FromSeconds(studiedCards.Count * 2),
                AverageResponseTime = 2.0, // 2 seconds per card
                StartTime = sessionStartTime,
                EndTime = DateTime.Now
            };

            return new StudySessionResult
            {
                Success = true,
                Message = $"Test study session completed! Studied {studiedCards.Count} cards.",
                SessionStatistics = sessionStats,
                StudiedCards = studiedCards
            };
        }

        // Access to protected field for testing
        private LeitnerBoxService _leitnerBoxService => base.GetType()
            .GetField("_leitnerBoxService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(this) as LeitnerBoxService ?? throw new InvalidOperationException("Could not access _leitnerBoxService");

        private List<Flashcard> GetCardsForStudySession(Deck deck, StudyMode studyMode, int maxCards)
        {
            // Simple implementation for testing - just return the first few cards
            var availableCards = deck.Flashcards.Where(f => f.IsActive).ToList();

            if (studyMode == StudyMode.Mixed)
            {
                availableCards = availableCards.OrderBy(x => Guid.NewGuid()).ToList();
            }
            else if (studyMode == StudyMode.BackToFront)
            {
                availableCards = availableCards.OrderByDescending(f => f.CreatedDate).ToList();
            }

            return availableCards.Take(maxCards).ToList();
        }
    }
}
