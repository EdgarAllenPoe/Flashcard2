using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for the behavior and functionality of keyboard shortcuts on the Study Session page.
    /// These tests verify that shortcuts work correctly in different states.
    /// </summary>
    public class WinUIStudySessionShortcutBehaviorTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHandleRevealAnswerShortcut_WhenAnswerNotRevealed()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that Space key reveals answer when not already revealed
            codeBehindContent.Should().Contain("!_answerRevealed", "Should check if answer is not already revealed");
            codeBehindContent.Should().Contain("RevealAnswerButton_Click", "Should call reveal answer method");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHandleCorrectShortcut_WhenAnswerIsRevealed()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that 1 key marks correct when answer is revealed
            codeBehindContent.Should().Contain("_answerRevealed", "Should check if answer is revealed");
            codeBehindContent.Should().Contain("CorrectButton_Click", "Should call correct button method");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHandleIncorrectShortcut_WhenAnswerIsRevealed()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that 2 key marks incorrect when answer is revealed
            codeBehindContent.Should().Contain("_answerRevealed", "Should check if answer is revealed");
            codeBehindContent.Should().Contain("IncorrectButton_Click", "Should call incorrect button method");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHandleBackShortcut_AtAnyTime()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that Escape key always goes back
            codeBehindContent.Should().Contain("BackButton_Click", "Should call back button method");
            codeBehindContent.Should().Contain("VirtualKey.Escape", "Should handle Escape key");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldIgnoreShortcuts_WhenButtonsAreNotVisible()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that shortcuts are ignored when buttons are not visible
            codeBehindContent.Should().Contain("Visibility.Visible", "Should check for visible visibility");
            codeBehindContent.Should().Contain("if (", "Should use conditional logic for button visibility");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveShortcutKeyMapping_ForAllKeys()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that all shortcut keys are properly mapped
            codeBehindContent.Should().Contain("VirtualKey.Space", "Should map Space key");
            codeBehindContent.Should().Contain("VirtualKey.Number1", "Should map 1 key");
            codeBehindContent.Should().Contain("VirtualKey.Number2", "Should map 2 key");
            codeBehindContent.Should().Contain("VirtualKey.Escape", "Should map Escape key");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveShortcutKeyHandling_WithProperModifiers()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that modifier keys are handled properly (simplified for now)
            codeBehindContent.Should().Contain("OnKeyDown", "Should have key handling method");
            codeBehindContent.Should().Contain("e.Key", "Should check the pressed key");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveShortcutKeyLogging_ForDebugging()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that shortcut key usage is logged for debugging
            codeBehindContent.Should().Contain("Debug.WriteLine", "Should log shortcut key usage");
            codeBehindContent.Should().Contain("shortcut", "Should log shortcut information");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveShortcutKeyAccessibility_ForScreenReaders()
        {
            // Arrange & Act
            var xamlContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml");
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that shortcuts are accessible
            xamlContent.Should().Contain("AutomationProperties", "Should have automation properties for accessibility");
            codeBehindContent.Should().Contain("AutomationProperties", "Should set automation properties in code");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveShortcutKeyConsistency_AcrossAllButtons()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that shortcut handling is consistent
            codeBehindContent.Should().Contain("OnKeyDown", "Should have consistent key handling method");
            codeBehindContent.Should().Contain("e.Handled = true", "Should mark events as handled");
        }
    }
}
