using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to ensure proper error capture and logging for debugging file picker issues.
    /// </summary>
    public class WinUIErrorCaptureTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerOperations_ShouldHaveDetailedErrorLogging_WhenErrorsOccur()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we have detailed error logging
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch exceptions");
            codeBehindContent.Should().Contain("ex.Message", "Should log exception message");
            codeBehindContent.Should().Contain("ex.StackTrace", "Should log stack trace for debugging");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerOperations_ShouldLogSpecificErrorDetails_WhenWindowHandleFails()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we log specific error details
            codeBehindContent.Should().Contain("ERROR: App.MainWindow is null!", "Should log window null errors");
            codeBehindContent.Should().Contain("Exception Type:", "Should log exception type");
            codeBehindContent.Should().Contain("Stack Trace:", "Should log stack trace");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerOperations_ShouldHaveTryCatchBlocks_ForAllFileOperations()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that both import and export have proper error handling
            codeBehindContent.Should().Contain("ImportDeckButton_Click", "Should have import button handler");
            codeBehindContent.Should().Contain("ExportAllButton_Click", "Should have export button handler");

            // Count try blocks to ensure both methods have error handling
            var tryCount = codeBehindContent.Split(new string[] { "try" }, StringSplitOptions.None).Length - 1;
            tryCount.Should().BeGreaterThanOrEqualTo(2, "Should have at least 2 try blocks for import and export");
        }
    }
}
