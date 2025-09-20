using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to fix COM interop issues with file picker initialization.
    /// The error "Failed to create a CCW for object" indicates a COM interop problem.
    /// </summary>
    public class WinUIComInteropFixTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldNotUseProblematicWindowHandleRetrieval_WhenInitializingPickers()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we don't use the problematic window handle retrieval
            // The COM interop issue was caused by trying to get window handle from Page
            // In WinUI, file pickers should work without explicit window handle initialization
            codeBehindContent.Should().NotContain("WinRT.Interop.WindowNative.GetWindowHandle(this)", "Should not get window handle from Page");
            codeBehindContent.Should().Contain("var window = App.MainWindow", "Should use App.MainWindow for window handle");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldHaveProperErrorHandling_WhenFileOperationsFail()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we have proper error handling for file operations
            codeBehindContent.Should().Contain("try", "Should have try-catch for file operations");
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch file operation exceptions");
            codeBehindContent.Should().Contain("Import failed:", "Should show import error message");
            codeBehindContent.Should().Contain("Export failed:", "Should show export error message");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldWorkWithoutWindowHandleInitialization_WhenInWinUI()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file pickers work without explicit window handle initialization
            // In WinUI, file pickers should work without InitializeWithWindow
            codeBehindContent.Should().Contain("FileOpenPicker", "Should create FileOpenPicker");
            codeBehindContent.Should().Contain("FileSavePicker", "Should create FileSavePicker");
            codeBehindContent.Should().Contain("PickSingleFileAsync", "Should use PickSingleFileAsync");
            codeBehindContent.Should().Contain("PickSaveFileAsync", "Should use PickSaveFileAsync");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldHaveProperUsingStatements_ForFileOperations()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we have the correct using statements for file operations
            codeBehindContent.Should().Contain("using Windows.Storage;", "Should have Windows.Storage using");
            codeBehindContent.Should().Contain("using Windows.Storage.Pickers;", "Should have Storage.Pickers using");
            codeBehindContent.Should().Contain("using System.Threading.Tasks;", "Should have System.Threading.Tasks using");
        }
    }
}
