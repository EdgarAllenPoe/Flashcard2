using Newtonsoft.Json;

namespace FlashcardApp.Models
{
    public class SessionState
    {
        [JsonProperty("deckId")]
        public string DeckId { get; set; } = string.Empty;

        [JsonProperty("studyMode")]
        public StudyMode StudyMode { get; set; } = StudyMode.FrontToBack;

        [JsonProperty("cardsToStudy")]
        public List<string> CardsToStudy { get; set; } = new List<string>();

        [JsonProperty("currentCardIndex")]
        public int CurrentCardIndex { get; set; } = 0;

        [JsonProperty("studiedCards")]
        public List<string> StudiedCards { get; set; } = new List<string>();

        [JsonProperty("incorrectCards")]
        public List<string> IncorrectCards { get; set; } = new List<string>();

        [JsonProperty("sessionStartTime")]
        public DateTime SessionStartTime { get; set; } = DateTime.Now;

        [JsonProperty("lastSaveTime")]
        public DateTime LastSaveTime { get; set; } = DateTime.Now;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; } = true;

        [JsonProperty("sessionStatistics")]
        public SessionStatistics SessionStatistics { get; set; } = new SessionStatistics();
    }

    public class SessionStatistics
    {
        [JsonProperty("totalCards")]
        public int TotalCards { get; set; } = 0;

        [JsonProperty("cardsStudied")]
        public int CardsStudied { get; set; } = 0;

        [JsonProperty("correctAnswers")]
        public int CorrectAnswers { get; set; } = 0;

        [JsonProperty("incorrectAnswers")]
        public int IncorrectAnswers { get; set; } = 0;

        [JsonProperty("totalStudyTime")]
        public TimeSpan TotalStudyTime { get; set; } = TimeSpan.Zero;

        [JsonProperty("sessionStartTime")]
        public DateTime SessionStartTime { get; set; } = DateTime.Now;

        [JsonProperty("lastActivityTime")]
        public DateTime LastActivityTime { get; set; } = DateTime.Now;

        public double SuccessRate => CardsStudied > 0 ? (double)CorrectAnswers / CardsStudied * 100 : 0;
    }
}

