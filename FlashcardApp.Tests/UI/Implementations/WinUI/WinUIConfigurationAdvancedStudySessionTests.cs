using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for Advanced Study Session Settings configuration section in Configuration page
    /// </summary>
    public class WinUIConfigurationAdvancedStudySessionTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveAdvancedStudySessionSection_WhenAdvancedStudySessionConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Advanced Study Session section
            configurationPageXamlContent.Should().Contain("Advanced Study Session", "Should have Advanced Study Session section");
            configurationPageXamlContent.Should().Contain("x:Name=\"AutoAdvanceCheckBox\"", "Should have Auto Advance checkbox");
            configurationPageXamlContent.Should().Contain("Content=\"Auto-advance to next card\"", "Should have proper content for auto advance checkbox");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveAutoAdvanceSettings_WhenAdvancedStudySessionConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Auto Advance settings
            configurationPageXamlContent.Should().Contain("x:Name=\"AutoAdvanceDelaySlider\"", "Should have Auto Advance Delay slider");
            configurationPageXamlContent.Should().Contain("Header=\"Auto-advance delay (seconds)\"", "Should have proper header for auto advance delay");
            configurationPageXamlContent.Should().Contain("Maximum=\"10\"", "Should have maximum of 10 seconds delay");
            configurationPageXamlContent.Should().Contain("Minimum=\"1\"", "Should have minimum of 1 second delay");
            configurationPageXamlContent.Should().Contain("Value=\"3\"", "Should have default value of 3 seconds delay");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveStatisticsSettings_WhenAdvancedStudySessionConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Statistics settings
            configurationPageXamlContent.Should().Contain("x:Name=\"ShowStatisticsCheckBox\"", "Should have Show Statistics checkbox");
            configurationPageXamlContent.Should().Contain("Content=\"Show statistics during study\"", "Should have proper content for show statistics checkbox");
            configurationPageXamlContent.Should().Contain("x:Name=\"ShowProgressCheckBox\"", "Should have Show Progress checkbox");
            configurationPageXamlContent.Should().Contain("Content=\"Show progress indicators\"", "Should have proper content for show progress checkbox");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveShuffleSettings_WhenAdvancedStudySessionConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Shuffle settings
            configurationPageXamlContent.Should().Contain("x:Name=\"ShuffleCardsCheckBox\"", "Should have Shuffle Cards checkbox");
            configurationPageXamlContent.Should().Contain("Content=\"Shuffle cards before study\"", "Should have proper content for shuffle cards checkbox");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveAdvancedStudySessionDataBinding_WhenAdvancedStudySessionConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Data binding for Advanced Study Session
            configurationPageCodeBehindContent.Should().Contain("StudySession.AutoAdvance", "Should bind to StudySession.AutoAdvance");
            configurationPageCodeBehindContent.Should().Contain("StudySession.AutoAdvanceDelay", "Should bind to StudySession.AutoAdvanceDelay");
            configurationPageCodeBehindContent.Should().Contain("StudySession.ShowStatistics", "Should bind to StudySession.ShowStatistics");
            configurationPageCodeBehindContent.Should().Contain("StudySession.ShowProgress", "Should bind to StudySession.ShowProgress");
            configurationPageCodeBehindContent.Should().Contain("StudySession.ShuffleCards", "Should bind to StudySession.ShuffleCards");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveAdvancedStudySessionValidation_WhenAdvancedStudySessionConfigIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Advanced Study Session validation
            configurationPageCodeBehindContent.Should().Contain("AutoAdvanceCheckBox", "Should reference AutoAdvanceCheckBox in code-behind");
            configurationPageCodeBehindContent.Should().Contain("AutoAdvanceDelaySlider", "Should reference AutoAdvanceDelaySlider in code-behind");
            configurationPageCodeBehindContent.Should().Contain("ShowStatisticsCheckBox", "Should reference ShowStatisticsCheckBox in code-behind");
            configurationPageCodeBehindContent.Should().Contain("ShowProgressCheckBox", "Should reference ShowProgressCheckBox in code-behind");
            configurationPageCodeBehindContent.Should().Contain("ShuffleCardsCheckBox", "Should reference ShuffleCardsCheckBox in code-behind");
        }
    }
}
