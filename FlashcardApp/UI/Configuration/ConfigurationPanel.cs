using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.UI.Configuration
{
    /// <summary>
    /// Represents a setting group
    /// </summary>
    public class SettingGroup
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsExpanded { get; set; } = false;
        public int Order { get; set; } = 0;
    }

    /// <summary>
    /// Represents a configuration setting
    /// </summary>
    public class ConfigurationSetting
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public object Value { get; set; } = new object();
        public object DefaultValue { get; set; } = new object();
        public bool IsEnabled { get; set; } = true;
        public List<string> Options { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents live preview configuration
    /// </summary>
    public class LivePreview
    {
        public bool IsEnabled { get; set; } = true;
        public int UpdateDelay { get; set; } = 300; // milliseconds
        public List<string> PreviewAreas { get; set; } = new List<string>();
        public bool ShowPreview { get; set; } = true;
    }

    /// <summary>
    /// Represents setting validation
    /// </summary>
    public class SettingValidation
    {
        public List<ValidationRule> ValidationRules { get; set; } = new List<ValidationRule>();
        public bool IsEnabled { get; set; } = true;
        public bool ShowWarnings { get; set; } = true;
    }

    /// <summary>
    /// Represents a validation rule
    /// </summary>
    public class ValidationRule
    {
        public string SettingName { get; set; } = string.Empty;
        public string RuleType { get; set; } = string.Empty;
        public object MinValue { get; set; } = new object();
        public object MaxValue { get; set; } = new object();
        public List<string> AllowedValues { get; set; } = new List<string>();
        public string ErrorMessage { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents reset options
    /// </summary>
    public class ResetOption
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public bool RequiresConfirmation { get; set; } = true;
    }

    /// <summary>
    /// Represents import/export options
    /// </summary>
    public class ImportExportOption
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents setting search
    /// </summary>
    public class SettingSearch
    {
        public bool IsEnabled { get; set; } = true;
        public string Placeholder { get; set; } = "Search settings...";
        public List<string> SearchFields { get; set; } = new List<string>();
        public bool CaseSensitive { get; set; } = false;
        public bool ShowResults { get; set; } = true;
    }

    /// <summary>
    /// Represents setting categories
    /// </summary>
    public class SettingCategory
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int Order { get; set; } = 0;
        public List<string> Settings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents setting dependencies
    /// </summary>
    public class SettingDependency
    {
        public string SettingName { get; set; } = string.Empty;
        public List<string> Dependencies { get; set; } = new List<string>();
        public string DependencyType { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents setting help
    /// </summary>
    public class SettingHelp
    {
        public Dictionary<string, string> HelpText { get; set; } = new Dictionary<string, string>();
        public bool IsEnabled { get; set; } = true;
        public string HelpFormat { get; set; } = "Markdown";
    }

    /// <summary>
    /// Represents configuration data
    /// </summary>
    public class ConfigurationData
    {
        public string Language { get; set; } = "en-US";
        public string Theme { get; set; } = "Light";
        public int FontSize { get; set; } = 14;
        public bool AutoSave { get; set; } = true;
        public bool Notifications { get; set; } = true;
        public bool Animations { get; set; } = true;
        public bool SoundEffects { get; set; } = false;
        public bool HighContrast { get; set; } = false;
    }

    /// <summary>
    /// Represents validation result
    /// </summary>
    public class ConfigurationValidation
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Intuitive settings panel with grouped options and live preview
    /// </summary>
    public class ConfigurationPanel
    {
        public List<SettingGroup> GetSettingGroups()
        {
            return new List<SettingGroup>
            {
                new SettingGroup
                {
                    Name = "General",
                    Description = "General application settings",
                    Icon = "⚙️",
                    IsExpanded = true,
                    Order = 1
                },
                new SettingGroup
                {
                    Name = "Study",
                    Description = "Study session preferences",
                    Icon = "📚",
                    IsExpanded = false,
                    Order = 2
                },
                new SettingGroup
                {
                    Name = "Appearance",
                    Description = "Visual appearance and themes",
                    Icon = "🎨",
                    IsExpanded = false,
                    Order = 3
                },
                new SettingGroup
                {
                    Name = "Accessibility",
                    Description = "Accessibility and usability options",
                    Icon = "♿",
                    IsExpanded = false,
                    Order = 4
                },
                new SettingGroup
                {
                    Name = "Advanced",
                    Description = "Advanced configuration options",
                    Icon = "🔧",
                    IsExpanded = false,
                    Order = 5
                }
            };
        }

        public List<ConfigurationSetting> GetGeneralSettings()
        {
            return new List<ConfigurationSetting>
            {
                new ConfigurationSetting
                {
                    Name = "Language",
                    Description = "Application language",
                    Type = "Dropdown",
                    Value = "en-US",
                    DefaultValue = "en-US",
                    IsEnabled = true,
                    Options = new List<string> { "en-US", "es-ES", "fr-FR", "de-DE", "ja-JP" }
                },
                new ConfigurationSetting
                {
                    Name = "AutoSave",
                    Description = "Automatically save changes",
                    Type = "Toggle",
                    Value = true,
                    DefaultValue = true,
                    IsEnabled = true,
                    Options = new List<string>()
                },
                new ConfigurationSetting
                {
                    Name = "Backup",
                    Description = "Automatic backup frequency",
                    Type = "Dropdown",
                    Value = "Daily",
                    DefaultValue = "Daily",
                    IsEnabled = true,
                    Options = new List<string> { "Never", "Daily", "Weekly", "Monthly" }
                },
                new ConfigurationSetting
                {
                    Name = "Notifications",
                    Description = "Enable system notifications",
                    Type = "Toggle",
                    Value = true,
                    DefaultValue = true,
                    IsEnabled = true,
                    Options = new List<string>()
                }
            };
        }

        public List<ConfigurationSetting> GetStudySettings()
        {
            return new List<ConfigurationSetting>
            {
                new ConfigurationSetting
                {
                    Name = "DefaultStudyMode",
                    Description = "Default study mode for new sessions",
                    Type = "Dropdown",
                    Value = "Front to Back",
                    DefaultValue = "Front to Back",
                    IsEnabled = true,
                    Options = new List<string> { "Front to Back", "Back to Front", "Mixed", "Review" }
                },
                new ConfigurationSetting
                {
                    Name = "AutoFlip",
                    Description = "Automatically flip cards after delay",
                    Type = "Toggle",
                    Value = false,
                    DefaultValue = false,
                    IsEnabled = true,
                    Options = new List<string>()
                },
                new ConfigurationSetting
                {
                    Name = "SoundEffects",
                    Description = "Play sound effects during study",
                    Type = "Toggle",
                    Value = false,
                    DefaultValue = false,
                    IsEnabled = true,
                    Options = new List<string>()
                },
                new ConfigurationSetting
                {
                    Name = "StudyReminders",
                    Description = "Enable study reminders",
                    Type = "Toggle",
                    Value = true,
                    DefaultValue = true,
                    IsEnabled = true,
                    Options = new List<string>()
                }
            };
        }

        public List<ConfigurationSetting> GetAppearanceSettings()
        {
            return new List<ConfigurationSetting>
            {
                new ConfigurationSetting
                {
                    Name = "Theme",
                    Description = "Application theme",
                    Type = "Dropdown",
                    Value = "Light",
                    DefaultValue = "Light",
                    IsEnabled = true,
                    Options = new List<string> { "Light", "Dark", "High Contrast", "Auto" }
                },
                new ConfigurationSetting
                {
                    Name = "FontSize",
                    Description = "Application font size",
                    Type = "Slider",
                    Value = 14,
                    DefaultValue = 14,
                    IsEnabled = true,
                    Options = new List<string> { "12", "14", "16", "18", "20" }
                },
                new ConfigurationSetting
                {
                    Name = "Animations",
                    Description = "Enable UI animations",
                    Type = "Toggle",
                    Value = true,
                    DefaultValue = true,
                    IsEnabled = true,
                    Options = new List<string>()
                },
                new ConfigurationSetting
                {
                    Name = "ColorScheme",
                    Description = "Color scheme preference",
                    Type = "Dropdown",
                    Value = "Default",
                    DefaultValue = "Default",
                    IsEnabled = true,
                    Options = new List<string> { "Default", "Blue", "Green", "Purple", "Custom" }
                }
            };
        }

        public List<ConfigurationSetting> GetAccessibilitySettings()
        {
            return new List<ConfigurationSetting>
            {
                new ConfigurationSetting
                {
                    Name = "HighContrast",
                    Description = "Enable high contrast mode",
                    Type = "Toggle",
                    Value = false,
                    DefaultValue = false,
                    IsEnabled = true,
                    Options = new List<string>()
                },
                new ConfigurationSetting
                {
                    Name = "ScreenReader",
                    Description = "Optimize for screen readers",
                    Type = "Toggle",
                    Value = false,
                    DefaultValue = false,
                    IsEnabled = true,
                    Options = new List<string>()
                },
                new ConfigurationSetting
                {
                    Name = "KeyboardNavigation",
                    Description = "Enhanced keyboard navigation",
                    Type = "Toggle",
                    Value = true,
                    DefaultValue = true,
                    IsEnabled = true,
                    Options = new List<string>()
                },
                new ConfigurationSetting
                {
                    Name = "ReducedMotion",
                    Description = "Reduce motion and animations",
                    Type = "Toggle",
                    Value = false,
                    DefaultValue = false,
                    IsEnabled = true,
                    Options = new List<string>()
                }
            };
        }

        public List<ConfigurationSetting> GetAdvancedSettings()
        {
            return new List<ConfigurationSetting>
            {
                new ConfigurationSetting
                {
                    Name = "DebugMode",
                    Description = "Enable debug mode",
                    Type = "Toggle",
                    Value = false,
                    DefaultValue = false,
                    IsEnabled = true,
                    Options = new List<string>()
                },
                new ConfigurationSetting
                {
                    Name = "LogLevel",
                    Description = "Application log level",
                    Type = "Dropdown",
                    Value = "Info",
                    DefaultValue = "Info",
                    IsEnabled = true,
                    Options = new List<string> { "Debug", "Info", "Warning", "Error" }
                },
                new ConfigurationSetting
                {
                    Name = "CacheSize",
                    Description = "Cache size in MB",
                    Type = "Slider",
                    Value = 100,
                    DefaultValue = 100,
                    IsEnabled = true,
                    Options = new List<string> { "50", "100", "200", "500" }
                },
                new ConfigurationSetting
                {
                    Name = "PerformanceMode",
                    Description = "Enable performance mode",
                    Type = "Toggle",
                    Value = false,
                    DefaultValue = false,
                    IsEnabled = true,
                    Options = new List<string>()
                }
            };
        }

        public LivePreview GetLivePreview()
        {
            return new LivePreview
            {
                IsEnabled = true,
                UpdateDelay = 300,
                PreviewAreas = new List<string> { "Theme", "FontSize", "ColorScheme", "Animations" },
                ShowPreview = true
            };
        }

        public SettingValidation GetSettingValidation()
        {
            return new SettingValidation
            {
                ValidationRules = new List<ValidationRule>
                {
                    new ValidationRule
                    {
                        SettingName = "FontSize",
                        RuleType = "Range",
                        MinValue = 12,
                        MaxValue = 24,
                        AllowedValues = new List<string>(),
                        ErrorMessage = "Font size must be between 12 and 24"
                    },
                    new ValidationRule
                    {
                        SettingName = "CacheSize",
                        RuleType = "Range",
                        MinValue = 10,
                        MaxValue = 1000,
                        AllowedValues = new List<string>(),
                        ErrorMessage = "Cache size must be between 10 and 1000 MB"
                    },
                    new ValidationRule
                    {
                        SettingName = "LogLevel",
                        RuleType = "Enum",
                        MinValue = null,
                        MaxValue = null,
                        AllowedValues = new List<string> { "Debug", "Info", "Warning", "Error" },
                        ErrorMessage = "Log level must be one of: Debug, Info, Warning, Error"
                    }
                },
                IsEnabled = true,
                ShowWarnings = true
            };
        }

        public List<ResetOption> GetSettingReset()
        {
            return new List<ResetOption>
            {
                new ResetOption
                {
                    Type = "ResetAll",
                    Label = "Reset All Settings",
                    Description = "Reset all settings to default values",
                    Icon = "🔄",
                    IsEnabled = true,
                    RequiresConfirmation = true
                },
                new ResetOption
                {
                    Type = "ResetGroup",
                    Label = "Reset Group",
                    Description = "Reset settings in current group",
                    Icon = "📁",
                    IsEnabled = true,
                    RequiresConfirmation = true
                },
                new ResetOption
                {
                    Type = "ResetSingle",
                    Label = "Reset Setting",
                    Description = "Reset current setting to default",
                    Icon = "↩️",
                    IsEnabled = true,
                    RequiresConfirmation = false
                },
                new ResetOption
                {
                    Type = "ResetToDefault",
                    Label = "Reset to Defaults",
                    Description = "Reset to factory default settings",
                    Icon = "🏭",
                    IsEnabled = true,
                    RequiresConfirmation = true
                }
            };
        }

        public List<ImportExportOption> GetSettingImportExport()
        {
            return new List<ImportExportOption>
            {
                new ImportExportOption
                {
                    Type = "Export",
                    Label = "Export Settings",
                    Description = "Export current settings to file",
                    Icon = "📤",
                    FileExtension = ".json",
                    IsEnabled = true
                },
                new ImportExportOption
                {
                    Type = "Import",
                    Label = "Import Settings",
                    Description = "Import settings from file",
                    Icon = "📥",
                    FileExtension = ".json",
                    IsEnabled = true
                },
                new ImportExportOption
                {
                    Type = "Backup",
                    Label = "Backup Settings",
                    Description = "Create backup of current settings",
                    Icon = "💾",
                    FileExtension = ".backup",
                    IsEnabled = true
                },
                new ImportExportOption
                {
                    Type = "Restore",
                    Label = "Restore Settings",
                    Description = "Restore settings from backup",
                    Icon = "🔄",
                    FileExtension = ".backup",
                    IsEnabled = true
                }
            };
        }

        public SettingSearch GetSettingSearch()
        {
            return new SettingSearch
            {
                IsEnabled = true,
                Placeholder = "Search settings...",
                SearchFields = new List<string> { "Name", "Description" },
                CaseSensitive = false,
                ShowResults = true
            };
        }

        public List<SettingCategory> GetSettingCategories()
        {
            return new List<SettingCategory>
            {
                new SettingCategory
                {
                    Name = "User Interface",
                    Description = "Interface and appearance settings",
                    Icon = "🖥️",
                    Order = 1,
                    Settings = new List<string> { "Theme", "FontSize", "ColorScheme", "Animations" }
                },
                new SettingCategory
                {
                    Name = "Study Behavior",
                    Description = "Study session and learning preferences",
                    Icon = "📚",
                    Order = 2,
                    Settings = new List<string> { "DefaultStudyMode", "AutoFlip", "SoundEffects", "StudyReminders" }
                },
                new SettingCategory
                {
                    Name = "Data Management",
                    Description = "Data storage and backup settings",
                    Icon = "💾",
                    Order = 3,
                    Settings = new List<string> { "AutoSave", "Backup", "CacheSize" }
                },
                new SettingCategory
                {
                    Name = "System",
                    Description = "System and performance settings",
                    Icon = "⚙️",
                    Order = 4,
                    Settings = new List<string> { "Language", "Notifications", "DebugMode", "LogLevel", "PerformanceMode" }
                }
            };
        }

        public List<SettingDependency> GetSettingDependencies()
        {
            return new List<SettingDependency>
            {
                new SettingDependency
                {
                    SettingName = "Animations",
                    Dependencies = new List<string> { "ReducedMotion" },
                    DependencyType = "DisableWhen",
                    IsEnabled = true
                },
                new SettingDependency
                {
                    SettingName = "SoundEffects",
                    Dependencies = new List<string> { "Notifications" },
                    DependencyType = "Require",
                    IsEnabled = true
                },
                new SettingDependency
                {
                    SettingName = "HighContrast",
                    Dependencies = new List<string> { "Theme" },
                    DependencyType = "Override",
                    IsEnabled = true
                }
            };
        }

        public SettingHelp GetSettingHelp()
        {
            return new SettingHelp
            {
                HelpText = new Dictionary<string, string>
                {
                    ["General"] = "General settings control basic application behavior and preferences.",
                    ["Study"] = "Study settings customize your learning experience and study sessions.",
                    ["Appearance"] = "Appearance settings control the visual look and feel of the application.",
                    ["Accessibility"] = "Accessibility settings improve usability for users with different needs."
                },
                IsEnabled = true,
                HelpFormat = "Markdown"
            };
        }

        public ConfigurationValidation ValidateConfiguration(ConfigurationData config)
        {
            var validation = new ConfigurationValidation { IsValid = true };

            // Validate language
            var validLanguages = new[] { "en-US", "es-ES", "fr-FR", "de-DE", "ja-JP" };
            if (!validLanguages.Contains(config.Language))
            {
                validation.IsValid = false;
                validation.Errors.Add($"Invalid language: {config.Language}. Valid languages are: {string.Join(", ", validLanguages)}");
            }

            // Validate theme
            var validThemes = new[] { "Light", "Dark", "High Contrast", "Auto" };
            if (!validThemes.Contains(config.Theme))
            {
                validation.IsValid = false;
                validation.Errors.Add($"Invalid theme: {config.Theme}. Valid themes are: {string.Join(", ", validThemes)}");
            }

            // Validate font size
            if (config.FontSize < 12 || config.FontSize > 24)
            {
                validation.IsValid = false;
                validation.Errors.Add($"Font size must be between 12 and 24, got: {config.FontSize}");
            }

            // Validate accessibility conflicts
            if (config.HighContrast && config.Theme != "High Contrast")
            {
                validation.Warnings.Add("High contrast mode may override theme settings");
            }

            return validation;
        }
    }
}
