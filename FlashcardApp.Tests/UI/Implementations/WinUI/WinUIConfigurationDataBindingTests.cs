using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for data binding implementation in Configuration page
    /// </summary>
    public class WinUIConfigurationDataBindingTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldImplementINotifyPropertyChanged_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - INotifyPropertyChanged implementation
            configurationPageCodeBehindContent.Should().Contain("INotifyPropertyChanged", "Should implement INotifyPropertyChanged interface");
            configurationPageCodeBehindContent.Should().Contain("public event PropertyChangedEventHandler PropertyChanged", "Should have PropertyChanged event");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHavePropertyChangedMethod_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - PropertyChanged method
            configurationPageCodeBehindContent.Should().Contain("protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)", "Should have OnPropertyChanged method with CallerMemberName attribute");
            configurationPageCodeBehindContent.Should().Contain("PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))", "Should invoke PropertyChanged event");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveBoundProperties_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Bound properties for Leitner Box
            configurationPageCodeBehindContent.Should().Contain("public int NumberOfBoxes", "Should have NumberOfBoxes property");
            configurationPageCodeBehindContent.Should().Contain("public string ReviewIntervals", "Should have ReviewIntervals property");
            configurationPageCodeBehindContent.Should().Contain("OnPropertyChanged()", "Should call OnPropertyChanged in property setters");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDailyLimitsProperties_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Bound properties for Daily Limits
            configurationPageCodeBehindContent.Should().Contain("public int MaxCardsPerDay", "Should have MaxCardsPerDay property");
            configurationPageCodeBehindContent.Should().Contain("public int MaxStudyTimeMinutes", "Should have MaxStudyTimeMinutes property");
            configurationPageCodeBehindContent.Should().Contain("public int BreakIntervalMinutes", "Should have BreakIntervalMinutes property");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveStudySessionProperties_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Bound properties for Study Session
            configurationPageCodeBehindContent.Should().Contain("public bool AutoAdvance", "Should have AutoAdvance property");
            configurationPageCodeBehindContent.Should().Contain("public int AutoAdvanceDelay", "Should have AutoAdvanceDelay property");
            configurationPageCodeBehindContent.Should().Contain("public bool ShowStatistics", "Should have ShowStatistics property");
            configurationPageCodeBehindContent.Should().Contain("public bool ShowProgress", "Should have ShowProgress property");
            configurationPageCodeBehindContent.Should().Contain("public bool ShuffleCards", "Should have ShuffleCards property");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveFilePathProperties_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Bound properties for File Paths
            configurationPageCodeBehindContent.Should().Contain("public string DeckStoragePath", "Should have DeckStoragePath property");
            configurationPageCodeBehindContent.Should().Contain("public string BackupDirectory", "Should have BackupDirectory property");
            configurationPageCodeBehindContent.Should().Contain("public string ExportPath", "Should have ExportPath property");
            configurationPageCodeBehindContent.Should().Contain("public string ImportPath", "Should have ImportPath property");
            configurationPageCodeBehindContent.Should().Contain("public string LogFilePath", "Should have LogFilePath property");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveStudyModeProperty_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Bound property for Study Mode
            configurationPageCodeBehindContent.Should().Contain("public string StudyMode", "Should have StudyMode property");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHavePropertySetters_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Property setters with validation
            configurationPageCodeBehindContent.Should().Contain("if (_numberOfBoxes != value)", "Should have NumberOfBoxes setter with change detection");
            configurationPageCodeBehindContent.Should().Contain("_numberOfBoxes = value", "Should assign value to _numberOfBoxes");
            configurationPageCodeBehindContent.Should().Contain("OnPropertyChanged()", "Should call OnPropertyChanged in NumberOfBoxes setter");
            configurationPageCodeBehindContent.Should().Contain("if (_maxCardsPerDay != value)", "Should have MaxCardsPerDay setter with change detection");
            configurationPageCodeBehindContent.Should().Contain("_maxCardsPerDay = value", "Should assign value to _maxCardsPerDay");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveBackingFields_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Backing fields
            configurationPageCodeBehindContent.Should().Contain("private int _numberOfBoxes", "Should have _numberOfBoxes backing field");
            configurationPageCodeBehindContent.Should().Contain("private string _reviewIntervals", "Should have _reviewIntervals backing field");
            configurationPageCodeBehindContent.Should().Contain("private int _maxCardsPerDay", "Should have _maxCardsPerDay backing field");
            configurationPageCodeBehindContent.Should().Contain("private bool _autoAdvance", "Should have _autoAdvance backing field");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDataBindingSetup_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Data binding setup
            configurationPageCodeBehindContent.Should().Contain("this.DataContext = this", "Should set DataContext to this");
            configurationPageCodeBehindContent.Should().Contain("LoadConfigurationIntoProperties()", "Should call LoadConfigurationIntoProperties method");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveLoadConfigurationIntoProperties_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Load configuration into properties method
            configurationPageCodeBehindContent.Should().Contain("private void LoadConfigurationIntoProperties()", "Should have LoadConfigurationIntoProperties method");
            configurationPageCodeBehindContent.Should().Contain("NumberOfBoxes = _currentConfiguration.LeitnerBoxes.NumberOfBoxes", "Should load NumberOfBoxes from configuration");
            configurationPageCodeBehindContent.Should().Contain("MaxCardsPerDay = _currentConfiguration.DailyLimits.MaxCardsPerDay", "Should load MaxCardsPerDay from configuration");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveSavePropertiesToConfiguration_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Save properties to configuration method
            configurationPageCodeBehindContent.Should().Contain("private void SavePropertiesToConfiguration()", "Should have SavePropertiesToConfiguration method");
            configurationPageCodeBehindContent.Should().Contain("_currentConfiguration.LeitnerBoxes.NumberOfBoxes = NumberOfBoxes", "Should save NumberOfBoxes to configuration");
            configurationPageCodeBehindContent.Should().Contain("_currentConfiguration.DailyLimits.MaxCardsPerDay = MaxCardsPerDay", "Should save MaxCardsPerDay to configuration");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveUsingStatements_WhenDataBindingIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Required using statements
            configurationPageCodeBehindContent.Should().Contain("using System.ComponentModel", "Should have System.ComponentModel using statement");
            configurationPageCodeBehindContent.Should().Contain("using System.Runtime.CompilerServices", "Should have System.Runtime.CompilerServices using statement");
        }
    }
}
