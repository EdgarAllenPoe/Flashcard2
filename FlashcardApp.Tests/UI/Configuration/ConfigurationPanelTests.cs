using FluentAssertions;
using FlashcardApp.UI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlashcardApp.Tests.UI.Configuration
{
    /// <summary>
    /// Tests for the intuitive settings panel with grouped options and live preview
    /// </summary>
    public class ConfigurationPanelTests
    {
        [Fact]
        public void ConfigurationPanel_ShouldDefineSettingGroups()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var settingGroups = configurationPanel.GetSettingGroups();

            // Assert
            settingGroups.Should().NotBeNull();
            settingGroups.Should().Contain(group => group.Name == "General");
            settingGroups.Should().Contain(group => group.Name == "Study");
            settingGroups.Should().Contain(group => group.Name == "Appearance");
            settingGroups.Should().Contain(group => group.Name == "Accessibility");
            settingGroups.Should().Contain(group => group.Name == "Advanced");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineGeneralSettings()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var generalSettings = configurationPanel.GetGeneralSettings();

            // Assert
            generalSettings.Should().NotBeNull();
            generalSettings.Should().Contain(setting => setting.Name == "Language");
            generalSettings.Should().Contain(setting => setting.Name == "AutoSave");
            generalSettings.Should().Contain(setting => setting.Name == "Backup");
            generalSettings.Should().Contain(setting => setting.Name == "Notifications");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineStudySettings()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var studySettings = configurationPanel.GetStudySettings();

            // Assert
            studySettings.Should().NotBeNull();
            studySettings.Should().Contain(setting => setting.Name == "DefaultStudyMode");
            studySettings.Should().Contain(setting => setting.Name == "AutoFlip");
            studySettings.Should().Contain(setting => setting.Name == "SoundEffects");
            studySettings.Should().Contain(setting => setting.Name == "StudyReminders");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineAppearanceSettings()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var appearanceSettings = configurationPanel.GetAppearanceSettings();

            // Assert
            appearanceSettings.Should().NotBeNull();
            appearanceSettings.Should().Contain(setting => setting.Name == "Theme");
            appearanceSettings.Should().Contain(setting => setting.Name == "FontSize");
            appearanceSettings.Should().Contain(setting => setting.Name == "Animations");
            appearanceSettings.Should().Contain(setting => setting.Name == "ColorScheme");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineAccessibilitySettings()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var accessibilitySettings = configurationPanel.GetAccessibilitySettings();

            // Assert
            accessibilitySettings.Should().NotBeNull();
            accessibilitySettings.Should().Contain(setting => setting.Name == "HighContrast");
            accessibilitySettings.Should().Contain(setting => setting.Name == "ScreenReader");
            accessibilitySettings.Should().Contain(setting => setting.Name == "KeyboardNavigation");
            accessibilitySettings.Should().Contain(setting => setting.Name == "ReducedMotion");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineAdvancedSettings()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var advancedSettings = configurationPanel.GetAdvancedSettings();

            // Assert
            advancedSettings.Should().NotBeNull();
            advancedSettings.Should().Contain(setting => setting.Name == "DebugMode");
            advancedSettings.Should().Contain(setting => setting.Name == "LogLevel");
            advancedSettings.Should().Contain(setting => setting.Name == "CacheSize");
            advancedSettings.Should().Contain(setting => setting.Name == "PerformanceMode");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineLivePreview()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var livePreview = configurationPanel.GetLivePreview();

            // Assert
            livePreview.Should().NotBeNull();
            livePreview.IsEnabled.Should().BeTrue();
            livePreview.UpdateDelay.Should().BeGreaterThan(0);
            livePreview.PreviewAreas.Should().NotBeEmpty();
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineSettingValidation()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var validation = configurationPanel.GetSettingValidation();

            // Assert
            validation.Should().NotBeNull();
            validation.ValidationRules.Should().NotBeEmpty();
            validation.ValidationRules.Should().Contain(rule => rule.SettingName == "FontSize");
            validation.ValidationRules.Should().Contain(rule => rule.SettingName == "CacheSize");
            validation.ValidationRules.Should().Contain(rule => rule.SettingName == "LogLevel");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineSettingReset()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var resetOptions = configurationPanel.GetSettingReset();

            // Assert
            resetOptions.Should().NotBeNull();
            resetOptions.Should().Contain(option => option.Type == "ResetAll");
            resetOptions.Should().Contain(option => option.Type == "ResetGroup");
            resetOptions.Should().Contain(option => option.Type == "ResetSingle");
            resetOptions.Should().Contain(option => option.Type == "ResetToDefault");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineSettingImportExport()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var importExport = configurationPanel.GetSettingImportExport();

            // Assert
            importExport.Should().NotBeNull();
            importExport.Should().Contain(option => option.Type == "Export");
            importExport.Should().Contain(option => option.Type == "Import");
            importExport.Should().Contain(option => option.Type == "Backup");
            importExport.Should().Contain(option => option.Type == "Restore");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineSettingSearch()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var search = configurationPanel.GetSettingSearch();

            // Assert
            search.Should().NotBeNull();
            search.IsEnabled.Should().BeTrue();
            search.Placeholder.Should().NotBeNullOrEmpty();
            search.SearchFields.Should().NotBeEmpty();
            search.SearchFields.Should().Contain("Name");
            search.SearchFields.Should().Contain("Description");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineSettingCategories()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var categories = configurationPanel.GetSettingCategories();

            // Assert
            categories.Should().NotBeNull();
            categories.Should().Contain(category => category.Name == "User Interface");
            categories.Should().Contain(category => category.Name == "Study Behavior");
            categories.Should().Contain(category => category.Name == "Data Management");
            categories.Should().Contain(category => category.Name == "System");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineSettingDependencies()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var dependencies = configurationPanel.GetSettingDependencies();

            // Assert
            dependencies.Should().NotBeNull();
            dependencies.Should().Contain(dep => dep.SettingName == "Animations");
            dependencies.Should().Contain(dep => dep.SettingName == "SoundEffects");
            dependencies.Should().Contain(dep => dep.SettingName == "HighContrast");
        }

        [Fact]
        public void ConfigurationPanel_ShouldDefineSettingHelp()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();

            // Act
            var help = configurationPanel.GetSettingHelp();

            // Assert
            help.Should().NotBeNull();
            help.HelpText.Should().ContainKey("General");
            help.HelpText.Should().ContainKey("Study");
            help.HelpText.Should().ContainKey("Appearance");
            help.HelpText.Should().ContainKey("Accessibility");
        }

        [Fact]
        public void ConfigurationPanel_ShouldValidateConfiguration()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();
            var config = new ConfigurationData
            {
                Language = "en-US",
                Theme = "Light",
                FontSize = 14,
                AutoSave = true
            };

            // Act
            var validation = configurationPanel.ValidateConfiguration(config);

            // Assert
            validation.Should().NotBeNull();
            validation.IsValid.Should().BeTrue();
            validation.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ConfigurationPanel_ShouldHandleInvalidConfiguration()
        {
            // Arrange
            var configurationPanel = new ConfigurationPanel();
            var invalidConfig = new ConfigurationData
            {
                Language = "invalid", // Invalid language code
                Theme = "Invalid", // Invalid theme
                FontSize = -5, // Invalid negative font size
                AutoSave = true
            };

            // Act
            var validation = configurationPanel.ValidateConfiguration(invalidConfig);

            // Assert
            validation.Should().NotBeNull();
            validation.IsValid.Should().BeFalse();
            validation.Errors.Should().NotBeEmpty();
        }
    }
}
