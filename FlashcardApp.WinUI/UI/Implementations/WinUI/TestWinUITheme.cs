using FlashcardApp.UI.Abstractions;
using System;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// Test-friendly WinUI implementation of IUITheme for unit testing
    /// This version can be instantiated without WinUI dependencies
    /// </summary>
    public class TestWinUITheme : IUITheme
    {
        public bool UseColors { get; set; } = true;
        public bool UseIcons { get; set; } = true;

        public UIColor GetColorForMessageType(MessageType type)
        {
            if (!UseColors) return UIColor.Default;

            return type switch
            {
                MessageType.Info => UIColor.Blue,
                MessageType.Success => UIColor.Green,
                MessageType.Warning => UIColor.Yellow,
                MessageType.Error => UIColor.Red,
                _ => UIColor.White, // Default
            };
        }

        public string GetIconForMessageType(MessageType type)
        {
            if (!UseIcons) return "";

            return type switch
            {
                MessageType.Info => "ℹ️",
                MessageType.Success => "✅",
                MessageType.Warning => "⚠️",
                MessageType.Error => "❌",
                _ => "",
            };
        }

        public UIColor GetColorForSection(SectionType type)
        {
            if (!UseColors) return UIColor.Default;

            return type switch
            {
                SectionType.Welcome => UIColor.Cyan,
                SectionType.MainMenu => UIColor.Cyan,
                SectionType.DeckList => UIColor.Cyan,
                SectionType.DeckManagement => UIColor.Yellow,
                SectionType.StudySession => UIColor.Green,
                SectionType.SessionResults => UIColor.Green,
                SectionType.Statistics => UIColor.Magenta,
                SectionType.Configuration => UIColor.Blue,
                SectionType.Help => UIColor.White,
                _ => UIColor.White, // Default
            };
        }

        public string GetIconForSection(SectionType type)
        {
            if (!UseIcons) return "";

            return type switch
            {
                SectionType.Welcome => "👋",
                SectionType.MainMenu => "📋",
                SectionType.DeckList => "📑",
                SectionType.DeckManagement => "🗂️",
                SectionType.StudySession => "📚",
                SectionType.SessionResults => "🎯",
                SectionType.Statistics => "📊",
                SectionType.Configuration => "⚙️",
                SectionType.Help => "❓",
                _ => "",
            };
        }

        public UIColor GetMenuOptionColor() => UseColors ? UIColor.Yellow : UIColor.Default;
        public UIColor GetMenuDescriptionColor() => UseColors ? UIColor.DarkGray : UIColor.Default;
        public UIColor GetInputPromptColor() => UseColors ? UIColor.Yellow : UIColor.Default;
        public UIColor GetStatisticsColor() => UseColors ? UIColor.Cyan : UIColor.Default;

        /// <summary>
        /// Test version - returns null since we don't need actual WinUI brushes in tests
        /// </summary>
        public object GetBrushForColor(UIColor color)
        {
            // For testing, we don't need actual WinUI brushes
            return null;
        }
    }
}
