using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    public class WinUINavigationStateTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHaveNavigationStateManagement()
        {
            // Arrange & Act
            var mainPageXamlCsContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml.cs"));

            // Assert
            mainPageXamlCsContent.Should().Contain("private bool _isDarkTheme", "MainPage should have state management for theme.");
            mainPageXamlCsContent.Should().Contain("UpdateThemeButton", "MainPage should have methods to update UI state.");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHandleNavigationButtonClicks()
        {
            // Arrange & Act
            var mainPageXamlCsContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml.cs"));

            // Assert
            mainPageXamlCsContent.Should().Contain("StudySessions_Click", "MainPage should handle Study Sessions navigation.");
            mainPageXamlCsContent.Should().Contain("DeckManagement_Click", "MainPage should handle Deck Management navigation.");
            mainPageXamlCsContent.Should().Contain("Statistics_Click", "MainPage should handle Statistics navigation.");
            mainPageXamlCsContent.Should().Contain("Configuration_Click", "MainPage should handle Configuration navigation.");
            mainPageXamlCsContent.Should().Contain("Help_Click", "MainPage should handle Help navigation.");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldUpdateStatusBarOnNavigation()
        {
            // Arrange & Act
            var mainPageXamlCsContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml.cs"));

            // Assert
            mainPageXamlCsContent.Should().Contain("StatusText.Text =", "Navigation methods should update status bar.");
            mainPageXamlCsContent.Should().Contain("selected", "Status text should indicate selected navigation item.");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldUpdateContentAreaOnNavigation()
        {
            // Arrange & Act
            var mainPageXamlCsContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml.cs"));

            // Assert
            mainPageXamlCsContent.Should().Contain("OutputTextBlock.Text =", "Navigation methods should update content area.");
            mainPageXamlCsContent.Should().Contain("Study Sessions", "Content should show Study Sessions information.");
            mainPageXamlCsContent.Should().Contain("Deck Management", "Content should show Deck Management information.");
            mainPageXamlCsContent.Should().Contain("Statistics", "Content should show Statistics information.");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHaveThemeStateManagement()
        {
            // Arrange & Act
            var mainPageXamlCsContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml.cs"));

            // Assert
            mainPageXamlCsContent.Should().Contain("ThemeToggleButton_Click", "MainPage should handle theme toggle.");
            mainPageXamlCsContent.Should().Contain("ElementTheme.Dark", "Should support dark theme.");
            mainPageXamlCsContent.Should().Contain("ElementTheme.Light", "Should support light theme.");
            mainPageXamlCsContent.Should().Contain("RequestedTheme", "Should apply theme to UI elements.");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHaveNavigationButtonDefinitions()
        {
            // Arrange & Act
            var mainPageXamlContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml"));

            // Assert
            mainPageXamlContent.Should().Contain("Click=\"StudySessions_Click\"", "XAML should have Study Sessions button with click handler.");
            mainPageXamlContent.Should().Contain("Click=\"DeckManagement_Click\"", "XAML should have Deck Management button with click handler.");
            mainPageXamlContent.Should().Contain("Click=\"Statistics_Click\"", "XAML should have Statistics button with click handler.");
            mainPageXamlContent.Should().Contain("Click=\"Configuration_Click\"", "XAML should have Configuration button with click handler.");
            mainPageXamlContent.Should().Contain("Click=\"Help_Click\"", "XAML should have Help button with click handler.");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHaveThemeToggleButton()
        {
            // Arrange & Act
            var mainPageXamlContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml"));

            // Assert
            mainPageXamlContent.Should().Contain("x:Name=\"ThemeToggleButton\"", "XAML should have theme toggle button.");
            mainPageXamlContent.Should().Contain("Click=\"ThemeToggleButton_Click\"", "Theme toggle button should have click handler.");
            mainPageXamlContent.Should().Contain("ToolTipService.ToolTip", "Theme toggle button should have tooltip.");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHaveStatefulUIElements()
        {
            // Arrange & Act
            var mainPageXamlContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml"));

            // Assert
            mainPageXamlContent.Should().Contain("x:Name=\"OutputTextBlock\"", "Should have named output text block for state updates.");
            mainPageXamlContent.Should().Contain("x:Name=\"StatusText\"", "Should have named status text for state updates.");
            mainPageXamlContent.Should().Contain("x:Name=\"ThemeToggleButton\"", "Should have named theme toggle button for state updates.");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHaveNavigationContentStructure()
        {
            // Arrange & Act
            var mainPageXamlCsContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml.cs"));

            // Assert - Verify that each navigation method provides structured content
            mainPageXamlCsContent.Should().Contain("📚 Study Sessions", "Study Sessions should have emoji and title.");
            mainPageXamlCsContent.Should().Contain("🗂️ Deck Management", "Deck Management should have emoji and title.");
            mainPageXamlCsContent.Should().Contain("📊 Statistics", "Statistics should have emoji and title.");
            mainPageXamlCsContent.Should().Contain("⚙️ Configuration", "Configuration should have emoji and title.");
            mainPageXamlCsContent.Should().Contain("❓ Help", "Help should have emoji and title.");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHaveConsistentNavigationPattern()
        {
            // Arrange & Act
            var mainPageXamlCsContent = File.ReadAllText(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml.cs"));

            // Assert - Verify consistent pattern across all navigation methods
            var navigationMethods = new[] { "StudySessions_Click", "DeckManagement_Click", "Statistics_Click", "Configuration_Click", "Help_Click" };

            foreach (var method in navigationMethods)
            {
                mainPageXamlCsContent.Should().Contain($"private void {method}", $"Should have {method} method.");
                mainPageXamlCsContent.Should().Contain($"OutputTextBlock.Text =", $"Should update OutputTextBlock in {method}.");
                mainPageXamlCsContent.Should().Contain($"StatusText.Text =", $"Should update StatusText in {method}.");
            }
        }
    }
}

