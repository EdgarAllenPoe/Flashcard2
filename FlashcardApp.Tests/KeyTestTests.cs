using FluentAssertions;
using FlashcardApp;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests
{
    public class KeyTestTests
    {
        [Theory]
        [InlineData(ConsoleKey.Spacebar, ' ', "q", true, "Space bar should be valid")]
        [InlineData(ConsoleKey.A, 'a', "q", true, "Letter A should be valid")]
        [InlineData(ConsoleKey.D1, '1', "q", true, "Number 1 should be valid")]
        [InlineData(ConsoleKey.Enter, '\r', "q", true, "Enter key should be valid")]
        [InlineData(ConsoleKey.Escape, '\0', "q", false, "Escape key should be invalid")]
        [InlineData(ConsoleKey.Q, 'q', "q", false, "Quit key 'q' should be invalid")]
        [InlineData(ConsoleKey.Q, 'Q', "q", false, "Quit key 'Q' should be invalid")]
        [InlineData(ConsoleKey.B, 'b', "q", true, "Letter B should be valid when quit key is 'q'")]
        [InlineData(ConsoleKey.Q, 'q', "x", true, "Letter Q should be valid when quit key is 'x'")]
        [InlineData(ConsoleKey.X, 'x', "x", false, "Letter X should be invalid when quit key is 'x'")]
        [InlineData(ConsoleKey.X, 'X', "x", false, "Letter X (uppercase) should be invalid when quit key is 'x'")]
        public void IsValidKeyForAnswerReveal_ShouldValidateCorrectly(
            ConsoleKey key,
            char keyChar,
            string quitKey,
            bool expected,
            string description)
        {
            // Arrange
            var keyInfo = new ConsoleKeyInfo(keyChar, key, false, false, false);

            // Act
            var result = KeyTest.IsValidKeyForAnswerReveal(keyInfo, quitKey);

            // Assert
            result.Should().Be(expected, description);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void IsValidKeyForAnswerReveal_WithNullQuitKey_ShouldThrowArgumentNullException()
        {
            // Arrange
            var keyInfo = new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false);

            // Act & Assert
            var action = () => KeyTest.IsValidKeyForAnswerReveal(keyInfo, null!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void IsValidKeyForAnswerReveal_WithEmptyQuitKey_ShouldAllowAllKeys()
        {
            // Arrange
            var keyInfo = new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false);

            // Act
            var result = KeyTest.IsValidKeyForAnswerReveal(keyInfo, string.Empty);

            // Assert
            result.Should().BeTrue("Empty quit key should allow all keys");
        }

        [Theory]
        [InlineData(ConsoleKey.Tab, '\t', "q", true, "Tab key should be valid")]
        [InlineData(ConsoleKey.Backspace, '\b', "q", true, "Backspace key should be valid")]
        [InlineData(ConsoleKey.Delete, '\0', "q", true, "Delete key should be valid")]
        [InlineData(ConsoleKey.UpArrow, '\0', "q", true, "Up arrow key should be valid")]
        [InlineData(ConsoleKey.DownArrow, '\0', "q", true, "Down arrow key should be valid")]
        [InlineData(ConsoleKey.LeftArrow, '\0', "q", true, "Left arrow key should be valid")]
        [InlineData(ConsoleKey.RightArrow, '\0', "q", true, "Right arrow key should be valid")]
        public void IsValidKeyForAnswerReveal_WithSpecialKeys_ShouldValidateCorrectly(
            ConsoleKey key,
            char keyChar,
            string quitKey,
            bool expected,
            string description)
        {
            // Arrange
            var keyInfo = new ConsoleKeyInfo(keyChar, key, false, false, false);

            // Act
            var result = KeyTest.IsValidKeyForAnswerReveal(keyInfo, quitKey);

            // Assert
            result.Should().Be(expected, description);
        }

        [Theory]
        [InlineData(ConsoleKey.F1, '\0', "q", true, "Function keys should be valid")]
        [InlineData(ConsoleKey.F12, '\0', "q", true, "Function keys should be valid")]
        [InlineData(ConsoleKey.Home, '\0', "q", true, "Home key should be valid")]
        [InlineData(ConsoleKey.End, '\0', "q", true, "End key should be valid")]
        [InlineData(ConsoleKey.PageUp, '\0', "q", true, "Page Up key should be valid")]
        [InlineData(ConsoleKey.PageDown, '\0', "q", true, "Page Down key should be valid")]
        public void IsValidKeyForAnswerReveal_WithNavigationKeys_ShouldValidateCorrectly(
            ConsoleKey key,
            char keyChar,
            string quitKey,
            bool expected,
            string description)
        {
            // Arrange
            var keyInfo = new ConsoleKeyInfo(keyChar, key, false, false, false);

            // Act
            var result = KeyTest.IsValidKeyForAnswerReveal(keyInfo, quitKey);

            // Assert
            result.Should().Be(expected, description);
        }
    }
}
