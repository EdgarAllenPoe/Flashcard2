using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for Leitner Box configuration section in Configuration page
    /// </summary>
    public class WinUIConfigurationLeitnerBoxTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveLeitnerBoxSection_WhenLeitnerBoxConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Leitner Box section
            configurationPageXamlContent.Should().Contain("Leitner Box Settings", "Should have Leitner Box Settings section");
            configurationPageXamlContent.Should().Contain("x:Name=\"NumberOfBoxesSlider\"", "Should have Number of Boxes slider");
            configurationPageXamlContent.Should().Contain("Header=\"Number of Leitner Boxes\"", "Should have proper header for boxes slider");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveLeitnerBoxControls_WhenLeitnerBoxConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Leitner Box controls
            configurationPageXamlContent.Should().Contain("Maximum=\"10\"", "Should have maximum of 10 boxes");
            configurationPageXamlContent.Should().Contain("Minimum=\"3\"", "Should have minimum of 3 boxes");
            configurationPageXamlContent.Should().Contain("Value=\"5\"", "Should have default value of 5 boxes");
            configurationPageXamlContent.Should().Contain("StepFrequency=\"1\"", "Should have step frequency of 1");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveReviewIntervalSettings_WhenLeitnerBoxConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Review interval settings
            configurationPageXamlContent.Should().Contain("Review Intervals", "Should have Review Intervals section");
            configurationPageXamlContent.Should().Contain("x:Name=\"ReviewIntervalTextBox\"", "Should have Review Interval text input");
            configurationPageXamlContent.Should().Contain("Header=\"Days between reviews\"", "Should have proper header for review intervals");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveLeitnerBoxValidation_WhenLeitnerBoxConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Leitner Box validation
            configurationPageCodeBehindContent.Should().Contain("NumberOfBoxesSlider", "Should reference NumberOfBoxesSlider in code-behind");
            configurationPageCodeBehindContent.Should().Contain("ReviewIntervalTextBox", "Should reference ReviewIntervalTextBox in code-behind");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveLeitnerBoxDataBinding_WhenLeitnerBoxConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Data binding for Leitner Box
            configurationPageCodeBehindContent.Should().Contain("LeitnerBoxes.NumberOfBoxes", "Should bind to LeitnerBoxes.NumberOfBoxes");
            configurationPageCodeBehindContent.Should().Contain("ReviewScheduling.BoxIntervals", "Should bind to ReviewScheduling.BoxIntervals");
        }
    }
}
