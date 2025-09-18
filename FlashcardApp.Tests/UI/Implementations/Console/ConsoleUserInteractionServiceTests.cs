using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.UI.Implementations.Console;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FlashcardApp.Tests.UI.Implementations.Console
{
    public class ConsoleUserInteractionServiceTests
    {
        private readonly Mock<IUIInput> _mockInput;
        private readonly Mock<IUIOutput> _mockOutput;
        private readonly Mock<IUITheme> _mockTheme;
        private readonly Mock<IUIRenderer> _mockRenderer;
        private readonly ConsoleUserInteractionService _service;

        public ConsoleUserInteractionServiceTests()
        {
            _mockInput = new Mock<IUIInput>();
            _mockOutput = new Mock<IUIOutput>();
            _mockTheme = new Mock<IUITheme>();
            _mockRenderer = new Mock<IUIRenderer>();
            _service = new ConsoleUserInteractionService(_mockInput.Object, _mockOutput.Object, _mockTheme.Object, _mockRenderer.Object);
        }

        [Fact]
        public void ConsoleUserInteractionService_ShouldImplementIUserInteractionService()
        {
            // Assert
            _service.Should().BeAssignableTo<IUserInteractionService>();
        }

        [Fact]
        public void Constructor_WithNullInput_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ConsoleUserInteractionService(null!, _mockOutput.Object, _mockTheme.Object, _mockRenderer.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("input");
        }

        [Fact]
        public void Constructor_WithNullOutput_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ConsoleUserInteractionService(_mockInput.Object, null!, _mockTheme.Object, _mockRenderer.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("output");
        }

        [Fact]
        public void Constructor_WithNullTheme_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ConsoleUserInteractionService(_mockInput.Object, _mockOutput.Object, null!, _mockRenderer.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("theme");
        }

        [Fact]
        public void Constructor_WithNullRenderer_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ConsoleUserInteractionService(_mockInput.Object, _mockOutput.Object, _mockTheme.Object, null!);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("renderer");
        }

        [Fact]
        public async Task SelectDeckAsync_WithValidSelection_ShouldReturnSelectedDeck()
        {
            // Arrange
            var decks = new List<Deck>
            {
                new Deck { Id = "1", Name = "Deck 1" },
                new Deck { Id = "2", Name = "Deck 2" }
            };

            _mockInput.Setup(x => x.ReadKeyAsync(true)).ReturnsAsync(new UIKeyInfo(UIKey.D1, '1'));
            _mockInput.Setup(x => x.ReadLineAsync()).ReturnsAsync("");

            // Act
            var result = await _service.SelectDeckAsync(decks, "Select a deck");

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be("1");
            result.Name.Should().Be("Deck 1");
            _mockOutput.Verify(x => x.Clear(), Times.Once);
            _mockRenderer.Verify(x => x.RenderSectionHeaderAsync("SELECT DECK", SectionType.DeckList), Times.Once);
        }

        [Fact]
        public async Task SelectDeckAsync_WithEscapeKey_ShouldReturnNull()
        {
            // Arrange
            var decks = new List<Deck>
            {
                new Deck { Id = "1", Name = "Deck 1" }
            };

            _mockInput.Setup(x => x.ReadKeyAsync(true)).ReturnsAsync(new UIKeyInfo(UIKey.Escape, '\0'));

            // Act
            var result = await _service.SelectDeckAsync(decks, "Select a deck");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task SelectStudyModeAsync_WithValidInput_ShouldReturnCorrectMode()
        {
            // Arrange
            _mockInput.Setup(x => x.GetImmediateChoiceAsync()).ReturnsAsync("2");

            // Act
            var result = await _service.SelectStudyModeAsync();

            // Assert
            result.Should().Be(StudyMode.BackToFront);
            _mockOutput.Verify(x => x.Clear(), Times.Once);
            _mockRenderer.Verify(x => x.RenderSectionHeaderAsync("SELECT STUDY MODE", SectionType.StudySession), Times.Once);
        }

        [Fact]
        public async Task SelectStudyModeAsync_WithEscapeKey_ShouldReturnDefaultMode()
        {
            // Arrange
            _mockInput.Setup(x => x.GetImmediateChoiceAsync()).ReturnsAsync("ESC");

            // Act
            var result = await _service.SelectStudyModeAsync();

            // Assert
            result.Should().Be(StudyMode.FrontToBack);
        }

        [Fact]
        public async Task GetMaxCardsForSessionAsync_WithValidInput_ShouldReturnCorrectValue()
        {
            // Arrange
            _mockInput.Setup(x => x.ReadKeyAsync(true)).ReturnsAsync(new UIKeyInfo(UIKey.D5, '5'));
            _mockInput.Setup(x => x.ReadLineAsync()).ReturnsAsync("0");

            // Act
            var result = await _service.GetMaxCardsForSessionAsync();

            // Assert
            result.Should().Be(50);
        }

        [Fact]
        public async Task GetMaxCardsForSessionAsync_WithEscapeKey_ShouldReturnDefaultValue()
        {
            // Arrange
            _mockInput.Setup(x => x.ReadKeyAsync(true)).ReturnsAsync(new UIKeyInfo(UIKey.Escape, '\0'));

            // Act
            var result = await _service.GetMaxCardsForSessionAsync();

            // Assert
            result.Should().Be(100);
        }

        [Fact]
        public async Task GetMaxCardsForSessionAsync_WithEnterKey_ShouldReturnDefaultValue()
        {
            // Arrange
            _mockInput.Setup(x => x.ReadKeyAsync(true)).ReturnsAsync(new UIKeyInfo(UIKey.Enter, '\r'));

            // Act
            var result = await _service.GetMaxCardsForSessionAsync();

            // Assert
            result.Should().Be(100);
        }

        [Fact]
        public async Task ConfirmActionAsync_WithYesInput_ShouldReturnTrue()
        {
            // Arrange
            _mockInput.Setup(x => x.ReadLineAsync()).ReturnsAsync("y");

            // Act
            var result = await _service.ConfirmActionAsync("Are you sure?");

            // Assert
            result.Should().BeTrue();
            _mockRenderer.Verify(x => x.RenderInputPromptAsync("Are you sure? (y/N)"), Times.Once);
        }

        [Fact]
        public async Task ConfirmActionAsync_WithNoInput_ShouldReturnFalse()
        {
            // Arrange
            _mockInput.Setup(x => x.ReadLineAsync()).ReturnsAsync("n");

            // Act
            var result = await _service.ConfirmActionAsync("Are you sure?");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetInputAsync_ShouldReturnUserInput()
        {
            // Arrange
            var expectedInput = "test input";
            _mockInput.Setup(x => x.ReadLineAsync()).ReturnsAsync(expectedInput);

            // Act
            var result = await _service.GetInputAsync("Enter text");

            // Assert
            result.Should().Be(expectedInput);
            _mockRenderer.Verify(x => x.RenderInputPromptAsync("Enter text"), Times.Once);
        }

        [Fact]
        public async Task GetValidatedInputAsync_WithValidInput_ShouldReturnInput()
        {
            // Arrange
            var expectedInput = "valid input";
            _mockInput.Setup(x => x.ReadLineAsync()).ReturnsAsync(expectedInput);

            // Act
            var result = await _service.GetValidatedInputAsync("Enter text", input => !string.IsNullOrEmpty(input));

            // Assert
            result.Should().Be(expectedInput);
            _mockRenderer.Verify(x => x.RenderInputPromptAsync("Enter text"), Times.Once);
        }

        [Fact]
        public async Task GetValidatedInputAsync_WithInvalidInput_ShouldRetry()
        {
            // Arrange
            _mockInput.SetupSequence(x => x.ReadLineAsync())
                .ReturnsAsync("") // Invalid input
                .ReturnsAsync("valid input"); // Valid input

            // Act
            var result = await _service.GetValidatedInputAsync("Enter text", input => !string.IsNullOrEmpty(input));

            // Assert
            result.Should().Be("valid input");
            _mockRenderer.Verify(x => x.RenderInputPromptAsync("Enter text"), Times.Exactly(2));
            _mockRenderer.Verify(x => x.RenderMessageAsync(It.Is<MessageDefinition>(m => m.Text == "Invalid input. Please try again." && m.Type == MessageType.Error)), Times.Once);
        }

        [Fact]
        public async Task SelectFromOptionsAsync_WithValidSelection_ShouldReturnSelectedOption()
        {
            // Arrange
            var options = new List<string> { "Option 1", "Option 2", "Option 3" };
            _mockInput.Setup(x => x.GetImmediateChoiceAsync()).ReturnsAsync("2");

            // Act
            var result = await _service.SelectFromOptionsAsync(options, "Select option");

            // Assert
            result.Should().Be("Option 2");
            _mockOutput.Verify(x => x.WriteLine(""), Times.AtLeastOnce);
            _mockRenderer.Verify(x => x.RenderInputPromptAsync("Select option"), Times.Once);
        }

        [Fact]
        public async Task SelectFromOptionsAsync_WithEscapeKey_ShouldReturnNull()
        {
            // Arrange
            var options = new List<string> { "Option 1", "Option 2" };
            _mockInput.Setup(x => x.GetImmediateChoiceAsync()).ReturnsAsync("ESC");

            // Act
            var result = await _service.SelectFromOptionsAsync(options, "Select option");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task WaitForAnyKeyAsync_ShouldCallRendererAndInput()
        {
            // Act
            await _service.WaitForAnyKeyAsync();

            // Assert
            _mockRenderer.Verify(x => x.RenderPressAnyKeyAsync(), Times.Once);
            _mockInput.Verify(x => x.WaitForAnyKeyAsync(), Times.Once);
        }

        [Fact]
        public async Task ShowMessageAsync_ShouldCallRenderer()
        {
            // Arrange
            var message = "Test message";
            var messageType = MessageType.Success;

            // Act
            await _service.ShowMessageAsync(message, messageType);

            // Assert
            _mockRenderer.Verify(x => x.RenderMessageAsync(It.Is<MessageDefinition>(m => m.Text == message && m.Type == messageType)), Times.Once);
        }

        [Fact]
        public async Task ShowMessageAsync_WithDefaultMessageType_ShouldUseInfo()
        {
            // Arrange
            var message = "Test message";

            // Act
            await _service.ShowMessageAsync(message);

            // Assert
            _mockRenderer.Verify(x => x.RenderMessageAsync(It.Is<MessageDefinition>(m => m.Text == message && m.Type == MessageType.Info)), Times.Once);
        }
    }
}
