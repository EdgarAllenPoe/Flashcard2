using FluentAssertions;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.Models;
using Moq;
using Xunit;

namespace FlashcardApp.Tests.UI.Abstractions
{
    public class UIRendererTests
    {
        [Fact]
        public void IUIRenderer_ShouldHaveRequiredMethods()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();

            // Act & Assert - Verify all required methods exist
            mockRenderer.Setup(x => x.RenderMenuAsync(It.IsAny<MenuDefinition>())).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderMessageAsync(It.IsAny<MessageDefinition>())).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderStatisticsAsync(It.IsAny<StatisticsData>())).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderDeckListAsync(It.IsAny<List<Deck>>())).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderSessionResultsAsync(It.IsAny<SessionResult>())).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderSectionHeaderAsync(It.IsAny<string>(), It.IsAny<SectionType>())).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderDeckCardAsync(It.IsAny<Deck>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderPressAnyKeyAsync()).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderInputPromptAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderWelcomeMessageAsync()).Returns(Task.CompletedTask);
            mockRenderer.Setup(x => x.RenderExitMessageAsync()).Returns(Task.CompletedTask);

            // All methods should be callable without throwing
            var menu = new MenuDefinition();
            var message = new MessageDefinition();
            var stats = new StatisticsData();
            var decks = new List<Deck>();
            var result = new SessionResult();
            var deck = new Deck();

            mockRenderer.Object.RenderMenuAsync(menu);
            mockRenderer.Object.RenderMessageAsync(message);
            mockRenderer.Object.RenderStatisticsAsync(stats);
            mockRenderer.Object.RenderDeckListAsync(decks);
            mockRenderer.Object.RenderSessionResultsAsync(result);
            mockRenderer.Object.RenderSectionHeaderAsync("Test", SectionType.MainMenu);
            mockRenderer.Object.RenderDeckCardAsync(deck, 1);
            mockRenderer.Object.RenderPressAnyKeyAsync();
            mockRenderer.Object.RenderInputPromptAsync("Test");
            mockRenderer.Object.RenderWelcomeMessageAsync();
            mockRenderer.Object.RenderExitMessageAsync();
        }

        [Fact]
        public async Task RenderMenuAsync_ShouldAcceptMenuDefinition()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();
            var menu = new MenuDefinition
            {
                Title = "Test Menu",
                SectionType = SectionType.MainMenu,
                Options = new List<MenuOption>
                {
                    new MenuOption { Key = "1", Title = "Option 1", Description = "First option" }
                }
            };

            // Act
            await mockRenderer.Object.RenderMenuAsync(menu);

            // Assert
            mockRenderer.Verify(x => x.RenderMenuAsync(menu), Times.Once);
        }

        [Fact]
        public async Task RenderMessageAsync_ShouldAcceptMessageDefinition()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();
            var message = new MessageDefinition
            {
                Text = "Test message",
                Type = MessageType.Info
            };

            // Act
            await mockRenderer.Object.RenderMessageAsync(message);

            // Assert
            mockRenderer.Verify(x => x.RenderMessageAsync(message), Times.Once);
        }

        [Fact]
        public async Task RenderStatisticsAsync_ShouldAcceptStatisticsData()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();
            var stats = new StatisticsData
            {
                Statistics = new Dictionary<string, object>
                {
                    ["TotalDecks"] = 5,
                    ["TotalCards"] = 100
                },
                Decks = new List<Deck>()
            };

            // Act
            await mockRenderer.Object.RenderStatisticsAsync(stats);

            // Assert
            mockRenderer.Verify(x => x.RenderStatisticsAsync(stats), Times.Once);
        }

        [Fact]
        public async Task RenderDeckListAsync_ShouldAcceptDeckList()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();
            var decks = new List<Deck>
            {
                new Deck { Name = "Test Deck 1" },
                new Deck { Name = "Test Deck 2" }
            };

            // Act
            await mockRenderer.Object.RenderDeckListAsync(decks);

            // Assert
            mockRenderer.Verify(x => x.RenderDeckListAsync(decks), Times.Once);
        }

        [Fact]
        public async Task RenderSessionResultsAsync_ShouldAcceptSessionResult()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();
            var result = new SessionResult
            {
                Success = true,
                Message = "Session completed",
                SessionStatistics = new SessionStatistics()
            };

            // Act
            await mockRenderer.Object.RenderSessionResultsAsync(result);

            // Assert
            mockRenderer.Verify(x => x.RenderSessionResultsAsync(result), Times.Once);
        }

        [Theory]
        [InlineData("Test Title", SectionType.MainMenu)]
        [InlineData("", SectionType.Default)]
        [InlineData("Very Long Title", SectionType.Statistics)]
        public async Task RenderSectionHeaderAsync_ShouldAcceptTitleAndSectionType(string title, SectionType sectionType)
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();

            // Act
            await mockRenderer.Object.RenderSectionHeaderAsync(title, sectionType);

            // Assert
            mockRenderer.Verify(x => x.RenderSectionHeaderAsync(title, sectionType), Times.Once);
        }

        [Fact]
        public async Task RenderDeckCardAsync_ShouldAcceptDeckAndNumber()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();
            var deck = new Deck { Name = "Test Deck" };

            // Act
            await mockRenderer.Object.RenderDeckCardAsync(deck, 1);

            // Assert
            mockRenderer.Verify(x => x.RenderDeckCardAsync(deck, 1), Times.Once);
        }

        [Fact]
        public async Task RenderPressAnyKeyAsync_ShouldComplete()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();

            // Act
            await mockRenderer.Object.RenderPressAnyKeyAsync();

            // Assert
            mockRenderer.Verify(x => x.RenderPressAnyKeyAsync(), Times.Once);
        }

        [Theory]
        [InlineData("Enter your choice")]
        [InlineData("")]
        [InlineData("Please provide input")]
        public async Task RenderInputPromptAsync_ShouldAcceptPrompt(string prompt)
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();

            // Act
            await mockRenderer.Object.RenderInputPromptAsync(prompt);

            // Assert
            mockRenderer.Verify(x => x.RenderInputPromptAsync(prompt), Times.Once);
        }

        [Fact]
        public async Task RenderWelcomeMessageAsync_ShouldComplete()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();

            // Act
            await mockRenderer.Object.RenderWelcomeMessageAsync();

            // Assert
            mockRenderer.Verify(x => x.RenderWelcomeMessageAsync(), Times.Once);
        }

        [Fact]
        public async Task RenderExitMessageAsync_ShouldComplete()
        {
            // Arrange
            var mockRenderer = new Mock<IUIRenderer>();

            // Act
            await mockRenderer.Object.RenderExitMessageAsync();

            // Assert
            mockRenderer.Verify(x => x.RenderExitMessageAsync(), Times.Once);
        }

        [Fact]
        public void MenuDefinition_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var menu = new MenuDefinition();

            // Assert
            menu.Title.Should().BeEmpty();
            menu.Options.Should().NotBeNull();
            menu.Options.Should().BeEmpty();
            menu.SectionType.Should().Be(SectionType.Default);
        }

        [Fact]
        public void MenuOption_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var option = new MenuOption();

            // Assert
            option.Key.Should().BeEmpty();
            option.Title.Should().BeEmpty();
            option.Description.Should().BeEmpty();
        }

        [Fact]
        public void MessageDefinition_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var message = new MessageDefinition();

            // Assert
            message.Text.Should().BeEmpty();
            message.Type.Should().Be(MessageType.Default);
        }

        [Fact]
        public void StatisticsData_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var stats = new StatisticsData();

            // Assert
            stats.Statistics.Should().NotBeNull();
            stats.Statistics.Should().BeEmpty();
            stats.Decks.Should().NotBeNull();
            stats.Decks.Should().BeEmpty();
        }

        [Fact]
        public void SessionResult_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var result = new SessionResult();

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().BeEmpty();
            result.SessionStatistics.Should().BeNull();
        }
    }
}
