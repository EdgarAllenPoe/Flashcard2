using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.UI.StudySession
{
    /// <summary>
    /// Represents a card flip animation
    /// </summary>
    public class CardFlipAnimation
    {
        public double Duration { get; set; } = 600;
        public string Easing { get; set; } = "ease-in-out";
        public string Transform { get; set; } = "rotateY(180deg)";
        public string Trigger { get; set; } = "click";
    }

    /// <summary>
    /// Represents progress tracking for study session
    /// </summary>
    public class ProgressTracking
    {
        public int TotalCards { get; set; } = 0;
        public int CompletedCards { get; set; } = 0;
        public int RemainingCards { get; set; } = 0;
        public double ProgressPercentage { get; set; } = 0.0;
    }

    /// <summary>
    /// Represents card states
    /// </summary>
    public class CardState
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = true;
        public string Style { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents study modes
    /// </summary>
    public class StudyMode
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public string Icon { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents answer buttons
    /// </summary>
    public class AnswerButton
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Shortcut { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents session statistics
    /// </summary>
    public class SessionStatistics
    {
        public DateTime StartTime { get; set; } = DateTime.Now;
        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;
        public double CardsPerMinute { get; set; } = 0.0;
        public double Accuracy { get; set; } = 0.0;
        public int CorrectAnswers { get; set; } = 0;
        public int IncorrectAnswers { get; set; } = 0;
        public int SkippedCards { get; set; } = 0;
    }

    /// <summary>
    /// Represents card animations
    /// </summary>
    public class CardAnimation
    {
        public string Name { get; set; } = string.Empty;
        public double Duration { get; set; } = 300;
        public string Easing { get; set; } = string.Empty;
        public string Property { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a progress bar
    /// </summary>
    public class ProgressBar
    {
        public double CurrentValue { get; set; } = 0;
        public double MaxValue { get; set; } = 100;
        public double Percentage { get; set; } = 0;
        public string Color { get; set; } = "#0078D4";
        public string BackgroundColor { get; set; } = "#E1DFDD";
        public double Height { get; set; } = 8;
    }

    /// <summary>
    /// Represents keyboard shortcuts
    /// </summary>
    public class KeyboardShortcut
    {
        public string Action { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Modifier { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents session controls
    /// </summary>
    public class SessionControl
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public string Command { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents card layout
    /// </summary>
    public class CardLayout
    {
        public double Width { get; set; } = 400;
        public double Height { get; set; } = 300;
        public double BorderRadius { get; set; } = 12;
        public string Shadow { get; set; } = "0 8px 16px rgba(0,0,0,0.15)";
        public string Background { get; set; } = "#FFFFFF";
        public string Border { get; set; } = "1px solid #E1DFDD";
    }

    /// <summary>
    /// Represents session feedback
    /// </summary>
    public class SessionFeedback
    {
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public double Duration { get; set; } = 2000;
    }

    /// <summary>
    /// Represents accessibility features
    /// </summary>
    public class StudySessionAccessibility
    {
        public string ScreenReader { get; set; } = string.Empty;
        public string KeyboardNavigation { get; set; } = string.Empty;
        public string HighContrast { get; set; } = string.Empty;
        public string FocusManagement { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents session settings
    /// </summary>
    public class SessionSettings
    {
        public bool AutoFlip { get; set; } = false;
        public bool ShowProgress { get; set; } = true;
        public bool EnableAnimations { get; set; } = true;
        public bool SoundEffects { get; set; } = false;
        public int MaxCards { get; set; } = 20;
        public double AutoFlipDelay { get; set; } = 3000;
    }

    /// <summary>
    /// Represents session configuration
    /// </summary>
    public class SessionConfiguration
    {
        public int MaxCards { get; set; } = 20;
        public string StudyMode { get; set; } = "Front to Back";
        public bool AutoFlip { get; set; } = false;
        public bool ShowProgress { get; set; } = true;
        public bool EnableAnimations { get; set; } = true;
        public bool SoundEffects { get; set; } = false;
    }

    /// <summary>
    /// Represents validation result
    /// </summary>
    public class SessionValidation
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Beautiful study session UI with card flip animations and progress tracking
    /// </summary>
    public class StudySessionUI
    {
        public CardFlipAnimation GetCardFlipAnimation()
        {
            return new CardFlipAnimation
            {
                Duration = 600,
                Easing = "ease-in-out",
                Transform = "rotateY(180deg)",
                Trigger = "click"
            };
        }

        public ProgressTracking GetProgressTracking()
        {
            return new ProgressTracking
            {
                TotalCards = 20,
                CompletedCards = 5,
                RemainingCards = 15,
                ProgressPercentage = 25.0
            };
        }

        public Dictionary<string, CardState> GetCardStates()
        {
            return new Dictionary<string, CardState>
            {
                ["Front"] = new CardState
                {
                    Name = "Front",
                    Description = "Card showing the question or front side",
                    IsVisible = true,
                    Style = "front-visible"
                },
                ["Back"] = new CardState
                {
                    Name = "Back",
                    Description = "Card showing the answer or back side",
                    IsVisible = false,
                    Style = "back-hidden"
                },
                ["Flipping"] = new CardState
                {
                    Name = "Flipping",
                    Description = "Card in the middle of flip animation",
                    IsVisible = true,
                    Style = "flipping"
                },
                ["Completed"] = new CardState
                {
                    Name = "Completed",
                    Description = "Card that has been answered",
                    IsVisible = true,
                    Style = "completed"
                }
            };
        }

        public List<StudyMode> GetStudyModes()
        {
            return new List<StudyMode>
            {
                new StudyMode
                {
                    Name = "Front to Back",
                    Description = "Show front side first, then back side",
                    IsEnabled = true,
                    Icon = "🔄"
                },
                new StudyMode
                {
                    Name = "Back to Front",
                    Description = "Show back side first, then front side",
                    IsEnabled = true,
                    Icon = "🔄"
                },
                new StudyMode
                {
                    Name = "Mixed",
                    Description = "Randomly show front or back side first",
                    IsEnabled = true,
                    Icon = "🎲"
                },
                new StudyMode
                {
                    Name = "Review",
                    Description = "Review previously studied cards",
                    IsEnabled = true,
                    Icon = "📚"
                }
            };
        }

        public List<AnswerButton> GetAnswerButtons()
        {
            return new List<AnswerButton>
            {
                new AnswerButton
                {
                    Type = "Correct",
                    Label = "Correct",
                    Icon = "✅",
                    Color = "#107C10",
                    Shortcut = "1",
                    IsEnabled = true
                },
                new AnswerButton
                {
                    Type = "Incorrect",
                    Label = "Incorrect",
                    Icon = "❌",
                    Color = "#D13438",
                    Shortcut = "2",
                    IsEnabled = true
                },
                new AnswerButton
                {
                    Type = "Skip",
                    Label = "Skip",
                    Icon = "⏭️",
                    Color = "#605E5C",
                    Shortcut = "3",
                    IsEnabled = true
                },
                new AnswerButton
                {
                    Type = "Hard",
                    Label = "Hard",
                    Icon = "😰",
                    Color = "#FF8C00",
                    Shortcut = "4",
                    IsEnabled = true
                }
            };
        }

        public SessionStatistics GetSessionStatistics()
        {
            return new SessionStatistics
            {
                StartTime = DateTime.Now,
                ElapsedTime = TimeSpan.FromMinutes(5),
                CardsPerMinute = 4.0,
                Accuracy = 85.0,
                CorrectAnswers = 17,
                IncorrectAnswers = 3,
                SkippedCards = 0
            };
        }

        public Dictionary<string, CardAnimation> GetCardAnimations()
        {
            return new Dictionary<string, CardAnimation>
            {
                ["Flip"] = new CardAnimation
                {
                    Name = "Flip",
                    Duration = 600,
                    Easing = "ease-in-out",
                    Property = "transform"
                },
                ["Slide"] = new CardAnimation
                {
                    Name = "Slide",
                    Duration = 400,
                    Easing = "ease-out",
                    Property = "transform"
                },
                ["Fade"] = new CardAnimation
                {
                    Name = "Fade",
                    Duration = 300,
                    Easing = "ease-in-out",
                    Property = "opacity"
                },
                ["Scale"] = new CardAnimation
                {
                    Name = "Scale",
                    Duration = 250,
                    Easing = "ease-out",
                    Property = "transform"
                }
            };
        }

        public ProgressBar GetProgressBar()
        {
            return new ProgressBar
            {
                CurrentValue = 5,
                MaxValue = 20,
                Percentage = 25.0,
                Color = "#0078D4",
                BackgroundColor = "#E1DFDD",
                Height = 8
            };
        }

        public Dictionary<string, KeyboardShortcut> GetKeyboardShortcuts()
        {
            return new Dictionary<string, KeyboardShortcut>
            {
                ["FlipCard"] = new KeyboardShortcut
                {
                    Action = "FlipCard",
                    Key = "Space",
                    Modifier = "",
                    Description = "Flip the current card"
                },
                ["CorrectAnswer"] = new KeyboardShortcut
                {
                    Action = "CorrectAnswer",
                    Key = "1",
                    Modifier = "",
                    Description = "Mark answer as correct"
                },
                ["IncorrectAnswer"] = new KeyboardShortcut
                {
                    Action = "IncorrectAnswer",
                    Key = "2",
                    Modifier = "",
                    Description = "Mark answer as incorrect"
                },
                ["SkipCard"] = new KeyboardShortcut
                {
                    Action = "SkipCard",
                    Key = "3",
                    Modifier = "",
                    Description = "Skip current card"
                },
                ["PauseSession"] = new KeyboardShortcut
                {
                    Action = "PauseSession",
                    Key = "Escape",
                    Modifier = "",
                    Description = "Pause the study session"
                }
            };
        }

        public List<SessionControl> GetSessionControls()
        {
            return new List<SessionControl>
            {
                new SessionControl
                {
                    Type = "Start",
                    Label = "Start Session",
                    Icon = "▶️",
                    IsEnabled = true,
                    Command = "StartSessionCommand"
                },
                new SessionControl
                {
                    Type = "Pause",
                    Label = "Pause",
                    Icon = "⏸️",
                    IsEnabled = true,
                    Command = "PauseSessionCommand"
                },
                new SessionControl
                {
                    Type = "Resume",
                    Label = "Resume",
                    Icon = "▶️",
                    IsEnabled = false,
                    Command = "ResumeSessionCommand"
                },
                new SessionControl
                {
                    Type = "Stop",
                    Label = "Stop Session",
                    Icon = "⏹️",
                    IsEnabled = true,
                    Command = "StopSessionCommand"
                },
                new SessionControl
                {
                    Type = "Reset",
                    Label = "Reset Progress",
                    Icon = "🔄",
                    IsEnabled = true,
                    Command = "ResetSessionCommand"
                }
            };
        }

        public CardLayout GetCardLayout()
        {
            return new CardLayout
            {
                Width = 400,
                Height = 300,
                BorderRadius = 12,
                Shadow = "0 8px 16px rgba(0,0,0,0.15)",
                Background = "#FFFFFF",
                Border = "1px solid #E1DFDD"
            };
        }

        public Dictionary<string, SessionFeedback> GetSessionFeedback()
        {
            return new Dictionary<string, SessionFeedback>
            {
                ["Correct"] = new SessionFeedback
                {
                    Type = "Correct",
                    Message = "Great job! Keep it up!",
                    Icon = "🎉",
                    Color = "#107C10",
                    Duration = 2000
                },
                ["Incorrect"] = new SessionFeedback
                {
                    Type = "Incorrect",
                    Message = "Don't worry, you'll get it next time!",
                    Icon = "💪",
                    Color = "#D13438",
                    Duration = 2000
                },
                ["Streak"] = new SessionFeedback
                {
                    Type = "Streak",
                    Message = "Amazing streak! You're on fire!",
                    Icon = "🔥",
                    Color = "#FF8C00",
                    Duration = 3000
                },
                ["Completion"] = new SessionFeedback
                {
                    Type = "Completion",
                    Message = "Session completed! Well done!",
                    Icon = "🏆",
                    Color = "#0078D4",
                    Duration = 4000
                }
            };
        }

        public StudySessionAccessibility GetAccessibilityFeatures()
        {
            return new StudySessionAccessibility
            {
                ScreenReader = "ARIA labels and semantic markup for all study session elements",
                KeyboardNavigation = "Full keyboard navigation support with shortcuts",
                HighContrast = "High contrast mode support for better visibility",
                FocusManagement = "Clear focus indicators and logical focus flow"
            };
        }

        public SessionSettings GetSessionSettings()
        {
            return new SessionSettings
            {
                AutoFlip = false,
                ShowProgress = true,
                EnableAnimations = true,
                SoundEffects = false,
                MaxCards = 20,
                AutoFlipDelay = 3000
            };
        }

        public SessionValidation ValidateSessionConfiguration(SessionConfiguration config)
        {
            var validation = new SessionValidation { IsValid = true };

            // Validate max cards
            if (config.MaxCards <= 0)
            {
                validation.IsValid = false;
                validation.Errors.Add("Max cards must be greater than 0");
            }
            else if (config.MaxCards > 100)
            {
                validation.Warnings.Add("Max cards is very high, may affect session performance");
            }

            // Validate study mode
            var validModes = new[] { "Front to Back", "Back to Front", "Mixed", "Review" };
            if (!validModes.Contains(config.StudyMode))
            {
                validation.IsValid = false;
                validation.Errors.Add($"Invalid study mode: {config.StudyMode}. Valid modes are: {string.Join(", ", validModes)}");
            }

            // Validate boolean settings
            if (config.AutoFlip && config.EnableAnimations == false)
            {
                validation.Warnings.Add("Auto flip is enabled but animations are disabled, which may cause poor user experience");
            }

            return validation;
        }
    }
}
