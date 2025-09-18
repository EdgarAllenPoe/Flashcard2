using FluentAssertions;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.UI.Implementations.Console;
using Xunit;

namespace FlashcardApp.Tests.UI.Implementations.Console
{
    public class ConsoleThemeTests
    {
        [Fact]
        public void ConsoleTheme_ShouldImplementIUITheme()
        {
            // Arrange & Act
            var consoleTheme = new ConsoleTheme();

            // Assert
            consoleTheme.Should().BeAssignableTo<IUITheme>();
        }

        [Fact]
        public void ConsoleTheme_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var consoleTheme = new ConsoleTheme();

            // Assert
            consoleTheme.UseColors.Should().BeTrue();
            consoleTheme.UseIcons.Should().BeFalse();
        }

        [Theory]
        [InlineData(MessageType.Success, UIColor.Green)]
        [InlineData(MessageType.Error, UIColor.Red)]
        [InlineData(MessageType.Info, UIColor.Cyan)]
        [InlineData(MessageType.Warning, UIColor.Yellow)]
        [InlineData(MessageType.Default, UIColor.White)]
        public void GetColorForMessageType_ShouldReturnCorrectColors(MessageType messageType, UIColor expectedColor)
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();

            // Act
            var result = consoleTheme.GetColorForMessageType(messageType);

            // Assert
            result.Should().Be(expectedColor);
        }

        [Theory]
        [InlineData(MessageType.Success, "✓")]
        [InlineData(MessageType.Error, "✗")]
        [InlineData(MessageType.Info, "ℹ")]
        [InlineData(MessageType.Warning, "⚠")]
        [InlineData(MessageType.Default, "")]
        public void GetIconForMessageType_ShouldReturnCorrectIcons(MessageType messageType, string expectedIcon)
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();
            consoleTheme.UseIcons = true;

            // Act
            var result = consoleTheme.GetIconForMessageType(messageType);

            // Assert
            result.Should().Be(expectedIcon);
        }

        [Fact]
        public void GetIconForMessageType_WhenUseIconsFalse_ShouldReturnEmptyString()
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();
            consoleTheme.UseIcons = false;

            // Act
            var result = consoleTheme.GetIconForMessageType(MessageType.Success);

            // Assert
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(SectionType.MainMenu, UIColor.Magenta)]
        [InlineData(SectionType.DeckManagement, UIColor.Blue)]
        [InlineData(SectionType.StudySession, UIColor.Green)]
        [InlineData(SectionType.Statistics, UIColor.Cyan)]
        [InlineData(SectionType.Configuration, UIColor.Yellow)]
        [InlineData(SectionType.Help, UIColor.Cyan)]
        [InlineData(SectionType.Welcome, UIColor.Cyan)]
        [InlineData(SectionType.SessionResults, UIColor.Green)]
        [InlineData(SectionType.DeckList, UIColor.Cyan)]
        [InlineData(SectionType.Default, UIColor.White)]
        public void GetColorForSection_ShouldReturnCorrectColors(SectionType sectionType, UIColor expectedColor)
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();

            // Act
            var result = consoleTheme.GetColorForSection(sectionType);

            // Assert
            result.Should().Be(expectedColor);
        }

        [Theory]
        [InlineData(SectionType.MainMenu, "📋")]
        [InlineData(SectionType.DeckManagement, "🗂")]
        [InlineData(SectionType.StudySession, "📚")]
        [InlineData(SectionType.Statistics, "📊")]
        [InlineData(SectionType.Configuration, "⚙")]
        [InlineData(SectionType.Help, "❓")]
        [InlineData(SectionType.Welcome, "👋")]
        [InlineData(SectionType.SessionResults, "🎯")]
        [InlineData(SectionType.DeckList, "📑")]
        [InlineData(SectionType.Default, "")]
        public void GetIconForSection_ShouldReturnCorrectIcons(SectionType sectionType, string expectedIcon)
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();
            consoleTheme.UseIcons = true;

            // Act
            var result = consoleTheme.GetIconForSection(sectionType);

            // Assert
            result.Should().Be(expectedIcon);
        }

        [Fact]
        public void GetIconForSection_WhenUseIconsFalse_ShouldReturnEmptyString()
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();
            consoleTheme.UseIcons = false;

            // Act
            var result = consoleTheme.GetIconForSection(SectionType.MainMenu);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetMenuOptionColor_ShouldReturnYellow()
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();

            // Act
            var result = consoleTheme.GetMenuOptionColor();

            // Assert
            result.Should().Be(UIColor.Yellow);
        }

        [Fact]
        public void GetMenuDescriptionColor_ShouldReturnDarkGray()
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();

            // Act
            var result = consoleTheme.GetMenuDescriptionColor();

            // Assert
            result.Should().Be(UIColor.DarkGray);
        }

        [Fact]
        public void GetInputPromptColor_ShouldReturnYellow()
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();

            // Act
            var result = consoleTheme.GetInputPromptColor();

            // Assert
            result.Should().Be(UIColor.Yellow);
        }

        [Fact]
        public void GetStatisticsColor_ShouldReturnCyan()
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();

            // Act
            var result = consoleTheme.GetStatisticsColor();

            // Assert
            result.Should().Be(UIColor.Cyan);
        }

        [Fact]
        public void UseColors_ShouldBeSettable()
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();

            // Act
            consoleTheme.UseColors = false;

            // Assert
            consoleTheme.UseColors.Should().BeFalse();
        }

        [Fact]
        public void UseIcons_ShouldBeSettable()
        {
            // Arrange
            var consoleTheme = new ConsoleTheme();

            // Act
            consoleTheme.UseIcons = true;

            // Assert
            consoleTheme.UseIcons.Should().BeTrue();
        }
    }
}
