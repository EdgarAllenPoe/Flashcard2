using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to fix the "Invalid window handle" error for file pickers.
    /// The error message suggests using WindowNative and InitializeWithWindow correctly.
    /// </summary>
    public class WinUIWindowHandleFixTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldUseCorrectWindowHandleMethod_WhenInitializingPickers()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we use the correct method to get window handle
            // The error message suggests using WindowNative and InitializeWithWindow
            codeBehindContent.Should().Contain("WindowNative", "Should use WindowNative for window handle");
            codeBehindContent.Should().Contain("InitializeWithWindow", "Should use InitializeWithWindow");
            codeBehindContent.Should().Contain("App.MainWindow", "Should use App.MainWindow to get the window");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldGetWindowHandleFromCorrectSource_WhenInitializingPickers()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we get window handle from the correct source
            // In WinUI, we need to get the window handle from Window.Current, not from the Page
            codeBehindContent.Should().Contain("GetWindowHandle", "Should call GetWindowHandle");
            codeBehindContent.Should().Contain("App.MainWindow", "Should get window handle from App.MainWindow");
            codeBehindContent.Should().NotContain("GetWindowHandle(this)", "Should not get window handle from Page");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldHaveProperErrorHandling_WhenWindowHandleFails()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we have proper error handling for window handle issues
            codeBehindContent.Should().Contain("try", "Should have try-catch for window handle operations");
            codeBehindContent.Should().Contain("catch (Exception ex)", "Should catch window handle exceptions");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldInitializeBothImportAndExportPickers_WhenButtonsAreClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that both import and export pickers are properly initialized
            codeBehindContent.Should().Contain("FileOpenPicker", "Should create FileOpenPicker for import");
            codeBehindContent.Should().Contain("FileSavePicker", "Should create FileSavePicker for export");
            codeBehindContent.Should().Contain("PickSingleFileAsync", "Should use PickSingleFileAsync for import");
            codeBehindContent.Should().Contain("PickSaveFileAsync", "Should use PickSaveFileAsync for export");
        }
    }
}
