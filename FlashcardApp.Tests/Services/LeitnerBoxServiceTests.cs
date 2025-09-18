using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;

namespace FlashcardApp.Tests.Services
{
    public class LeitnerBoxServiceTests
    {
        private readonly LeitnerBoxService _service;
        private readonly AppConfiguration _config;
        private readonly string _testConfigPath;

        public LeitnerBoxServiceTests()
        {
            _testConfigPath = Path.Combine(Path.GetTempPath(), $"test_config_{Guid.NewGuid()}.json");
            _config = CreateDefaultConfiguration();
            
            File.WriteAllText(_testConfigPath, Newtonsoft.Json.JsonConvert.SerializeObject(_config));
            var configService = new ConfigurationService(_testConfigPath);
            _service = new LeitnerBoxService(configService);
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
                        new() { BoxNumber = 0, CorrectAnswersNeeded = 1 },
                        new() { BoxNumber = 1, CorrectAnswersNeeded = 2 },
                        new() { BoxNumber = 2, CorrectAnswersNeeded = 3 },
                        new() { BoxNumber = 3, CorrectAnswersNeeded = 4 },
                        new() { BoxNumber = 4, CorrectAnswersNeeded = 5 }
                    },
                    DemotionRules = new List<DemotionRule>
                    {
                        new() { BoxNumber = 1, IncorrectAnswersNeeded = 1, DemoteToBox = 0 },
                        new() { BoxNumber = 2, IncorrectAnswersNeeded = 1, DemoteToBox = 0 },
                        new() { BoxNumber = 3, IncorrectAnswersNeeded = 1, DemoteToBox = 1 },
                        new() { BoxNumber = 4, IncorrectAnswersNeeded = 1, DemoteToBox = 2 }
                    }
                },
                ReviewScheduling = new ReviewSchedulingConfiguration
                {
                    BoxIntervals = new List<BoxInterval>
                    {
                        new() { BoxNumber = 0, IntervalDays = 1 },
                        new() { BoxNumber = 1, IntervalDays = 3 },
                        new() { BoxNumber = 2, IntervalDays = 7 },
                        new() { BoxNumber = 3, IntervalDays = 14 },
                        new() { BoxNumber = 4, IntervalDays = 30 }
                    }
                }
            };
        }

        [Fact]
        public void ProcessCorrectAnswer_ShouldIncrementCorrectAnswers()
        {
            // Arrange
            var flashcard = new Flashcard { CurrentBox = 1 }; // Box 1 needs 2 correct answers, so won't be promoted

            // Act
            _service.ProcessCorrectAnswer(flashcard);

            // Assert
            flashcard.Statistics.CorrectAnswers.Should().Be(1);
            flashcard.Statistics.TotalReviews.Should().Be(1);
            flashcard.Statistics.Streak.Should().Be(1);
            flashcard.CurrentBox.Should().Be(1); // Should remain in box 1
        }

        [Fact]
        public void ProcessCorrectAnswer_ShouldPromoteCardWhenStreakReachesRequired()
        {
            // Arrange
            var flashcard = new Flashcard { CurrentBox = 0 };

            // Act
            _service.ProcessCorrectAnswer(flashcard);

            // Assert
            flashcard.CurrentBox.Should().Be(1);
            flashcard.Statistics.Streak.Should().Be(0); // Reset after promotion
        }

        [Fact]
        public void ProcessIncorrectAnswer_ShouldIncrementIncorrectAnswers()
        {
            // Arrange
            var flashcard = new Flashcard { CurrentBox = 1 };

            // Act
            _service.ProcessIncorrectAnswer(flashcard);

            // Assert
            flashcard.Statistics.IncorrectAnswers.Should().Be(1);
            flashcard.Statistics.TotalReviews.Should().Be(1);
            flashcard.Statistics.Streak.Should().Be(0);
        }

        [Fact]
        public void ProcessIncorrectAnswer_ShouldDemoteCardAccordingToRule()
        {
            // Arrange
            var flashcard = new Flashcard { CurrentBox = 1 };

            // Act
            _service.ProcessIncorrectAnswer(flashcard);

            // Assert
            flashcard.CurrentBox.Should().Be(0); // Should demote to box 0
        }

        [Fact]
        public void ProcessIncorrectAnswer_ShouldNotDemoteFromBox0()
        {
            // Arrange
            var flashcard = new Flashcard { CurrentBox = 0 };

            // Act
            _service.ProcessIncorrectAnswer(flashcard);

            // Assert
            flashcard.CurrentBox.Should().Be(0); // Should stay in box 0
        }

        [Fact]
        public void UpdateNextReviewDate_ShouldSetCorrectInterval()
        {
            // Arrange
            var flashcard = new Flashcard { CurrentBox = 1 };

            // Act
            _service.UpdateNextReviewDate(flashcard);

            // Assert
            flashcard.NextReviewDate.Should().NotBeNull();
            var expectedDate = DateTime.Now.AddDays(3); // Box 1 has 3-day interval
            flashcard.NextReviewDate.Should().BeCloseTo(expectedDate, TimeSpan.FromMinutes(1));
        }

        [Fact]
        public void GetCardsDueForReview_ShouldReturnOnlyDueCards()
        {
            // Arrange
            var deck = new Deck();
            var dueCard = new Flashcard 
            { 
                IsActive = true, 
                NextReviewDate = DateTime.Now.AddDays(-1) // Due yesterday
            };
            var notDueCard = new Flashcard 
            { 
                IsActive = true, 
                NextReviewDate = DateTime.Now.AddDays(1) // Due tomorrow
            };
            var inactiveCard = new Flashcard 
            { 
                IsActive = false, 
                NextReviewDate = DateTime.Now.AddDays(-1) // Due but inactive
            };

            deck.Flashcards.Add(dueCard);
            deck.Flashcards.Add(notDueCard);
            deck.Flashcards.Add(inactiveCard);

            // Act
            var result = _service.GetCardsDueForReview(deck);

            // Assert
            result.Should().Contain(dueCard);
            result.Should().NotContain(notDueCard);
            result.Should().NotContain(inactiveCard);
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetNewCards_ShouldReturnOnlyNewCards()
        {
            // Arrange
            var deck = new Deck();
            var newCard = new Flashcard 
            { 
                IsActive = true, 
                CurrentBox = 0, 
                Statistics = new FlashcardStatistics { TotalReviews = 0 }
            };
            var reviewedCard = new Flashcard 
            { 
                IsActive = true, 
                CurrentBox = 0, 
                Statistics = new FlashcardStatistics { TotalReviews = 1 }
            };
            var inactiveCard = new Flashcard 
            { 
                IsActive = false, 
                CurrentBox = 0, 
                Statistics = new FlashcardStatistics { TotalReviews = 0 }
            };

            deck.Flashcards.Add(newCard);
            deck.Flashcards.Add(reviewedCard);
            deck.Flashcards.Add(inactiveCard);

            // Act
            var result = _service.GetNewCards(deck, 10);

            // Assert
            result.Should().Contain(newCard);
            result.Should().NotContain(reviewedCard);
            result.Should().NotContain(inactiveCard);
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetBoxStatistics_ShouldReturnCorrectCounts()
        {
            // Arrange
            var deck = new Deck();
            var card1 = new Flashcard { IsActive = true, CurrentBox = 0 };
            var card2 = new Flashcard { IsActive = true, CurrentBox = 0 };
            var card3 = new Flashcard { IsActive = true, CurrentBox = 1 };
            var inactiveCard = new Flashcard { IsActive = false, CurrentBox = 0 };

            deck.Flashcards.Add(card1);
            deck.Flashcards.Add(card2);
            deck.Flashcards.Add(card3);
            deck.Flashcards.Add(inactiveCard);

            // Act
            var result = _service.GetBoxStatistics(deck);

            // Assert
            result[0].Should().Be(2); // 2 active cards in box 0
            result[1].Should().Be(1); // 1 active card in box 1
            result[2].Should().Be(0); // 0 active cards in box 2
            result[3].Should().Be(0); // 0 active cards in box 3
            result[4].Should().Be(0); // 0 active cards in box 4
        }

        #region Box Statistics Tests

        [Fact]
        public void GetBoxStatistics_WithInactiveCards_ShouldOnlyCountActiveCards()
        {
            // Arrange
            var deck = new Deck { Name = "Test Deck" };
            var card1 = new Flashcard { CurrentBox = 0, IsActive = true };
            var card2 = new Flashcard { CurrentBox = 1, IsActive = true };
            var card3 = new Flashcard { CurrentBox = 1, IsActive = true };
            var card4 = new Flashcard { CurrentBox = 2, IsActive = true };
            var card5 = new Flashcard { CurrentBox = 0, IsActive = false }; // Inactive card
            
            deck.Flashcards.AddRange(new[] { card1, card2, card3, card4, card5 });

            // Act
            var statistics = _service.GetBoxStatistics(deck);

            // Assert
            statistics.Should().HaveCount(5); // Default config has 5 boxes
            statistics[0].Should().Be(1); // Box 0 has 1 active card
            statistics[1].Should().Be(2); // Box 1 has 2 active cards
            statistics[2].Should().Be(1); // Box 2 has 1 active card
            statistics[3].Should().Be(0); // Box 3 has 0 active cards
            statistics[4].Should().Be(0); // Box 4 has 0 active cards
        }

        [Fact]
        public void GetBoxStatistics_EmptyDeck_ShouldReturnEmptyDictionary()
        {
            // Arrange
            var deck = new Deck { Name = "Empty Deck" };

            // Act
            var statistics = _service.GetBoxStatistics(deck);

            // Assert
            statistics.Should().HaveCount(5); // Default config has 5 boxes
            statistics[0].Should().Be(0); // All boxes should have 0 cards
            statistics[1].Should().Be(0);
            statistics[2].Should().Be(0);
            statistics[3].Should().Be(0);
            statistics[4].Should().Be(0);
        }

        #endregion

        #region Study Session Statistics Tests

        [Fact]
        public void CalculateStudySessionStatistics_ShouldCalculateCorrectly()
        {
            // Arrange
            var studiedCards = new List<Flashcard>
            {
                new Flashcard { Statistics = new FlashcardStatistics { TotalReviews = 1, CorrectAnswers = 1, IncorrectAnswers = 0 } },
                new Flashcard { Statistics = new FlashcardStatistics { TotalReviews = 1, CorrectAnswers = 0, IncorrectAnswers = 1 } },
                new Flashcard { Statistics = new FlashcardStatistics { TotalReviews = 1, CorrectAnswers = 1, IncorrectAnswers = 0 } }
            };
            var sessionTime = TimeSpan.FromMinutes(5);

            // Act
            var statistics = _service.CalculateStudySessionStatistics(studiedCards, sessionTime);

            // Assert
            statistics.TotalCards.Should().Be(3);
            statistics.TotalReviews.Should().Be(3); // All cards have TotalReviews > 0
            statistics.CorrectAnswers.Should().Be(2); // 1 + 0 + 1
            statistics.IncorrectAnswers.Should().Be(1); // 0 + 1 + 0
            statistics.SuccessRate.Should().BeApproximately(66.67, 0.01);
            statistics.SessionTime.Should().Be(sessionTime);
        }

        [Fact]
        public void CalculateStudySessionStatistics_EmptyList_ShouldReturnZeroStatistics()
        {
            // Arrange
            var studiedCards = new List<Flashcard>();
            var sessionTime = TimeSpan.FromMinutes(5);

            // Act
            var statistics = _service.CalculateStudySessionStatistics(studiedCards, sessionTime);

            // Assert
            statistics.TotalCards.Should().Be(0);
            statistics.CorrectAnswers.Should().Be(0);
            statistics.IncorrectAnswers.Should().Be(0);
            statistics.SuccessRate.Should().Be(0);
            statistics.SessionTime.Should().Be(sessionTime);
        }

        #endregion

        #region Flashcard Statistics Tests

        [Fact]
        public void UpdateFlashcardStatistics_ShouldUpdateCorrectly()
        {
            // Arrange
            var flashcard = new Flashcard
            {
                Statistics = new FlashcardStatistics
                {
                    TotalReviews = 5,
                    CorrectAnswers = 3,
                    IncorrectAnswers = 2,
                    AverageResponseTime = 2.0
                }
            };
            var responseTime = TimeSpan.FromSeconds(3);
            var isCorrect = true;

            // Act
            _service.UpdateFlashcardStatistics(flashcard, responseTime, isCorrect);

            // Assert
            flashcard.Statistics.TotalReviews.Should().Be(5); // Not incremented by this method
            flashcard.Statistics.CorrectAnswers.Should().Be(3); // Not incremented by this method
            flashcard.Statistics.IncorrectAnswers.Should().Be(2); // Not incremented by this method
            flashcard.Statistics.AverageResponseTime.Should().BeApproximately(2.2, 0.01);
        }

        [Fact]
        public void UpdateFlashcardStatistics_IncorrectAnswer_ShouldUpdateCorrectly()
        {
            // Arrange
            var flashcard = new Flashcard
            {
                Statistics = new FlashcardStatistics
                {
                    TotalReviews = 5,
                    CorrectAnswers = 3,
                    IncorrectAnswers = 2,
                    AverageResponseTime = 2.0
                }
            };
            var responseTime = TimeSpan.FromSeconds(4);
            var isCorrect = false;

            // Act
            _service.UpdateFlashcardStatistics(flashcard, responseTime, isCorrect);

            // Assert
            flashcard.Statistics.TotalReviews.Should().Be(5); // Not incremented by this method
            flashcard.Statistics.CorrectAnswers.Should().Be(3); // Not incremented by this method
            flashcard.Statistics.IncorrectAnswers.Should().Be(2); // Not incremented by this method
            flashcard.Statistics.AverageResponseTime.Should().BeApproximately(2.4, 0.01);
        }

        #endregion

        #region Review Date Tests

        [Fact]
        public void IsCardDueForReview_ShouldReturnTrueForDueCard()
        {
            // Arrange
            var flashcard = new Flashcard
            {
                NextReviewDate = DateTime.Now.AddMinutes(-1) // Due 1 minute ago
            };

            // Act
            var isDue = _service.IsCardDueForReview(flashcard);

            // Assert
            isDue.Should().BeTrue();
        }

        [Fact]
        public void IsCardDueForReview_ShouldReturnFalseForNotDueCard()
        {
            // Arrange
            var flashcard = new Flashcard
            {
                NextReviewDate = DateTime.Now.AddMinutes(1) // Due in 1 minute
            };

            // Act
            var isDue = _service.IsCardDueForReview(flashcard);

            // Assert
            isDue.Should().BeFalse();
        }

        [Fact]
        public void IsCardDueForReview_NullNextReviewDate_ShouldReturnTrue()
        {
            // Arrange
            var flashcard = new Flashcard
            {
                NextReviewDate = null
            };

            // Act
            var isDue = _service.IsCardDueForReview(flashcard);

            // Assert
            isDue.Should().BeTrue();
        }

        [Fact]
        public void GetDaysUntilNextReview_ShouldReturnCorrectDays()
        {
            // Arrange
            var flashcard = new Flashcard
            {
                NextReviewDate = DateTime.Now.AddDays(3)
            };

            // Act
            var days = _service.GetDaysUntilNextReview(flashcard);

            // Assert
            days.Should().BeInRange(2, 3); // Allow for timing differences
        }

        [Fact]
        public void GetDaysUntilNextReview_NullNextReviewDate_ShouldReturnZero()
        {
            // Arrange
            var flashcard = new Flashcard
            {
                NextReviewDate = null
            };

            // Act
            var days = _service.GetDaysUntilNextReview(flashcard);

            // Assert
            days.Should().Be(0);
        }

        [Fact]
        public void GetDaysUntilNextReview_PastDate_ShouldReturnZero()
        {
            // Arrange
            var flashcard = new Flashcard
            {
                NextReviewDate = DateTime.Now.AddDays(-2)
            };

            // Act
            var days = _service.GetDaysUntilNextReview(flashcard);

            // Assert
            days.Should().Be(0); // Math.Max(0, days) prevents negative values
        }

        #endregion
    }
}