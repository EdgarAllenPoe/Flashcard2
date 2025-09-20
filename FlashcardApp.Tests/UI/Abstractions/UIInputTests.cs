using FluentAssertions;
using FlashcardApp.UI.Abstractions;
using Moq;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Abstractions
{
    public class UIInputTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public async Task IUIInput_ShouldHaveRequiredMethods()
        {
            // Arrange
            var mockInput = new Mock<IUIInput>();

            // Act & Assert - Verify all required methods exist
            mockInput.Setup(x => x.ReadLineAsync()).ReturnsAsync("test");
            mockInput.Setup(x => x.ReadKeyAsync(It.IsAny<bool>())).ReturnsAsync(new UIKeyInfo(UIKey.A, 'a'));
            mockInput.Setup(x => x.GetImmediateChoiceAsync()).ReturnsAsync("1");
            mockInput.Setup(x => x.GetUserChoiceAsync()).ReturnsAsync("test");
            mockInput.Setup(x => x.WaitForAnyKeyAsync()).Returns(Task.CompletedTask);

            // All methods should be callable without throwing
            var result1 = await mockInput.Object.ReadLineAsync();
            var result2 = await mockInput.Object.ReadKeyAsync();
            var result3 = await mockInput.Object.GetImmediateChoiceAsync();
            var result4 = await mockInput.Object.GetUserChoiceAsync();
            await mockInput.Object.WaitForAnyKeyAsync();

            result1.Should().Be("test");
            result2.Key.Should().Be(UIKey.A);
            result3.Should().Be("1");
            result4.Should().Be("test");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public async Task ReadLineAsync_ShouldReturnString()
        {
            // Arrange
            var mockInput = new Mock<IUIInput>();
            var expectedInput = "test input";
            mockInput.Setup(x => x.ReadLineAsync()).ReturnsAsync(expectedInput);

            // Act
            var result = await mockInput.Object.ReadLineAsync();

            // Assert
            result.Should().Be(expectedInput);
            mockInput.Verify(x => x.ReadLineAsync(), Times.Once);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public async Task ReadKeyAsync_ShouldReturnUIKeyInfo()
        {
            // Arrange
            var mockInput = new Mock<IUIInput>();
            var expectedKeyInfo = new UIKeyInfo(UIKey.Enter, '\r');
            mockInput.Setup(x => x.ReadKeyAsync(It.IsAny<bool>())).ReturnsAsync(expectedKeyInfo);

            // Act
            var result = await mockInput.Object.ReadKeyAsync();

            // Assert
            result.Should().Be(expectedKeyInfo);
            mockInput.Verify(x => x.ReadKeyAsync(false), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ReadKeyAsync_ShouldRespectInterceptParameter(bool intercept)
        {
            // Arrange
            var mockInput = new Mock<IUIInput>();
            var expectedKeyInfo = new UIKeyInfo(UIKey.Space, ' ');
            mockInput.Setup(x => x.ReadKeyAsync(intercept)).ReturnsAsync(expectedKeyInfo);

            // Act
            var result = await mockInput.Object.ReadKeyAsync(intercept);

            // Assert
            result.Should().Be(expectedKeyInfo);
            mockInput.Verify(x => x.ReadKeyAsync(intercept), Times.Once);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public async Task GetImmediateChoiceAsync_ShouldReturnString()
        {
            // Arrange
            var mockInput = new Mock<IUIInput>();
            var expectedChoice = "1";
            mockInput.Setup(x => x.GetImmediateChoiceAsync()).ReturnsAsync(expectedChoice);

            // Act
            var result = await mockInput.Object.GetImmediateChoiceAsync();

            // Assert
            result.Should().Be(expectedChoice);
            mockInput.Verify(x => x.GetImmediateChoiceAsync(), Times.Once);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public async Task GetUserChoiceAsync_ShouldReturnString()
        {
            // Arrange
            var mockInput = new Mock<IUIInput>();
            var expectedChoice = "test choice";
            mockInput.Setup(x => x.GetUserChoiceAsync()).ReturnsAsync(expectedChoice);

            // Act
            var result = await mockInput.Object.GetUserChoiceAsync();

            // Assert
            result.Should().Be(expectedChoice);
            mockInput.Verify(x => x.GetUserChoiceAsync(), Times.Once);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public async Task WaitForAnyKeyAsync_ShouldComplete()
        {
            // Arrange
            var mockInput = new Mock<IUIInput>();
            mockInput.Setup(x => x.WaitForAnyKeyAsync()).Returns(Task.CompletedTask);

            // Act
            await mockInput.Object.WaitForAnyKeyAsync();

            // Assert
            mockInput.Verify(x => x.WaitForAnyKeyAsync(), Times.Once);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void UIKeyInfo_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var keyInfo = new UIKeyInfo(UIKey.Enter, '\r', true);

            // Assert
            keyInfo.Key.Should().Be(UIKey.Enter);
            keyInfo.KeyChar.Should().Be('\r');
            keyInfo.HasKeyChar.Should().BeTrue();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void UIKeyInfo_ShouldHandleNoKeyChar()
        {
            // Arrange & Act
            var keyInfo = new UIKeyInfo(UIKey.F1, '\0', false);

            // Assert
            keyInfo.Key.Should().Be(UIKey.F1);
            keyInfo.KeyChar.Should().Be('\0');
            keyInfo.HasKeyChar.Should().BeFalse();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void UIKey_ShouldHaveAllExpectedValues()
        {
            // Arrange & Act
            var keyValues = Enum.GetValues<UIKey>();

            // Assert - Letters
            keyValues.Should().Contain(UIKey.A);
            keyValues.Should().Contain(UIKey.Z);

            // Assert - Numbers
            keyValues.Should().Contain(UIKey.D0);
            keyValues.Should().Contain(UIKey.D9);

            // Assert - Special keys
            keyValues.Should().Contain(UIKey.Enter);
            keyValues.Should().Contain(UIKey.Escape);
            keyValues.Should().Contain(UIKey.Space);
            keyValues.Should().Contain(UIKey.Backspace);
            keyValues.Should().Contain(UIKey.Delete);
            keyValues.Should().Contain(UIKey.Tab);

            // Assert - Arrow keys
            keyValues.Should().Contain(UIKey.UpArrow);
            keyValues.Should().Contain(UIKey.DownArrow);
            keyValues.Should().Contain(UIKey.LeftArrow);
            keyValues.Should().Contain(UIKey.RightArrow);

            // Assert - Function keys
            keyValues.Should().Contain(UIKey.F1);
            keyValues.Should().Contain(UIKey.F12);

            // Assert - Navigation keys
            keyValues.Should().Contain(UIKey.Home);
            keyValues.Should().Contain(UIKey.End);
            keyValues.Should().Contain(UIKey.PageUp);
            keyValues.Should().Contain(UIKey.PageDown);

            // Assert - Other
            keyValues.Should().Contain(UIKey.Unknown);
        }
    }
}
