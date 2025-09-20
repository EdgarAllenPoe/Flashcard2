using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests to fix the "Window.Current is null" issue in WinUI file pickers.
    /// The problem is that Window.Current returns null, so we need an alternative approach.
    /// </summary>
    public class WinUIWindowCurrentFixTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldUseAppMainWindow_InsteadOfWindowCurrent()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we use App.MainWindow instead of Window.Current
            codeBehindContent.Should().Contain("App.MainWindow", "Should use App.MainWindow for window reference");
            codeBehindContent.Should().NotContain("Window.Current", "Should not use Window.Current as it returns null in WinUI 3");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldUseAlternativeWindowRetrieval_WhenWindowCurrentFails()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we use an alternative method to get the window
            // In WinUI, we can get the window from the Page's XamlRoot or use a different approach
            codeBehindContent.Should().Contain("GetWindowHandle", "Should still get window handle");
            codeBehindContent.Should().Contain("InitializeWithWindow", "Should still initialize with window");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void FilePickerInitialization_ShouldHandleNullWindowGracefully_WhenWindowRetrievalFails()
        {
            // Arrange & Act
            var codeBehindContent = File.ReadAllText("../../../../Views/DeckManagementPage.xaml.cs");

            // Assert - Check that we have proper null handling
            codeBehindContent.Should().Contain("if (window != null)", "Should check for null window");
            codeBehindContent.Should().Contain("ERROR: App.MainWindow is null!", "Should show specific error message for App.MainWindow");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void AppClass_ShouldHaveMainWindowProperty_ForWindowAccess()
        {
            // Arrange & Act
            var appContent = File.ReadAllText("../../../../App.xaml.cs");

            // Assert - Check that App class has MainWindow property
            appContent.Should().Contain("public static Window MainWindow", "Should have public static MainWindow property");
            appContent.Should().Contain("MainWindow = window", "Should assign window to MainWindow property");
        }
    }
}
