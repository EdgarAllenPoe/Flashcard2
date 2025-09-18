using FluentAssertions;
using FlashcardApp.Models;
using Xunit;

namespace FlashcardApp.Tests.Models
{
    public class FlashcardTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var flashcard = new Flashcard();

            // Assert
            flashcard.Id.Should().NotBeNullOrEmpty();
            flashcard.Front.Should().BeEmpty();
            flashcard.Back.Should().BeEmpty();
            flashcard.Tags.Should().NotBeNull();
            flashcard.Tags.Should().BeEmpty();
            flashcard.Statistics.Should().NotBeNull();
            flashcard.CurrentBox.Should().Be(0);
            flashcard.IsActive.Should().BeTrue();
            flashcard.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            flashcard.LastReviewed.Should().BeNull();
            flashcard.NextReviewDate.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithParameters_ShouldSetProperties()
        {
            // Arrange
            var front = "What is the capital of France?";
            var back = "Paris";
            var tags = new List<string> { "geography", "capitals" };

            // Act
            var flashcard = new Flashcard
            {
                Front = front,
                Back = back,
                Tags = tags
            };

            // Assert
            flashcard.Front.Should().Be(front);
            flashcard.Back.Should().Be(back);
            flashcard.Tags.Should().BeEquivalentTo(tags);
        }

        [Fact]
        public void Id_ShouldBeUniqueForEachInstance()
        {
            // Act
            var flashcard1 = new Flashcard();
            var flashcard2 = new Flashcard();

            // Assert
            flashcard1.Id.Should().NotBe(flashcard2.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void CurrentBox_ShouldAcceptValidValues(int boxNumber)
        {
            // Arrange
            var flashcard = new Flashcard();

            // Act
            flashcard.CurrentBox = boxNumber;

            // Assert
            flashcard.CurrentBox.Should().Be(boxNumber);
        }

        [Fact]
        public void IsActive_WhenSetToFalse_ShouldBeInactive()
        {
            // Arrange
            var flashcard = new Flashcard();

            // Act
            flashcard.IsActive = false;

            // Assert
            flashcard.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Statistics_ShouldBeInitializedWithDefaultValues()
        {
            // Act
            var flashcard = new Flashcard();

            // Assert
            flashcard.Statistics.Should().NotBeNull();
            flashcard.Statistics.TotalReviews.Should().Be(0);
            flashcard.Statistics.CorrectAnswers.Should().Be(0);
            flashcard.Statistics.IncorrectAnswers.Should().Be(0);
            flashcard.Statistics.AverageResponseTime.Should().Be(0.0);
            flashcard.Statistics.TotalStudyTime.Should().Be(TimeSpan.Zero);
            flashcard.Statistics.LastStudySession.Should().BeNull();
            flashcard.Statistics.Streak.Should().Be(0);
            flashcard.Statistics.LongestStreak.Should().Be(0);
        }

        [Fact]
        public void Tags_ShouldAllowAddingAndRemovingTags()
        {
            // Arrange
            var flashcard = new Flashcard();
            var tag1 = "geography";
            var tag2 = "capitals";

            // Act
            flashcard.Tags.Add(tag1);
            flashcard.Tags.Add(tag2);

            // Assert
            flashcard.Tags.Should().Contain(tag1);
            flashcard.Tags.Should().Contain(tag2);
            flashcard.Tags.Should().HaveCount(2);

            // Act - Remove tag
            flashcard.Tags.Remove(tag1);

            // Assert
            flashcard.Tags.Should().NotContain(tag1);
            flashcard.Tags.Should().Contain(tag2);
            flashcard.Tags.Should().HaveCount(1);
        }

        [Fact]
        public void LastReviewed_WhenSet_ShouldUpdateCorrectly()
        {
            // Arrange
            var flashcard = new Flashcard();
            var reviewDate = DateTime.Now.AddDays(-1);

            // Act
            flashcard.LastReviewed = reviewDate;

            // Assert
            flashcard.LastReviewed.Should().Be(reviewDate);
        }

        [Fact]
        public void NextReviewDate_WhenSet_ShouldUpdateCorrectly()
        {
            // Arrange
            var flashcard = new Flashcard();
            var nextReview = DateTime.Now.AddDays(7);

            // Act
            flashcard.NextReviewDate = nextReview;

            // Assert
            flashcard.NextReviewDate.Should().Be(nextReview);
        }

        [Theory]
        [InlineData(0, 0, 0.0)] // No reviews
        [InlineData(10, 8, 80.0)] // 80% success rate
        [InlineData(5, 5, 100.0)] // 100% success rate
        [InlineData(4, 0, 0.0)] // 0% success rate
        [InlineData(3, 2, 66.67)] // ~66.67% success rate
        public void SuccessRate_ShouldCalculateCorrectly(int totalReviews, int correctAnswers, double expectedRate)
        {
            // Arrange
            var flashcard = new Flashcard
            {
                Statistics = new FlashcardStatistics
                {
                    TotalReviews = totalReviews,
                    CorrectAnswers = correctAnswers
                }
            };

            // Act
            var successRate = flashcard.Statistics.SuccessRate;

            // Assert
            successRate.Should().BeApproximately(expectedRate, 0.01);
        }
    }
}