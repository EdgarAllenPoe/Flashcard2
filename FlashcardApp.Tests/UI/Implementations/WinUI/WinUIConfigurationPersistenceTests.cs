using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    /// <summary>
    /// Tests for configuration persistence functionality in Configuration page
    /// </summary>
    public class WinUIConfigurationPersistenceTests
    {
        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveAutoSaveOnChange_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Auto-save functionality
            configurationPageCodeBehindContent.Should().Contain("private void SaveConfigurationOnChange()", "Should have SaveConfigurationOnChange method");
            configurationPageCodeBehindContent.Should().Contain("_configurationService.SaveConfiguration()", "Should call SaveConfiguration for persistence");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveControlValueChangedEvents_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Control value changed events
            configurationPageCodeBehindContent.Should().Contain("NumberOfBoxesSlider.ValueChanged", "Should have NumberOfBoxesSlider value changed event");
            configurationPageCodeBehindContent.Should().Contain("MaxCardsPerDaySlider.ValueChanged", "Should have MaxCardsPerDaySlider value changed event");
            configurationPageCodeBehindContent.Should().Contain("MaxStudyTimeSlider.ValueChanged", "Should have MaxStudyTimeSlider value changed event");
            configurationPageCodeBehindContent.Should().Contain("AutoAdvanceDelaySlider.ValueChanged", "Should have AutoAdvanceDelaySlider value changed event");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveCheckBoxChangedEvents_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - CheckBox changed events
            configurationPageCodeBehindContent.Should().Contain("AutoAdvanceCheckBox.Checked", "Should have AutoAdvanceCheckBox checked event");
            configurationPageCodeBehindContent.Should().Contain("AutoAdvanceCheckBox.Unchecked", "Should have AutoAdvanceCheckBox unchecked event");
            configurationPageCodeBehindContent.Should().Contain("ShowStatisticsCheckBox.Checked", "Should have ShowStatisticsCheckBox checked event");
            configurationPageCodeBehindContent.Should().Contain("ShowStatisticsCheckBox.Unchecked", "Should have ShowStatisticsCheckBox unchecked event");
            configurationPageCodeBehindContent.Should().Contain("ShowProgressCheckBox.Checked", "Should have ShowProgressCheckBox checked event");
            configurationPageCodeBehindContent.Should().Contain("ShowProgressCheckBox.Unchecked", "Should have ShowProgressCheckBox unchecked event");
            configurationPageCodeBehindContent.Should().Contain("ShuffleCardsCheckBox.Checked", "Should have ShuffleCardsCheckBox checked event");
            configurationPageCodeBehindContent.Should().Contain("ShuffleCardsCheckBox.Unchecked", "Should have ShuffleCardsCheckBox unchecked event");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveTextBoxTextChangedEvents_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - TextBox text changed events
            configurationPageCodeBehindContent.Should().Contain("ReviewIntervalTextBox.TextChanged", "Should have ReviewIntervalTextBox text changed event");
            configurationPageCodeBehindContent.Should().Contain("DeckStoragePathTextBox.TextChanged", "Should have DeckStoragePathTextBox text changed event");
            configurationPageCodeBehindContent.Should().Contain("BackupDirectoryTextBox.TextChanged", "Should have BackupDirectoryTextBox text changed event");
            configurationPageCodeBehindContent.Should().Contain("ExportPathTextBox.TextChanged", "Should have ExportPathTextBox text changed event");
            configurationPageCodeBehindContent.Should().Contain("ImportPathTextBox.TextChanged", "Should have ImportPathTextBox text changed event");
            configurationPageCodeBehindContent.Should().Contain("LogFilePathTextBox.TextChanged", "Should have LogFilePathTextBox text changed event");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveComboBoxSelectionChangedEvents_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - ComboBox selection changed events
            configurationPageCodeBehindContent.Should().Contain("StudyModeComboBox.SelectionChanged", "Should have StudyModeComboBox selection changed event");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveDebouncedSave_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Debounced save functionality
            configurationPageCodeBehindContent.Should().Contain("private System.Threading.Timer _saveTimer", "Should have save timer for debouncing");
            configurationPageCodeBehindContent.Should().Contain("_saveTimer?.Dispose()", "Should dispose save timer");
            configurationPageCodeBehindContent.Should().Contain("_saveTimer = new System.Threading.Timer", "Should create new save timer");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveSaveUIToConfiguration_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Save UI to configuration method
            configurationPageCodeBehindContent.Should().Contain("private void SaveUIToConfiguration()", "Should have SaveUIToConfiguration method");
            configurationPageCodeBehindContent.Should().Contain("_currentConfiguration.LeitnerBoxes.NumberOfBoxes = (int)NumberOfBoxesSlider.Value", "Should save NumberOfBoxes from slider");
            configurationPageCodeBehindContent.Should().Contain("_currentConfiguration.DailyLimits.MaxCardsPerDay = (int)MaxCardsPerDaySlider.Value", "Should save MaxCardsPerDay from slider");
            configurationPageCodeBehindContent.Should().Contain("_currentConfiguration.StudySession.AutoAdvance = AutoAdvanceCheckBox.IsChecked == true", "Should save AutoAdvance from checkbox");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveAutoSaveStatusUpdates_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Auto-save status updates
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Configuration auto-saved\")", "Should show auto-save status");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Configuration saved successfully\")", "Should show save status");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHaveErrorHandlingForAutoSave_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageCodeBehindContent = TestDataProvider.Xaml.ConfigurationPageCodeBehind;

            // Assert - Error handling for auto-save
            configurationPageCodeBehindContent.Should().Contain("catch (Exception ex)", "Should have exception handling in auto-save");
            configurationPageCodeBehindContent.Should().Contain("UpdateStatus(\"Error auto-saving configuration\")", "Should show error for auto-save failures");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ConfigurationPage_ShouldHavePageUnloadedCleanup_WhenPersistenceIsImplemented()
        {
            // Arrange & Act
            var configurationPageXamlContent = TestDataProvider.Xaml.ConfigurationPage;

            // Assert - Page unloaded event for cleanup
            configurationPageXamlContent.Should().Contain("Unloaded=\"Page_Unloaded\"", "Should have Page_Unloaded event in XAML");
            TestDataProvider.Xaml.ConfigurationPageCodeBehind.Should().Contain("private void Page_Unloaded(object sender, RoutedEventArgs e)", "Should have Page_Unloaded event handler");
            TestDataProvider.Xaml.ConfigurationPageCodeBehind.Should().Contain("_saveTimer?.Dispose()", "Should dispose save timer on page unload");
        }
    }
}
