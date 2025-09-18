using FluentAssertions;
using FlashcardApp.UI.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlashcardApp.Tests.UI.Help
{
    /// <summary>
    /// Tests for the contextual help system with search and interactive guides
    /// </summary>
    public class HelpSystemTests
    {
        [Fact]
        public void HelpSystem_ShouldDefineHelpTopics()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpTopics = helpSystem.GetHelpTopics();

            // Assert
            helpTopics.Should().NotBeNull();
            helpTopics.Should().Contain(topic => topic.Name == "Getting Started");
            helpTopics.Should().Contain(topic => topic.Name == "Study Sessions");
            helpTopics.Should().Contain(topic => topic.Name == "Deck Management");
            helpTopics.Should().Contain(topic => topic.Name == "Statistics");
            helpTopics.Should().Contain(topic => topic.Name == "Settings");
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpSearch()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpSearch = helpSystem.GetHelpSearch();

            // Assert
            helpSearch.Should().NotBeNull();
            helpSearch.IsEnabled.Should().BeTrue();
            helpSearch.Placeholder.Should().NotBeNullOrEmpty();
            helpSearch.SearchFields.Should().NotBeEmpty();
            helpSearch.SearchFields.Should().Contain("Title");
            helpSearch.SearchFields.Should().Contain("Content");
            helpSearch.SearchFields.Should().Contain("Tags");
        }

        [Fact]
        public void HelpSystem_ShouldDefineInteractiveGuides()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var interactiveGuides = helpSystem.GetInteractiveGuides();

            // Assert
            interactiveGuides.Should().NotBeNull();
            interactiveGuides.Should().Contain(guide => guide.Name == "First Study Session");
            interactiveGuides.Should().Contain(guide => guide.Name == "Creating Your First Deck");
            interactiveGuides.Should().Contain(guide => guide.Name == "Understanding Statistics");
            interactiveGuides.Should().Contain(guide => guide.Name == "Customizing Settings");
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpCategories()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpCategories = helpSystem.GetHelpCategories();

            // Assert
            helpCategories.Should().NotBeNull();
            helpCategories.Should().Contain(category => category.Name == "Basics");
            helpCategories.Should().Contain(category => category.Name == "Study");
            helpCategories.Should().Contain(category => category.Name == "Management");
            helpCategories.Should().Contain(category => category.Name == "Advanced");
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpContent()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpContent = helpSystem.GetHelpContent();

            // Assert
            helpContent.Should().NotBeNull();
            helpContent.Should().ContainKey("Getting Started");
            helpContent.Should().ContainKey("Study Sessions");
            helpContent.Should().ContainKey("Deck Management");
            helpContent.Should().ContainKey("Statistics");
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpNavigation()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpNavigation = helpSystem.GetHelpNavigation();

            // Assert
            helpNavigation.Should().NotBeNull();
            helpNavigation.Breadcrumbs.Should().NotBeEmpty();
            helpNavigation.PreviousTopic.Should().NotBeNullOrEmpty();
            helpNavigation.NextTopic.Should().NotBeNullOrEmpty();
            helpNavigation.RelatedTopics.Should().NotBeEmpty();
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpFeedback()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpFeedback = helpSystem.GetHelpFeedback();

            // Assert
            helpFeedback.Should().NotBeNull();
            helpFeedback.Should().Contain(feedback => feedback.Type == "Helpful");
            helpFeedback.Should().Contain(feedback => feedback.Type == "Not Helpful");
            helpFeedback.Should().Contain(feedback => feedback.Type == "Suggest Improvement");
            helpFeedback.Should().Contain(feedback => feedback.Type == "Report Issue");
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpBookmarks()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpBookmarks = helpSystem.GetHelpBookmarks();

            // Assert
            helpBookmarks.Should().NotBeNull();
            helpBookmarks.Bookmarks.Should().NotBeEmpty();
            helpBookmarks.Bookmarks.Should().Contain(bookmark => bookmark.TopicName == "Getting Started");
            helpBookmarks.Bookmarks.Should().Contain(bookmark => bookmark.TopicName == "Study Sessions");
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpHistory()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpHistory = helpSystem.GetHelpHistory();

            // Assert
            helpHistory.Should().NotBeNull();
            helpHistory.RecentTopics.Should().NotBeEmpty();
            helpHistory.RecentTopics.Should().Contain(topic => topic.TopicName == "Getting Started");
            helpHistory.RecentTopics.Should().Contain(topic => topic.TopicName == "Study Sessions");
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpAccessibility()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpAccessibility = helpSystem.GetHelpAccessibility();

            // Assert
            helpAccessibility.Should().NotBeNull();
            helpAccessibility.ScreenReader.Should().NotBeNullOrEmpty();
            helpAccessibility.KeyboardNavigation.Should().NotBeNullOrEmpty();
            helpAccessibility.HighContrast.Should().NotBeNullOrEmpty();
            helpAccessibility.TextSize.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpPrinting()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpPrinting = helpSystem.GetHelpPrinting();

            // Assert
            helpPrinting.Should().NotBeNull();
            helpPrinting.IsEnabled.Should().BeTrue();
            helpPrinting.PrintFormats.Should().NotBeEmpty();
            helpPrinting.PrintFormats.Should().Contain("PDF");
            helpPrinting.PrintFormats.Should().Contain("HTML");
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpSharing()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpSharing = helpSystem.GetHelpSharing();

            // Assert
            helpSharing.Should().NotBeNull();
            helpSharing.Should().Contain(option => option.Type == "Copy Link");
            helpSharing.Should().Contain(option => option.Type == "Share Topic");
            helpSharing.Should().Contain(option => option.Type == "Export PDF");
            helpSharing.Should().Contain(option => option.Type == "Print");
        }

        [Fact]
        public void HelpSystem_ShouldDefineHelpContext()
        {
            // Arrange
            var helpSystem = new HelpSystem();

            // Act
            var helpContext = helpSystem.GetHelpContext();

            // Assert
            helpContext.Should().NotBeNull();
            helpContext.CurrentPage.Should().NotBeNullOrEmpty();
            helpContext.SuggestedTopics.Should().NotBeEmpty();
            helpContext.ContextualHelp.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void HelpSystem_ShouldValidateHelpRequest()
        {
            // Arrange
            var helpSystem = new HelpSystem();
            var helpRequest = new HelpRequest
            {
                Topic = "Getting Started",
                SearchQuery = "how to start",
                Category = "Basics"
            };

            // Act
            var validation = helpSystem.ValidateHelpRequest(helpRequest);

            // Assert
            validation.Should().NotBeNull();
            validation.IsValid.Should().BeTrue();
            validation.Errors.Should().BeEmpty();
        }

        [Fact]
        public void HelpSystem_ShouldHandleInvalidHelpRequest()
        {
            // Arrange
            var helpSystem = new HelpSystem();
            var invalidRequest = new HelpRequest
            {
                Topic = "", // Invalid empty topic
                SearchQuery = "", // Invalid empty search
                Category = "Invalid" // Invalid category
            };

            // Act
            var validation = helpSystem.ValidateHelpRequest(invalidRequest);

            // Assert
            validation.Should().NotBeNull();
            validation.IsValid.Should().BeFalse();
            validation.Errors.Should().NotBeEmpty();
        }
    }
}
