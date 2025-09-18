using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.UI.Statistics
{
    /// <summary>
    /// Represents a chart type
    /// </summary>
    public class ChartType
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents progress metrics
    /// </summary>
    public class ProgressMetrics
    {
        public int TotalCards { get; set; } = 0;
        public int StudiedCards { get; set; } = 0;
        public double MasteryLevel { get; set; } = 0.0;
        public int StudyStreak { get; set; } = 0;
        public double AverageScore { get; set; } = 0.0;
        public int TotalStudyTime { get; set; } = 0; // in minutes
    }

    /// <summary>
    /// Represents time-based statistics
    /// </summary>
    public class TimeBasedStatistics
    {
        public List<DailyStat> DailyStats { get; set; } = new List<DailyStat>();
        public List<WeeklyStat> WeeklyStats { get; set; } = new List<WeeklyStat>();
        public List<MonthlyStat> MonthlyStats { get; set; } = new List<MonthlyStat>();
        public List<YearlyStat> YearlyStats { get; set; } = new List<YearlyStat>();
    }

    /// <summary>
    /// Represents daily statistics
    /// </summary>
    public class DailyStat
    {
        public DateTime Date { get; set; }
        public int CardsStudied { get; set; }
        public double Accuracy { get; set; }
        public int StudyTime { get; set; } // in minutes
    }

    /// <summary>
    /// Represents weekly statistics
    /// </summary>
    public class WeeklyStat
    {
        public DateTime WeekStart { get; set; }
        public int CardsStudied { get; set; }
        public double AverageAccuracy { get; set; }
        public int TotalStudyTime { get; set; } // in minutes
    }

    /// <summary>
    /// Represents monthly statistics
    /// </summary>
    public class MonthlyStat
    {
        public DateTime Month { get; set; }
        public int CardsStudied { get; set; }
        public double AverageAccuracy { get; set; }
        public int TotalStudyTime { get; set; } // in minutes
    }

    /// <summary>
    /// Represents yearly statistics
    /// </summary>
    public class YearlyStat
    {
        public int Year { get; set; }
        public int CardsStudied { get; set; }
        public double AverageAccuracy { get; set; }
        public int TotalStudyTime { get; set; } // in minutes
    }

    /// <summary>
    /// Represents deck statistics
    /// </summary>
    public class DeckStatistic
    {
        public string DeckName { get; set; } = string.Empty;
        public int TotalCards { get; set; }
        public int StudiedCards { get; set; }
        public double MasteryLevel { get; set; }
        public double AverageScore { get; set; }
        public DateTime LastStudied { get; set; }
        public int StudyCount { get; set; }
    }

    /// <summary>
    /// Represents performance indicators
    /// </summary>
    public class PerformanceIndicator
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Trend { get; set; } = string.Empty; // "up", "down", "stable"
        public string Color { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents visualization options
    /// </summary>
    public class VisualizationOptions
    {
        public string ChartTheme { get; set; } = "Light";
        public string ColorScheme { get; set; } = "Default";
        public double AnimationSpeed { get; set; } = 1.0;
        public bool DataLabels { get; set; } = true;
        public bool ShowGrid { get; set; } = true;
        public bool ShowLegend { get; set; } = true;
    }

    /// <summary>
    /// Represents dashboard widget
    /// </summary>
    public class DashboardWidget
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsVisible { get; set; } = true;
    }

    /// <summary>
    /// Represents dashboard layout
    /// </summary>
    public class DashboardLayout
    {
        public List<DashboardWidget> Widgets { get; set; } = new List<DashboardWidget>();
        public string LayoutType { get; set; } = "Grid";
        public int Columns { get; set; } = 4;
        public int Rows { get; set; } = 3;
    }

    /// <summary>
    /// Represents data filter
    /// </summary>
    public class DataFilter
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public List<string> Options { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents export option
    /// </summary>
    public class ExportOption
    {
        public string Format { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents real-time updates
    /// </summary>
    public class RealTimeUpdates
    {
        public bool IsEnabled { get; set; } = true;
        public int UpdateInterval { get; set; } = 30; // seconds
        public bool AutoRefresh { get; set; } = true;
        public bool ShowNotifications { get; set; } = true;
    }

    /// <summary>
    /// Represents achievement
    /// </summary>
    public class Achievement
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsUnlocked { get; set; } = false;
        public DateTime UnlockedAt { get; set; }
        public string Criteria { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents achievement system
    /// </summary>
    public class AchievementSystem
    {
        public List<Achievement> Achievements { get; set; } = new List<Achievement>();
        public int TotalAchievements { get; set; }
        public int UnlockedAchievements { get; set; }
        public double CompletionPercentage { get; set; }
    }

    /// <summary>
    /// Represents comparison tool
    /// </summary>
    public class ComparisonTool
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public List<string> ComparisonOptions { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents accessibility features
    /// </summary>
    public class StatisticsAccessibility
    {
        public string ScreenReader { get; set; } = string.Empty;
        public string KeyboardNavigation { get; set; } = string.Empty;
        public string HighContrast { get; set; } = string.Empty;
        public string DataTables { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents customization options
    /// </summary>
    public class CustomizationOptions
    {
        public List<string> Widgets { get; set; } = new List<string>();
        public List<string> Layouts { get; set; } = new List<string>();
        public List<string> Themes { get; set; } = new List<string>();
        public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// Represents dashboard configuration
    /// </summary>
    public class DashboardConfiguration
    {
        public string Layout { get; set; } = "Grid";
        public string Theme { get; set; } = "Light";
        public bool AutoRefresh { get; set; } = true;
        public int UpdateInterval { get; set; } = 30;
        public bool ShowAnimations { get; set; } = true;
    }

    /// <summary>
    /// Represents validation result
    /// </summary>
    public class DashboardValidation
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Comprehensive statistics dashboard with charts and visualizations
    /// </summary>
    public class StatisticsDashboard
    {
        public List<ChartType> GetChartTypes()
        {
            return new List<ChartType>
            {
                new ChartType
                {
                    Type = "Line",
                    Name = "Line Chart",
                    Description = "Show trends over time",
                    Icon = "📈",
                    IsEnabled = true
                },
                new ChartType
                {
                    Type = "Bar",
                    Name = "Bar Chart",
                    Description = "Compare values across categories",
                    Icon = "📊",
                    IsEnabled = true
                },
                new ChartType
                {
                    Type = "Pie",
                    Name = "Pie Chart",
                    Description = "Show proportions and percentages",
                    Icon = "🥧",
                    IsEnabled = true
                },
                new ChartType
                {
                    Type = "Area",
                    Name = "Area Chart",
                    Description = "Show cumulative data over time",
                    Icon = "📈",
                    IsEnabled = true
                },
                new ChartType
                {
                    Type = "Scatter",
                    Name = "Scatter Plot",
                    Description = "Show relationships between variables",
                    Icon = "🔵",
                    IsEnabled = true
                }
            };
        }

        public ProgressMetrics GetProgressMetrics()
        {
            return new ProgressMetrics
            {
                TotalCards = 500,
                StudiedCards = 350,
                MasteryLevel = 70.0,
                StudyStreak = 12,
                AverageScore = 85.5,
                TotalStudyTime = 1200 // 20 hours
            };
        }

        public TimeBasedStatistics GetTimeBasedStatistics()
        {
            return new TimeBasedStatistics
            {
                DailyStats = new List<DailyStat>
                {
                    new DailyStat { Date = DateTime.Today.AddDays(-6), CardsStudied = 25, Accuracy = 85.0, StudyTime = 30 },
                    new DailyStat { Date = DateTime.Today.AddDays(-5), CardsStudied = 30, Accuracy = 90.0, StudyTime = 35 },
                    new DailyStat { Date = DateTime.Today.AddDays(-4), CardsStudied = 20, Accuracy = 80.0, StudyTime = 25 },
                    new DailyStat { Date = DateTime.Today.AddDays(-3), CardsStudied = 35, Accuracy = 88.0, StudyTime = 40 },
                    new DailyStat { Date = DateTime.Today.AddDays(-2), CardsStudied = 28, Accuracy = 92.0, StudyTime = 32 },
                    new DailyStat { Date = DateTime.Today.AddDays(-1), CardsStudied = 32, Accuracy = 87.0, StudyTime = 38 },
                    new DailyStat { Date = DateTime.Today, CardsStudied = 40, Accuracy = 95.0, StudyTime = 45 }
                },
                WeeklyStats = new List<WeeklyStat>
                {
                    new WeeklyStat { WeekStart = DateTime.Today.AddDays(-28), CardsStudied = 180, AverageAccuracy = 85.0, TotalStudyTime = 200 },
                    new WeeklyStat { WeekStart = DateTime.Today.AddDays(-21), CardsStudied = 195, AverageAccuracy = 88.0, TotalStudyTime = 220 },
                    new WeeklyStat { WeekStart = DateTime.Today.AddDays(-14), CardsStudied = 210, AverageAccuracy = 90.0, TotalStudyTime = 240 },
                    new WeeklyStat { WeekStart = DateTime.Today.AddDays(-7), CardsStudied = 190, AverageAccuracy = 89.0, TotalStudyTime = 210 }
                },
                MonthlyStats = new List<MonthlyStat>
                {
                    new MonthlyStat { Month = DateTime.Today.AddMonths(-3), CardsStudied = 800, AverageAccuracy = 82.0, TotalStudyTime = 900 },
                    new MonthlyStat { Month = DateTime.Today.AddMonths(-2), CardsStudied = 850, AverageAccuracy = 85.0, TotalStudyTime = 950 },
                    new MonthlyStat { Month = DateTime.Today.AddMonths(-1), CardsStudied = 900, AverageAccuracy = 88.0, TotalStudyTime = 1000 },
                    new MonthlyStat { Month = DateTime.Today, CardsStudied = 775, AverageAccuracy = 90.0, TotalStudyTime = 850 }
                },
                YearlyStats = new List<YearlyStat>
                {
                    new YearlyStat { Year = DateTime.Today.Year - 1, CardsStudied = 8000, AverageAccuracy = 80.0, TotalStudyTime = 9000 },
                    new YearlyStat { Year = DateTime.Today.Year, CardsStudied = 3325, AverageAccuracy = 88.0, TotalStudyTime = 3700 }
                }
            };
        }

        public List<DeckStatistic> GetDeckStatistics()
        {
            return new List<DeckStatistic>
            {
                new DeckStatistic
                {
                    DeckName = "French Vocabulary",
                    TotalCards = 150,
                    StudiedCards = 120,
                    MasteryLevel = 80.0,
                    AverageScore = 85.5,
                    LastStudied = DateTime.Now.AddHours(-2),
                    StudyCount = 25
                },
                new DeckStatistic
                {
                    DeckName = "Math Formulas",
                    TotalCards = 80,
                    StudiedCards = 65,
                    MasteryLevel = 81.25,
                    AverageScore = 88.0,
                    LastStudied = DateTime.Now.AddHours(-5),
                    StudyCount = 18
                },
                new DeckStatistic
                {
                    DeckName = "Science Facts",
                    TotalCards = 200,
                    StudiedCards = 165,
                    MasteryLevel = 82.5,
                    AverageScore = 90.0,
                    LastStudied = DateTime.Now.AddHours(-1),
                    StudyCount = 32
                }
            };
        }

        public List<PerformanceIndicator> GetPerformanceIndicators()
        {
            return new List<PerformanceIndicator>
            {
                new PerformanceIndicator
                {
                    Type = "Accuracy",
                    Name = "Overall Accuracy",
                    Value = 88.5,
                    Unit = "%",
                    Trend = "up",
                    Color = "#107C10"
                },
                new PerformanceIndicator
                {
                    Type = "Speed",
                    Name = "Cards per Minute",
                    Value = 4.2,
                    Unit = "cards/min",
                    Trend = "up",
                    Color = "#0078D4"
                },
                new PerformanceIndicator
                {
                    Type = "Retention",
                    Name = "Retention Rate",
                    Value = 92.0,
                    Unit = "%",
                    Trend = "stable",
                    Color = "#FF8C00"
                },
                new PerformanceIndicator
                {
                    Type = "Consistency",
                    Name = "Study Consistency",
                    Value = 85.0,
                    Unit = "%",
                    Trend = "up",
                    Color = "#D13438"
                }
            };
        }

        public Dictionary<string, object> GetVisualizationOptions()
        {
            return new Dictionary<string, object>
            {
                ["ChartTheme"] = "Light",
                ["ColorScheme"] = "Default",
                ["AnimationSpeed"] = 1.0,
                ["DataLabels"] = true,
                ["ShowGrid"] = true,
                ["ShowLegend"] = true
            };
        }

        public DashboardLayout GetDashboardLayout()
        {
            return new DashboardLayout
            {
                Widgets = new List<DashboardWidget>
                {
                    new DashboardWidget
                    {
                        Type = "ProgressChart",
                        Title = "Study Progress",
                        Description = "Overall learning progress over time",
                        Width = 2,
                        Height = 2,
                        X = 0,
                        Y = 0,
                        IsVisible = true
                    },
                    new DashboardWidget
                    {
                        Type = "PerformanceMetrics",
                        Title = "Performance Metrics",
                        Description = "Key performance indicators",
                        Width = 2,
                        Height = 1,
                        X = 2,
                        Y = 0,
                        IsVisible = true
                    },
                    new DashboardWidget
                    {
                        Type = "StudyHistory",
                        Title = "Study History",
                        Description = "Recent study sessions",
                        Width = 2,
                        Height = 2,
                        X = 0,
                        Y = 2,
                        IsVisible = true
                    },
                    new DashboardWidget
                    {
                        Type = "Achievements",
                        Title = "Achievements",
                        Description = "Unlocked achievements and badges",
                        Width = 2,
                        Height = 1,
                        X = 2,
                        Y = 1,
                        IsVisible = true
                    }
                },
                LayoutType = "Grid",
                Columns = 4,
                Rows = 3
            };
        }

        public List<DataFilter> GetDataFilters()
        {
            return new List<DataFilter>
            {
                new DataFilter
                {
                    Type = "DateRange",
                    Name = "Date Range",
                    Value = "Last 30 days",
                    IsActive = true,
                    Options = new List<string> { "Last 7 days", "Last 30 days", "Last 3 months", "Last year", "All time" }
                },
                new DataFilter
                {
                    Type = "DeckSelection",
                    Name = "Deck Selection",
                    Value = "All decks",
                    IsActive = false,
                    Options = new List<string> { "All decks", "French Vocabulary", "Math Formulas", "Science Facts" }
                },
                new DataFilter
                {
                    Type = "StudyMode",
                    Name = "Study Mode",
                    Value = "All modes",
                    IsActive = false,
                    Options = new List<string> { "All modes", "Front to Back", "Back to Front", "Mixed", "Review" }
                },
                new DataFilter
                {
                    Type = "Difficulty",
                    Name = "Difficulty Level",
                    Value = "All levels",
                    IsActive = false,
                    Options = new List<string> { "All levels", "Easy", "Medium", "Hard" }
                }
            };
        }

        public List<ExportOption> GetExportOptions()
        {
            return new List<ExportOption>
            {
                new ExportOption
                {
                    Format = "PDF",
                    Name = "PDF Report",
                    Description = "Export statistics as PDF document",
                    Icon = "📄",
                    IsEnabled = true
                },
                new ExportOption
                {
                    Format = "CSV",
                    Name = "CSV Data",
                    Description = "Export raw data as CSV file",
                    Icon = "📊",
                    IsEnabled = true
                },
                new ExportOption
                {
                    Format = "PNG",
                    Name = "PNG Image",
                    Description = "Export charts as PNG images",
                    Icon = "🖼️",
                    IsEnabled = true
                },
                new ExportOption
                {
                    Format = "JSON",
                    Name = "JSON Data",
                    Description = "Export data as JSON format",
                    Icon = "📋",
                    IsEnabled = true
                }
            };
        }

        public RealTimeUpdates GetRealTimeUpdates()
        {
            return new RealTimeUpdates
            {
                IsEnabled = true,
                UpdateInterval = 30,
                AutoRefresh = true,
                ShowNotifications = true
            };
        }

        public AchievementSystem GetAchievementSystem()
        {
            return new AchievementSystem
            {
                Achievements = new List<Achievement>
                {
                    new Achievement
                    {
                        Type = "StudyStreak",
                        Name = "Streak Master",
                        Description = "Study for 7 consecutive days",
                        Icon = "🔥",
                        IsUnlocked = true,
                        UnlockedAt = DateTime.Now.AddDays(-5),
                        Criteria = "Study for 7 days in a row"
                    },
                    new Achievement
                    {
                        Type = "MasteryLevel",
                        Name = "Master Learner",
                        Description = "Achieve 90% mastery in any deck",
                        Icon = "🎓",
                        IsUnlocked = true,
                        UnlockedAt = DateTime.Now.AddDays(-3),
                        Criteria = "Reach 90% mastery level"
                    },
                    new Achievement
                    {
                        Type = "SpeedRecord",
                        Name = "Speed Demon",
                        Description = "Study 50 cards in under 10 minutes",
                        Icon = "⚡",
                        IsUnlocked = false,
                        UnlockedAt = DateTime.MinValue,
                        Criteria = "Study 50 cards in 10 minutes"
                    },
                    new Achievement
                    {
                        Type = "AccuracyGoal",
                        Name = "Precision Master",
                        Description = "Achieve 95% accuracy for 100 cards",
                        Icon = "🎯",
                        IsUnlocked = false,
                        UnlockedAt = DateTime.MinValue,
                        Criteria = "95% accuracy for 100 cards"
                    }
                },
                TotalAchievements = 4,
                UnlockedAchievements = 2,
                CompletionPercentage = 50.0
            };
        }

        public List<ComparisonTool> GetComparisonTools()
        {
            return new List<ComparisonTool>
            {
                new ComparisonTool
                {
                    Type = "DeckComparison",
                    Name = "Deck Comparison",
                    Description = "Compare performance across different decks",
                    IsEnabled = true,
                    ComparisonOptions = new List<string> { "Accuracy", "Speed", "Mastery Level", "Study Time" }
                },
                new ComparisonTool
                {
                    Type = "TimeComparison",
                    Name = "Time Comparison",
                    Description = "Compare performance across different time periods",
                    IsEnabled = true,
                    ComparisonOptions = new List<string> { "Daily", "Weekly", "Monthly", "Yearly" }
                },
                new ComparisonTool
                {
                    Type = "PerformanceComparison",
                    Name = "Performance Comparison",
                    Description = "Compare different performance metrics",
                    IsEnabled = true,
                    ComparisonOptions = new List<string> { "Accuracy vs Speed", "Study Time vs Mastery", "Consistency vs Progress" }
                }
            };
        }

        public StatisticsAccessibility GetAccessibilityFeatures()
        {
            return new StatisticsAccessibility
            {
                ScreenReader = "ARIA labels and semantic markup for all chart elements",
                KeyboardNavigation = "Full keyboard navigation support for dashboard widgets",
                HighContrast = "High contrast mode support for better chart visibility",
                DataTables = "Alternative data table views for all chart data"
            };
        }

        public CustomizationOptions GetCustomizationOptions()
        {
            return new CustomizationOptions
            {
                Widgets = new List<string> { "ProgressChart", "PerformanceMetrics", "StudyHistory", "Achievements", "DeckComparison" },
                Layouts = new List<string> { "Grid", "List", "Compact", "Custom" },
                Themes = new List<string> { "Light", "Dark", "High Contrast", "Custom" },
                Settings = new Dictionary<string, object>
                {
                    ["AutoRefresh"] = true,
                    ["UpdateInterval"] = 30,
                    ["ShowAnimations"] = true,
                    ["DefaultChartType"] = "Line"
                }
            };
        }

        public DashboardValidation ValidateDashboardConfiguration(DashboardConfiguration config)
        {
            var validation = new DashboardValidation { IsValid = true };

            // Validate layout
            var validLayouts = new[] { "Grid", "List", "Compact", "Custom" };
            if (!validLayouts.Contains(config.Layout))
            {
                validation.IsValid = false;
                validation.Errors.Add($"Invalid layout: {config.Layout}. Valid layouts are: {string.Join(", ", validLayouts)}");
            }

            // Validate theme
            var validThemes = new[] { "Light", "Dark", "High Contrast", "Custom" };
            if (!validThemes.Contains(config.Theme))
            {
                validation.IsValid = false;
                validation.Errors.Add($"Invalid theme: {config.Theme}. Valid themes are: {string.Join(", ", validThemes)}");
            }

            // Validate update interval
            if (config.UpdateInterval <= 0)
            {
                validation.IsValid = false;
                validation.Errors.Add("Update interval must be greater than 0");
            }
            else if (config.UpdateInterval < 10)
            {
                validation.Warnings.Add("Update interval is very short, may affect performance");
            }
            else if (config.UpdateInterval > 300)
            {
                validation.Warnings.Add("Update interval is very long, data may not be current");
            }

            return validation;
        }
    }
}
