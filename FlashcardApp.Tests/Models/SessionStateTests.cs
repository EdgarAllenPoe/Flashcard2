using FluentAssertions;
using FlashcardApp.Models;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.Models
{
    public class SessionStateTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var sessionState = new SessionState();

            // Assert
            sessionState.DeckId.Should().BeEmpty();
            sessionState.IsActive.Should().BeTrue(); // Default is true
            sessionState.SessionStartTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            sessionState.LastSaveTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            sessionState.SessionStatistics.Should().NotBeNull();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void Constructor_WithParameters_ShouldSetProperties()
        {
            // Arrange
            var deckId = "test-deck-123";
            var isActive = true;
            var startTime = DateTime.Now.AddMinutes(-10);

            // Act
            var sessionState = new SessionState
            {
                DeckId = deckId,
                IsActive = isActive,
                SessionStartTime = startTime
            };

            // Assert
            sessionState.DeckId.Should().Be(deckId);
            sessionState.IsActive.Should().Be(isActive);
            sessionState.SessionStartTime.Should().Be(startTime);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void SessionStatistics_ShouldBeInitializedWithDefaultValues()
        {
            // Act
            var sessionState = new SessionState();

            // Assert
            sessionState.SessionStatistics.Should().NotBeNull();
            sessionState.SessionStatistics.TotalCards.Should().Be(0);
            sessionState.SessionStatistics.CardsStudied.Should().Be(0);
            sessionState.SessionStatistics.CorrectAnswers.Should().Be(0);
            sessionState.SessionStatistics.IncorrectAnswers.Should().Be(0);
            sessionState.SessionStatistics.TotalStudyTime.Should().Be(TimeSpan.Zero);
            sessionState.SessionStatistics.SessionStartTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            sessionState.SessionStatistics.LastActivityTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData(0, 0, 0.0)] // No cards studied
        [InlineData(10, 8, 80.0)] // 80% success rate
        [InlineData(5, 5, 100.0)] // 100% success rate
        [InlineData(4, 0, 0.0)] // 0% success rate
        [InlineData(3, 2, 66.67)] // ~66.67% success rate
        public void SuccessRate_ShouldCalculateCorrectly(int cardsStudied, int correctAnswers, double expectedRate)
        {
            // Arrange
            var sessionState = new SessionState
            {
                SessionStatistics = new SessionStatistics
                {
                    CardsStudied = cardsStudied,
                    CorrectAnswers = correctAnswers
                }
            };

            // Act
            var successRate = sessionState.SessionStatistics.SuccessRate;

            // Assert
            successRate.Should().BeApproximately(expectedRate, 0.01);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void IsActive_WhenSetToTrue_ShouldBeActive()
        {
            // Arrange
            var sessionState = new SessionState();

            // Act
            sessionState.IsActive = true;

            // Assert
            sessionState.IsActive.Should().BeTrue();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void SessionStartTime_WhenSet_ShouldUpdateCorrectly()
        {
            // Arrange
            var sessionState = new SessionState();
            var startTime = DateTime.Now.AddHours(-2);

            // Act
            sessionState.SessionStartTime = startTime;

            // Assert
            sessionState.SessionStartTime.Should().Be(startTime);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void LastSaveTime_WhenSet_ShouldUpdateCorrectly()
        {
            // Arrange
            var sessionState = new SessionState();
            var saveTime = DateTime.Now.AddMinutes(-5);

            // Act
            sessionState.LastSaveTime = saveTime;

            // Assert
            sessionState.LastSaveTime.Should().Be(saveTime);
        }
    }
}
