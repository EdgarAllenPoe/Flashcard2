using FluentAssertions;
using FlashcardApp.UI.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlashcardApp.Tests.UI.Statistics
{
    /// <summary>
    /// Tests for the comprehensive statistics dashboard with charts and visualizations
    /// </summary>
    public class StatisticsDashboardTests
    {
        [Fact]
        public void StatisticsDashboard_ShouldDefineChartTypes()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var chartTypes = statisticsDashboard.GetChartTypes();

            // Assert
            chartTypes.Should().NotBeNull();
            chartTypes.Should().Contain(chart => chart.Type == "Line");
            chartTypes.Should().Contain(chart => chart.Type == "Bar");
            chartTypes.Should().Contain(chart => chart.Type == "Pie");
            chartTypes.Should().Contain(chart => chart.Type == "Area");
            chartTypes.Should().Contain(chart => chart.Type == "Scatter");
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineProgressMetrics()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var progressMetrics = statisticsDashboard.GetProgressMetrics();

            // Assert
            progressMetrics.Should().NotBeNull();
            progressMetrics.TotalCards.Should().BeGreaterThanOrEqualTo(0);
            progressMetrics.StudiedCards.Should().BeGreaterThanOrEqualTo(0);
            progressMetrics.MasteryLevel.Should().BeInRange(0, 100);
            progressMetrics.StudyStreak.Should().BeGreaterThanOrEqualTo(0);
            progressMetrics.AverageScore.Should().BeInRange(0, 100);
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineTimeBasedStatistics()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var timeStats = statisticsDashboard.GetTimeBasedStatistics();

            // Assert
            timeStats.Should().NotBeNull();
            timeStats.DailyStats.Should().NotBeEmpty();
            timeStats.WeeklyStats.Should().NotBeEmpty();
            timeStats.MonthlyStats.Should().NotBeEmpty();
            timeStats.YearlyStats.Should().NotBeEmpty();
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineDeckStatistics()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var deckStats = statisticsDashboard.GetDeckStatistics();

            // Assert
            deckStats.Should().NotBeNull();
            deckStats.Should().NotBeEmpty();
            deckStats.Should().Contain(deck => deck.DeckName == "French Vocabulary");
            deckStats.Should().Contain(deck => deck.DeckName == "Math Formulas");
            deckStats.Should().Contain(deck => deck.DeckName == "Science Facts");
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefinePerformanceIndicators()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var performanceIndicators = statisticsDashboard.GetPerformanceIndicators();

            // Assert
            performanceIndicators.Should().NotBeNull();
            performanceIndicators.Should().Contain(indicator => indicator.Type == "Accuracy");
            performanceIndicators.Should().Contain(indicator => indicator.Type == "Speed");
            performanceIndicators.Should().Contain(indicator => indicator.Type == "Retention");
            performanceIndicators.Should().Contain(indicator => indicator.Type == "Consistency");
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineVisualizationOptions()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var visualizationOptions = statisticsDashboard.GetVisualizationOptions();

            // Assert
            visualizationOptions.Should().NotBeNull();
            visualizationOptions.Should().ContainKey("ChartTheme");
            visualizationOptions.Should().ContainKey("ColorScheme");
            visualizationOptions.Should().ContainKey("AnimationSpeed");
            visualizationOptions.Should().ContainKey("DataLabels");
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineDashboardLayout()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var dashboardLayout = statisticsDashboard.GetDashboardLayout();

            // Assert
            dashboardLayout.Should().NotBeNull();
            dashboardLayout.Widgets.Should().NotBeEmpty();
            dashboardLayout.Widgets.Should().Contain(widget => widget.Type == "ProgressChart");
            dashboardLayout.Widgets.Should().Contain(widget => widget.Type == "PerformanceMetrics");
            dashboardLayout.Widgets.Should().Contain(widget => widget.Type == "StudyHistory");
            dashboardLayout.Widgets.Should().Contain(widget => widget.Type == "Achievements");
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineDataFilters()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var dataFilters = statisticsDashboard.GetDataFilters();

            // Assert
            dataFilters.Should().NotBeNull();
            dataFilters.Should().Contain(filter => filter.Type == "DateRange");
            dataFilters.Should().Contain(filter => filter.Type == "DeckSelection");
            dataFilters.Should().Contain(filter => filter.Type == "StudyMode");
            dataFilters.Should().Contain(filter => filter.Type == "Difficulty");
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineExportOptions()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var exportOptions = statisticsDashboard.GetExportOptions();

            // Assert
            exportOptions.Should().NotBeNull();
            exportOptions.Should().Contain(option => option.Format == "PDF");
            exportOptions.Should().Contain(option => option.Format == "CSV");
            exportOptions.Should().Contain(option => option.Format == "PNG");
            exportOptions.Should().Contain(option => option.Format == "JSON");
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineRealTimeUpdates()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var realTimeUpdates = statisticsDashboard.GetRealTimeUpdates();

            // Assert
            realTimeUpdates.Should().NotBeNull();
            realTimeUpdates.IsEnabled.Should().BeTrue();
            realTimeUpdates.UpdateInterval.Should().BeGreaterThan(0);
            realTimeUpdates.AutoRefresh.Should().BeTrue();
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineAchievementSystem()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var achievements = statisticsDashboard.GetAchievementSystem();

            // Assert
            achievements.Should().NotBeNull();
            achievements.Achievements.Should().Contain(achievement => achievement.Type == "StudyStreak");
            achievements.Achievements.Should().Contain(achievement => achievement.Type == "MasteryLevel");
            achievements.Achievements.Should().Contain(achievement => achievement.Type == "SpeedRecord");
            achievements.Achievements.Should().Contain(achievement => achievement.Type == "AccuracyGoal");
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineComparisonTools()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var comparisonTools = statisticsDashboard.GetComparisonTools();

            // Assert
            comparisonTools.Should().NotBeNull();
            comparisonTools.Should().Contain(tool => tool.Type == "DeckComparison");
            comparisonTools.Should().Contain(tool => tool.Type == "TimeComparison");
            comparisonTools.Should().Contain(tool => tool.Type == "PerformanceComparison");
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineAccessibilityFeatures()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var accessibility = statisticsDashboard.GetAccessibilityFeatures();

            // Assert
            accessibility.Should().NotBeNull();
            accessibility.ScreenReader.Should().NotBeNullOrEmpty();
            accessibility.KeyboardNavigation.Should().NotBeNullOrEmpty();
            accessibility.HighContrast.Should().NotBeNullOrEmpty();
            accessibility.DataTables.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void StatisticsDashboard_ShouldDefineCustomizationOptions()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();

            // Act
            var customization = statisticsDashboard.GetCustomizationOptions();

            // Assert
            customization.Should().NotBeNull();
            customization.Widgets.Should().NotBeEmpty();
            customization.Layouts.Should().NotBeEmpty();
            customization.Themes.Should().NotBeEmpty();
            customization.Settings.Should().NotBeEmpty();
        }

        [Fact]
        public void StatisticsDashboard_ShouldValidateDashboardConfiguration()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();
            var config = new DashboardConfiguration
            {
                Layout = "Grid",
                Theme = "Light",
                AutoRefresh = true,
                UpdateInterval = 30
            };

            // Act
            var validation = statisticsDashboard.ValidateDashboardConfiguration(config);

            // Assert
            validation.Should().NotBeNull();
            validation.IsValid.Should().BeTrue();
            validation.Errors.Should().BeEmpty();
        }

        [Fact]
        public void StatisticsDashboard_ShouldHandleInvalidConfiguration()
        {
            // Arrange
            var statisticsDashboard = new StatisticsDashboard();
            var invalidConfig = new DashboardConfiguration
            {
                Layout = "Invalid", // Invalid layout
                Theme = "Light",
                AutoRefresh = true,
                UpdateInterval = -5 // Invalid negative interval
            };

            // Act
            var validation = statisticsDashboard.ValidateDashboardConfiguration(invalidConfig);

            // Assert
            validation.Should().NotBeNull();
            validation.IsValid.Should().BeFalse();
            validation.Errors.Should().NotBeEmpty();
        }
    }
}
