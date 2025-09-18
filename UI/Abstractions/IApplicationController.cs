using System.Collections.Generic;
using System.Threading.Tasks;
using FlashcardApp.Models;

namespace FlashcardApp.UI.Abstractions
{
    /// <summary>
    /// Abstraction for application business logic, enabling UI-agnostic operations
    /// </summary>
    public interface IApplicationController
    {
        /// <summary>
        /// Starts a study session
        /// </summary>
        /// <param name="request">The study session request</param>
        /// <returns>The result of the study session</returns>
        Task<ApplicationResult> StartStudySessionAsync(StudySessionRequest request);

        /// <summary>
        /// Manages deck operations
        /// </summary>
        /// <param name="request">The deck management request</param>
        /// <returns>The result of the deck management operation</returns>
        Task<ApplicationResult> ManageDecksAsync(DeckManagementRequest request);

        /// <summary>
        /// Views statistics
        /// </summary>
        /// <param name="request">The statistics request</param>
        /// <returns>The result containing statistics data</returns>
        Task<ApplicationResult> ViewStatisticsAsync(StatisticsRequest request);

        /// <summary>
        /// Configures application settings
        /// </summary>
        /// <param name="request">The configuration request</param>
        /// <returns>The result of the configuration operation</returns>
        Task<ApplicationResult> ConfigureSettingsAsync(ConfigurationRequest request);

        /// <summary>
        /// Shows help information
        /// </summary>
        /// <param name="request">The help request</param>
        /// <returns>The result containing help data</returns>
        Task<ApplicationResult> ShowHelpAsync(HelpRequest request);

        /// <summary>
        /// Gets all available decks
        /// </summary>
        /// <returns>List of all decks</returns>
        Task<List<Deck>> GetAllDecksAsync();

        /// <summary>
        /// Gets the current application configuration
        /// </summary>
        /// <returns>The current configuration</returns>
        Task<AppConfiguration> GetConfigurationAsync();
    }

    /// <summary>
    /// Base class for application requests
    /// </summary>
    public abstract class ApplicationRequest
    {
        public string RequestId { get; set; } = System.Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Request for starting a study session
    /// </summary>
    public class StudySessionRequest : ApplicationRequest
    {
        public Deck? SelectedDeck { get; set; }
        public StudyMode StudyMode { get; set; }
        public int MaxCards { get; set; }
    }

    /// <summary>
    /// Request for deck management operations
    /// </summary>
    public class DeckManagementRequest : ApplicationRequest
    {
        public DeckManagementAction Action { get; set; }
        public Deck? SelectedDeck { get; set; }
        public string? DeckName { get; set; }
        public string? Description { get; set; }
        public List<string>? Tags { get; set; }
        public string? FilePath { get; set; }
    }

    /// <summary>
    /// Request for viewing statistics
    /// </summary>
    public class StatisticsRequest : ApplicationRequest
    {
        public string? DeckId { get; set; }
        public bool IncludeOverall { get; set; } = true;
    }

    /// <summary>
    /// Request for configuration operations
    /// </summary>
    public class ConfigurationRequest : ApplicationRequest
    {
        public ConfigurationAction Action { get; set; }
        public string? SettingName { get; set; }
        public object? Value { get; set; }
    }

    /// <summary>
    /// Request for help information
    /// </summary>
    public class HelpRequest : ApplicationRequest
    {
        public string? Topic { get; set; }
    }

    /// <summary>
    /// Result of an application operation
    /// </summary>
    public class ApplicationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// Types of deck management actions
    /// </summary>
    public enum DeckManagementAction
    {
        Create,
        Edit,
        Delete,
        View,
        Import,
        Export,
        List
    }

    /// <summary>
    /// Types of configuration actions
    /// </summary>
    public enum ConfigurationAction
    {
        View,
        Update,
        Reset,
        Get
    }
}
