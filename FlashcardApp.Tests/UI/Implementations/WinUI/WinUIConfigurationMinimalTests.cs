using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for minimal Configuration page functionality
    /// </summary>
    public class WinUIConfigurationMinimalTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveMinimalStructure_WhenCreated()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Basic structure
            configurationPageXamlContent.Should().Contain("x:Class=\"FlashcardApp.WinUI.Views.ConfigurationPage\"", "Should have correct class declaration");
            configurationPageXamlContent.Should().Contain("xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"", "Should have XAML namespace");
            configurationPageXamlContent.Should().Contain("xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"", "Should have XAML x namespace");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveBasicLayout_WhenCreated()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Basic layout structure
            configurationPageXamlContent.Should().Contain("<Grid>", "Should have Grid layout");
            configurationPageXamlContent.Should().Contain("<Grid.RowDefinitions>", "Should have row definitions");
            configurationPageXamlContent.Should().Contain("<RowDefinition Height=\"Auto\" />", "Should have Auto height row");
            configurationPageXamlContent.Should().Contain("<RowDefinition Height=\"*\" />", "Should have star height row");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveHeader_WhenCreated()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Header elements
            configurationPageXamlContent.Should().Contain("Configuration Header", "Should have header comment");
            configurationPageXamlContent.Should().Contain("⚙️ Configuration", "Should have configuration title");
            configurationPageXamlContent.Should().Contain("x:Name=\"BackButton\"", "Should have back button");
            configurationPageXamlContent.Should().Contain("← Back", "Should have back button text");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveContentArea_WhenCreated()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Content area
            configurationPageXamlContent.Should().Contain("<ScrollViewer", "Should have ScrollViewer for content");
            configurationPageXamlContent.Should().Contain("<StackPanel", "Should have StackPanel for content");
            configurationPageXamlContent.Should().Contain("Configuration Settings", "Should have settings title");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveStatusBar_WhenCreated()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Status bar
            configurationPageXamlContent.Should().Contain("Status Bar", "Should have status bar comment");
            configurationPageXamlContent.Should().Contain("x:Name=\"StatusText\"", "Should have status text element");
            configurationPageXamlContent.Should().Contain("Ready", "Should have default status text");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveCodeBehindStructure_WhenCreated()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Code-behind structure
            configurationPageCodeBehindContent.Should().Contain("public partial class ConfigurationPage : Page", "Should inherit from Page");
            configurationPageCodeBehindContent.Should().Contain("public ConfigurationPage()", "Should have constructor");
            configurationPageCodeBehindContent.Should().Contain("this.InitializeComponent()", "Should call InitializeComponent");
            configurationPageCodeBehindContent.Should().Contain("BackButton_Click", "Should have back button click handler");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveErrorHandling_WhenCreated()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Error handling
            configurationPageCodeBehindContent.Should().Contain("try", "Should have try block");
            configurationPageCodeBehindContent.Should().Contain("catch", "Should have catch block");
            configurationPageCodeBehindContent.Should().Contain("System.Diagnostics.Debug.WriteLine", "Should have debug logging");
        }
    }
}
