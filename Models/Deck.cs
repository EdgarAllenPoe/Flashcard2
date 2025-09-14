using Newtonsoft.Json;

namespace FlashcardApp.Models
{
    public class Deck
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("flashcards")]
        public List<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; } = DateTime.Now;

        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonProperty("statistics")]
        public DeckStatistics Statistics { get; set; } = new DeckStatistics();

        public int TotalCards => Flashcards.Count;
        public int ActiveCards => Flashcards.Count(f => f.IsActive);
        public int CardsInBox(int boxNumber) => Flashcards.Count(f => f.IsActive && f.CurrentBox == boxNumber);
    }

    public class DeckStatistics
    {
        [JsonProperty("totalStudySessions")]
        public int TotalStudySessions { get; set; } = 0;

        [JsonProperty("totalStudyTime")]
        public TimeSpan TotalStudyTime { get; set; } = TimeSpan.Zero;

        [JsonProperty("averageSessionTime")]
        public TimeSpan AverageSessionTime { get; set; } = TimeSpan.Zero;

        [JsonProperty("lastStudySession")]
        public DateTime? LastStudySession { get; set; }

        [JsonProperty("cardsMastered")]
        public int CardsMastered { get; set; } = 0;

        [JsonProperty("overallSuccessRate")]
        public double OverallSuccessRate { get; set; } = 0.0;

        [JsonProperty("studyStreak")]
        public int StudyStreak { get; set; } = 0;

        [JsonProperty("longestStudyStreak")]
        public int LongestStudyStreak { get; set; } = 0;
    }
}

