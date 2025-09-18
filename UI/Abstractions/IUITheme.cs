using System.Collections.Generic;

namespace FlashcardApp.UI.Abstractions
{
    /// <summary>
    /// Abstraction for UI theming and styling, enabling platform-specific implementations
    /// </summary>
    public interface IUITheme
    {
        /// <summary>
        /// Whether colors should be used in the UI
        /// </summary>
        bool UseColors { get; }

        /// <summary>
        /// Whether icons should be used in the UI
        /// </summary>
        bool UseIcons { get; }

        /// <summary>
        /// Gets the color for a specific message type
        /// </summary>
        /// <param name="messageType">The type of message</param>
        /// <returns>The appropriate color</returns>
        UIColor GetColorForMessageType(MessageType messageType);

        /// <summary>
        /// Gets the icon for a specific message type
        /// </summary>
        /// <param name="messageType">The type of message</param>
        /// <returns>The appropriate icon</returns>
        string GetIconForMessageType(MessageType messageType);

        /// <summary>
        /// Gets the color for a specific section header
        /// </summary>
        /// <param name="sectionType">The type of section</param>
        /// <returns>The appropriate color</returns>
        UIColor GetColorForSection(SectionType sectionType);

        /// <summary>
        /// Gets the icon for a specific section header
        /// </summary>
        /// <param name="sectionType">The type of section</param>
        /// <returns>The appropriate icon</returns>
        string GetIconForSection(SectionType sectionType);

        /// <summary>
        /// Gets the color for menu options
        /// </summary>
        /// <returns>The menu option color</returns>
        UIColor GetMenuOptionColor();

        /// <summary>
        /// Gets the color for menu descriptions
        /// </summary>
        /// <returns>The menu description color</returns>
        UIColor GetMenuDescriptionColor();

        /// <summary>
        /// Gets the color for input prompts
        /// </summary>
        /// <returns>The input prompt color</returns>
        UIColor GetInputPromptColor();

        /// <summary>
        /// Gets the color for statistics display
        /// </summary>
        /// <returns>The statistics color</returns>
        UIColor GetStatisticsColor();
    }

    /// <summary>
    /// Types of messages that can be displayed
    /// </summary>
    public enum MessageType
    {
        Success,
        Error,
        Info,
        Warning,
        Default
    }

    /// <summary>
    /// Types of sections that can be displayed
    /// </summary>
    public enum SectionType
    {
        MainMenu,
        DeckManagement,
        StudySession,
        Statistics,
        Configuration,
        Help,
        Welcome,
        SessionResults,
        DeckList,
        Default
    }
}
