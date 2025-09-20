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
    public class ConsoleRendererTests
    {
        private readonly Mock<IUIOutput> _mockOutput;
        private readonly Mock<IUITheme> _mockTheme;
        private readonly ConsoleRenderer _renderer;

        public ConsoleRendererTests()
        {
            _mockOutput = new Mock<IUIOutput>();
            _mockTheme = new Mock<IUITheme>();
            _renderer = new ConsoleRenderer(_mockOutput.Object, _mockTheme.Object);
        }

        [Fact]
        public void ConsoleRenderer_ShouldImplementIUIRenderer()
        {
            // Assert
            _renderer.Should().BeAssignableTo<IUIRenderer>();
        }

        [Fact]
        public void Constructor_WithNullOutput_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ConsoleRenderer(null!, _mockTheme.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("output");
        }

        [Fact]
        public void Constructor_WithNullTheme_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var action = () => new ConsoleRenderer(_mockOutput.Object, null!);
            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("theme");
        }

        [Fact]
        public async Task RenderMenuAsync_ShouldCallThemeAndOutputMethods()
        {
            // Arrange
            var menu = new MenuDefinition
            {
                Title = "Test Menu",
                SectionType = SectionType.MainMenu,
                Options = new List<MenuOption>
                {
                    new MenuOption { Key = "1", Title = "Option 1", Description = "First option" },
                    new MenuOption { Key = "2", Title = "Option 2", Description = "Second option" }
                }
            };

            _mockTheme.Setup(x => x.GetColorForSection(SectionType.MainMenu)).Returns(UIColor.Blue);
            _mockTheme.Setup(x => x.GetIconForSection(SectionType.MainMenu)).Returns("📋");
            _mockTheme.Setup(x => x.GetMenuOptionColor()).Returns(UIColor.Yellow);
            _mockTheme.Setup(x => x.GetMenuDescriptionColor()).Returns(UIColor.DarkGray);

            // Act
            await _renderer.RenderMenuAsync(menu);

            // Assert
            _mockTheme.Verify(x => x.GetColorForSection(SectionType.MainMenu), Times.Once);
            _mockTheme.Verify(x => x.GetIconForSection(SectionType.MainMenu), Times.Once);
            _mockTheme.Verify(x => x.GetMenuOptionColor(), Times.AtLeastOnce);
            _mockTheme.Verify(x => x.GetMenuDescriptionColor(), Times.AtLeastOnce);
            _mockOutput.Verify(x => x.SetForegroundColor(It.IsAny<UIColor>()), Times.AtLeastOnce);
            _mockOutput.Verify(x => x.ResetColor(), Times.AtLeastOnce);
            _mockOutput.Verify(x => x.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task RenderMessageAsync_ShouldCallThemeAndOutputMethods()
        {
            // Arrange
            var message = new MessageDefinition
            {
                Text = "Test message",
                Type = MessageType.Success
            };

            _mockTheme.Setup(x => x.GetColorForMessageType(MessageType.Success)).Returns(UIColor.Green);
            _mockTheme.Setup(x => x.GetIconForMessageType(MessageType.Success)).Returns("✓");

            // Act
            await _renderer.RenderMessageAsync(message);

            // Assert
            _mockTheme.Verify(x => x.GetColorForMessageType(MessageType.Success), Times.Once);
            _mockTheme.Verify(x => x.GetIconForMessageType(MessageType.Success), Times.Once);
            _mockOutput.Verify(x => x.SetForegroundColor(UIColor.Green), Times.Once);
            _mockOutput.Verify(x => x.WriteLine("    ✓ Test message"), Times.Once);
            _mockOutput.Verify(x => x.ResetColor(), Times.Once);
        }

        [Fact]
        public async Task RenderStatisticsAsync_ShouldCallThemeAndOutputMethods()
        {
            // Arrange
            var stats = new StatisticsData
            {
                Statistics = new Dictionary<string, object>
                {
                    ["TotalDecks"] = 5,
                    ["TotalCards"] = 100
                },
                Decks = new List<Deck>
                {
                    new Deck { Id = "1", Name = "Test Deck" }
                }
            };

            _mockTheme.Setup(x => x.GetStatisticsColor()).Returns(UIColor.Cyan);

            // Act
            await _renderer.RenderStatisticsAsync(stats);

            // Assert
            _mockTheme.Verify(x => x.GetStatisticsColor(), Times.AtLeastOnce);
            _mockOutput.Verify(x => x.SetForegroundColor(UIColor.Cyan), Times.AtLeastOnce);
            _mockOutput.Verify(x => x.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
            _mockOutput.Verify(x => x.ResetColor(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task RenderDeckListAsync_WithEmptyList_ShouldShowInfoMessage()
        {
            // Arrange
            var decks = new List<Deck>();

            _mockTheme.Setup(x => x.GetColorForSection(SectionType.DeckList)).Returns(UIColor.Cyan);
            _mockTheme.Setup(x => x.GetIconForSection(SectionType.DeckList)).Returns("📑");
            _mockTheme.Setup(x => x.GetColorForMessageType(MessageType.Info)).Returns(UIColor.Yellow);
            _mockTheme.Setup(x => x.GetIconForMessageType(MessageType.Info)).Returns("ℹ");

            // Act
            await _renderer.RenderDeckListAsync(decks);

            // Assert
            _mockTheme.Verify(x => x.GetColorForSection(SectionType.DeckList), Times.Once);
            _mockTheme.Verify(x => x.GetIconForSection(SectionType.DeckList), Times.Once);
            _mockTheme.Verify(x => x.GetColorForMessageType(MessageType.Info), Times.Once);
            _mockTheme.Verify(x => x.GetIconForMessageType(MessageType.Info), Times.Once);
            _mockOutput.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("No decks found"))), Times.Once);
        }

        [Fact]
        public async Task RenderSessionResultsAsync_WithHighSuccessRate_ShouldShowSuccessMessage()
        {
            // Arrange
            var result = new SessionResult
            {
                Success = true,
                Message = "Session completed",
                SessionStatistics = new SessionStatistics
                {
                    CardsStudied = 10,
                    CorrectAnswers = 9,
                    IncorrectAnswers = 1,
                    TotalStudyTime = TimeSpan.FromMinutes(5)
                }
            };

            _mockTheme.Setup(x => x.GetColorForSection(SectionType.SessionResults)).Returns(UIColor.Green);
            _mockTheme.Setup(x => x.GetIconForSection(SectionType.SessionResults)).Returns("🎯");
            _mockTheme.Setup(x => x.GetStatisticsColor()).Returns(UIColor.Cyan);
            _mockTheme.Setup(x => x.GetColorForMessageType(MessageType.Success)).Returns(UIColor.Green);
            _mockTheme.Setup(x => x.GetIconForMessageType(MessageType.Success)).Returns("✓");

            // Act
            await _renderer.RenderSessionResultsAsync(result);

            // Assert
            _mockTheme.Verify(x => x.GetColorForSection(SectionType.SessionResults), Times.Once);
            _mockTheme.Verify(x => x.GetIconForSection(SectionType.SessionResults), Times.Once);
            _mockTheme.Verify(x => x.GetStatisticsColor(), Times.AtLeastOnce);
            _mockTheme.Verify(x => x.GetColorForMessageType(MessageType.Success), Times.Once);
            _mockTheme.Verify(x => x.GetIconForMessageType(MessageType.Success), Times.Once);
            _mockOutput.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("Excellent work"))), Times.Once);
        }

        [Fact]
        public async Task RenderWelcomeMessageAsync_ShouldCallThemeAndOutputMethods()
        {
            // Arrange
            _mockTheme.Setup(x => x.GetColorForSection(SectionType.Welcome)).Returns(UIColor.Cyan);
            _mockTheme.Setup(x => x.GetIconForSection(SectionType.Welcome)).Returns("👋");

            // Act
            await _renderer.RenderWelcomeMessageAsync();

            // Assert
            _mockOutput.Verify(x => x.Clear(), Times.Once);
            _mockTheme.Verify(x => x.GetColorForSection(SectionType.Welcome), Times.Once);
            _mockTheme.Verify(x => x.GetIconForSection(SectionType.Welcome), Times.Once);
            _mockOutput.Verify(x => x.SetForegroundColor(UIColor.Cyan), Times.AtLeastOnce);
            _mockOutput.Verify(x => x.SetForegroundColor(UIColor.Green), Times.AtLeastOnce);
            _mockOutput.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("FLASHCARD APP v2.0"))), Times.Once);
            _mockOutput.Verify(x => x.ResetColor(), Times.AtLeastOnce);
        }

        [Fact, Trait("Category", "Slow")]
        public async Task RenderExitMessageAsync_ShouldCallThemeAndOutputMethods()
        {
            // Arrange
            _mockTheme.Setup(x => x.GetColorForSection(SectionType.Welcome)).Returns(UIColor.Cyan);

            // Act
            await _renderer.RenderExitMessageAsync();

            // Assert
            _mockOutput.Verify(x => x.Clear(), Times.Once);
            _mockTheme.Verify(x => x.GetColorForSection(SectionType.Welcome), Times.Once);
            _mockOutput.Verify(x => x.SetForegroundColor(UIColor.Cyan), Times.AtLeastOnce);
            _mockOutput.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains("THANK YOU FOR LEARNING"))), Times.Once);
            _mockOutput.Verify(x => x.ResetColor(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task RenderPressAnyKeyAsync_ShouldCallThemeAndOutputMethods()
        {
            // Arrange
            _mockTheme.Setup(x => x.GetMenuDescriptionColor()).Returns(UIColor.DarkGray);

            // Act
            await _renderer.RenderPressAnyKeyAsync();

            // Assert
            _mockTheme.Verify(x => x.GetMenuDescriptionColor(), Times.Once);
            _mockOutput.Verify(x => x.SetForegroundColor(UIColor.DarkGray), Times.Once);
            _mockOutput.Verify(x => x.WriteLine("    Press any key to continue..."), Times.Once);
            _mockOutput.Verify(x => x.ResetColor(), Times.Once);
        }

        [Fact]
        public async Task RenderInputPromptAsync_ShouldCallThemeAndOutputMethods()
        {
            // Arrange
            var prompt = "Enter your choice";
            _mockTheme.Setup(x => x.GetInputPromptColor()).Returns(UIColor.Yellow);

            // Act
            await _renderer.RenderInputPromptAsync(prompt);

            // Assert
            _mockTheme.Verify(x => x.GetInputPromptColor(), Times.Once);
            _mockOutput.Verify(x => x.SetForegroundColor(UIColor.Yellow), Times.Once);
            _mockOutput.Verify(x => x.Write($"    {prompt}: "), Times.Once);
            _mockOutput.Verify(x => x.ResetColor(), Times.Once);
        }
    }
}
