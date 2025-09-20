using FluentAssertions;
using FlashcardApp.Models;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.Models
{
    public class ConfigurationTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void AppConfiguration_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var config = new AppConfiguration();

            // Assert
            config.LeitnerBoxes.Should().NotBeNull();
            config.StudySession.Should().NotBeNull();
            config.FilePaths.Should().NotBeNull();
            config.ReviewScheduling.Should().NotBeNull();
            config.DailyLimits.Should().NotBeNull();
            config.UI.Should().NotBeNull();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void LeitnerBoxConfiguration_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var config = new LeitnerBoxConfiguration();

            // Assert
            config.NumberOfBoxes.Should().Be(5);
            config.PromotionRules.Should().NotBeNull();
            config.PromotionRules.Should().HaveCount(5);
            config.DemotionRules.Should().NotBeNull();
            config.DemotionRules.Should().HaveCount(4);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void PromotionRules_ShouldHaveCorrectDefaultValues()
        {
            // Act
            var config = new LeitnerBoxConfiguration();

            // Assert
            config.PromotionRules[0].BoxNumber.Should().Be(0);
            config.PromotionRules[0].CorrectAnswersNeeded.Should().Be(1);

            config.PromotionRules[1].BoxNumber.Should().Be(1);
            config.PromotionRules[1].CorrectAnswersNeeded.Should().Be(2);

            config.PromotionRules[2].BoxNumber.Should().Be(2);
            config.PromotionRules[2].CorrectAnswersNeeded.Should().Be(3);

            config.PromotionRules[3].BoxNumber.Should().Be(3);
            config.PromotionRules[3].CorrectAnswersNeeded.Should().Be(4);

            config.PromotionRules[4].BoxNumber.Should().Be(4);
            config.PromotionRules[4].CorrectAnswersNeeded.Should().Be(5);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DemotionRules_ShouldHaveCorrectDefaultValues()
        {
            // Act
            var config = new LeitnerBoxConfiguration();

            // Assert
            config.DemotionRules[0].BoxNumber.Should().Be(1);
            config.DemotionRules[0].IncorrectAnswersNeeded.Should().Be(1);
            config.DemotionRules[0].DemoteToBox.Should().Be(0);

            config.DemotionRules[1].BoxNumber.Should().Be(2);
            config.DemotionRules[1].IncorrectAnswersNeeded.Should().Be(1);
            config.DemotionRules[1].DemoteToBox.Should().Be(0);

            config.DemotionRules[2].BoxNumber.Should().Be(3);
            config.DemotionRules[2].IncorrectAnswersNeeded.Should().Be(1);
            config.DemotionRules[2].DemoteToBox.Should().Be(1);

            config.DemotionRules[3].BoxNumber.Should().Be(4);
            config.DemotionRules[3].IncorrectAnswersNeeded.Should().Be(1);
            config.DemotionRules[3].DemoteToBox.Should().Be(2);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionConfiguration_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var config = new StudySessionConfiguration();

            // Assert
            config.DefaultStudyMode.Should().Be(StudyMode.FrontToBack);
            config.ShowStatistics.Should().BeTrue();
            config.AutoAdvance.Should().BeFalse();
            config.AutoAdvanceDelay.Should().Be(3);
            config.ShuffleCards.Should().BeTrue();
            config.ShowProgress.Should().BeTrue();
            config.KeyboardShortcuts.Should().NotBeNull();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void KeyboardShortcuts_ShouldHaveCorrectDefaultValues()
        {
            // Act
            var config = new StudySessionConfiguration();

            // Assert
            config.KeyboardShortcuts.CorrectAnswer.Should().Be("1");
            config.KeyboardShortcuts.IncorrectAnswer.Should().Be("2");
            config.KeyboardShortcuts.ShowAnswer.Should().Be("space");
            config.KeyboardShortcuts.Quit.Should().Be("q");
            config.KeyboardShortcuts.Skip.Should().Be("s");
            config.KeyboardShortcuts.FlipCard.Should().Be("f");
            config.KeyboardShortcuts.ShowStatistics.Should().Be("t");
            config.KeyboardShortcuts.Help.Should().Be("h");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePathConfiguration_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var config = new FilePathConfiguration();

            // Assert
            config.DecksDirectory.Should().Be("decks");
            config.ConfigFileName.Should().Be("config.json");
            config.DeckFileExtension.Should().Be(".json");
            config.BackupDirectory.Should().Be("backups");
            config.ExportDirectory.Should().Be("exports");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ReviewSchedulingConfiguration_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var config = new ReviewSchedulingConfiguration();

            // Assert
            config.BoxIntervals.Should().NotBeNull();
            config.BoxIntervals.Should().HaveCount(5);
            config.NewCardInterval.Should().Be(1);
            config.MaxNewCardsPerDay.Should().Be(20);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void BoxIntervals_ShouldHaveCorrectDefaultValues()
        {
            // Act
            var config = new ReviewSchedulingConfiguration();

            // Assert
            config.BoxIntervals[0].BoxNumber.Should().Be(0);
            config.BoxIntervals[0].IntervalDays.Should().Be(1);

            config.BoxIntervals[1].BoxNumber.Should().Be(1);
            config.BoxIntervals[1].IntervalDays.Should().Be(3);

            config.BoxIntervals[2].BoxNumber.Should().Be(2);
            config.BoxIntervals[2].IntervalDays.Should().Be(7);

            config.BoxIntervals[3].BoxNumber.Should().Be(3);
            config.BoxIntervals[3].IntervalDays.Should().Be(14);

            config.BoxIntervals[4].BoxNumber.Should().Be(4);
            config.BoxIntervals[4].IntervalDays.Should().Be(30);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void DailyLimitsConfiguration_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var config = new DailyLimitsConfiguration();

            // Assert
            config.MaxCardsPerDay.Should().Be(100);
            config.MinCardsPerDay.Should().Be(5);
            config.MaxStudyTimePerDay.Should().Be(TimeSpan.FromHours(2));
            config.MinStudyTimePerDay.Should().Be(TimeSpan.FromMinutes(5));
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void UIConfiguration_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var config = new UIConfiguration();

            // Assert
            config.UseColors.Should().BeTrue();
            config.UseIcons.Should().BeTrue();
            config.ShowWelcomeMessage.Should().BeTrue();
            config.ClearScreenOnMenuChange.Should().BeTrue();
            config.ShowDetailedStatistics.Should().BeTrue();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(10)]
        public void NumberOfBoxes_ShouldAcceptValidValues(int numberOfBoxes)
        {
            // Arrange
            var config = new LeitnerBoxConfiguration();

            // Act
            config.NumberOfBoxes = numberOfBoxes;

            // Assert
            config.NumberOfBoxes.Should().Be(numberOfBoxes);
        }

        [Theory]
        [InlineData(StudyMode.FrontToBack)]
        [InlineData(StudyMode.BackToFront)]
        [InlineData(StudyMode.Mixed)]
        public void DefaultStudyMode_ShouldAcceptValidValues(StudyMode studyMode)
        {
            // Arrange
            var config = new StudySessionConfiguration();

            // Act
            config.DefaultStudyMode = studyMode;

            // Assert
            config.DefaultStudyMode.Should().Be(studyMode);
        }
    }
}
