using FlashcardApp.UI.Abstractions;

namespace FlashcardApp.UI.Implementations.Console
{
    /// <summary>
    /// Console-specific implementation of IUITheme
    /// </summary>
    public class ConsoleTheme : IUITheme
    {
        public bool UseColors { get; set; } = true;
        public bool UseIcons { get; set; } = false;

        public UIColor GetColorForMessageType(MessageType messageType)
        {
            return messageType switch
            {
                MessageType.Success => UIColor.Green,
                MessageType.Error => UIColor.Red,
                MessageType.Info => UIColor.Cyan,
                MessageType.Warning => UIColor.Yellow,
                MessageType.Default => UIColor.White,
                _ => UIColor.White
            };
        }

        public string GetIconForMessageType(MessageType messageType)
        {
            if (!UseIcons) return string.Empty;

            return messageType switch
            {
                MessageType.Success => "✓",
                MessageType.Error => "✗",
                MessageType.Info => "ℹ",
                MessageType.Warning => "⚠",
                MessageType.Default => "",
                _ => ""
            };
        }

        public UIColor GetColorForSection(SectionType sectionType)
        {
            return sectionType switch
            {
                SectionType.MainMenu => UIColor.Magenta,
                SectionType.DeckManagement => UIColor.Blue,
                SectionType.StudySession => UIColor.Green,
                SectionType.Statistics => UIColor.Cyan,
                SectionType.Configuration => UIColor.Yellow,
                SectionType.Help => UIColor.Cyan,
                SectionType.Welcome => UIColor.Cyan,
                SectionType.SessionResults => UIColor.Green,
                SectionType.DeckList => UIColor.Cyan,
                SectionType.Default => UIColor.White,
                _ => UIColor.White
            };
        }

        public string GetIconForSection(SectionType sectionType)
        {
            if (!UseIcons) return string.Empty;

            return sectionType switch
            {
                SectionType.MainMenu => "📋",
                SectionType.DeckManagement => "🗂",
                SectionType.StudySession => "📚",
                SectionType.Statistics => "📊",
                SectionType.Configuration => "⚙",
                SectionType.Help => "❓",
                SectionType.Welcome => "👋",
                SectionType.SessionResults => "🎯",
                SectionType.DeckList => "📑",
                SectionType.Default => "",
                _ => ""
            };
        }

        public UIColor GetMenuOptionColor()
        {
            return UIColor.Yellow;
        }

        public UIColor GetMenuDescriptionColor()
        {
            return UIColor.DarkGray;
        }

        public UIColor GetInputPromptColor()
        {
            return UIColor.Yellow;
        }

        public UIColor GetStatisticsColor()
        {
            return UIColor.Cyan;
        }
    }
}
