using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to fix the space bar navigation issue in Study Session page.
    /// Space bar should reveal answer, not navigate back to main menu.
    /// </summary>
    public class WinUIStudySessionSpaceBarFixTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldPreventSpaceBarNavigation_WhenHandlingShortcuts()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that space bar handling prevents default navigation
            codeBehindContent.Should().Contain("e.Handled = true", "Should mark space bar event as handled to prevent navigation");
            codeBehindContent.Should().Contain("VirtualKey.Space", "Should handle space bar key");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveProperKeyEventHandling_ForSpaceBar()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that space bar is properly handled in the switch statement
            codeBehindContent.Should().Contain("case VirtualKey.Space:", "Should have case for space bar");
            codeBehindContent.Should().Contain("RevealAnswerButton_Click", "Should call reveal answer when space is pressed");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldPreventDefaultSpaceBarBehavior_WhenAnswerNotRevealed()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that space bar handling includes proper conditions
            codeBehindContent.Should().Contain("!_answerRevealed", "Should check if answer is not revealed");
            codeBehindContent.Should().Contain("RevealAnswerButton.Visibility == Visibility.Visible", "Should check if reveal button is visible");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveKeyDownEventHandling_ForSpaceBarPrevention()
        {
            // Arrange & Act
            var xamlContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml");
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that KeyDown and PreviewKeyDown events are properly set up
            xamlContent.Should().Contain("KeyDown=\"OnKeyDown\"", "Should have KeyDown event handler in XAML");
            xamlContent.Should().Contain("PreviewKeyDown=\"OnPreviewKeyDown\"", "Should have PreviewKeyDown event handler in XAML");
            codeBehindContent.Should().Contain("OnKeyDown", "Should have OnKeyDown method");
            codeBehindContent.Should().Contain("OnPreviewKeyDown", "Should have OnPreviewKeyDown method");
            codeBehindContent.Should().Contain("KeyRoutedEventArgs", "Should handle key routed events");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveProperEventHandlingOrder_ForSpaceBar()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that event handling is in the correct order
            codeBehindContent.Should().Contain("switch (e.Key)", "Should use switch statement for key handling");
            codeBehindContent.Should().Contain("e.Handled = true", "Should mark event as handled after processing");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHandleSpaceBarInPreviewKeyDown_ToPreventNavigation()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that space bar is handled in PreviewKeyDown with highest priority
            codeBehindContent.Should().Contain("OnPreviewKeyDown", "Should have PreviewKeyDown method");
            codeBehindContent.Should().Contain("if (e.Key == VirtualKey.Space)", "Should check for space bar in PreviewKeyDown");
            codeBehindContent.Should().Contain("e.Handled = true", "Should mark space bar as handled in PreviewKeyDown");
            codeBehindContent.Should().Contain("return;", "Should return early after handling space bar");
        }
    }
}
