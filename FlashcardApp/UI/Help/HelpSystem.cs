using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.UI.Help
{
    /// <summary>
    /// Represents a help topic
    /// </summary>
    public class HelpTopic
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public int Order { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
    }

    /// <summary>
    /// Represents help search functionality
    /// </summary>
    public class HelpSearch
    {
        public bool IsEnabled { get; set; } = true;
        public string Placeholder { get; set; } = "Search help...";
        public List<string> SearchFields { get; set; } = new List<string>();
        public bool CaseSensitive { get; set; } = false;
        public bool FuzzyMatching { get; set; } = true;
        public int MaxResults { get; set; } = 20;
    }

    /// <summary>
    /// Represents an interactive guide
    /// </summary>
    public class InteractiveGuide
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<GuideStep> Steps { get; set; } = new List<GuideStep>();
        public bool IsEnabled { get; set; } = true;
        public int EstimatedTime { get; set; } = 5; // minutes
        public string Difficulty { get; set; } = "Beginner";
    }

    /// <summary>
    /// Represents a guide step
    /// </summary>
    public class GuideStep
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string ExpectedResult { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public int Order { get; set; } = 0;
    }

    /// <summary>
    /// Represents a help category
    /// </summary>
    public class HelpCategory
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<string> Topics { get; set; } = new List<string>();
        public int Order { get; set; } = 0;
        public bool IsExpanded { get; set; } = false;
    }

    /// <summary>
    /// Represents help content
    /// </summary>
    public class HelpContent
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Format { get; set; } = "Markdown";
        public List<string> Images { get; set; } = new List<string>();
        public List<string> Videos { get; set; } = new List<string>();
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Represents help navigation
    /// </summary>
    public class HelpNavigation
    {
        public List<string> Breadcrumbs { get; set; } = new List<string>();
        public string PreviousTopic { get; set; } = string.Empty;
        public string NextTopic { get; set; } = string.Empty;
        public List<string> RelatedTopics { get; set; } = new List<string>();
        public string CurrentTopic { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents help feedback
    /// </summary>
    public class HelpFeedback
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public bool RequiresComment { get; set; } = false;
    }

    /// <summary>
    /// Represents help bookmarks
    /// </summary>
    public class HelpBookmarks
    {
        public List<HelpBookmark> Bookmarks { get; set; } = new List<HelpBookmark>();
        public bool IsEnabled { get; set; } = true;
        public int MaxBookmarks { get; set; } = 50;
    }

    /// <summary>
    /// Represents a help bookmark
    /// </summary>
    public class HelpBookmark
    {
        public string TopicName { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents help history
    /// </summary>
    public class HelpHistory
    {
        public List<HelpHistoryItem> RecentTopics { get; set; } = new List<HelpHistoryItem>();
        public bool IsEnabled { get; set; } = true;
        public int MaxHistoryItems { get; set; } = 100;
    }

    /// <summary>
    /// Represents a help history item
    /// </summary>
    public class HelpHistoryItem
    {
        public string TopicName { get; set; } = string.Empty;
        public DateTime AccessedAt { get; set; } = DateTime.Now;
        public int AccessCount { get; set; } = 1;
        public TimeSpan TimeSpent { get; set; } = TimeSpan.Zero;
    }

    /// <summary>
    /// Represents help accessibility
    /// </summary>
    public class HelpAccessibility
    {
        public string ScreenReader { get; set; } = string.Empty;
        public string KeyboardNavigation { get; set; } = string.Empty;
        public string HighContrast { get; set; } = string.Empty;
        public string TextSize { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents help printing
    /// </summary>
    public class HelpPrinting
    {
        public bool IsEnabled { get; set; } = true;
        public List<string> PrintFormats { get; set; } = new List<string>();
        public bool IncludeImages { get; set; } = true;
        public bool IncludeNavigation { get; set; } = false;
    }

    /// <summary>
    /// Represents help sharing
    /// </summary>
    public class HelpSharing
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents help context
    /// </summary>
    public class HelpContext
    {
        public string CurrentPage { get; set; } = string.Empty;
        public List<string> SuggestedTopics { get; set; } = new List<string>();
        public string ContextualHelp { get; set; } = string.Empty;
        public Dictionary<string, string> PageContext { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// Represents a help request
    /// </summary>
    public class HelpRequest
    {
        public string Topic { get; set; } = string.Empty;
        public string SearchQuery { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents validation result
    /// </summary>
    public class HelpValidation
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Contextual help system with search and interactive guides
    /// </summary>
    public class HelpSystem
    {
        public List<HelpTopic> GetHelpTopics()
        {
            return new List<HelpTopic>
            {
                new HelpTopic
                {
                    Name = "Getting Started",
                    Description = "Learn the basics of using the flashcard app",
                    Content = "Welcome to the flashcard app! This guide will help you get started with creating and studying flashcards.",
                    Category = "Basics",
                    Tags = new List<string> { "beginner", "tutorial", "setup" },
                    Order = 1,
                    IsVisible = true
                },
                new HelpTopic
                {
                    Name = "Study Sessions",
                    Description = "How to conduct effective study sessions",
                    Content = "Study sessions are the core of your learning experience. Learn how to optimize your study time.",
                    Category = "Study",
                    Tags = new List<string> { "study", "learning", "sessions" },
                    Order = 2,
                    IsVisible = true
                },
                new HelpTopic
                {
                    Name = "Deck Management",
                    Description = "Creating, organizing, and managing your flashcard decks",
                    Content = "Organize your learning materials with effective deck management strategies.",
                    Category = "Management",
                    Tags = new List<string> { "decks", "organization", "management" },
                    Order = 3,
                    IsVisible = true
                },
                new HelpTopic
                {
                    Name = "Statistics",
                    Description = "Understanding your learning progress and statistics",
                    Content = "Track your learning progress with comprehensive statistics and analytics.",
                    Category = "Advanced",
                    Tags = new List<string> { "statistics", "progress", "analytics" },
                    Order = 4,
                    IsVisible = true
                },
                new HelpTopic
                {
                    Name = "Settings",
                    Description = "Customizing the application to your preferences",
                    Content = "Personalize your experience with comprehensive settings and preferences.",
                    Category = "Advanced",
                    Tags = new List<string> { "settings", "preferences", "customization" },
                    Order = 5,
                    IsVisible = true
                }
            };
        }

        public HelpSearch GetHelpSearch()
        {
            return new HelpSearch
            {
                IsEnabled = true,
                Placeholder = "Search help...",
                SearchFields = new List<string> { "Title", "Content", "Tags" },
                CaseSensitive = false,
                FuzzyMatching = true,
                MaxResults = 20
            };
        }

        public List<InteractiveGuide> GetInteractiveGuides()
        {
            return new List<InteractiveGuide>
            {
                new InteractiveGuide
                {
                    Name = "First Study Session",
                    Description = "Complete your first study session step by step",
                    Steps = new List<GuideStep>
                    {
                        new GuideStep
                        {
                            Title = "Select a Deck",
                            Description = "Choose a deck to study from your collection",
                            Action = "Click on a deck in the deck list",
                            ExpectedResult = "Deck is selected and ready for study",
                            IsCompleted = false,
                            Order = 1
                        },
                        new GuideStep
                        {
                            Title = "Start Study Session",
                            Description = "Begin your study session",
                            Action = "Click the 'Study' button",
                            ExpectedResult = "Study session interface opens",
                            IsCompleted = false,
                            Order = 2
                        }
                    },
                    IsEnabled = true,
                    EstimatedTime = 5,
                    Difficulty = "Beginner"
                },
                new InteractiveGuide
                {
                    Name = "Creating Your First Deck",
                    Description = "Learn how to create and organize your first flashcard deck",
                    Steps = new List<GuideStep>
                    {
                        new GuideStep
                        {
                            Title = "Create New Deck",
                            Description = "Create a new flashcard deck",
                            Action = "Click 'Create Deck' button",
                            ExpectedResult = "New deck creation dialog opens",
                            IsCompleted = false,
                            Order = 1
                        },
                        new GuideStep
                        {
                            Title = "Add Cards",
                            Description = "Add flashcards to your deck",
                            Action = "Click 'Add Card' and enter front/back content",
                            ExpectedResult = "Cards are added to the deck",
                            IsCompleted = false,
                            Order = 2
                        }
                    },
                    IsEnabled = true,
                    EstimatedTime = 10,
                    Difficulty = "Beginner"
                },
                new InteractiveGuide
                {
                    Name = "Understanding Statistics",
                    Description = "Learn how to interpret your learning statistics",
                    Steps = new List<GuideStep>
                    {
                        new GuideStep
                        {
                            Title = "View Statistics",
                            Description = "Access the statistics dashboard",
                            Action = "Navigate to the Statistics section",
                            ExpectedResult = "Statistics dashboard is displayed",
                            IsCompleted = false,
                            Order = 1
                        },
                        new GuideStep
                        {
                            Title = "Interpret Metrics",
                            Description = "Understand what the statistics mean",
                            Action = "Review the different metrics and charts",
                            ExpectedResult = "You understand your learning progress",
                            IsCompleted = false,
                            Order = 2
                        }
                    },
                    IsEnabled = true,
                    EstimatedTime = 8,
                    Difficulty = "Intermediate"
                },
                new InteractiveGuide
                {
                    Name = "Customizing Settings",
                    Description = "Personalize the application to your preferences",
                    Steps = new List<GuideStep>
                    {
                        new GuideStep
                        {
                            Title = "Open Settings",
                            Description = "Access the settings panel",
                            Action = "Click on the Settings icon",
                            ExpectedResult = "Settings panel opens",
                            IsCompleted = false,
                            Order = 1
                        },
                        new GuideStep
                        {
                            Title = "Customize Appearance",
                            Description = "Change theme and appearance settings",
                            Action = "Modify theme, font size, and color options",
                            ExpectedResult = "Appearance changes are applied",
                            IsCompleted = false,
                            Order = 2
                        }
                    },
                    IsEnabled = true,
                    EstimatedTime = 6,
                    Difficulty = "Beginner"
                }
            };
        }

        public List<HelpCategory> GetHelpCategories()
        {
            return new List<HelpCategory>
            {
                new HelpCategory
                {
                    Name = "Basics",
                    Description = "Essential information for getting started",
                    Icon = "📚",
                    Topics = new List<string> { "Getting Started", "First Steps", "Basic Navigation" },
                    Order = 1,
                    IsExpanded = true
                },
                new HelpCategory
                {
                    Name = "Study",
                    Description = "Study session and learning techniques",
                    Icon = "🎓",
                    Topics = new List<string> { "Study Sessions", "Learning Methods", "Study Tips" },
                    Order = 2,
                    IsExpanded = false
                },
                new HelpCategory
                {
                    Name = "Management",
                    Description = "Organizing and managing your content",
                    Icon = "📁",
                    Topics = new List<string> { "Deck Management", "Card Organization", "Import/Export" },
                    Order = 3,
                    IsExpanded = false
                },
                new HelpCategory
                {
                    Name = "Advanced",
                    Description = "Advanced features and customization",
                    Icon = "⚙️",
                    Topics = new List<string> { "Statistics", "Settings", "Advanced Features" },
                    Order = 4,
                    IsExpanded = false
                }
            };
        }

        public Dictionary<string, HelpContent> GetHelpContent()
        {
            return new Dictionary<string, HelpContent>
            {
                ["Getting Started"] = new HelpContent
                {
                    Title = "Getting Started",
                    Content = "# Getting Started\n\nWelcome to the flashcard app! This guide will help you get started with creating and studying flashcards.\n\n## What are Flashcards?\n\nFlashcards are a proven learning technique that helps you memorize information through spaced repetition.\n\n## Your First Steps\n\n1. Create your first deck\n2. Add some cards\n3. Start studying!",
                    Format = "Markdown",
                    Images = new List<string> { "getting-started-1.png", "getting-started-2.png" },
                    Videos = new List<string>(),
                    LastUpdated = DateTime.Now.AddDays(-1)
                },
                ["Study Sessions"] = new HelpContent
                {
                    Title = "Study Sessions",
                    Content = "# Study Sessions\n\nStudy sessions are the core of your learning experience. Here's how to make the most of them.\n\n## Starting a Session\n\n1. Select a deck\n2. Choose your study mode\n3. Begin studying\n\n## Study Modes\n\n- **Front to Back**: See the question first\n- **Back to Front**: See the answer first\n- **Mixed**: Random order",
                    Format = "Markdown",
                    Images = new List<string> { "study-session-1.png" },
                    Videos = new List<string> { "study-session-demo.mp4" },
                    LastUpdated = DateTime.Now.AddDays(-2)
                },
                ["Deck Management"] = new HelpContent
                {
                    Title = "Deck Management",
                    Content = "# Deck Management\n\nOrganize your learning materials with effective deck management strategies.\n\n## Creating Decks\n\n1. Click 'Create Deck'\n2. Enter deck name and description\n3. Add tags for organization\n\n## Organizing Decks\n\n- Use descriptive names\n- Add relevant tags\n- Group related decks",
                    Format = "Markdown",
                    Images = new List<string> { "deck-management-1.png", "deck-management-2.png" },
                    Videos = new List<string>(),
                    LastUpdated = DateTime.Now.AddDays(-3)
                },
                ["Statistics"] = new HelpContent
                {
                    Title = "Statistics",
                    Content = "# Statistics\n\nTrack your learning progress with comprehensive statistics and analytics.\n\n## Key Metrics\n\n- **Accuracy**: How often you get answers correct\n- **Speed**: How quickly you answer questions\n- **Retention**: How well you remember information\n\n## Using Statistics\n\nStatistics help you identify areas for improvement and track your learning progress over time.",
                    Format = "Markdown",
                    Images = new List<string> { "statistics-1.png" },
                    Videos = new List<string>(),
                    LastUpdated = DateTime.Now.AddDays(-4)
                }
            };
        }

        public HelpNavigation GetHelpNavigation()
        {
            return new HelpNavigation
            {
                Breadcrumbs = new List<string> { "Help", "Getting Started" },
                PreviousTopic = "Introduction",
                NextTopic = "Study Sessions",
                RelatedTopics = new List<string> { "Study Sessions", "Deck Management", "Settings" },
                CurrentTopic = "Getting Started"
            };
        }

        public List<HelpFeedback> GetHelpFeedback()
        {
            return new List<HelpFeedback>
            {
                new HelpFeedback
                {
                    Type = "Helpful",
                    Label = "Was this helpful?",
                    Description = "Rate this help topic",
                    Icon = "👍",
                    IsEnabled = true,
                    RequiresComment = false
                },
                new HelpFeedback
                {
                    Type = "Not Helpful",
                    Label = "Not helpful",
                    Description = "This didn't help me",
                    Icon = "👎",
                    IsEnabled = true,
                    RequiresComment = true
                },
                new HelpFeedback
                {
                    Type = "Suggest Improvement",
                    Label = "Suggest improvement",
                    Description = "Help us improve this topic",
                    Icon = "💡",
                    IsEnabled = true,
                    RequiresComment = true
                },
                new HelpFeedback
                {
                    Type = "Report Issue",
                    Label = "Report issue",
                    Description = "Report a problem with this topic",
                    Icon = "🐛",
                    IsEnabled = true,
                    RequiresComment = true
                }
            };
        }

        public HelpBookmarks GetHelpBookmarks()
        {
            return new HelpBookmarks
            {
                Bookmarks = new List<HelpBookmark>
                {
                    new HelpBookmark
                    {
                        TopicName = "Getting Started",
                        Section = "Introduction",
                        CreatedAt = DateTime.Now.AddDays(-1),
                        Notes = "Important for new users"
                    },
                    new HelpBookmark
                    {
                        TopicName = "Study Sessions",
                        Section = "Study Modes",
                        CreatedAt = DateTime.Now.AddDays(-2),
                        Notes = "Different ways to study"
                    }
                },
                IsEnabled = true,
                MaxBookmarks = 50
            };
        }

        public HelpHistory GetHelpHistory()
        {
            return new HelpHistory
            {
                RecentTopics = new List<HelpHistoryItem>
                {
                    new HelpHistoryItem
                    {
                        TopicName = "Getting Started",
                        AccessedAt = DateTime.Now.AddHours(-1),
                        AccessCount = 3,
                        TimeSpent = TimeSpan.FromMinutes(5)
                    },
                    new HelpHistoryItem
                    {
                        TopicName = "Study Sessions",
                        AccessedAt = DateTime.Now.AddHours(-2),
                        AccessCount = 2,
                        TimeSpent = TimeSpan.FromMinutes(8)
                    }
                },
                IsEnabled = true,
                MaxHistoryItems = 100
            };
        }

        public HelpAccessibility GetHelpAccessibility()
        {
            return new HelpAccessibility
            {
                ScreenReader = "ARIA labels and semantic markup for all help content",
                KeyboardNavigation = "Full keyboard navigation support for help system",
                HighContrast = "High contrast mode support for better readability",
                TextSize = "Adjustable text size for better accessibility"
            };
        }

        public HelpPrinting GetHelpPrinting()
        {
            return new HelpPrinting
            {
                IsEnabled = true,
                PrintFormats = new List<string> { "PDF", "HTML" },
                IncludeImages = true,
                IncludeNavigation = false
            };
        }

        public List<HelpSharing> GetHelpSharing()
        {
            return new List<HelpSharing>
            {
                new HelpSharing
                {
                    Type = "Copy Link",
                    Label = "Copy Link",
                    Description = "Copy a link to this help topic",
                    Icon = "🔗",
                    IsEnabled = true
                },
                new HelpSharing
                {
                    Type = "Share Topic",
                    Label = "Share Topic",
                    Description = "Share this help topic with others",
                    Icon = "📤",
                    IsEnabled = true
                },
                new HelpSharing
                {
                    Type = "Export PDF",
                    Label = "Export PDF",
                    Description = "Export this topic as PDF",
                    Icon = "📄",
                    IsEnabled = true
                },
                new HelpSharing
                {
                    Type = "Print",
                    Label = "Print",
                    Description = "Print this help topic",
                    Icon = "🖨️",
                    IsEnabled = true
                }
            };
        }

        public HelpContext GetHelpContext()
        {
            return new HelpContext
            {
                CurrentPage = "Study Session",
                SuggestedTopics = new List<string> { "Study Sessions", "Study Tips", "Learning Methods" },
                ContextualHelp = "You're currently in a study session. Here are some helpful topics related to studying.",
                PageContext = new Dictionary<string, string>
                {
                    ["Page"] = "Study Session",
                    ["Mode"] = "Front to Back",
                    ["Deck"] = "French Vocabulary"
                }
            };
        }

        public HelpValidation ValidateHelpRequest(HelpRequest request)
        {
            var validation = new HelpValidation { IsValid = true };

            // Validate topic
            if (string.IsNullOrWhiteSpace(request.Topic))
            {
                validation.IsValid = false;
                validation.Errors.Add("Topic is required");
            }

            // Validate search query
            if (string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                validation.IsValid = false;
                validation.Errors.Add("Search query is required");
            }

            // Validate category
            var validCategories = new[] { "Basics", "Study", "Management", "Advanced" };
            if (!string.IsNullOrEmpty(request.Category) && !validCategories.Contains(request.Category))
            {
                validation.IsValid = false;
                validation.Errors.Add($"Invalid category: {request.Category}. Valid categories are: {string.Join(", ", validCategories)}");
            }

            // Validate search query length
            if (!string.IsNullOrEmpty(request.SearchQuery) && request.SearchQuery.Length < 2)
            {
                validation.Warnings.Add("Search query is very short, consider using more specific terms");
            }

            return validation;
        }
    }
}
