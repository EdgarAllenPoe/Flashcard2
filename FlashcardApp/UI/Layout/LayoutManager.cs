using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.UI.Layout
{
    /// <summary>
    /// Represents a navigation item
    /// </summary>
    public class NavigationItem
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsEnabled { get; set; } = true;
        public List<NavigationItem> Children { get; set; } = new List<NavigationItem>();
    }

    /// <summary>
    /// Represents the navigation structure
    /// </summary>
    public class NavigationStructure
    {
        public List<NavigationItem> Items { get; set; } = new List<NavigationItem>();
        public string ActiveItem { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a header action
    /// </summary>
    public class HeaderAction
    {
        public string Type { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Tooltip { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public string Command { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the header layout
    /// </summary>
    public class HeaderLayout
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public List<HeaderAction> Actions { get; set; } = new List<HeaderAction>();
        public double Height { get; set; } = 60;
    }

    /// <summary>
    /// Represents a content area
    /// </summary>
    public class ContentArea
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Width { get; set; }
        public double Height { get; set; }
        public string Position { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = true;
    }

    /// <summary>
    /// Represents responsive layout configuration
    /// </summary>
    public class ResponsiveLayout
    {
        public string Name { get; set; } = string.Empty;
        public double MinWidth { get; set; }
        public double MaxWidth { get; set; }
        public Dictionary<string, ContentArea> ContentAreas { get; set; } = new Dictionary<string, ContentArea>();
    }

    /// <summary>
    /// Represents layout breakpoints
    /// </summary>
    public class LayoutBreakpoints
    {
        public double Mobile { get; set; } = 768;
        public double Tablet { get; set; } = 1024;
        public double Desktop { get; set; } = 1440;
        public double LargeDesktop { get; set; } = 1920;
    }

    /// <summary>
    /// Represents a navigation state
    /// </summary>
    public class NavigationState
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsExpanded { get; set; }
        public bool IsVisible { get; set; } = true;
    }

    /// <summary>
    /// Represents the layout grid
    /// </summary>
    public class LayoutGrid
    {
        public int Columns { get; set; } = 12;
        public int Rows { get; set; } = 8;
        public double Gap { get; set; } = 16;
        public string Template { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents accessibility features
    /// </summary>
    public class LayoutAccessibility
    {
        public string KeyboardNavigation { get; set; } = string.Empty;
        public string ScreenReader { get; set; } = string.Empty;
        public string FocusManagement { get; set; } = string.Empty;
        public string HighContrast { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents layout animations
    /// </summary>
    public class LayoutAnimation
    {
        public string Name { get; set; } = string.Empty;
        public double Duration { get; set; } = 300;
        public string Easing { get; set; } = string.Empty;
        public string Property { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents layout customization options
    /// </summary>
    public class LayoutCustomization
    {
        public List<string> CustomizableElements { get; set; } = new List<string>();
        public Dictionary<string, object> DefaultValues { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// Represents layout configuration
    /// </summary>
    public class LayoutConfiguration
    {
        public double NavigationWidth { get; set; } = 250;
        public double HeaderHeight { get; set; } = 60;
        public double ContentPadding { get; set; } = 16;
        public string SidebarPosition { get; set; } = "Left";
    }

    /// <summary>
    /// Represents layout validation result
    /// </summary>
    public class LayoutValidation
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents a layout preset
    /// </summary>
    public class LayoutPreset
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public LayoutConfiguration Configuration { get; set; } = new LayoutConfiguration();
    }

    /// <summary>
    /// Manages the modern layout system for the Flashcard App
    /// </summary>
    public class LayoutManager
    {
        public NavigationStructure GetNavigationStructure()
        {
            return new NavigationStructure
            {
                Items = new List<NavigationItem>
                {
                    new NavigationItem
                    {
                        Name = "Study Session",
                        Icon = "📚",
                        Route = "/study",
                        IsActive = false,
                        IsEnabled = true
                    },
                    new NavigationItem
                    {
                        Name = "Deck Management",
                        Icon = "🗂️",
                        Route = "/decks",
                        IsActive = false,
                        IsEnabled = true
                    },
                    new NavigationItem
                    {
                        Name = "Statistics",
                        Icon = "📊",
                        Route = "/statistics",
                        IsActive = false,
                        IsEnabled = true
                    },
                    new NavigationItem
                    {
                        Name = "Configuration",
                        Icon = "⚙️",
                        Route = "/settings",
                        IsActive = false,
                        IsEnabled = true
                    },
                    new NavigationItem
                    {
                        Name = "Help & Guide",
                        Icon = "❓",
                        Route = "/help",
                        IsActive = false,
                        IsEnabled = true
                    }
                },
                ActiveItem = "Study Session"
            };
        }

        public HeaderLayout GetHeaderLayout()
        {
            return new HeaderLayout
            {
                Title = "Flashcard App",
                Subtitle = "Master your learning with spaced repetition",
                Actions = new List<HeaderAction>
                {
                    new HeaderAction
                    {
                        Type = "Search",
                        Icon = "🔍",
                        Tooltip = "Search flashcards and decks",
                        IsEnabled = true,
                        Command = "SearchCommand"
                    },
                    new HeaderAction
                    {
                        Type = "ThemeToggle",
                        Icon = "🌙",
                        Tooltip = "Toggle dark/light theme",
                        IsEnabled = true,
                        Command = "ThemeToggleCommand"
                    },
                    new HeaderAction
                    {
                        Type = "Settings",
                        Icon = "⚙️",
                        Tooltip = "Open settings",
                        IsEnabled = true,
                        Command = "SettingsCommand"
                    }
                },
                Height = 60
            };
        }

        public Dictionary<string, ContentArea> GetContentAreas()
        {
            return new Dictionary<string, ContentArea>
            {
                ["MainContent"] = new ContentArea
                {
                    Name = "MainContent",
                    Type = "Primary",
                    Width = 800,
                    Height = 600,
                    Position = "Center",
                    IsVisible = true
                },
                ["Sidebar"] = new ContentArea
                {
                    Name = "Sidebar",
                    Type = "Secondary",
                    Width = 250,
                    Height = 600,
                    Position = "Left",
                    IsVisible = true
                },
                ["Footer"] = new ContentArea
                {
                    Name = "Footer",
                    Type = "Tertiary",
                    Width = 800,
                    Height = 40,
                    Position = "Bottom",
                    IsVisible = true
                }
            };
        }

        public Dictionary<string, ResponsiveLayout> GetResponsiveLayouts()
        {
            return new Dictionary<string, ResponsiveLayout>
            {
                ["Mobile"] = new ResponsiveLayout
                {
                    Name = "Mobile",
                    MinWidth = 0,
                    MaxWidth = 768,
                    ContentAreas = new Dictionary<string, ContentArea>
                    {
                        ["MainContent"] = new ContentArea { Name = "MainContent", Type = "Primary", Width = 100, Height = 100, Position = "Full", IsVisible = true },
                        ["Sidebar"] = new ContentArea { Name = "Sidebar", Type = "Secondary", Width = 0, Height = 0, Position = "Hidden", IsVisible = false }
                    }
                },
                ["Tablet"] = new ResponsiveLayout
                {
                    Name = "Tablet",
                    MinWidth = 769,
                    MaxWidth = 1024,
                    ContentAreas = new Dictionary<string, ContentArea>
                    {
                        ["MainContent"] = new ContentArea { Name = "MainContent", Type = "Primary", Width = 70, Height = 100, Position = "Right", IsVisible = true },
                        ["Sidebar"] = new ContentArea { Name = "Sidebar", Type = "Secondary", Width = 30, Height = 100, Position = "Left", IsVisible = true }
                    }
                },
                ["Desktop"] = new ResponsiveLayout
                {
                    Name = "Desktop",
                    MinWidth = 1025,
                    MaxWidth = 1440,
                    ContentAreas = new Dictionary<string, ContentArea>
                    {
                        ["MainContent"] = new ContentArea { Name = "MainContent", Type = "Primary", Width = 75, Height = 100, Position = "Right", IsVisible = true },
                        ["Sidebar"] = new ContentArea { Name = "Sidebar", Type = "Secondary", Width = 25, Height = 100, Position = "Left", IsVisible = true }
                    }
                },
                ["LargeDesktop"] = new ResponsiveLayout
                {
                    Name = "LargeDesktop",
                    MinWidth = 1441,
                    MaxWidth = double.MaxValue,
                    ContentAreas = new Dictionary<string, ContentArea>
                    {
                        ["MainContent"] = new ContentArea { Name = "MainContent", Type = "Primary", Width = 80, Height = 100, Position = "Right", IsVisible = true },
                        ["Sidebar"] = new ContentArea { Name = "Sidebar", Type = "Secondary", Width = 20, Height = 100, Position = "Left", IsVisible = true }
                    }
                }
            };
        }

        public LayoutBreakpoints GetLayoutBreakpoints()
        {
            return new LayoutBreakpoints
            {
                Mobile = 768,
                Tablet = 1024,
                Desktop = 1440,
                LargeDesktop = 1920
            };
        }

        public List<NavigationState> GetNavigationStates()
        {
            return new List<NavigationState>
            {
                new NavigationState
                {
                    Name = "Expanded",
                    Description = "Full navigation with icons and text",
                    IsExpanded = true,
                    IsVisible = true
                },
                new NavigationState
                {
                    Name = "Collapsed",
                    Description = "Compact navigation with icons only",
                    IsExpanded = false,
                    IsVisible = true
                },
                new NavigationState
                {
                    Name = "Hidden",
                    Description = "Navigation completely hidden",
                    IsExpanded = false,
                    IsVisible = false
                }
            };
        }

        public LayoutGrid GetLayoutGrid()
        {
            return new LayoutGrid
            {
                Columns = 12,
                Rows = 8,
                Gap = 16,
                Template = "repeat(12, 1fr) / repeat(8, 1fr)"
            };
        }

        public LayoutAccessibility GetAccessibilityFeatures()
        {
            return new LayoutAccessibility
            {
                KeyboardNavigation = "Full keyboard navigation support with tab order",
                ScreenReader = "ARIA labels and semantic markup for all layout elements",
                FocusManagement = "Visible focus indicators and logical focus flow",
                HighContrast = "High contrast mode support with enhanced visibility"
            };
        }

        public Dictionary<string, LayoutAnimation> GetLayoutAnimations()
        {
            return new Dictionary<string, LayoutAnimation>
            {
                ["NavigationToggle"] = new LayoutAnimation
                {
                    Name = "NavigationToggle",
                    Duration = 300,
                    Easing = "ease-in-out",
                    Property = "width"
                },
                ["ContentTransition"] = new LayoutAnimation
                {
                    Name = "ContentTransition",
                    Duration = 250,
                    Easing = "ease-out",
                    Property = "opacity"
                },
                ["HeaderResize"] = new LayoutAnimation
                {
                    Name = "HeaderResize",
                    Duration = 200,
                    Easing = "ease-in-out",
                    Property = "height"
                },
                ["SidebarSlide"] = new LayoutAnimation
                {
                    Name = "SidebarSlide",
                    Duration = 350,
                    Easing = "ease-in-out",
                    Property = "transform"
                }
            };
        }

        public LayoutCustomization GetLayoutCustomization()
        {
            return new LayoutCustomization
            {
                CustomizableElements = new List<string>
                {
                    "NavigationWidth",
                    "HeaderHeight",
                    "ContentPadding",
                    "SidebarPosition"
                },
                DefaultValues = new Dictionary<string, object>
                {
                    ["NavigationWidth"] = 250.0,
                    ["HeaderHeight"] = 60.0,
                    ["ContentPadding"] = 16.0,
                    ["SidebarPosition"] = "Left"
                }
            };
        }

        public LayoutValidation ValidateLayoutConstraints(LayoutConfiguration layout)
        {
            var validation = new LayoutValidation { IsValid = true };

            // Validate navigation width
            if (layout.NavigationWidth < 0)
            {
                validation.IsValid = false;
                validation.Errors.Add("Navigation width cannot be negative");
            }
            else if (layout.NavigationWidth > 500)
            {
                validation.Warnings.Add("Navigation width is very large, may affect usability");
            }

            // Validate header height
            if (layout.HeaderHeight <= 0)
            {
                validation.IsValid = false;
                validation.Errors.Add("Header height must be greater than 0");
            }
            else if (layout.HeaderHeight > 120)
            {
                validation.Warnings.Add("Header height is very large, may waste screen space");
            }

            // Validate content padding
            if (layout.ContentPadding < 0)
            {
                validation.IsValid = false;
                validation.Errors.Add("Content padding cannot be negative");
            }
            else if (layout.ContentPadding > 50)
            {
                validation.Warnings.Add("Content padding is very large, may affect content visibility");
            }

            // Validate sidebar position
            var validPositions = new[] { "Left", "Right", "Top", "Bottom" };
            if (!validPositions.Contains(layout.SidebarPosition))
            {
                validation.IsValid = false;
                validation.Errors.Add($"Invalid sidebar position: {layout.SidebarPosition}. Valid positions are: {string.Join(", ", validPositions)}");
            }

            return validation;
        }

        public string GenerateLayoutCSS(LayoutConfiguration layout)
        {
            return $@"
                :root {{
                    --navigation-width: {layout.NavigationWidth}px;
                    --header-height: {layout.HeaderHeight}px;
                    --content-padding: {layout.ContentPadding}px;
                    --sidebar-position: {layout.SidebarPosition.ToLower()};
                }}
                
                .layout-container {{
                    display: grid;
                    grid-template-columns: var(--navigation-width) 1fr;
                    grid-template-rows: var(--header-height) 1fr;
                    height: 100vh;
                }}
                
                .header {{
                    grid-column: 1 / -1;
                    height: var(--header-height);
                }}
                
                .sidebar {{
                    width: var(--navigation-width);
                    position: var(--sidebar-position);
                }}
                
                .main-content {{
                    padding: var(--content-padding);
                }}
            ";
        }

        public Dictionary<string, LayoutPreset> GetLayoutPresets()
        {
            return new Dictionary<string, LayoutPreset>
            {
                ["Default"] = new LayoutPreset
                {
                    Name = "Default",
                    Description = "Standard layout with balanced proportions",
                    Configuration = new LayoutConfiguration
                    {
                        NavigationWidth = 250,
                        HeaderHeight = 60,
                        ContentPadding = 16,
                        SidebarPosition = "Left"
                    }
                },
                ["Compact"] = new LayoutPreset
                {
                    Name = "Compact",
                    Description = "Compact layout for smaller screens",
                    Configuration = new LayoutConfiguration
                    {
                        NavigationWidth = 200,
                        HeaderHeight = 50,
                        ContentPadding = 12,
                        SidebarPosition = "Left"
                    }
                },
                ["Spacious"] = new LayoutPreset
                {
                    Name = "Spacious",
                    Description = "Spacious layout with generous padding",
                    Configuration = new LayoutConfiguration
                    {
                        NavigationWidth = 300,
                        HeaderHeight = 80,
                        ContentPadding = 24,
                        SidebarPosition = "Left"
                    }
                },
                ["Minimal"] = new LayoutPreset
                {
                    Name = "Minimal",
                    Description = "Minimal layout with hidden navigation",
                    Configuration = new LayoutConfiguration
                    {
                        NavigationWidth = 0,
                        HeaderHeight = 40,
                        ContentPadding = 8,
                        SidebarPosition = "Hidden"
                    }
                }
            };
        }
    }
}
