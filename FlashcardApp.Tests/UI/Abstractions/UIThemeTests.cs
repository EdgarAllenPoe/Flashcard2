using FluentAssertions;
using FlashcardApp.UI.Abstractions;
using Moq;
using Xunit;

namespace FlashcardApp.Tests.UI.Abstractions
{
    public class UIThemeTests
    {
        [Fact]
        public void IUITheme_ShouldHaveRequiredProperties()
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();

            // Act & Assert - Verify all required properties exist
            mockTheme.Setup(x => x.UseColors).Returns(true);
            mockTheme.Setup(x => x.UseIcons).Returns(false);

            mockTheme.Object.UseColors.Should().BeTrue();
            mockTheme.Object.UseIcons.Should().BeFalse();
        }

        [Fact]
        public void IUITheme_ShouldHaveRequiredMethods()
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();

            // Act & Assert - Verify all required methods exist
            mockTheme.Setup(x => x.GetColorForMessageType(It.IsAny<MessageType>())).Returns(UIColor.Default);
            mockTheme.Setup(x => x.GetIconForMessageType(It.IsAny<MessageType>())).Returns("");
            mockTheme.Setup(x => x.GetColorForSection(It.IsAny<SectionType>())).Returns(UIColor.Default);
            mockTheme.Setup(x => x.GetIconForSection(It.IsAny<SectionType>())).Returns("");
            mockTheme.Setup(x => x.GetMenuOptionColor()).Returns(UIColor.Default);
            mockTheme.Setup(x => x.GetMenuDescriptionColor()).Returns(UIColor.Default);
            mockTheme.Setup(x => x.GetInputPromptColor()).Returns(UIColor.Default);
            mockTheme.Setup(x => x.GetStatisticsColor()).Returns(UIColor.Default);

            // All methods should be callable without throwing
            mockTheme.Object.GetColorForMessageType(MessageType.Success);
            mockTheme.Object.GetIconForMessageType(MessageType.Success);
            mockTheme.Object.GetColorForSection(SectionType.MainMenu);
            mockTheme.Object.GetIconForSection(SectionType.MainMenu);
            mockTheme.Object.GetMenuOptionColor();
            mockTheme.Object.GetMenuDescriptionColor();
            mockTheme.Object.GetInputPromptColor();
            mockTheme.Object.GetStatisticsColor();
        }

        [Theory]
        [InlineData(MessageType.Success)]
        [InlineData(MessageType.Error)]
        [InlineData(MessageType.Info)]
        [InlineData(MessageType.Warning)]
        [InlineData(MessageType.Default)]
        public void GetColorForMessageType_ShouldAcceptAllMessageTypes(MessageType messageType)
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();
            var expectedColor = UIColor.Green;
            mockTheme.Setup(x => x.GetColorForMessageType(messageType)).Returns(expectedColor);

            // Act
            var result = mockTheme.Object.GetColorForMessageType(messageType);

            // Assert
            result.Should().Be(expectedColor);
            mockTheme.Verify(x => x.GetColorForMessageType(messageType), Times.Once);
        }

        [Theory]
        [InlineData(MessageType.Success)]
        [InlineData(MessageType.Error)]
        [InlineData(MessageType.Info)]
        [InlineData(MessageType.Warning)]
        [InlineData(MessageType.Default)]
        public void GetIconForMessageType_ShouldAcceptAllMessageTypes(MessageType messageType)
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();
            var expectedIcon = "✓";
            mockTheme.Setup(x => x.GetIconForMessageType(messageType)).Returns(expectedIcon);

            // Act
            var result = mockTheme.Object.GetIconForMessageType(messageType);

            // Assert
            result.Should().Be(expectedIcon);
            mockTheme.Verify(x => x.GetIconForMessageType(messageType), Times.Once);
        }

        [Theory]
        [InlineData(SectionType.MainMenu)]
        [InlineData(SectionType.DeckManagement)]
        [InlineData(SectionType.StudySession)]
        [InlineData(SectionType.Statistics)]
        [InlineData(SectionType.Configuration)]
        [InlineData(SectionType.Help)]
        [InlineData(SectionType.Welcome)]
        [InlineData(SectionType.SessionResults)]
        [InlineData(SectionType.DeckList)]
        [InlineData(SectionType.Default)]
        public void GetColorForSection_ShouldAcceptAllSectionTypes(SectionType sectionType)
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();
            var expectedColor = UIColor.Cyan;
            mockTheme.Setup(x => x.GetColorForSection(sectionType)).Returns(expectedColor);

            // Act
            var result = mockTheme.Object.GetColorForSection(sectionType);

            // Assert
            result.Should().Be(expectedColor);
            mockTheme.Verify(x => x.GetColorForSection(sectionType), Times.Once);
        }

        [Theory]
        [InlineData(SectionType.MainMenu)]
        [InlineData(SectionType.DeckManagement)]
        [InlineData(SectionType.StudySession)]
        [InlineData(SectionType.Statistics)]
        [InlineData(SectionType.Configuration)]
        [InlineData(SectionType.Help)]
        [InlineData(SectionType.Welcome)]
        [InlineData(SectionType.SessionResults)]
        [InlineData(SectionType.DeckList)]
        [InlineData(SectionType.Default)]
        public void GetIconForSection_ShouldAcceptAllSectionTypes(SectionType sectionType)
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();
            var expectedIcon = "📋";
            mockTheme.Setup(x => x.GetIconForSection(sectionType)).Returns(expectedIcon);

            // Act
            var result = mockTheme.Object.GetIconForSection(sectionType);

            // Assert
            result.Should().Be(expectedIcon);
            mockTheme.Verify(x => x.GetIconForSection(sectionType), Times.Once);
        }

        [Fact]
        public void GetMenuOptionColor_ShouldReturnUIColor()
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();
            var expectedColor = UIColor.Yellow;
            mockTheme.Setup(x => x.GetMenuOptionColor()).Returns(expectedColor);

            // Act
            var result = mockTheme.Object.GetMenuOptionColor();

            // Assert
            result.Should().Be(expectedColor);
            mockTheme.Verify(x => x.GetMenuOptionColor(), Times.Once);
        }

        [Fact]
        public void GetMenuDescriptionColor_ShouldReturnUIColor()
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();
            var expectedColor = UIColor.DarkGray;
            mockTheme.Setup(x => x.GetMenuDescriptionColor()).Returns(expectedColor);

            // Act
            var result = mockTheme.Object.GetMenuDescriptionColor();

            // Assert
            result.Should().Be(expectedColor);
            mockTheme.Verify(x => x.GetMenuDescriptionColor(), Times.Once);
        }

        [Fact]
        public void GetInputPromptColor_ShouldReturnUIColor()
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();
            var expectedColor = UIColor.Yellow;
            mockTheme.Setup(x => x.GetInputPromptColor()).Returns(expectedColor);

            // Act
            var result = mockTheme.Object.GetInputPromptColor();

            // Assert
            result.Should().Be(expectedColor);
            mockTheme.Verify(x => x.GetInputPromptColor(), Times.Once);
        }

        [Fact]
        public void GetStatisticsColor_ShouldReturnUIColor()
        {
            // Arrange
            var mockTheme = new Mock<IUITheme>();
            var expectedColor = UIColor.Cyan;
            mockTheme.Setup(x => x.GetStatisticsColor()).Returns(expectedColor);

            // Act
            var result = mockTheme.Object.GetStatisticsColor();

            // Assert
            result.Should().Be(expectedColor);
            mockTheme.Verify(x => x.GetStatisticsColor(), Times.Once);
        }

        [Fact]
        public void MessageType_ShouldHaveAllExpectedValues()
        {
            // Arrange & Act
            var messageTypes = Enum.GetValues<MessageType>();

            // Assert
            messageTypes.Should().Contain(MessageType.Success);
            messageTypes.Should().Contain(MessageType.Error);
            messageTypes.Should().Contain(MessageType.Info);
            messageTypes.Should().Contain(MessageType.Warning);
            messageTypes.Should().Contain(MessageType.Default);
        }

        [Fact]
        public void SectionType_ShouldHaveAllExpectedValues()
        {
            // Arrange & Act
            var sectionTypes = Enum.GetValues<SectionType>();

            // Assert
            sectionTypes.Should().Contain(SectionType.MainMenu);
            sectionTypes.Should().Contain(SectionType.DeckManagement);
            sectionTypes.Should().Contain(SectionType.StudySession);
            sectionTypes.Should().Contain(SectionType.Statistics);
            sectionTypes.Should().Contain(SectionType.Configuration);
            sectionTypes.Should().Contain(SectionType.Help);
            sectionTypes.Should().Contain(SectionType.Welcome);
            sectionTypes.Should().Contain(SectionType.SessionResults);
            sectionTypes.Should().Contain(SectionType.DeckList);
            sectionTypes.Should().Contain(SectionType.Default);
        }
    }
}
