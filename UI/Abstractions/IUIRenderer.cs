using System.Collections.Generic;
using System.Threading.Tasks;
using FlashcardApp.Models;

namespace FlashcardApp.UI.Abstractions
{
    /// <summary>
    /// Abstraction for UI rendering operations, enabling platform-specific implementations
    /// </summary>
    public interface IUIRenderer
    {
        /// <summary>
        /// Renders a menu with options
        /// </summary>
        /// <param name="menu">The menu definition to render</param>
        Task RenderMenuAsync(MenuDefinition menu);

        /// <summary>
        /// Renders a message to the user
        /// </summary>
        /// <param name="message">The message definition to render</param>
        Task RenderMessageAsync(MessageDefinition message);

        /// <summary>
        /// Renders statistics data
        /// </summary>
        /// <param name="stats">The statistics to render</param>
        Task RenderStatisticsAsync(StatisticsData stats);

        /// <summary>
        /// Renders a list of decks
        /// </summary>
        /// <param name="decks">The decks to render</param>
        Task RenderDeckListAsync(List<Deck> decks);

        /// <summary>
        /// Renders session results
        /// </summary>
        /// <param name="result">The session result to render</param>
        Task RenderSessionResultsAsync(SessionResult result);

        /// <summary>
        /// Renders a section header
        /// </summary>
        /// <param name="title">The section title</param>
        /// <param name="sectionType">The type of section</param>
        Task RenderSectionHeaderAsync(string title, SectionType sectionType);

        /// <summary>
        /// Renders a deck card with information
        /// </summary>
        /// <param name="deck">The deck to render</param>
        /// <param name="number">The deck number</param>
        Task RenderDeckCardAsync(Deck deck, int number);

        /// <summary>
        /// Renders a press any key prompt
        /// </summary>
        Task RenderPressAnyKeyAsync();

        /// <summary>
        /// Renders an input prompt
        /// </summary>
        /// <param name="prompt">The prompt text</param>
        Task RenderInputPromptAsync(string prompt);

        /// <summary>
        /// Renders a welcome message
        /// </summary>
        Task RenderWelcomeMessageAsync();

        /// <summary>
        /// Renders an exit message
        /// </summary>
        Task RenderExitMessageAsync();
    }

    /// <summary>
    /// Definition of a menu to be rendered
    /// </summary>
    public class MenuDefinition
    {
        public string Title { get; set; } = string.Empty;
        public List<MenuOption> Options { get; set; } = new List<MenuOption>();
        public SectionType SectionType { get; set; } = SectionType.Default;
    }

    /// <summary>
    /// Definition of a menu option
    /// </summary>
    public class MenuOption
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Definition of a message to be rendered
    /// </summary>
    public class MessageDefinition
    {
        public string Text { get; set; } = string.Empty;
        public MessageType Type { get; set; } = MessageType.Default;
    }

    /// <summary>
    /// Statistics data to be rendered
    /// </summary>
    public class StatisticsData
    {
        public Dictionary<string, object> Statistics { get; set; } = new Dictionary<string, object>();
        public List<Deck> Decks { get; set; } = new List<Deck>();
    }

    /// <summary>
    /// Session result data to be rendered
    /// </summary>
    public class SessionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public SessionStatistics? SessionStatistics { get; set; }
    }
}
