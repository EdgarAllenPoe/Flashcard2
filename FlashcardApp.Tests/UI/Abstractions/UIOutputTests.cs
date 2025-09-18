using FluentAssertions;
using FlashcardApp.UI.Abstractions;
using Moq;
using Xunit;

namespace FlashcardApp.Tests.UI.Abstractions
{
    public class UIOutputTests
    {
        [Fact]
        public void IUIOutput_ShouldHaveRequiredMethods()
        {
            // Arrange
            var mockOutput = new Mock<IUIOutput>();

            // Act & Assert - Verify all required methods exist
            mockOutput.Setup(x => x.WriteLine(It.IsAny<string>()));
            mockOutput.Setup(x => x.Write(It.IsAny<string>()));
            mockOutput.Setup(x => x.Clear());
            mockOutput.Setup(x => x.SetForegroundColor(It.IsAny<UIColor>()));
            mockOutput.Setup(x => x.ResetColor());
            mockOutput.Setup(x => x.SetTitle(It.IsAny<string>()));
            mockOutput.Setup(x => x.SetCursorVisible(It.IsAny<bool>()));

            // All methods should be callable without throwing
            mockOutput.Object.WriteLine("test");
            mockOutput.Object.Write("test");
            mockOutput.Object.Clear();
            mockOutput.Object.SetForegroundColor(UIColor.Red);
            mockOutput.Object.ResetColor();
            mockOutput.Object.SetTitle("test");
            mockOutput.Object.SetCursorVisible(true);
        }

        [Theory]
        [InlineData("Hello World")]
        [InlineData("")]
        [InlineData("Special characters: !@#$%^&*()")]
        [InlineData("Unicode: 你好世界 🌍")]
        public void WriteLine_ShouldAcceptVariousStrings(string input)
        {
            // Arrange
            var mockOutput = new Mock<IUIOutput>();

            // Act
            mockOutput.Object.WriteLine(input);

            // Assert
            mockOutput.Verify(x => x.WriteLine(input), Times.Once);
        }

        [Theory]
        [InlineData("Hello World")]
        [InlineData("")]
        [InlineData("Special characters: !@#$%^&*()")]
        [InlineData("Unicode: 你好世界 🌍")]
        public void Write_ShouldAcceptVariousStrings(string input)
        {
            // Arrange
            var mockOutput = new Mock<IUIOutput>();

            // Act
            mockOutput.Object.Write(input);

            // Assert
            mockOutput.Verify(x => x.Write(input), Times.Once);
        }

        [Theory]
        [InlineData(UIColor.Red)]
        [InlineData(UIColor.Green)]
        [InlineData(UIColor.Blue)]
        [InlineData(UIColor.Default)]
        [InlineData(UIColor.DarkGray)]
        public void SetForegroundColor_ShouldAcceptAllColors(UIColor color)
        {
            // Arrange
            var mockOutput = new Mock<IUIOutput>();

            // Act
            mockOutput.Object.SetForegroundColor(color);

            // Assert
            mockOutput.Verify(x => x.SetForegroundColor(color), Times.Once);
        }

        [Theory]
        [InlineData("Test Title")]
        [InlineData("")]
        [InlineData("Very Long Title That Might Exceed Normal Limits")]
        [InlineData("Special Characters: !@#$%^&*()")]
        public void SetTitle_ShouldAcceptVariousTitles(string title)
        {
            // Arrange
            var mockOutput = new Mock<IUIOutput>();

            // Act
            mockOutput.Object.SetTitle(title);

            // Assert
            mockOutput.Verify(x => x.SetTitle(title), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetCursorVisible_ShouldAcceptBooleanValues(bool visible)
        {
            // Arrange
            var mockOutput = new Mock<IUIOutput>();

            // Act
            mockOutput.Object.SetCursorVisible(visible);

            // Assert
            mockOutput.Verify(x => x.SetCursorVisible(visible), Times.Once);
        }

        [Fact]
        public void UIColor_ShouldHaveAllExpectedValues()
        {
            // Arrange & Act
            var colorValues = Enum.GetValues<UIColor>();

            // Assert
            colorValues.Should().Contain(UIColor.Default);
            colorValues.Should().Contain(UIColor.Black);
            colorValues.Should().Contain(UIColor.White);
            colorValues.Should().Contain(UIColor.Red);
            colorValues.Should().Contain(UIColor.Green);
            colorValues.Should().Contain(UIColor.Blue);
            colorValues.Should().Contain(UIColor.Yellow);
            colorValues.Should().Contain(UIColor.Cyan);
            colorValues.Should().Contain(UIColor.Magenta);
            colorValues.Should().Contain(UIColor.DarkGray);
            colorValues.Should().Contain(UIColor.Gray);
            colorValues.Should().Contain(UIColor.DarkRed);
            colorValues.Should().Contain(UIColor.DarkGreen);
            colorValues.Should().Contain(UIColor.DarkBlue);
            colorValues.Should().Contain(UIColor.DarkYellow);
            colorValues.Should().Contain(UIColor.DarkCyan);
            colorValues.Should().Contain(UIColor.DarkMagenta);
        }
    }
}
