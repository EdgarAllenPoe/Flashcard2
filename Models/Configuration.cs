using Newtonsoft.Json;

namespace FlashcardApp.Models
{
    public class AppConfiguration
    {
        [JsonProperty("leitnerBoxes")]
        public LeitnerBoxConfiguration LeitnerBoxes { get; set; } = new LeitnerBoxConfiguration();

        [JsonProperty("studySession")]
        public StudySessionConfiguration StudySession { get; set; } = new StudySessionConfiguration();

        [JsonProperty("filePaths")]
        public FilePathConfiguration FilePaths { get; set; } = new FilePathConfiguration();

        [JsonProperty("reviewScheduling")]
        public ReviewSchedulingConfiguration ReviewScheduling { get; set; } = new ReviewSchedulingConfiguration();

        [JsonProperty("dailyLimits")]
        public DailyLimitsConfiguration DailyLimits { get; set; } = new DailyLimitsConfiguration();

        [JsonProperty("ui")]
        public UIConfiguration UI { get; set; } = new UIConfiguration();
    }

    public class LeitnerBoxConfiguration
    {
        [JsonProperty("numberOfBoxes")]
        public int NumberOfBoxes { get; set; } = 5;

        [JsonProperty("promotionRules")]
        public List<PromotionRule> PromotionRules { get; set; } = new List<PromotionRule>
        {
            new PromotionRule { BoxNumber = 0, CorrectAnswersNeeded = 1 },
            new PromotionRule { BoxNumber = 1, CorrectAnswersNeeded = 2 },
            new PromotionRule { BoxNumber = 2, CorrectAnswersNeeded = 3 },
            new PromotionRule { BoxNumber = 3, CorrectAnswersNeeded = 4 },
            new PromotionRule { BoxNumber = 4, CorrectAnswersNeeded = 5 }
        };

        [JsonProperty("demotionRules")]
        public List<DemotionRule> DemotionRules { get; set; } = new List<DemotionRule>
        {
            new DemotionRule { BoxNumber = 1, IncorrectAnswersNeeded = 1, DemoteToBox = 0 },
            new DemotionRule { BoxNumber = 2, IncorrectAnswersNeeded = 1, DemoteToBox = 0 },
            new DemotionRule { BoxNumber = 3, IncorrectAnswersNeeded = 1, DemoteToBox = 1 },
            new DemotionRule { BoxNumber = 4, IncorrectAnswersNeeded = 1, DemoteToBox = 2 }
        };
    }

    public class PromotionRule
    {
        [JsonProperty("boxNumber")]
        public int BoxNumber { get; set; }

        [JsonProperty("correctAnswersNeeded")]
        public int CorrectAnswersNeeded { get; set; }
    }

    public class DemotionRule
    {
        [JsonProperty("boxNumber")]
        public int BoxNumber { get; set; }

        [JsonProperty("incorrectAnswersNeeded")]
        public int IncorrectAnswersNeeded { get; set; }

        [JsonProperty("demoteToBox")]
        public int DemoteToBox { get; set; }
    }

    public class StudySessionConfiguration
    {
        [JsonProperty("defaultStudyMode")]
        public StudyMode DefaultStudyMode { get; set; } = StudyMode.FrontToBack;

        [JsonProperty("showStatistics")]
        public bool ShowStatistics { get; set; } = true;

        [JsonProperty("autoAdvance")]
        public bool AutoAdvance { get; set; } = false;

        [JsonProperty("autoAdvanceDelay")]
        public int AutoAdvanceDelay { get; set; } = 3; // seconds

        [JsonProperty("shuffleCards")]
        public bool ShuffleCards { get; set; } = true;

        [JsonProperty("showProgress")]
        public bool ShowProgress { get; set; } = true;

        [JsonProperty("keyboardShortcuts")]
        public KeyboardShortcutsConfiguration KeyboardShortcuts { get; set; } = new KeyboardShortcutsConfiguration();
    }

    public class KeyboardShortcutsConfiguration
    {
        [JsonProperty("correctAnswer")]
        public string CorrectAnswer { get; set; } = "1";

        [JsonProperty("incorrectAnswer")]
        public string IncorrectAnswer { get; set; } = "2";

        [JsonProperty("showAnswer")]
        public string ShowAnswer { get; set; } = "space";

        [JsonProperty("quit")]
        public string Quit { get; set; } = "q";

        [JsonProperty("skip")]
        public string Skip { get; set; } = "s";

        [JsonProperty("flipCard")]
        public string FlipCard { get; set; } = "f";

        [JsonProperty("showStatistics")]
        public string ShowStatistics { get; set; } = "t";

        [JsonProperty("help")]
        public string Help { get; set; } = "h";
    }

    public class FilePathConfiguration
    {
        [JsonProperty("decksDirectory")]
        public string DecksDirectory { get; set; } = "decks";

        [JsonProperty("configFileName")]
        public string ConfigFileName { get; set; } = "config.json";

        [JsonProperty("deckFileExtension")]
        public string DeckFileExtension { get; set; } = ".json";

        [JsonProperty("backupDirectory")]
        public string BackupDirectory { get; set; } = "backups";

        [JsonProperty("exportDirectory")]
        public string ExportDirectory { get; set; } = "exports";
    }

    public class ReviewSchedulingConfiguration
    {
        [JsonProperty("boxIntervals")]
        public List<BoxInterval> BoxIntervals { get; set; } = new List<BoxInterval>
        {
            new BoxInterval { BoxNumber = 0, IntervalDays = 1 },
            new BoxInterval { BoxNumber = 1, IntervalDays = 3 },
            new BoxInterval { BoxNumber = 2, IntervalDays = 7 },
            new BoxInterval { BoxNumber = 3, IntervalDays = 14 },
            new BoxInterval { BoxNumber = 4, IntervalDays = 30 }
        };

        [JsonProperty("newCardInterval")]
        public int NewCardInterval { get; set; } = 1; // days

        [JsonProperty("maxNewCardsPerDay")]
        public int MaxNewCardsPerDay { get; set; } = 20;
    }

    public class BoxInterval
    {
        [JsonProperty("boxNumber")]
        public int BoxNumber { get; set; }

        [JsonProperty("intervalDays")]
        public int IntervalDays { get; set; }
    }

    public class DailyLimitsConfiguration
    {
        [JsonProperty("maxCardsPerDay")]
        public int MaxCardsPerDay { get; set; } = 100;

        [JsonProperty("minCardsPerDay")]
        public int MinCardsPerDay { get; set; } = 5;

        [JsonProperty("maxStudyTimePerDay")]
        public TimeSpan MaxStudyTimePerDay { get; set; } = TimeSpan.FromHours(2);

        [JsonProperty("minStudyTimePerDay")]
        public TimeSpan MinStudyTimePerDay { get; set; } = TimeSpan.FromMinutes(5);
    }

    public class UIConfiguration
    {
        [JsonProperty("useColors")]
        public bool UseColors { get; set; } = true;

        [JsonProperty("useIcons")]
        public bool UseIcons { get; set; } = true;

        [JsonProperty("showWelcomeMessage")]
        public bool ShowWelcomeMessage { get; set; } = true;

        [JsonProperty("clearScreenOnMenuChange")]
        public bool ClearScreenOnMenuChange { get; set; } = true;

        [JsonProperty("showDetailedStatistics")]
        public bool ShowDetailedStatistics { get; set; } = true;
    }

    public enum StudyMode
    {
        FrontToBack,
        BackToFront,
        Mixed
    }
}

