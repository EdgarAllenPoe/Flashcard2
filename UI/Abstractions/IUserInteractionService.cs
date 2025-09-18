using System.Collections.Generic;
using System.Threading.Tasks;
using FlashcardApp.Models;

namespace FlashcardApp.UI.Abstractions
{
    /// <summary>
    /// Abstraction for user interaction operations, enabling platform-specific implementations
    /// </summary>
    public interface IUserInteractionService
    {
        /// <summary>
        /// Prompts the user to select a deck from a list
        /// </summary>
        /// <param name="decks">The list of decks to choose from</param>
        /// <param name="prompt">The prompt message</param>
        /// <returns>The selected deck, or null if cancelled</returns>
        Task<Deck?> SelectDeckAsync(List<Deck> decks, string prompt);

        /// <summary>
        /// Prompts the user to select a study mode
        /// </summary>
        /// <returns>The selected study mode</returns>
        Task<StudyMode> SelectStudyModeAsync();

        /// <summary>
        /// Prompts the user for the maximum number of cards for a session
        /// </summary>
        /// <returns>The maximum number of cards</returns>
        Task<int> GetMaxCardsForSessionAsync();

        /// <summary>
        /// Prompts the user to confirm an action
        /// </summary>
        /// <param name="message">The confirmation message</param>
        /// <returns>True if confirmed, false otherwise</returns>
        Task<bool> ConfirmActionAsync(string message);

        /// <summary>
        /// Prompts the user for input with a specific prompt
        /// </summary>
        /// <param name="prompt">The prompt message</param>
        /// <returns>The user's input</returns>
        Task<string> GetInputAsync(string prompt);

        /// <summary>
        /// Prompts the user for input with validation
        /// </summary>
        /// <param name="prompt">The prompt message</param>
        /// <param name="validator">The validation function</param>
        /// <returns>The validated user input</returns>
        Task<string> GetValidatedInputAsync(string prompt, Func<string, bool> validator);

        /// <summary>
        /// Prompts the user to select from a list of options
        /// </summary>
        /// <param name="options">The list of options</param>
        /// <param name="prompt">The prompt message</param>
        /// <returns>The selected option, or null if cancelled</returns>
        Task<string?> SelectFromOptionsAsync(List<string> options, string prompt);

        /// <summary>
        /// Prompts the user for a file path
        /// </summary>
        /// <param name="prompt">The prompt message</param>
        /// <param name="allowBrowse">Whether to allow browsing for files</param>
        /// <returns>The file path, or null if cancelled</returns>
        Task<string?> GetFilePathAsync(string prompt, bool allowBrowse = true);

        /// <summary>
        /// Waits for the user to press any key
        /// </summary>
        Task WaitForAnyKeyAsync();

        /// <summary>
        /// Shows a message and waits for user acknowledgment
        /// </summary>
        /// <param name="message">The message to show</param>
        /// <param name="messageType">The type of message</param>
        Task ShowMessageAsync(string message, MessageType messageType = MessageType.Info);

        /// <summary>
        /// Displays a welcome message
        /// </summary>
        Task DisplayWelcomeMessage();

        /// <summary>
        /// Displays an exit message
        /// </summary>
        Task DisplayExitMessage();

        /// <summary>
        /// Displays a section header
        /// </summary>
        /// <param name="title">The section title</param>
        /// <param name="sectionType">The type of section</param>
        Task DisplaySectionHeaderAsync(string title, SectionType sectionType);

        /// <summary>
        /// Displays a menu and gets user choice
        /// </summary>
        /// <param name="menu">The menu definition</param>
        /// <returns>The user's choice</returns>
        Task<string> GetMenuChoiceAsync(MenuDefinition menu);

        /// <summary>
        /// Displays a list of decks
        /// </summary>
        /// <param name="decks">The decks to display</param>
        Task DisplayDeckListAsync(IEnumerable<Deck> decks);

        /// <summary>
        /// Displays session results
        /// </summary>
        /// <param name="results">The session results</param>
        Task DisplaySessionResultsAsync(SessionResult results);

        /// <summary>
        /// Displays statistics
        /// </summary>
        /// <param name="statistics">The statistics to display</param>
        Task DisplayStatisticsAsync(IEnumerable<StatisticsData> statistics);

        /// <summary>
        /// Gets immediate choice from user (single key press)
        /// </summary>
        /// <returns>The user's choice</returns>
        Task<string> GetImmediateChoiceAsync();

        /// <summary>
        /// Clears the screen
        /// </summary>
        Task ClearScreenAsync();

        /// <summary>
        /// Sets the console title
        /// </summary>
        /// <param name="title">The title to set</param>
        Task SetTitleAsync(string title);

        /// <summary>
        /// Sets cursor visibility
        /// </summary>
        /// <param name="visible">Whether cursor should be visible</param>
        Task SetCursorVisibleAsync(bool visible);

        /// <summary>
        /// Confirms an action with the user
        /// </summary>
        /// <param name="message">The confirmation message</param>
        /// <returns>True if confirmed, false otherwise</returns>
        Task<bool> ConfirmAsync(string message);
    }
}
