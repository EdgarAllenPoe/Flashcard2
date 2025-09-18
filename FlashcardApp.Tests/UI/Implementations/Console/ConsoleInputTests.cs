using FluentAssertions;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.UI.Implementations.Console;
using Xunit;

namespace FlashcardApp.Tests.UI.Implementations.Console
{
    public class ConsoleInputTests
    {
        [Fact]
        public void ConsoleInput_ShouldImplementIUIInput()
        {
            // Arrange & Act
            var consoleInput = new ConsoleInput();

            // Assert
            consoleInput.Should().BeAssignableTo<IUIInput>();
        }

        [Fact]
        public void ReadLineAsync_ShouldReturnTask()
        {
            // Arrange
            var consoleInput = new ConsoleInput();

            // Act
            var task = consoleInput.ReadLineAsync();

            // Assert
            task.Should().NotBeNull();
            task.Should().BeAssignableTo<Task<string>>();
            
            // Note: We can't easily test the actual input without mocking or complex setup
            // This test verifies the method signature and return type
        }

        [Fact]
        public void ReadKeyAsync_ShouldReturnTask()
        {
            // Arrange
            var consoleInput = new ConsoleInput();

            // Act
            var task = consoleInput.ReadKeyAsync();

            // Assert
            task.Should().NotBeNull();
            task.Should().BeAssignableTo<Task<UIKeyInfo>>();
        }

        [Fact]
        public void GetImmediateChoiceAsync_ShouldReturnTask()
        {
            // Arrange
            var consoleInput = new ConsoleInput();

            // Act
            var task = consoleInput.GetImmediateChoiceAsync();

            // Assert
            task.Should().NotBeNull();
            task.Should().BeAssignableTo<Task<string>>();
        }

        [Fact]
        public void GetUserChoiceAsync_ShouldReturnTask()
        {
            // Arrange
            var consoleInput = new ConsoleInput();

            // Act
            var task = consoleInput.GetUserChoiceAsync();

            // Assert
            task.Should().NotBeNull();
            task.Should().BeAssignableTo<Task<string>>();
        }

        [Fact]
        public void WaitForAnyKeyAsync_ShouldReturnTask()
        {
            // Arrange
            var consoleInput = new ConsoleInput();

            // Act
            var task = consoleInput.WaitForAnyKeyAsync();

            // Assert
            task.Should().NotBeNull();
            task.Should().BeAssignableTo<Task>();
        }

        [Fact]
        public void ReadKeyAsync_WithInterceptTrue_ShouldReturnTask()
        {
            // Arrange
            var consoleInput = new ConsoleInput();

            // Act
            var task = consoleInput.ReadKeyAsync(true);

            // Assert
            task.Should().NotBeNull();
            task.Should().BeAssignableTo<Task<UIKeyInfo>>();
        }

        [Fact]
        public void ReadKeyAsync_WithInterceptFalse_ShouldReturnTask()
        {
            // Arrange
            var consoleInput = new ConsoleInput();

            // Act
            var task = consoleInput.ReadKeyAsync(false);

            // Assert
            task.Should().NotBeNull();
            task.Should().BeAssignableTo<Task<UIKeyInfo>>();
        }
    }
}
