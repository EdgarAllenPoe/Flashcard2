using FlashcardApp.UI.Abstractions;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// WinUI-specific implementation of IUITheme for theming and styling
    /// </summary>
    public class WinUITheme : IUITheme
    {
        public bool UseColors => true;
        public bool UseIcons => true;

        public UIColor GetColorForMessageType(MessageType messageType)
        {
            return messageType switch
            {
                MessageType.Success => UIColor.Green,
                MessageType.Error => UIColor.Red,
                MessageType.Warning => UIColor.Yellow,
                MessageType.Info => UIColor.Cyan,
                _ => UIColor.Default
            };
        }

        public string GetIconForMessageType(MessageType messageType)
        {
            return messageType switch
            {
                MessageType.Success => "✅",
                MessageType.Error => "❌",
                MessageType.Warning => "⚠️",
                MessageType.Info => "ℹ️",
                _ => ""
            };
        }

        public UIColor GetColorForSection(SectionType sectionType)
        {
            return sectionType switch
            {
                SectionType.MainMenu => UIColor.Cyan,
                SectionType.DeckManagement => UIColor.Blue,
                SectionType.StudySession => UIColor.Green,
                SectionType.Statistics => UIColor.Yellow,
                SectionType.Configuration => UIColor.Magenta,
                SectionType.Help => UIColor.DarkCyan,
                SectionType.Welcome => UIColor.Green,
                SectionType.SessionResults => UIColor.DarkGreen,
                SectionType.DeckList => UIColor.Blue,
                _ => UIColor.Default
            };
        }

        public string GetIconForSection(SectionType sectionType)
        {
            return sectionType switch
            {
                SectionType.MainMenu => "🏠",
                SectionType.DeckManagement => "📚",
                SectionType.StudySession => "📖",
                SectionType.Statistics => "📊",
                SectionType.Configuration => "⚙️",
                SectionType.Help => "❓",
                SectionType.Welcome => "👋",
                SectionType.SessionResults => "📈",
                SectionType.DeckList => "📋",
                _ => ""
            };
        }

        public UIColor GetMenuOptionColor() => UIColor.White;
        public UIColor GetMenuDescriptionColor() => UIColor.Gray;
        public UIColor GetInputPromptColor() => UIColor.Yellow;
        public UIColor GetStatisticsColor() => UIColor.Cyan;
    }
}
