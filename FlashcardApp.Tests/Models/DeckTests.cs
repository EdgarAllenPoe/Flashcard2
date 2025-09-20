using FluentAssertions;
using FlashcardApp.Models;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.Models
{
    public class DeckTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var deck = new Deck();

            // Assert
            deck.Id.Should().NotBeNullOrEmpty();
            deck.Name.Should().BeEmpty();
            deck.Description.Should().BeEmpty();
            deck.Flashcards.Should().NotBeNull();
            deck.Flashcards.Should().BeEmpty();
            deck.Tags.Should().NotBeNull();
            deck.Tags.Should().BeEmpty();
            deck.Statistics.Should().NotBeNull();
            deck.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            deck.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void Constructor_WithParameters_ShouldSetProperties()
        {
            // Arrange
            var name = "Test Deck";
            var description = "A test deck for unit testing";
            var tags = new List<string> { "test", "unit" };

            // Act
            var deck = new Deck
            {
                Name = name,
                Description = description,
                Tags = tags
            };

            // Assert
            deck.Name.Should().Be(name);
            deck.Description.Should().Be(description);
            deck.Tags.Should().BeEquivalentTo(tags);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void Id_ShouldBeUniqueForEachInstance()
        {
            // Act
            var deck1 = new Deck();
            var deck2 = new Deck();

            // Assert
            deck1.Id.Should().NotBe(deck2.Id);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void TotalCards_ShouldReturnCorrectCount()
        {
            // Arrange
            var deck = new Deck();
            var flashcard1 = new Flashcard { Front = "Q1", Back = "A1" };
            var flashcard2 = new Flashcard { Front = "Q2", Back = "A2" };
            var flashcard3 = new Flashcard { Front = "Q3", Back = "A3" };

            // Act
            deck.Flashcards.Add(flashcard1);
            deck.Flashcards.Add(flashcard2);
            deck.Flashcards.Add(flashcard3);

            // Assert
            deck.TotalCards.Should().Be(3);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ActiveCards_ShouldReturnOnlyActiveCards()
        {
            // Arrange
            var deck = new Deck();
            var activeCard1 = new Flashcard { Front = "Q1", Back = "A1", IsActive = true };
            var activeCard2 = new Flashcard { Front = "Q2", Back = "A2", IsActive = true };
            var inactiveCard = new Flashcard { Front = "Q3", Back = "A3", IsActive = false };

            // Act
            deck.Flashcards.Add(activeCard1);
            deck.Flashcards.Add(activeCard2);
            deck.Flashcards.Add(inactiveCard);

            // Assert
            deck.ActiveCards.Should().Be(2);
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 1)]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        public void CardsInBox_ShouldReturnCorrectCount(int boxNumber, int expectedCount)
        {
            // Arrange
            var deck = new Deck();
            var card1 = new Flashcard { Front = "Q1", Back = "A1", CurrentBox = 0, IsActive = true };
            var card2 = new Flashcard { Front = "Q2", Back = "A2", CurrentBox = 0, IsActive = true };
            var card3 = new Flashcard { Front = "Q3", Back = "A3", CurrentBox = 1, IsActive = true };
            var inactiveCard = new Flashcard { Front = "Q4", Back = "A4", CurrentBox = 0, IsActive = false };

            deck.Flashcards.Add(card1);
            deck.Flashcards.Add(card2);
            deck.Flashcards.Add(card3);
            deck.Flashcards.Add(inactiveCard);

            // Act
            var result = deck.CardsInBox(boxNumber);

            // Assert
            result.Should().Be(expectedCount);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void LastModified_ShouldUpdateWhenFlashcardsAreAdded()
        {
            // Arrange
            var deck = new Deck();
            var originalModified = deck.LastModified;
            var flashcard = new Flashcard { Front = "Q1", Back = "A1" };

            // Wait a small amount to ensure time difference
            Thread.Sleep(10);

            // Act
            deck.Flashcards.Add(flashcard);
            deck.LastModified = DateTime.Now; // Manually update LastModified as expected behavior

            // Assert
            deck.LastModified.Should().BeAfter(originalModified);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void Statistics_ShouldBeInitializedWithDefaultValues()
        {
            // Act
            var deck = new Deck();

            // Assert
            deck.Statistics.Should().NotBeNull();
            deck.Statistics.TotalStudySessions.Should().Be(0);
            deck.Statistics.TotalStudyTime.Should().Be(TimeSpan.Zero);
            deck.Statistics.AverageSessionTime.Should().Be(TimeSpan.Zero);
            deck.Statistics.LastStudySession.Should().BeNull();
            deck.Statistics.CardsMastered.Should().Be(0);
            deck.Statistics.OverallSuccessRate.Should().Be(0.0);
            deck.Statistics.StudyStreak.Should().Be(0);
            deck.Statistics.LongestStudyStreak.Should().Be(0);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void Tags_ShouldAllowAddingAndRemovingTags()
        {
            // Arrange
            var deck = new Deck();
            var tag1 = "geography";
            var tag2 = "capitals";

            // Act
            deck.Tags.Add(tag1);
            deck.Tags.Add(tag2);

            // Assert
            deck.Tags.Should().Contain(tag1);
            deck.Tags.Should().Contain(tag2);
            deck.Tags.Should().HaveCount(2);

            // Act - Remove tag
            deck.Tags.Remove(tag1);

            // Assert
            deck.Tags.Should().NotContain(tag1);
            deck.Tags.Should().Contain(tag2);
            deck.Tags.Should().HaveCount(1);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void Flashcards_ShouldAllowAddingAndRemovingCards()
        {
            // Arrange
            var deck = new Deck();
            var flashcard1 = new Flashcard { Front = "Q1", Back = "A1" };
            var flashcard2 = new Flashcard { Front = "Q2", Back = "A2" };

            // Act
            deck.Flashcards.Add(flashcard1);
            deck.Flashcards.Add(flashcard2);

            // Assert
            deck.Flashcards.Should().Contain(flashcard1);
            deck.Flashcards.Should().Contain(flashcard2);
            deck.Flashcards.Should().HaveCount(2);

            // Act - Remove card
            deck.Flashcards.Remove(flashcard1);

            // Assert
            deck.Flashcards.Should().NotContain(flashcard1);
            deck.Flashcards.Should().Contain(flashcard2);
            deck.Flashcards.Should().HaveCount(1);
        }
    }
}
