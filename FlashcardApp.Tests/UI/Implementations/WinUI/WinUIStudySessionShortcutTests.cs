using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for keyboard shortcuts on the Study Session page.
    /// Following TDD methodology to add shortcut key functionality.
    /// </summary>
    public class WinUIStudySessionShortcutTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveKeyboardShortcuts_ForAllActionButtons()
        {
            // Arrange & Act
            var xamlContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml");
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that keyboard shortcuts are implemented
            codeBehindContent.Should().Contain("KeyDown", "Should handle KeyDown events for shortcuts");
            codeBehindContent.Should().Contain("VirtualKey.Space", "Should have Space key shortcut for Reveal Answer");
            codeBehindContent.Should().Contain("VirtualKey.Number1", "Should have 1 key shortcut for Correct");
            codeBehindContent.Should().Contain("VirtualKey.Number2", "Should have 2 key shortcut for Incorrect");
            codeBehindContent.Should().Contain("VirtualKey.Escape", "Should have Escape key shortcut for Back");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveKeyDownEventHandler_ForKeyboardShortcuts()
        {
            // Arrange & Act
            var xamlContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml");
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that KeyDown event is properly wired up
            xamlContent.Should().Contain("KeyDown=", "Should have KeyDown event handler in XAML");
            codeBehindContent.Should().Contain("OnKeyDown", "Should have OnKeyDown method in code-behind");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveShortcutKeyConstants_ForMaintainability()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that shortcut keys are defined as constants
            codeBehindContent.Should().Contain("const", "Should have constants for shortcut keys");
            codeBehindContent.Should().Contain("REVEAL_ANSWER_KEY", "Should have constant for reveal answer key");
            codeBehindContent.Should().Contain("CORRECT_KEY", "Should have constant for correct key");
            codeBehindContent.Should().Contain("INCORRECT_KEY", "Should have constant for incorrect key");
            codeBehindContent.Should().Contain("BACK_KEY", "Should have constant for back key");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHandleShortcutsOnlyWhenButtonsAreVisible()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that shortcuts respect button visibility
            codeBehindContent.Should().Contain("Visibility.Visible", "Should check button visibility before handling shortcuts");
            codeBehindContent.Should().Contain("RevealAnswerButton.Visibility", "Should check reveal answer button visibility");
            codeBehindContent.Should().Contain("CorrectButton.Visibility", "Should check correct button visibility");
            codeBehindContent.Should().Contain("IncorrectButton.Visibility", "Should check incorrect button visibility");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveShortcutTooltips_ForUserGuidance()
        {
            // Arrange & Act
            var xamlContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml");

            // Assert - Check that buttons have shortcut information in tooltips
            xamlContent.Should().Contain("ToolTipService.ToolTip", "Should have tooltips for buttons");
            xamlContent.Should().Contain("(Space)", "Should show Space key shortcut in tooltip");
            xamlContent.Should().Contain("(1)", "Should show 1 key shortcut in tooltip");
            xamlContent.Should().Contain("(2)", "Should show 2 key shortcut in tooltip");
            xamlContent.Should().Contain("(Esc)", "Should show Escape key shortcut in tooltip");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveKeyboardFocusManagement_ForShortcuts()
        {
            // Arrange & Act
            var xamlContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml");
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that keyboard focus is properly managed
            codeBehindContent.Should().Contain("Focus", "Should manage keyboard focus for shortcuts");
            xamlContent.Should().Contain("IsTabStop", "Should configure tab stop for keyboard navigation");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveShortcutKeyValidation_ForInvalidKeys()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that invalid keys are handled gracefully
            codeBehindContent.Should().Contain("switch", "Should use switch statement for key handling");
            codeBehindContent.Should().Contain("default:", "Should have default case for unhandled keys");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void StudySessionPage_ShouldHaveShortcutKeyDocumentation_ForMaintainability()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/StudySessionPage.xaml.cs");

            // Assert - Check that shortcut keys are documented
            codeBehindContent.Should().Contain("/// <summary>", "Should have XML documentation");
            codeBehindContent.Should().Contain("keyboard shortcut", "Should document keyboard shortcuts");
            codeBehindContent.Should().Contain("Space - Reveal Answer", "Should document Space key shortcut");
            codeBehindContent.Should().Contain("1 - Correct", "Should document 1 key shortcut");
            codeBehindContent.Should().Contain("2 - Incorrect", "Should document 2 key shortcut");
            codeBehindContent.Should().Contain("Escape - Back", "Should document Escape key shortcut");
        }
    }
}
