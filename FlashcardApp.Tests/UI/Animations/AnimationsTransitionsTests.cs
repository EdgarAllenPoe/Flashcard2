using FluentAssertions;
using FlashcardApp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlashcardApp.Tests.UI.Animations
{
    /// <summary>
    /// Tests for smooth animations and transitions throughout the application
    /// </summary>
    public class AnimationsTransitionsTests
    {
        [Fact]
        public void AnimationsTransitions_ShouldDefineAnimationTypes()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var animationTypes = animationsTransitions.GetAnimationTypes();

            // Assert
            animationTypes.Should().NotBeNull();
            animationTypes.Should().Contain(animation => animation.Type == "Fade");
            animationTypes.Should().Contain(animation => animation.Type == "Slide");
            animationTypes.Should().Contain(animation => animation.Type == "Scale");
            animationTypes.Should().Contain(animation => animation.Type == "Rotate");
            animationTypes.Should().Contain(animation => animation.Type == "Flip");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineTransitionTypes()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var transitionTypes = animationsTransitions.GetTransitionTypes();

            // Assert
            transitionTypes.Should().NotBeNull();
            transitionTypes.Should().Contain(transition => transition.Type == "PageTransition");
            transitionTypes.Should().Contain(transition => transition.Type == "ModalTransition");
            transitionTypes.Should().Contain(transition => transition.Type == "ListTransition");
            transitionTypes.Should().Contain(transition => transition.Type == "CardTransition");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineEasingFunctions()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var easingFunctions = animationsTransitions.GetEasingFunctions();

            // Assert
            easingFunctions.Should().NotBeNull();
            easingFunctions.Should().Contain(easing => easing.Name == "EaseIn");
            easingFunctions.Should().Contain(easing => easing.Name == "EaseOut");
            easingFunctions.Should().Contain(easing => easing.Name == "EaseInOut");
            easingFunctions.Should().Contain(easing => easing.Name == "Linear");
            easingFunctions.Should().Contain(easing => easing.Name == "Bounce");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineAnimationDurations()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var animationDurations = animationsTransitions.GetAnimationDurations();

            // Assert
            animationDurations.Should().NotBeNull();
            animationDurations.Should().Contain(duration => duration.Name == "Fast");
            animationDurations.Should().Contain(duration => duration.Name == "Normal");
            animationDurations.Should().Contain(duration => duration.Name == "Slow");
            animationDurations.Should().Contain(duration => duration.Name == "VerySlow");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefinePageTransitions()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var pageTransitions = animationsTransitions.GetPageTransitions();

            // Assert
            pageTransitions.Should().NotBeNull();
            pageTransitions.Should().Contain(transition => transition.Name == "SlideIn");
            pageTransitions.Should().Contain(transition => transition.Name == "FadeIn");
            pageTransitions.Should().Contain(transition => transition.Name == "ScaleIn");
            pageTransitions.Should().Contain(transition => transition.Name == "SlideUp");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineModalTransitions()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var modalTransitions = animationsTransitions.GetModalTransitions();

            // Assert
            modalTransitions.Should().NotBeNull();
            modalTransitions.Should().Contain(transition => transition.Name == "ModalSlideUp");
            modalTransitions.Should().Contain(transition => transition.Name == "ModalFadeIn");
            modalTransitions.Should().Contain(transition => transition.Name == "ModalScaleIn");
            modalTransitions.Should().Contain(transition => transition.Name == "ModalSlideIn");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineListTransitions()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var listTransitions = animationsTransitions.GetListTransitions();

            // Assert
            listTransitions.Should().NotBeNull();
            listTransitions.Should().Contain(transition => transition.Name == "StaggeredFadeIn");
            listTransitions.Should().Contain(transition => transition.Name == "SlideInFromRight");
            listTransitions.Should().Contain(transition => transition.Name == "ScaleIn");
            listTransitions.Should().Contain(transition => transition.Name == "BounceIn");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineCardTransitions()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var cardTransitions = animationsTransitions.GetCardTransitions();

            // Assert
            cardTransitions.Should().NotBeNull();
            cardTransitions.Should().Contain(transition => transition.Name == "CardFlip");
            cardTransitions.Should().Contain(transition => transition.Name == "CardHover");
            cardTransitions.Should().Contain(transition => transition.Name == "CardClick");
            cardTransitions.Should().Contain(transition => transition.Name == "CardSlide");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineAnimationPerformance()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var animationPerformance = animationsTransitions.GetAnimationPerformance();

            // Assert
            animationPerformance.Should().NotBeNull();
            animationPerformance.TargetFPS.Should().Be(60);
            animationPerformance.GPUAcceleration.Should().BeTrue();
            animationPerformance.ReducedMotion.Should().BeTrue();
            animationPerformance.PerformanceMode.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineAnimationSettings()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var animationSettings = animationsTransitions.GetAnimationSettings();

            // Assert
            animationSettings.Should().NotBeNull();
            animationSettings.IsEnabled.Should().BeTrue();
            animationSettings.DefaultDuration.Should().BeGreaterThan(0);
            animationSettings.DefaultEasing.Should().NotBeNullOrEmpty();
            animationSettings.ReducedMotion.Should().BeFalse();
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineAnimationTriggers()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var animationTriggers = animationsTransitions.GetAnimationTriggers();

            // Assert
            animationTriggers.Should().NotBeNull();
            animationTriggers.Should().Contain(trigger => trigger.Type == "OnLoad");
            animationTriggers.Should().Contain(trigger => trigger.Type == "OnClick");
            animationTriggers.Should().Contain(trigger => trigger.Type == "OnHover");
            animationTriggers.Should().Contain(trigger => trigger.Type == "OnFocus");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineAnimationSequences()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var animationSequences = animationsTransitions.GetAnimationSequences();

            // Assert
            animationSequences.Should().NotBeNull();
            animationSequences.Should().Contain(sequence => sequence.Name == "PageLoad");
            animationSequences.Should().Contain(sequence => sequence.Name == "CardFlip");
            animationSequences.Should().Contain(sequence => sequence.Name == "ListUpdate");
            animationSequences.Should().Contain(sequence => sequence.Name == "ModalOpen");
        }

        [Fact]
        public void AnimationsTransitions_ShouldDefineAccessibilityFeatures()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();

            // Act
            var accessibilityFeatures = animationsTransitions.GetAccessibilityFeatures();

            // Assert
            accessibilityFeatures.Should().NotBeNull();
            accessibilityFeatures.ReducedMotion.Should().NotBeNullOrEmpty();
            accessibilityFeatures.HighContrast.Should().NotBeNullOrEmpty();
            accessibilityFeatures.ScreenReader.Should().NotBeNullOrEmpty();
            accessibilityFeatures.KeyboardNavigation.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void AnimationsTransitions_ShouldValidateAnimationConfiguration()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();
            var config = new AnimationConfiguration
            {
                Duration = 300,
                Easing = "EaseInOut",
                Delay = 0,
                Iterations = 1
            };

            // Act
            var validation = animationsTransitions.ValidateAnimationConfiguration(config);

            // Assert
            validation.Should().NotBeNull();
            validation.IsValid.Should().BeTrue();
            validation.Errors.Should().BeEmpty();
        }

        [Fact]
        public void AnimationsTransitions_ShouldHandleInvalidAnimationConfiguration()
        {
            // Arrange
            var animationsTransitions = new AnimationsTransitions();
            var invalidConfig = new AnimationConfiguration
            {
                Duration = -100, // Invalid negative duration
                Easing = "Invalid", // Invalid easing function
                Delay = -50, // Invalid negative delay
                Iterations = 0 // Invalid zero iterations
            };

            // Act
            var validation = animationsTransitions.ValidateAnimationConfiguration(invalidConfig);

            // Assert
            validation.Should().NotBeNull();
            validation.IsValid.Should().BeFalse();
            validation.Errors.Should().NotBeEmpty();
        }
    }
}
