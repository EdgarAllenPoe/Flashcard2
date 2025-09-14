using Newtonsoft.Json;

namespace FlashcardApp.Models
{
    public class Flashcard
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("front")]
        public string Front { get; set; } = string.Empty;

        [JsonProperty("back")]
        public string Back { get; set; } = string.Empty;

        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonProperty("statistics")]
        public FlashcardStatistics Statistics { get; set; } = new FlashcardStatistics();

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [JsonProperty("lastReviewed")]
        public DateTime? LastReviewed { get; set; }

        [JsonProperty("nextReviewDate")]
        public DateTime? NextReviewDate { get; set; }

        [JsonProperty("currentBox")]
        public int CurrentBox { get; set; } = 0;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; } = true;
    }

    public class FlashcardStatistics
    {
        [JsonProperty("totalReviews")]
        public int TotalReviews { get; set; } = 0;

        [JsonProperty("correctAnswers")]
        public int CorrectAnswers { get; set; } = 0;

        [JsonProperty("incorrectAnswers")]
        public int IncorrectAnswers { get; set; } = 0;

        [JsonProperty("averageResponseTime")]
        public double AverageResponseTime { get; set; } = 0.0;

        [JsonProperty("totalStudyTime")]
        public TimeSpan TotalStudyTime { get; set; } = TimeSpan.Zero;

        [JsonProperty("lastStudySession")]
        public DateTime? LastStudySession { get; set; }

        [JsonProperty("streak")]
        public int Streak { get; set; } = 0;

        [JsonProperty("longestStreak")]
        public int LongestStreak { get; set; } = 0;

        public double SuccessRate => TotalReviews > 0 ? (double)CorrectAnswers / TotalReviews * 100 : 0;
    }
}

