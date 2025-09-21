using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for Daily Limits configuration section in Configuration page
    /// </summary>
    public class WinUIConfigurationDailyLimitsTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDailyLimitsSection_WhenDailyLimitsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Daily Limits section
            configurationPageXamlContent.Should().Contain("Daily Limits", "Should have Daily Limits section");
            configurationPageXamlContent.Should().Contain("x:Name=\"MaxCardsPerDaySlider\"", "Should have Max Cards Per Day slider");
            configurationPageXamlContent.Should().Contain("Header=\"Maximum cards per day\"", "Should have proper header for cards slider");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDailyLimitsControls_WhenDailyLimitsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Daily Limits controls
            configurationPageXamlContent.Should().Contain("Maximum=\"100\"", "Should have maximum of 100 cards per day");
            configurationPageXamlContent.Should().Contain("Minimum=\"1\"", "Should have minimum of 1 card per day");
            configurationPageXamlContent.Should().Contain("Value=\"20\"", "Should have default value of 20 cards per day");
            configurationPageXamlContent.Should().Contain("x:Name=\"MaxStudyTimeSlider\"", "Should have Max Study Time slider");
            configurationPageXamlContent.Should().Contain("Header=\"Maximum study time (minutes)\"", "Should have proper header for study time slider");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveStudyTimeSettings_WhenDailyLimitsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Study time settings
            configurationPageXamlContent.Should().Contain("Maximum=\"120\"", "Should have maximum of 120 minutes study time");
            configurationPageXamlContent.Should().Contain("Minimum=\"5\"", "Should have minimum of 5 minutes study time");
            configurationPageXamlContent.Should().Contain("Value=\"30\"", "Should have default value of 30 minutes study time");
            configurationPageXamlContent.Should().Contain("x:Name=\"BreakIntervalSlider\"", "Should have Break Interval slider");
            configurationPageXamlContent.Should().Contain("Header=\"Break interval (minutes)\"", "Should have proper header for break interval");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveBreakIntervalSettings_WhenDailyLimitsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Break interval settings
            configurationPageXamlContent.Should().Contain("Maximum=\"60\"", "Should have maximum of 60 minutes break interval");
            configurationPageXamlContent.Should().Contain("Minimum=\"1\"", "Should have minimum of 1 minute break interval");
            configurationPageXamlContent.Should().Contain("Value=\"5\"", "Should have default value of 5 minutes break interval");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDailyLimitsDataBinding_WhenDailyLimitsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Data binding for Daily Limits
            configurationPageCodeBehindContent.Should().Contain("DailyLimits.MaxCardsPerDay", "Should bind to DailyLimits.MaxCardsPerDay");
            configurationPageCodeBehindContent.Should().Contain("DailyLimits.MaxStudyTimePerDay", "Should bind to DailyLimits.MaxStudyTimePerDay");
            configurationPageCodeBehindContent.Should().Contain("BreakIntervalSlider.Value = 5", "Should set default break interval value");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDailyLimitsValidation_WhenDailyLimitsConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Daily Limits validation
            configurationPageCodeBehindContent.Should().Contain("MaxCardsPerDaySlider", "Should reference MaxCardsPerDaySlider in code-behind");
            configurationPageCodeBehindContent.Should().Contain("MaxStudyTimeSlider", "Should reference MaxStudyTimeSlider in code-behind");
            configurationPageCodeBehindContent.Should().Contain("BreakIntervalSlider", "Should reference BreakIntervalSlider in code-behind");
        }
    }
}
