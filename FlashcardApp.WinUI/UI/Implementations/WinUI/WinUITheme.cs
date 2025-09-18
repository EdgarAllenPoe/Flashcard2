using Microsoft.UI.Xaml.Media;
using FlashcardApp.UI.Abstractions;
using System;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// WinUI 3 implementation of IUITheme for Windows Native applications
    /// </summary>
    public class WinUITheme : IUITheme
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
        /// Gets a WinUI Brush for the specified UIColor
        /// </summary>
        public Brush GetBrushForColor(UIColor color)
        {
            return color switch
            {
                UIColor.Black => new SolidColorBrush(Microsoft.UI.Colors.Black),
                UIColor.DarkBlue => new SolidColorBrush(Microsoft.UI.Colors.DarkBlue),
                UIColor.DarkGreen => new SolidColorBrush(Microsoft.UI.Colors.DarkGreen),
                UIColor.DarkCyan => new SolidColorBrush(Microsoft.UI.Colors.DarkCyan),
                UIColor.DarkRed => new SolidColorBrush(Microsoft.UI.Colors.DarkRed),
                UIColor.DarkMagenta => new SolidColorBrush(Microsoft.UI.Colors.DarkMagenta),
                UIColor.DarkYellow => new SolidColorBrush(Microsoft.UI.Colors.Orange),
                UIColor.Gray => new SolidColorBrush(Microsoft.UI.Colors.Gray),
                UIColor.DarkGray => new SolidColorBrush(Microsoft.UI.Colors.DarkGray),
                UIColor.Blue => new SolidColorBrush(Microsoft.UI.Colors.Blue),
                UIColor.Green => new SolidColorBrush(Microsoft.UI.Colors.Green),
                UIColor.Cyan => new SolidColorBrush(Microsoft.UI.Colors.Cyan),
                UIColor.Red => new SolidColorBrush(Microsoft.UI.Colors.Red),
                UIColor.Magenta => new SolidColorBrush(Microsoft.UI.Colors.Magenta),
                UIColor.Yellow => new SolidColorBrush(Microsoft.UI.Colors.Yellow),
                UIColor.White => new SolidColorBrush(Microsoft.UI.Colors.White),
                _ => new SolidColorBrush(Microsoft.UI.Colors.White), // Default color
            };
        }
    }
}
