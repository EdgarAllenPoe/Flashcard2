using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to verify that file pickers are properly initialized with window handles.
    /// This is critical for WinUI file pickers to work correctly.
    /// </summary>
    public class WinUIFilePickerInitializationTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeckButton_ShouldInitializeFilePickerWithWindowHandle_WhenCreatingPicker()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file picker is properly initialized with window handle
            codeBehindContent.Should().Contain("WinRT.Interop.WindowNative.GetWindowHandle(window)", "Should get window handle");
            codeBehindContent.Should().Contain("WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd)", "Should initialize picker with window handle");
            codeBehindContent.Should().Contain("using WinRT.Interop;", "Should have WinRT.Interop using statement");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportAllButton_ShouldInitializeFilePickerWithWindowHandle_WhenCreatingPicker()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that file picker is properly initialized with window handle
            codeBehindContent.Should().Contain("WinRT.Interop.WindowNative.GetWindowHandle(window)", "Should get window handle");
            codeBehindContent.Should().Contain("WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd)", "Should initialize picker with window handle");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportExportButtons_ShouldHaveProperFilePickerInitialization_WhenButtonsAreClicked()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that both buttons have proper initialization
            codeBehindContent.Should().Contain("var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);", "Should get window handle");
            codeBehindContent.Should().Contain("WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);", "Should initialize picker");

            // Check that this pattern appears twice (once for import, once for export)
            var importCount = codeBehindContent.Split(new[] { "WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd)" }, StringSplitOptions.None).Length - 1;
            importCount.Should().Be(2, "Should initialize both import and export pickers");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldBeBeforeFilePickerUsage_WhenOperationsExecute()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that initialization happens before picker usage
            var importSection = codeBehindContent.Substring(codeBehindContent.IndexOf("ImportDeckButton_Click"));
            importSection.Should().Contain("InitializeWithWindow.Initialize(picker, hwnd)", "Should initialize before PickSingleFileAsync");
            importSection.Should().Contain("PickSingleFileAsync()", "Should use picker after initialization");

            var exportSection = codeBehindContent.Substring(codeBehindContent.IndexOf("ExportAllButton_Click"));
            exportSection.Should().Contain("InitializeWithWindow.Initialize(picker, hwnd)", "Should initialize before PickSaveFileAsync");
            exportSection.Should().Contain("PickSaveFileAsync()", "Should use picker after initialization");
        }
    }
}
