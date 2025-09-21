using FluentAssertions;
using FlashcardApp.UI.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlashcardApp.Tests.UI.UX
{
    /// <summary>
    /// Tests for UI/UX Analysis to identify current issues and define modern design requirements
    /// </summary>
    public class UIAnalysisTests
    {
        [Fact]
        public void UIAnalysis_ShouldIdentifyCurrentUIIssues()
        {
            // Arrange
            var analyzer = new UIAnalyzer();

            // Act
            var issues = analyzer.IdentifyCurrentUIIssues();

            // Assert
            issues.Should().NotBeNull();
            issues.Should().NotBeEmpty();
            issues.Should().Contain(issue => issue.Category == "Layout");
            issues.Should().Contain(issue => issue.Category == "Visual Design");
            issues.Should().Contain(issue => issue.Category == "User Experience");
            issues.Should().Contain(issue => issue.Category == "Accessibility");
        }

        [Fact]
        public void UIAnalysis_ShouldDefineModernDesignRequirements()
        {
            // Arrange
            var analyzer = new UIAnalyzer();

            // Act
            var requirements = analyzer.DefineModernDesignRequirements();

            // Assert
            requirements.Should().NotBeNull();
            requirements.Should().NotBeEmpty();
            requirements.Should().Contain(req => req.Category == "Design System");
            requirements.Should().Contain(req => req.Category == "Layout");
            requirements.Should().Contain(req => req.Category == "Interactions");
            requirements.Should().Contain(req => req.Category == "Accessibility");
        }

        [Fact]
        public void UIAnalysis_ShouldPrioritizeIssuesByImpact()
        {
            // Arrange
            var analyzer = new UIAnalyzer();
            var issues = analyzer.IdentifyCurrentUIIssues();

            // Act
            var prioritizedIssues = analyzer.PrioritizeIssuesByImpact(issues);

            // Assert
            prioritizedIssues.Should().NotBeNull();
            prioritizedIssues.Should().NotBeEmpty();
            prioritizedIssues.Should().BeInDescendingOrder(issue => issue.ImpactScore);

            // High impact issues should be at the top
            prioritizedIssues.First().ImpactScore.Should().BeGreaterThan(7);
        }

        [Fact]
        public void UIAnalysis_ShouldCreateDesignRecommendations()
        {
            // Arrange
            var analyzer = new UIAnalyzer();
            var issues = analyzer.IdentifyCurrentUIIssues();
            var requirements = analyzer.DefineModernDesignRequirements();

            // Act
            var recommendations = analyzer.CreateDesignRecommendations(issues, requirements);

            // Assert
            recommendations.Should().NotBeNull();
            recommendations.Should().NotBeEmpty();
            recommendations.Should().Contain(rec => rec.Priority == "High");
            recommendations.Should().Contain(rec => rec.Priority == "Medium");
            recommendations.Should().Contain(rec => rec.Priority == "Low");
        }

        [Fact]
        public void UIAnalysis_ShouldIdentifyAccessibilityGaps()
        {
            // Arrange
            var analyzer = new UIAnalyzer();

            // Act
            var accessibilityGaps = analyzer.IdentifyAccessibilityGaps();

            // Assert
            accessibilityGaps.Should().NotBeNull();
            accessibilityGaps.Should().NotBeEmpty();
            accessibilityGaps.Should().Contain(gap => gap.Type == "Keyboard Navigation");
            accessibilityGaps.Should().Contain(gap => gap.Type == "Screen Reader Support");
            accessibilityGaps.Should().Contain(gap => gap.Type == "Color Contrast");
            accessibilityGaps.Should().Contain(gap => gap.Type == "Focus Management");
        }

        [Fact]
        public void UIAnalysis_ShouldDefineUserPersonas()
        {
            // Arrange
            var analyzer = new UIAnalyzer();

            // Act
            var personas = analyzer.DefineUserPersonas();

            // Assert
            personas.Should().NotBeNull();
            personas.Should().NotBeEmpty();
            personas.Should().Contain(persona => persona.Type == "Student");
            personas.Should().Contain(persona => persona.Type == "Professional");
            personas.Should().Contain(persona => persona.Type == "Educator");
            personas.Should().Contain(persona => persona.Type == "Accessibility User");
        }

        [Fact]
        public void UIAnalysis_ShouldCreateUserJourneyMap()
        {
            // Arrange
            var analyzer = new UIAnalyzer();
            var personas = analyzer.DefineUserPersonas();

            // Act
            var journeyMap = analyzer.CreateUserJourneyMap(personas.First());

            // Assert
            journeyMap.Should().NotBeNull();
            journeyMap.Should().NotBeEmpty();
            journeyMap.Should().Contain(step => step.Phase == "Discovery");
            journeyMap.Should().Contain(step => step.Phase == "Onboarding");
            journeyMap.Should().Contain(step => step.Phase == "Daily Use");
            journeyMap.Should().Contain(step => step.Phase == "Advanced Features");
        }

        [Fact]
        public void UIAnalysis_ShouldDefineSuccessMetrics()
        {
            // Arrange
            var analyzer = new UIAnalyzer();

            // Act
            var metrics = analyzer.DefineSuccessMetrics();

            // Assert
            metrics.Should().NotBeNull();
            metrics.Should().NotBeEmpty();
            metrics.Should().Contain(metric => metric.Category == "Usability");
            metrics.Should().Contain(metric => metric.Category == "Performance");
            metrics.Should().Contain(metric => metric.Category == "Accessibility");
            metrics.Should().Contain(metric => metric.Category == "User Satisfaction");
        }
    }

}
