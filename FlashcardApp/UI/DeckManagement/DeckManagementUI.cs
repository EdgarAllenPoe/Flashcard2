using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.UI.DeckManagement
{
    /// <summary>
    /// Represents deck card layout
    /// </summary>
    public class DeckCardLayout
    {
        public double Width { get; set; } = 300;
        public double Height { get; set; } = 200;
        public double BorderRadius { get; set; } = 12;
        public string Shadow { get; set; } = "0 4px 8px rgba(0,0,0,0.1)";
        public string Background { get; set; } = "#FFFFFF";
        public string Border { get; set; } = "1px solid #E1DFDD";
    }

    /// <summary>
    /// Represents deck list layout
    /// </summary>
    public class DeckListLayout
    {
        public double ItemHeight { get; set; } = 80;
        public double Spacing { get; set; } = 8;
        public string Padding { get; set; } = "16px";
        public string Background { get; set; } = "#FFFFFF";
        public string Border { get; set; } = "1px solid #E1DFDD";
    }

    /// <summary>
    /// Represents deck actions
    /// </summary>
    public class DeckAction
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public string Command { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents deck filters
    /// </summary>
    public class DeckFilter
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public int Count { get; set; } = 0;
    }

    /// <summary>
    /// Represents deck sort options
    /// </summary>
    public class DeckSortOption
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsAscending { get; set; } = true;
        public bool IsActive { get; set; } = false;
    }

    /// <summary>
    /// Represents deck search
    /// </summary>
    public class DeckSearch
    {
        public string Placeholder { get; set; } = "Search decks...";
        public List<string> SearchFields { get; set; } = new List<string>();
        public bool IsEnabled { get; set; } = true;
        public string Icon { get; set; } = "🔍";
    }

    /// <summary>
    /// Represents deck statistics
    /// </summary>
    public class DeckStatistics
    {
        public int TotalCards { get; set; } = 0;
        public int StudiedCards { get; set; } = 0;
        public double MasteryLevel { get; set; } = 0.0;
        public DateTime LastStudied { get; set; } = DateTime.MinValue;
        public int StudyStreak { get; set; } = 0;
        public double AverageScore { get; set; } = 0.0;
    }

    /// <summary>
    /// Represents deck tags
    /// </summary>
    public class DeckTag
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int UsageCount { get; set; } = 0;
    }

    /// <summary>
    /// Represents deck view modes
    /// </summary>
    public class DeckViewMode
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents deck animations
    /// </summary>
    public class DeckAnimation
    {
        public string Name { get; set; } = string.Empty;
        public double Duration { get; set; } = 300;
        public string Easing { get; set; } = string.Empty;
        public string Property { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents context menu item
    /// </summary>
    public class ContextMenuItem
    {
        public string Action { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public string Shortcut { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents deck context menu
    /// </summary>
    public class DeckContextMenu
    {
        public List<ContextMenuItem> Items { get; set; } = new List<ContextMenuItem>();
        public bool IsEnabled { get; set; } = true;
        public string Trigger { get; set; } = "right-click";
    }

    /// <summary>
    /// Represents deck drag and drop
    /// </summary>
    public class DeckDragDrop
    {
        public bool IsEnabled { get; set; } = true;
        public string DragPreview { get; set; } = string.Empty;
        public List<string> DropZones { get; set; } = new List<string>();
        public string Feedback { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents deck bulk actions
    /// </summary>
    public class DeckBulkAction
    {
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public string Command { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents deck accessibility
    /// </summary>
    public class DeckAccessibility
    {
        public string ScreenReader { get; set; } = string.Empty;
        public string KeyboardNavigation { get; set; } = string.Empty;
        public string HighContrast { get; set; } = string.Empty;
        public string FocusManagement { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents deck information
    /// </summary>
    public class DeckInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public int CardCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Represents deck validation result
    /// </summary>
    public class DeckValidation
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Modern deck management interface with cards, lists, and actions
    /// </summary>
    public class DeckManagementUI
    {
        public DeckCardLayout GetDeckCardLayout()
        {
            return new DeckCardLayout
            {
                Width = 300,
                Height = 200,
                BorderRadius = 12,
                Shadow = "0 4px 8px rgba(0,0,0,0.1)",
                Background = "#FFFFFF",
                Border = "1px solid #E1DFDD"
            };
        }

        public DeckListLayout GetDeckListLayout()
        {
            return new DeckListLayout
            {
                ItemHeight = 80,
                Spacing = 8,
                Padding = "16px",
                Background = "#FFFFFF",
                Border = "1px solid #E1DFDD"
            };
        }

        public List<DeckAction> GetDeckActions()
        {
            return new List<DeckAction>
            {
                new DeckAction
                {
                    Type = "Create",
                    Label = "Create Deck",
                    Icon = "➕",
                    Color = "#0078D4",
                    IsEnabled = true,
                    Command = "CreateDeckCommand"
                },
                new DeckAction
                {
                    Type = "Edit",
                    Label = "Edit Deck",
                    Icon = "✏️",
                    Color = "#106EBE",
                    IsEnabled = true,
                    Command = "EditDeckCommand"
                },
                new DeckAction
                {
                    Type = "Delete",
                    Label = "Delete Deck",
                    Icon = "🗑️",
                    Color = "#D13438",
                    IsEnabled = true,
                    Command = "DeleteDeckCommand"
                },
                new DeckAction
                {
                    Type = "Study",
                    Label = "Study Deck",
                    Icon = "📚",
                    Color = "#107C10",
                    IsEnabled = true,
                    Command = "StudyDeckCommand"
                },
                new DeckAction
                {
                    Type = "Import",
                    Label = "Import Deck",
                    Icon = "📥",
                    Color = "#FF8C00",
                    IsEnabled = true,
                    Command = "ImportDeckCommand"
                },
                new DeckAction
                {
                    Type = "Export",
                    Label = "Export Deck",
                    Icon = "📤",
                    Color = "#605E5C",
                    IsEnabled = true,
                    Command = "ExportDeckCommand"
                }
            };
        }

        public List<DeckFilter> GetDeckFilters()
        {
            return new List<DeckFilter>
            {
                new DeckFilter
                {
                    Type = "All",
                    Label = "All Decks",
                    Icon = "📚",
                    IsActive = true,
                    Count = 25
                },
                new DeckFilter
                {
                    Type = "Recent",
                    Label = "Recently Used",
                    Icon = "🕒",
                    IsActive = false,
                    Count = 8
                },
                new DeckFilter
                {
                    Type = "Favorites",
                    Label = "Favorites",
                    Icon = "⭐",
                    IsActive = false,
                    Count = 5
                },
                new DeckFilter
                {
                    Type = "Tags",
                    Label = "By Tags",
                    Icon = "🏷️",
                    IsActive = false,
                    Count = 12
                }
            };
        }

        public List<DeckSortOption> GetDeckSortOptions()
        {
            return new List<DeckSortOption>
            {
                new DeckSortOption
                {
                    Type = "Name",
                    Label = "Name",
                    Icon = "🔤",
                    IsAscending = true,
                    IsActive = true
                },
                new DeckSortOption
                {
                    Type = "Created",
                    Label = "Created Date",
                    Icon = "📅",
                    IsAscending = false,
                    IsActive = false
                },
                new DeckSortOption
                {
                    Type = "Modified",
                    Label = "Modified Date",
                    Icon = "🔄",
                    IsAscending = false,
                    IsActive = false
                },
                new DeckSortOption
                {
                    Type = "Cards",
                    Label = "Card Count",
                    Icon = "🔢",
                    IsAscending = false,
                    IsActive = false
                },
                new DeckSortOption
                {
                    Type = "Progress",
                    Label = "Study Progress",
                    Icon = "📊",
                    IsAscending = false,
                    IsActive = false
                }
            };
        }

        public DeckSearch GetDeckSearch()
        {
            return new DeckSearch
            {
                Placeholder = "Search decks...",
                SearchFields = new List<string> { "Name", "Description", "Tags" },
                IsEnabled = true,
                Icon = "🔍"
            };
        }

        public DeckStatistics GetDeckStatistics()
        {
            return new DeckStatistics
            {
                TotalCards = 150,
                StudiedCards = 120,
                MasteryLevel = 80.0,
                LastStudied = DateTime.Now.AddHours(-2),
                StudyStreak = 7,
                AverageScore = 85.5
            };
        }

        public List<DeckTag> GetDeckTags()
        {
            return new List<DeckTag>
            {
                new DeckTag
                {
                    Name = "Language",
                    Color = "#0078D4",
                    Icon = "🌍",
                    UsageCount = 8
                },
                new DeckTag
                {
                    Name = "Science",
                    Color = "#107C10",
                    Icon = "🔬",
                    UsageCount = 6
                },
                new DeckTag
                {
                    Name = "History",
                    Color = "#D13438",
                    Icon = "📜",
                    UsageCount = 4
                },
                new DeckTag
                {
                    Name = "Math",
                    Color = "#FF8C00",
                    Icon = "🔢",
                    UsageCount = 5
                }
            };
        }

        public List<DeckViewMode> GetDeckViewModes()
        {
            return new List<DeckViewMode>
            {
                new DeckViewMode
                {
                    Type = "Grid",
                    Label = "Grid View",
                    Icon = "⊞",
                    IsActive = true,
                    Description = "Display decks as cards in a grid"
                },
                new DeckViewMode
                {
                    Type = "List",
                    Label = "List View",
                    Icon = "☰",
                    IsActive = false,
                    Description = "Display decks as items in a list"
                },
                new DeckViewMode
                {
                    Type = "Compact",
                    Label = "Compact View",
                    Icon = "☷",
                    IsActive = false,
                    Description = "Display decks in a compact format"
                }
            };
        }

        public Dictionary<string, DeckAnimation> GetDeckAnimations()
        {
            return new Dictionary<string, DeckAnimation>
            {
                ["CardHover"] = new DeckAnimation
                {
                    Name = "CardHover",
                    Duration = 200,
                    Easing = "ease-out",
                    Property = "transform"
                },
                ["CardClick"] = new DeckAnimation
                {
                    Name = "CardClick",
                    Duration = 150,
                    Easing = "ease-in-out",
                    Property = "transform"
                },
                ["ListScroll"] = new DeckAnimation
                {
                    Name = "ListScroll",
                    Duration = 300,
                    Easing = "ease-out",
                    Property = "transform"
                },
                ["FilterChange"] = new DeckAnimation
                {
                    Name = "FilterChange",
                    Duration = 250,
                    Easing = "ease-in-out",
                    Property = "opacity"
                }
            };
        }

        public DeckContextMenu GetDeckContextMenu()
        {
            return new DeckContextMenu
            {
                Items = new List<ContextMenuItem>
                {
                    new ContextMenuItem
                    {
                        Action = "Study",
                        Label = "Study Deck",
                        Icon = "📚",
                        IsEnabled = true,
                        Shortcut = "Enter"
                    },
                    new ContextMenuItem
                    {
                        Action = "Edit",
                        Label = "Edit Deck",
                        Icon = "✏️",
                        IsEnabled = true,
                        Shortcut = "F2"
                    },
                    new ContextMenuItem
                    {
                        Action = "Duplicate",
                        Label = "Duplicate Deck",
                        Icon = "📋",
                        IsEnabled = true,
                        Shortcut = "Ctrl+D"
                    },
                    new ContextMenuItem
                    {
                        Action = "Delete",
                        Label = "Delete Deck",
                        Icon = "🗑️",
                        IsEnabled = true,
                        Shortcut = "Delete"
                    }
                },
                IsEnabled = true,
                Trigger = "right-click"
            };
        }

        public DeckDragDrop GetDeckDragDrop()
        {
            return new DeckDragDrop
            {
                IsEnabled = true,
                DragPreview = "Deck card with shadow",
                DropZones = new List<string> { "Trash", "Folder", "Export" },
                Feedback = "Drop to move or organize"
            };
        }

        public List<DeckBulkAction> GetDeckBulkActions()
        {
            return new List<DeckBulkAction>
            {
                new DeckBulkAction
                {
                    Type = "SelectAll",
                    Label = "Select All",
                    Icon = "☑️",
                    IsEnabled = true,
                    Command = "SelectAllCommand"
                },
                new DeckBulkAction
                {
                    Type = "DeleteSelected",
                    Label = "Delete Selected",
                    Icon = "🗑️",
                    IsEnabled = true,
                    Command = "DeleteSelectedCommand"
                },
                new DeckBulkAction
                {
                    Type = "ExportSelected",
                    Label = "Export Selected",
                    Icon = "📤",
                    IsEnabled = true,
                    Command = "ExportSelectedCommand"
                },
                new DeckBulkAction
                {
                    Type = "TagSelected",
                    Label = "Tag Selected",
                    Icon = "🏷️",
                    IsEnabled = true,
                    Command = "TagSelectedCommand"
                }
            };
        }

        public DeckAccessibility GetDeckAccessibility()
        {
            return new DeckAccessibility
            {
                ScreenReader = "ARIA labels and semantic markup for all deck management elements",
                KeyboardNavigation = "Full keyboard navigation support with tab order",
                HighContrast = "High contrast mode support for better visibility",
                FocusManagement = "Clear focus indicators and logical focus flow"
            };
        }

        public DeckValidation ValidateDeck(DeckInfo deck)
        {
            var validation = new DeckValidation { IsValid = true };

            // Validate deck name
            if (string.IsNullOrWhiteSpace(deck.Name))
            {
                validation.IsValid = false;
                validation.Errors.Add("Deck name is required");
            }
            else if (deck.Name.Length > 100)
            {
                validation.Warnings.Add("Deck name is very long, may affect display");
            }

            // Validate description
            if (!string.IsNullOrEmpty(deck.Description) && deck.Description.Length > 500)
            {
                validation.Warnings.Add("Deck description is very long, may affect display");
            }

            // Validate card count
            if (deck.CardCount < 0)
            {
                validation.IsValid = false;
                validation.Errors.Add("Card count cannot be negative");
            }
            else if (deck.CardCount > 1000)
            {
                validation.Warnings.Add("Deck has many cards, may affect performance");
            }

            // Validate tags
            if (deck.Tags.Count > 10)
            {
                validation.Warnings.Add("Too many tags, consider reducing for better organization");
            }

            return validation;
        }
    }
}
