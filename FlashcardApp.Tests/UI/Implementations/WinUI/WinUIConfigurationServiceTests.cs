using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for ConfigurationService integration in Configuration page
    /// </summary>
    public class WinUIConfigurationServiceTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveConfigurationService_WhenServiceIntegrationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - ConfigurationService integration
            configurationPageCodeBehindContent.Should().Contain("ConfigurationService", "Should have ConfigurationService field");
            configurationPageCodeBehindContent.Should().Contain("_configurationService", "Should have private ConfigurationService field");
            configurationPageCodeBehindContent.Should().Contain("new ConfigurationService()", "Should instantiate ConfigurationService");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveAppConfiguration_WhenServiceIntegrationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - AppConfiguration integration
            configurationPageCodeBehindContent.Should().Contain("AppConfiguration", "Should have AppConfiguration field");
            configurationPageCodeBehindContent.Should().Contain("_currentConfiguration", "Should have private AppConfiguration field");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDataContext_WhenServiceIntegrationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - DataContext setup
            configurationPageCodeBehindContent.Should().Contain("this.DataContext = this", "Should set DataContext for data binding");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveLoadConfiguration_WhenServiceIntegrationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - LoadConfiguration method
            configurationPageCodeBehindContent.Should().Contain("LoadConfiguration", "Should have LoadConfiguration method");
            configurationPageCodeBehindContent.Should().Contain("_configurationService.GetConfiguration()", "Should call GetConfiguration");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveSaveConfiguration_WhenServiceIntegrationIsAdded()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - SaveConfiguration integration
            configurationPageCodeBehindContent.Should().Contain("_configurationService.SaveConfiguration()", "Should call SaveConfiguration");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHavePageLoadedEvent_WhenServiceIntegrationIsAdded()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Page_Loaded event
            configurationPageXamlContent.Should().Contain("Loaded=\"Page_Loaded\"", "Should have Page_Loaded event in XAML");
            configurationPageCodeBehindContent.Should().Contain("Page_Loaded", "Should have Page_Loaded event handler");
            configurationPageCodeBehindContent.Should().Contain("LoadConfiguration()", "Should call LoadConfiguration in Page_Loaded");
        }
    }
}
