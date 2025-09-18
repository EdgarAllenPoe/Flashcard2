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
    }
}