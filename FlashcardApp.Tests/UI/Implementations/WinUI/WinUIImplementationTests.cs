using FluentAssertions;
using Xunit;
using FlashcardApp.UI.Abstractions;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to verify that WinUI implementations exist and can be instantiated
    /// This follows TDD principles by testing the contract rather than implementation details
    /// </summary>
    public class WinUIImplementationTests
    {
        [Fact]
        public void WinUITheme_ShouldImplementIUITheme()
        {
            // Arrange & Act
            var theme = CreateWinUITheme();

            // Assert
            theme.Should().NotBeNull();
            theme.Should().BeAssignableTo<IUITheme>();
        }

        [Fact]
        public void WinUITheme_ShouldProvideValidColors()
        {
            // Arrange
            var theme = CreateWinUITheme();

            // Act & Assert
            theme.GetColorForMessageType(MessageType.Info).Should().Be(UIColor.Blue);
            theme.GetColorForMessageType(MessageType.Success).Should().Be(UIColor.Green);
            theme.GetColorForMessageType(MessageType.Warning).Should().Be(UIColor.Yellow);
            theme.GetColorForMessageType(MessageType.Error).Should().Be(UIColor.Red);
            theme.GetColorForMessageType(MessageType.Default).Should().Be(UIColor.White);
        }

        [Fact]
        public void WinUITheme_ShouldProvideValidSectionColors()
        {
            // Arrange
            var theme = CreateWinUITheme();

            // Act & Assert
            theme.GetColorForSection(SectionType.MainMenu).Should().Be(UIColor.Cyan);
            theme.GetColorForSection(SectionType.StudySession).Should().Be(UIColor.Green);
            theme.GetColorForSection(SectionType.DeckManagement).Should().Be(UIColor.Yellow);
            theme.GetColorForSection(SectionType.Statistics).Should().Be(UIColor.Magenta);
            theme.GetColorForSection(SectionType.Configuration).Should().Be(UIColor.Blue);
            theme.GetColorForSection(SectionType.Help).Should().Be(UIColor.White);
        }

        [Fact]
        public void WinUITheme_ShouldProvideValidIcons()
        {
            // Arrange
            var theme = CreateWinUITheme();

            // Act & Assert
            theme.GetIconForMessageType(MessageType.Success).Should().NotBeNullOrEmpty();
            theme.GetIconForMessageType(MessageType.Error).Should().NotBeNullOrEmpty();
            theme.GetIconForMessageType(MessageType.Info).Should().NotBeNullOrEmpty();
            theme.GetIconForMessageType(MessageType.Warning).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void WinUITheme_ShouldProvideValidSectionIcons()
        {
            // Arrange
            var theme = CreateWinUITheme();

            // Act & Assert
            theme.GetIconForSection(SectionType.MainMenu).Should().NotBeNullOrEmpty();
            theme.GetIconForSection(SectionType.StudySession).Should().NotBeNullOrEmpty();
            theme.GetIconForSection(SectionType.DeckManagement).Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Creates a WinUI theme instance using reflection to avoid WinUI dependencies in tests
        /// This is a TDD approach that tests the contract without requiring WinUI assemblies
        /// </summary>
        private IUITheme CreateWinUITheme()
        {
            // Use reflection to create the WinUI theme without direct dependencies
            var winUIPath = Path.Combine("..", "..", "..", "..", "FlashcardApp.WinUI", "bin", "Debug", "net8.0-windows10.0.19041.0", "win-x64", "FlashcardApp.WinUI.dll");
            var assembly = System.Reflection.Assembly.LoadFrom(winUIPath);
            var type = assembly.GetType("FlashcardApp.WinUI.UI.Implementations.WinUI.WinUITheme");
            
            if (type == null)
            {
                throw new InvalidOperationException("WinUITheme class not found. Make sure FlashcardApp.WinUI is built.");
            }

            var instance = Activator.CreateInstance(type);
            if (instance is not IUITheme theme)
            {
                throw new InvalidOperationException("WinUITheme does not implement IUITheme interface.");
            }

            return theme;
        }
    }
}
