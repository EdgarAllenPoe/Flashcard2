using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for basic UI structure in Configuration page
    /// </summary>
    public class WinUIConfigurationBasicUITests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveActionButtons_WhenBasicUIIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Action buttons
            configurationPageXamlContent.Should().Contain("x:Name=\"SaveButton\"", "Should have Save button");
            configurationPageXamlContent.Should().Contain("x:Name=\"ResetButton\"", "Should have Reset button");
            configurationPageXamlContent.Should().Contain("Click=\"SaveButton_Click\"", "Should have Save button click handler");
            configurationPageXamlContent.Should().Contain("Click=\"ResetButton_Click\"", "Should have Reset button click handler");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveBasicConfigurationSection_WhenBasicUIIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Basic configuration section
            configurationPageXamlContent.Should().Contain("Study Session", "Should have Study Session section");
            configurationPageXamlContent.Should().Contain("x:Name=\"StudyModeComboBox\"", "Should have Study Mode ComboBox");
            configurationPageXamlContent.Should().Contain("Header=\"Default Study Mode\"", "Should have Study Mode header");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveCodeBehindEventHandlers_WhenBasicUIIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Event handlers
            configurationPageCodeBehindContent.Should().Contain("SaveButton_Click", "Should have Save button click handler");
            configurationPageCodeBehindContent.Should().Contain("ResetButton_Click", "Should have Reset button click handler");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveStatusUpdates_WhenBasicUIIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Status updates
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus", "Should have UpdateStatus method");
            configurationPageCodeBehindContent.Should().Contain("StatusText.Text", "Should update StatusText");
        }
    }
}
