using FlashcardApp.Models;

namespace FlashcardApp.Services
{
    public class LeitnerBoxService
    {
        private readonly ConfigurationService _configService;

        public LeitnerBoxService(ConfigurationService configService)
        {
            _configService = configService;
        }

        public void ProcessCorrectAnswer(Flashcard flashcard)
        {
            var config = _configService.GetConfiguration();
            var promotionRule = config.LeitnerBoxes.PromotionRules
                .FirstOrDefault(r => r.BoxNumber == flashcard.CurrentBox);

            if (promotionRule != null)
            {
                // Increment correct streak
                flashcard.Statistics.CorrectAnswers++;
                flashcard.Statistics.Streak++;

                if (flashcard.Statistics.Streak > flashcard.Statistics.LongestStreak)
                {
                    flashcard.Statistics.LongestStreak = flashcard.Statistics.Streak;
                }

                // Check if card should be promoted
                if (flashcard.Statistics.Streak >= promotionRule.CorrectAnswersNeeded)
                {
                    if (flashcard.CurrentBox < config.LeitnerBoxes.NumberOfBoxes - 1)
                    {
                        flashcard.CurrentBox++;
                        flashcard.Statistics.Streak = 0; // Reset streak after promotion
                    }
                }
            }

            // Update review date
            UpdateNextReviewDate(flashcard);
            flashcard.LastReviewed = DateTime.Now;
            flashcard.Statistics.TotalReviews++;
        }

        public void ProcessIncorrectAnswer(Flashcard flashcard)
        {
            var config = _configService.GetConfiguration();
            var demotionRule = config.LeitnerBoxes.DemotionRules
                .FirstOrDefault(r => r.BoxNumber == flashcard.CurrentBox);

            // Reset streak
            flashcard.Statistics.Streak = 0;
            flashcard.Statistics.IncorrectAnswers++;

            // Check if card should be demoted
            if (demotionRule != null)
            {
                flashcard.CurrentBox = demotionRule.DemoteToBox;
            }
            else if (flashcard.CurrentBox > 0)
            {
                // Default demotion: go back to box 0
                flashcard.CurrentBox = 0;
            }

            // Update review date (sooner for incorrect answers)
            UpdateNextReviewDate(flashcard);
            flashcard.LastReviewed = DateTime.Now;
            flashcard.Statistics.TotalReviews++;
        }

        public void UpdateNextReviewDate(Flashcard flashcard)
        {
            var config = _configService.GetConfiguration();
            var boxInterval = config.ReviewScheduling.BoxIntervals
                .FirstOrDefault(i => i.BoxNumber == flashcard.CurrentBox);

            if (boxInterval != null)
            {
                flashcard.NextReviewDate = DateTime.Now.AddDays(boxInterval.IntervalDays);
            }
            else
            {
                // Default interval based on box number
                int intervalDays = (int)Math.Pow(2, flashcard.CurrentBox);
                flashcard.NextReviewDate = DateTime.Now.AddDays(intervalDays);
            }
        }

        public List<Flashcard> GetCardsDueForReview(Deck deck)
        {
            var now = DateTime.Now;
            return deck.Flashcards.Where(f =>
                f.IsActive &&
                (f.NextReviewDate == null || f.NextReviewDate <= now)
            ).ToList();
        }

        public List<Flashcard> GetCardsInBox(Deck deck, int boxNumber)
        {
            return deck.Flashcards.Where(f =>
                f.IsActive && f.CurrentBox == boxNumber
            ).ToList();
        }

        public List<Flashcard> GetNewCards(Deck deck, int maxNewCards = 20)
        {
            var config = _configService.GetConfiguration();
            var newCards = deck.Flashcards.Where(f =>
                f.IsActive &&
                f.CurrentBox == 0 &&
                f.Statistics.TotalReviews == 0
            ).Take(maxNewCards).ToList();

            return newCards;
        }

        public List<Flashcard> GetCardsForStudySession(Deck deck, StudyMode studyMode, int maxCards = 50)
        {
            var cards = new List<Flashcard>();
            var config = _configService.GetConfiguration();

            // Get cards due for review
            var dueCards = GetCardsDueForReview(deck);
            cards.AddRange(dueCards);

            // Add new cards if we haven't reached the limit
            if (cards.Count < maxCards)
            {
                var newCards = GetNewCards(deck, config.ReviewScheduling.MaxNewCardsPerDay);
                cards.AddRange(newCards);
            }

            // Limit total cards
            if (cards.Count > maxCards)
            {
                cards = cards.Take(maxCards).ToList();
            }

            // Shuffle if configured
            if (config.StudySession.ShuffleCards)
            {
                var random = new Random();
                cards = cards.OrderBy(x => random.Next()).ToList();
            }

            return cards;
        }

        public Dictionary<int, int> GetBoxStatistics(Deck deck)
        {
            var config = _configService.GetConfiguration();
            var boxStats = new Dictionary<int, int>();

            for (int i = 0; i < config.LeitnerBoxes.NumberOfBoxes; i++)
            {
                boxStats[i] = GetCardsInBox(deck, i).Count;
            }

            return boxStats;
        }

        public StudySessionStatistics CalculateStudySessionStatistics(List<Flashcard> studiedCards, TimeSpan sessionTime)
        {
            var stats = new StudySessionStatistics
            {
                TotalCards = studiedCards.Count,
                SessionTime = sessionTime,
                StartTime = DateTime.Now - sessionTime,
                EndTime = DateTime.Now
            };

            foreach (var card in studiedCards)
            {
                if (card.Statistics.TotalReviews > 0)
                {
                    stats.TotalReviews++;
                    stats.CorrectAnswers += card.Statistics.CorrectAnswers;
                    stats.IncorrectAnswers += card.Statistics.IncorrectAnswers;
                    stats.TotalStudyTime += card.Statistics.TotalStudyTime;
                }
            }

            stats.SuccessRate = stats.TotalReviews > 0 ? (double)stats.CorrectAnswers / stats.TotalReviews * 100 : 0;
            stats.AverageResponseTime = stats.TotalReviews > 0 ? stats.TotalStudyTime.TotalSeconds / stats.TotalReviews : 0;

            return stats;
        }

        public void UpdateFlashcardStatistics(Flashcard flashcard, TimeSpan responseTime, bool isCorrect)
        {
            flashcard.Statistics.TotalStudyTime += responseTime;
            flashcard.Statistics.LastStudySession = DateTime.Now;

            // Update average response time
            if (flashcard.Statistics.TotalReviews > 0)
            {
                flashcard.Statistics.AverageResponseTime =
                    (flashcard.Statistics.AverageResponseTime * (flashcard.Statistics.TotalReviews - 1) + responseTime.TotalSeconds)
                    / flashcard.Statistics.TotalReviews;
            }
            else
            {
                flashcard.Statistics.AverageResponseTime = responseTime.TotalSeconds;
            }
        }

        public bool IsCardDueForReview(Flashcard flashcard)
        {
            return flashcard.NextReviewDate == null || flashcard.NextReviewDate <= DateTime.Now;
        }

        public int GetDaysUntilNextReview(Flashcard flashcard)
        {
            if (flashcard.NextReviewDate == null)
                return 0;

            var days = (flashcard.NextReviewDate.Value - DateTime.Now).Days;
            return Math.Max(0, days);
        }
    }

    public class StudySessionStatistics
    {
        public int TotalCards { get; set; }
        public int TotalReviews { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
        public double SuccessRate { get; set; }
        public TimeSpan SessionTime { get; set; }
        public TimeSpan TotalStudyTime { get; set; }
        public double AverageResponseTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
