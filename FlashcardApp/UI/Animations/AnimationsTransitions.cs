using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.UI.Animations
{
    /// <summary>
    /// Represents an animation type
    /// </summary>
    public class AnimationType
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Property { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents a transition type
    /// </summary>
    public class TransitionType
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents an easing function
    /// </summary>
    public class EasingFunction
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents animation duration
    /// </summary>
    public class AnimationDuration
    {
        public string Name { get; set; } = string.Empty;
        public int Milliseconds { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents a page transition
    /// </summary>
    public class PageTransition
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AnimationType { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Easing { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents a modal transition
    /// </summary>
    public class ModalTransition
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AnimationType { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Easing { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents a list transition
    /// </summary>
    public class ListTransition
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AnimationType { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int StaggerDelay { get; set; }
        public string Easing { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents a card transition
    /// </summary>
    public class CardTransition
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AnimationType { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Easing { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents animation performance settings
    /// </summary>
    public class AnimationPerformance
    {
        public int TargetFPS { get; set; } = 60;
        public bool GPUAcceleration { get; set; } = true;
        public bool ReducedMotion { get; set; } = true;
        public string PerformanceMode { get; set; } = "Balanced";
        public bool HardwareAcceleration { get; set; } = true;
    }

    /// <summary>
    /// Represents animation settings
    /// </summary>
    public class AnimationSettings
    {
        public bool IsEnabled { get; set; } = true;
        public int DefaultDuration { get; set; } = 300;
        public string DefaultEasing { get; set; } = "EaseInOut";
        public bool ReducedMotion { get; set; } = false;
        public bool HighPerformance { get; set; } = false;
    }

    /// <summary>
    /// Represents an animation trigger
    /// </summary>
    public class AnimationTrigger
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents an animation sequence
    /// </summary>
    public class AnimationSequence
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<AnimationStep> Steps { get; set; } = new List<AnimationStep>();
        public int TotalDuration { get; set; }
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents an animation step
    /// </summary>
    public class AnimationStep
    {
        public string Name { get; set; } = string.Empty;
        public string AnimationType { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int Delay { get; set; }
        public string Easing { get; set; } = string.Empty;
        public int Order { get; set; }
    }

    /// <summary>
    /// Represents accessibility features for animations
    /// </summary>
    public class AnimationAccessibility
    {
        public string ReducedMotion { get; set; } = string.Empty;
        public string HighContrast { get; set; } = string.Empty;
        public string ScreenReader { get; set; } = string.Empty;
        public string KeyboardNavigation { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents animation configuration
    /// </summary>
    public class AnimationConfiguration
    {
        public int Duration { get; set; } = 300;
        public string Easing { get; set; } = "EaseInOut";
        public int Delay { get; set; } = 0;
        public int Iterations { get; set; } = 1;
        public bool AutoReverse { get; set; } = false;
    }

    /// <summary>
    /// Represents validation result
    /// </summary>
    public class AnimationValidation
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Smooth animations and transitions throughout the application
    /// </summary>
    public class AnimationsTransitions
    {
        public List<AnimationType> GetAnimationTypes()
        {
            return new List<AnimationType>
            {
                new AnimationType
                {
                    Type = "Fade",
                    Name = "Fade Animation",
                    Description = "Fade in/out animation using opacity",
                    Property = "opacity",
                    IsEnabled = true
                },
                new AnimationType
                {
                    Type = "Slide",
                    Name = "Slide Animation",
                    Description = "Slide animation using transform",
                    Property = "transform",
                    IsEnabled = true
                },
                new AnimationType
                {
                    Type = "Scale",
                    Name = "Scale Animation",
                    Description = "Scale animation using transform",
                    Property = "transform",
                    IsEnabled = true
                },
                new AnimationType
                {
                    Type = "Rotate",
                    Name = "Rotate Animation",
                    Description = "Rotation animation using transform",
                    Property = "transform",
                    IsEnabled = true
                },
                new AnimationType
                {
                    Type = "Flip",
                    Name = "Flip Animation",
                    Description = "3D flip animation using transform",
                    Property = "transform",
                    IsEnabled = true
                }
            };
        }

        public List<TransitionType> GetTransitionTypes()
        {
            return new List<TransitionType>
            {
                new TransitionType
                {
                    Type = "PageTransition",
                    Name = "Page Transition",
                    Description = "Smooth transitions between pages",
                    Direction = "horizontal",
                    IsEnabled = true
                },
                new TransitionType
                {
                    Type = "ModalTransition",
                    Name = "Modal Transition",
                    Description = "Smooth modal open/close transitions",
                    Direction = "vertical",
                    IsEnabled = true
                },
                new TransitionType
                {
                    Type = "ListTransition",
                    Name = "List Transition",
                    Description = "Smooth list item transitions",
                    Direction = "vertical",
                    IsEnabled = true
                },
                new TransitionType
                {
                    Type = "CardTransition",
                    Name = "Card Transition",
                    Description = "Smooth card interactions",
                    Direction = "all",
                    IsEnabled = true
                }
            };
        }

        public List<EasingFunction> GetEasingFunctions()
        {
            return new List<EasingFunction>
            {
                new EasingFunction
                {
                    Name = "EaseIn",
                    Description = "Slow start, fast finish",
                    Function = "cubic-bezier(0.4, 0, 1, 1)",
                    IsEnabled = true
                },
                new EasingFunction
                {
                    Name = "EaseOut",
                    Description = "Fast start, slow finish",
                    Function = "cubic-bezier(0, 0, 0.2, 1)",
                    IsEnabled = true
                },
                new EasingFunction
                {
                    Name = "EaseInOut",
                    Description = "Slow start and finish",
                    Function = "cubic-bezier(0.4, 0, 0.2, 1)",
                    IsEnabled = true
                },
                new EasingFunction
                {
                    Name = "Linear",
                    Description = "Constant speed",
                    Function = "linear",
                    IsEnabled = true
                },
                new EasingFunction
                {
                    Name = "Bounce",
                    Description = "Bouncing effect",
                    Function = "cubic-bezier(0.68, -0.55, 0.265, 1.55)",
                    IsEnabled = true
                }
            };
        }

        public List<AnimationDuration> GetAnimationDurations()
        {
            return new List<AnimationDuration>
            {
                new AnimationDuration
                {
                    Name = "Fast",
                    Milliseconds = 150,
                    Description = "Quick animations for immediate feedback",
                    IsEnabled = true
                },
                new AnimationDuration
                {
                    Name = "Normal",
                    Milliseconds = 300,
                    Description = "Standard animation duration",
                    IsEnabled = true
                },
                new AnimationDuration
                {
                    Name = "Slow",
                    Milliseconds = 500,
                    Description = "Slower animations for emphasis",
                    IsEnabled = true
                },
                new AnimationDuration
                {
                    Name = "VerySlow",
                    Milliseconds = 800,
                    Description = "Very slow animations for dramatic effect",
                    IsEnabled = true
                }
            };
        }

        public List<PageTransition> GetPageTransitions()
        {
            return new List<PageTransition>
            {
                new PageTransition
                {
                    Name = "SlideIn",
                    Description = "Slide in from the right",
                    AnimationType = "Slide",
                    Duration = 300,
                    Easing = "EaseInOut",
                    IsEnabled = true
                },
                new PageTransition
                {
                    Name = "FadeIn",
                    Description = "Fade in smoothly",
                    AnimationType = "Fade",
                    Duration = 250,
                    Easing = "EaseInOut",
                    IsEnabled = true
                },
                new PageTransition
                {
                    Name = "ScaleIn",
                    Description = "Scale in from center",
                    AnimationType = "Scale",
                    Duration = 400,
                    Easing = "EaseOut",
                    IsEnabled = true
                },
                new PageTransition
                {
                    Name = "SlideUp",
                    Description = "Slide up from bottom",
                    AnimationType = "Slide",
                    Duration = 350,
                    Easing = "EaseInOut",
                    IsEnabled = true
                }
            };
        }

        public List<ModalTransition> GetModalTransitions()
        {
            return new List<ModalTransition>
            {
                new ModalTransition
                {
                    Name = "ModalSlideUp",
                    Description = "Modal slides up from bottom",
                    AnimationType = "Slide",
                    Duration = 300,
                    Easing = "EaseOut",
                    IsEnabled = true
                },
                new ModalTransition
                {
                    Name = "ModalFadeIn",
                    Description = "Modal fades in with backdrop",
                    AnimationType = "Fade",
                    Duration = 250,
                    Easing = "EaseInOut",
                    IsEnabled = true
                },
                new ModalTransition
                {
                    Name = "ModalScaleIn",
                    Description = "Modal scales in from center",
                    AnimationType = "Scale",
                    Duration = 350,
                    Easing = "EaseOut",
                    IsEnabled = true
                },
                new ModalTransition
                {
                    Name = "ModalSlideIn",
                    Description = "Modal slides in from right",
                    AnimationType = "Slide",
                    Duration = 300,
                    Easing = "EaseInOut",
                    IsEnabled = true
                }
            };
        }

        public List<ListTransition> GetListTransitions()
        {
            return new List<ListTransition>
            {
                new ListTransition
                {
                    Name = "StaggeredFadeIn",
                    Description = "Items fade in with staggered timing",
                    AnimationType = "Fade",
                    Duration = 300,
                    StaggerDelay = 50,
                    Easing = "EaseInOut",
                    IsEnabled = true
                },
                new ListTransition
                {
                    Name = "SlideInFromRight",
                    Description = "Items slide in from the right",
                    AnimationType = "Slide",
                    Duration = 250,
                    StaggerDelay = 75,
                    Easing = "EaseOut",
                    IsEnabled = true
                },
                new ListTransition
                {
                    Name = "ScaleIn",
                    Description = "Items scale in with bounce",
                    AnimationType = "Scale",
                    Duration = 400,
                    StaggerDelay = 100,
                    Easing = "Bounce",
                    IsEnabled = true
                },
                new ListTransition
                {
                    Name = "BounceIn",
                    Description = "Items bounce in with elastic effect",
                    AnimationType = "Scale",
                    Duration = 500,
                    StaggerDelay = 80,
                    Easing = "Bounce",
                    IsEnabled = true
                }
            };
        }

        public List<CardTransition> GetCardTransitions()
        {
            return new List<CardTransition>
            {
                new CardTransition
                {
                    Name = "CardFlip",
                    Description = "3D flip animation for cards",
                    AnimationType = "Flip",
                    Duration = 600,
                    Easing = "EaseInOut",
                    IsEnabled = true
                },
                new CardTransition
                {
                    Name = "CardHover",
                    Description = "Subtle hover effect for cards",
                    AnimationType = "Scale",
                    Duration = 200,
                    Easing = "EaseOut",
                    IsEnabled = true
                },
                new CardTransition
                {
                    Name = "CardClick",
                    Description = "Click feedback animation",
                    AnimationType = "Scale",
                    Duration = 150,
                    Easing = "EaseInOut",
                    IsEnabled = true
                },
                new CardTransition
                {
                    Name = "CardSlide",
                    Description = "Slide animation for card interactions",
                    AnimationType = "Slide",
                    Duration = 300,
                    Easing = "EaseInOut",
                    IsEnabled = true
                }
            };
        }

        public AnimationPerformance GetAnimationPerformance()
        {
            return new AnimationPerformance
            {
                TargetFPS = 60,
                GPUAcceleration = true,
                ReducedMotion = true,
                PerformanceMode = "Balanced",
                HardwareAcceleration = true
            };
        }

        public AnimationSettings GetAnimationSettings()
        {
            return new AnimationSettings
            {
                IsEnabled = true,
                DefaultDuration = 300,
                DefaultEasing = "EaseInOut",
                ReducedMotion = false,
                HighPerformance = false
            };
        }

        public List<AnimationTrigger> GetAnimationTriggers()
        {
            return new List<AnimationTrigger>
            {
                new AnimationTrigger
                {
                    Type = "OnLoad",
                    Name = "On Load",
                    Description = "Animation triggers when element loads",
                    IsEnabled = true
                },
                new AnimationTrigger
                {
                    Type = "OnClick",
                    Name = "On Click",
                    Description = "Animation triggers on click",
                    IsEnabled = true
                },
                new AnimationTrigger
                {
                    Type = "OnHover",
                    Name = "On Hover",
                    Description = "Animation triggers on hover",
                    IsEnabled = true
                },
                new AnimationTrigger
                {
                    Type = "OnFocus",
                    Name = "On Focus",
                    Description = "Animation triggers on focus",
                    IsEnabled = true
                }
            };
        }

        public List<AnimationSequence> GetAnimationSequences()
        {
            return new List<AnimationSequence>
            {
                new AnimationSequence
                {
                    Name = "PageLoad",
                    Description = "Complete page load animation sequence",
                    Steps = new List<AnimationStep>
                    {
                        new AnimationStep
                        {
                            Name = "FadeIn",
                            AnimationType = "Fade",
                            Duration = 300,
                            Delay = 0,
                            Easing = "EaseInOut",
                            Order = 1
                        },
                        new AnimationStep
                        {
                            Name = "SlideIn",
                            AnimationType = "Slide",
                            Duration = 400,
                            Delay = 100,
                            Easing = "EaseOut",
                            Order = 2
                        }
                    },
                    TotalDuration = 800,
                    IsEnabled = true
                },
                new AnimationSequence
                {
                    Name = "CardFlip",
                    Description = "Card flip animation sequence",
                    Steps = new List<AnimationStep>
                    {
                        new AnimationStep
                        {
                            Name = "FlipStart",
                            AnimationType = "Flip",
                            Duration = 300,
                            Delay = 0,
                            Easing = "EaseInOut",
                            Order = 1
                        },
                        new AnimationStep
                        {
                            Name = "FlipComplete",
                            AnimationType = "Flip",
                            Duration = 300,
                            Delay = 300,
                            Easing = "EaseInOut",
                            Order = 2
                        }
                    },
                    TotalDuration = 600,
                    IsEnabled = true
                },
                new AnimationSequence
                {
                    Name = "ListUpdate",
                    Description = "List update animation sequence",
                    Steps = new List<AnimationStep>
                    {
                        new AnimationStep
                        {
                            Name = "FadeOut",
                            AnimationType = "Fade",
                            Duration = 200,
                            Delay = 0,
                            Easing = "EaseIn",
                            Order = 1
                        },
                        new AnimationStep
                        {
                            Name = "FadeIn",
                            AnimationType = "Fade",
                            Duration = 300,
                            Delay = 200,
                            Easing = "EaseOut",
                            Order = 2
                        }
                    },
                    TotalDuration = 500,
                    IsEnabled = true
                },
                new AnimationSequence
                {
                    Name = "ModalOpen",
                    Description = "Modal open animation sequence",
                    Steps = new List<AnimationStep>
                    {
                        new AnimationStep
                        {
                            Name = "BackdropFade",
                            AnimationType = "Fade",
                            Duration = 250,
                            Delay = 0,
                            Easing = "EaseInOut",
                            Order = 1
                        },
                        new AnimationStep
                        {
                            Name = "ModalSlide",
                            AnimationType = "Slide",
                            Duration = 300,
                            Delay = 50,
                            Easing = "EaseOut",
                            Order = 2
                        }
                    },
                    TotalDuration = 600,
                    IsEnabled = true
                }
            };
        }

        public AnimationAccessibility GetAccessibilityFeatures()
        {
            return new AnimationAccessibility
            {
                ReducedMotion = "Respect user's reduced motion preferences",
                HighContrast = "Ensure animations work with high contrast mode",
                ScreenReader = "Provide alternative feedback for screen readers",
                KeyboardNavigation = "Ensure animations don't interfere with keyboard navigation"
            };
        }

        public AnimationValidation ValidateAnimationConfiguration(AnimationConfiguration config)
        {
            var validation = new AnimationValidation { IsValid = true };

            // Validate duration
            if (config.Duration <= 0)
            {
                validation.IsValid = false;
                validation.Errors.Add("Duration must be greater than 0");
            }
            else if (config.Duration > 2000)
            {
                validation.Warnings.Add("Duration is very long, may affect user experience");
            }

            // Validate easing
            var validEasings = new[] { "EaseIn", "EaseOut", "EaseInOut", "Linear", "Bounce" };
            if (!validEasings.Contains(config.Easing))
            {
                validation.IsValid = false;
                validation.Errors.Add($"Invalid easing function: {config.Easing}. Valid easings are: {string.Join(", ", validEasings)}");
            }

            // Validate delay
            if (config.Delay < 0)
            {
                validation.IsValid = false;
                validation.Errors.Add("Delay cannot be negative");
            }

            // Validate iterations
            if (config.Iterations <= 0)
            {
                validation.IsValid = false;
                validation.Errors.Add("Iterations must be greater than 0");
            }
            else if (config.Iterations > 10)
            {
                validation.Warnings.Add("High number of iterations may cause performance issues");
            }

            return validation;
        }
    }
}
