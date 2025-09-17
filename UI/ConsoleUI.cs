using FlashcardApp.Models;
using FlashcardApp.Services;

namespace FlashcardApp.UI
{
    public class ConsoleUI
    {
        private readonly ConfigurationService _configService;
        private readonly DeckService _deckService;
        private readonly StudySessionService _studySessionService;
        private readonly LeitnerBoxService _leitnerBoxService;
        private readonly bool _emojiSupport;

        public ConsoleUI(ConfigurationService configService, DeckService deckService, 
                        StudySessionService studySessionService, LeitnerBoxService leitnerBoxService)
        {
            _configService = configService;
            _deckService = deckService;
            _studySessionService = studySessionService;
            _leitnerBoxService = leitnerBoxService;
            _emojiSupport = TestEmojiSupport();
        }

        private bool TestEmojiSupport()
        {
            // Always return false - we're using ASCII symbols instead of emojis
                return false;
        }


        private void CheckEmojiDisplayAndOfferFallback()
        {
            var config = _configService.GetConfiguration();
            
            // Only show this check once per session and only if emojis are enabled
            if (config.UI.UseIcons && _emojiSupport)
            {
                Console.WriteLine();
                Console.WriteLine("    If you see boxes or strange characters instead of emojis,");
                Console.WriteLine("    you can disable them in Configuration > UI Settings > Toggle Use Icons");
                Console.WriteLine();
            }
        }

        public void Run()
        {
            var config = _configService.GetConfiguration();
            
            // Set console properties for better appearance
            Console.Title = "Flashcard App - Leitner Box System";
            Console.CursorVisible = true;
            
            if (config.UI.ShowWelcomeMessage)
            {
                ShowWelcomeMessage();
            }

            while (true)
            {
                ShowMainMenu();
                var choice = GetImmediateChoice();
                
                if (config.UI.ClearScreenOnMenuChange)
                {
                    Console.Clear();
                }

                switch (choice)
                {
                    case "1":
                        StudySession();
                        break;
                    case "2":
                        var shouldPause = ManageDecks();
                        if (!shouldPause) continue; // Skip pause if ESC was pressed
                        break;
                    case "3":
                        ViewStatistics();
                        break;
                    case "4":
                        var configShouldPause = ConfigurationMenu();
                        if (!configShouldPause) continue; // Skip pause if ESC was pressed
                        break;
                    case "5":
                        ShowHelp();
                        break;
                    case "q":
                    case "ESC":
                        ShowExitMessage();
                        return;
                    default:
                        ShowInvalidChoice();
                        break;
                }

                if (choice != "ESC" && choice != "q")
                {
                    ShowPressAnyKey();
                }
            }
        }

        private void ShowWelcomeMessage()
        {
            var config = _configService.GetConfiguration();
            
            Console.Clear();
            
            // Beautiful welcome message with emojis but no ASCII boxes
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine();
            Console.WriteLine("    FLASHCARD APP v2.0");
            Console.WriteLine();
            Console.WriteLine("    Advanced Leitner Box Spaced Repetition");
            Console.WriteLine();
            Console.WriteLine("    Beautiful • Modern • Effective • Smart");
            Console.WriteLine();
            
            Console.ResetColor();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            Console.WriteLine();
            Console.WriteLine("    Welcome to your personalized learning journey!");
            Console.WriteLine("    Master any subject with scientifically-proven spaced repetition");
            Console.WriteLine("    Enjoy a beautiful, intuitive interface designed for focus");
            
            Console.ResetColor();
            Console.WriteLine();
            
            // Check emoji display and offer fallback
            CheckEmojiDisplayAndOfferFallback();
            
            ShowPressAnyKey();
        }

        private void ShowMainMenu()
        {
            var config = _configService.GetConfiguration();
            
            Console.Clear();
            
            // Beautiful main menu with modern design
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            
            Console.WriteLine();
            Console.WriteLine("    MAIN MENU");
            Console.WriteLine();
            
            Console.ResetColor();
            Console.WriteLine();
            
            // Menu options with clean text
            ShowMenuOption("1", "Start Study Session", "Begin your learning journey with spaced repetition");
            ShowMenuOption("2", "Manage Decks", "Create, edit, and organize your flashcard collections");
            ShowMenuOption("3", "View Statistics", "Track your progress and learning analytics");
            ShowMenuOption("4", "Configuration", "Customize your learning experience");
            ShowMenuOption("5", "Help & Guide", "Learn how to use the app effectively");
            ShowMenuOption("ESC", "Exit", "Close the application");
            
            Console.WriteLine();
            ShowInputPrompt("Enter your choice");
        }

        private void ShowMenuOption(string number, string title, string description)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.Write($"    {number}. ");
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.Write(title);
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            
            Console.WriteLine($" - {description}");
            Console.ResetColor();
        }

        private void StudySession()
        {
            var decks = _deckService.LoadAllDecks();
            
            if (!decks.Any())
            {
                ShowErrorMessage("No decks available. Please create a deck first.");
                return;
            }

            var selectedDeck = SelectDeck(decks, "Select a deck to study:");
            if (selectedDeck == null) 
            {
                ShowInfoMessage("Study session cancelled.");
                return;
            }

            var studyMode = SelectStudyMode();
            var maxCards = GetMaxCardsForSession();

            var result = _studySessionService.StartStudySession(selectedDeck, studyMode, maxCards);
            
            if (result.Success)
            {
                ShowSessionResults(result);
            }
            else
            {
                ShowErrorMessage(result.Message);
            }
        }

        private bool ManageDecks()
        {
            while (true)
            {
                Console.Clear();
                ShowDeckManagementMenu();
                var choice = GetImmediateChoice();

                switch (choice)
                {
                    case "1":
                        CreateNewDeck();
                        break;
                    case "2":
                        ViewAllDecks();
                        break;
                    case "3":
                        var shouldPause = EditDeck();
                        if (!shouldPause) continue; // Skip pause if ESC was pressed
                        break;
                    case "4":
                        DeleteDeck();
                        break;
                    case "5":
                        ImportDeck();
                        break;
                    case "6":
                        ExportDeck();
                        break;
                    case "ESC":
                        return false; // No pause needed for ESC
                    default:
                        ShowInvalidChoice();
                        break;
                }

                if (choice != "ESC")
                {
                    ShowPressAnyKey();
                }
            }
        }

        private void ViewStatistics()
        {
            var decks = _deckService.LoadAllDecks();
            
            if (!decks.Any())
            {
                ShowErrorMessage("No decks available.");
                return;
            }

            Console.Clear();
            ShowStatisticsHeader();

            foreach (var deck in decks)
            {
                ShowDeckStatistics(deck);
                Console.WriteLine();
            }

            var overallStats = CalculateOverallStatistics(decks);
            ShowOverallStatistics(overallStats);
        }

        private bool ConfigurationMenu()
        {
            while (true)
            {
                Console.Clear();
                ShowConfigurationMenu();
                var choice = GetImmediateChoice();

                switch (choice)
                {
                    case "1":
                        var viewShouldPause = ViewCurrentConfiguration();
                        if (!viewShouldPause) continue; // Skip pause if ESC was pressed
                        break;
                    case "2":
                        var boxShouldPause = EditLeitnerBoxSettings();
                        if (!boxShouldPause) continue; // Skip pause if ESC was pressed
                        break;
                    case "3":
                        var sessionShouldPause = EditStudySessionSettings();
                        if (!sessionShouldPause) continue; // Skip pause if ESC was pressed
                        break;
                    case "4":
                        var limitsShouldPause = EditDailyLimits();
                        if (!limitsShouldPause) continue; // Skip pause if ESC was pressed
                        break;
                    case "5":
                        var uiShouldPause = EditUISettings();
                        if (!uiShouldPause) continue; // Skip pause if ESC was pressed
                        break;
                    case "6":
                        var resetShouldPause = ResetToDefaults();
                        if (!resetShouldPause) continue; // Skip pause if ESC was pressed
                        break;
                    case "ESC":
                        return false; // No pause needed for ESC
                    default:
                        ShowInvalidChoice();
                        break;
                }

                if (choice != "ESC")
                {
                    ShowPressAnyKey();
                }
            }
        }

        private void ShowDeckManagementMenu()
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            
            Console.WriteLine();
            Console.WriteLine("    DECK MANAGEMENT");
            Console.WriteLine();
            
            Console.ResetColor();
            Console.WriteLine();
            
            ShowMenuOption("1", "Create New Deck", "Start a new flashcard collection");
            ShowMenuOption("2", "View All Decks", "Browse your existing decks");
            ShowMenuOption("3", "Edit Deck", "Modify deck properties and cards");
            ShowMenuOption("4", "Delete Deck", "Remove a deck permanently");
            ShowMenuOption("5", "Import Deck", "Load decks from external files");
            ShowMenuOption("6", "Export Deck", "Save decks to external files");
            ShowMenuOption("ESC", "Back to Main Menu", "Return to the main menu");
            
            Console.WriteLine();
            ShowInputPrompt("Enter your choice");
        }

        private void CreateNewDeck()
        {
            Console.Clear();
            ShowSectionHeader("CREATE NEW DECK", ConsoleColor.Green);
            
            Console.WriteLine();
            ShowInputPrompt("Deck name");
            var name = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(name))
            {
                ShowErrorMessage("Deck name cannot be empty.");
                return;
            }

            ShowInputPrompt("Description (optional)");
            var description = Console.ReadLine() ?? "";

            ShowInputPrompt("Tags (comma-separated, optional)");
            var tagsInput = Console.ReadLine() ?? "";
            var tags = tagsInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .ToList();

            var deck = _deckService.CreateNewDeck(name, description, tags);
            
            if (_deckService.SaveDeck(deck))
            {
                ShowSuccessMessage($"Deck '{name}' created successfully!");
            }
            else
            {
                ShowErrorMessage("Failed to create deck.");
            }
        }

        private void ViewAllDecks()
        {
            var decks = _deckService.LoadAllDecks();
            
            Console.Clear();
            ShowSectionHeader("ALL DECKS", ConsoleColor.Cyan);
            
            if (!decks.Any())
            {
                ShowInfoMessage("No decks found. Create your first deck to get started!");
                return;
            }

            Console.WriteLine();
            for (int i = 0; i < decks.Count; i++)
            {
                var deck = decks[i];
                ShowDeckCard(deck, i + 1);
            }
        }

        private void ShowDeckCard(Deck deck, int number)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.WriteLine($"    Deck #{number}: {deck.Name}");
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            if (!string.IsNullOrEmpty(deck.Description))
            {
                Console.WriteLine($"    {deck.Description}");
            }
            
            Console.WriteLine($"    Cards: {deck.ActiveCards}/{deck.TotalCards} | Created: {deck.CreatedDate:yyyy-MM-dd}");
            Console.WriteLine($"    Last Modified: {deck.LastModified:yyyy-MM-dd HH:mm}");
            
            if (deck.Tags.Any())
            {
                Console.WriteLine($"    Tags: {string.Join(", ", deck.Tags)}");
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        private Deck? SelectDeck(List<Deck> decks, string prompt)
        {
            while (true)
            {
                Console.Clear();
                ShowSectionHeader("SELECT DECK", ConsoleColor.Magenta);
                Console.WriteLine();
                
                for (int i = 0; i < decks.Count; i++)
                {
                    var deck = decks[i];
                    Console.WriteLine($"    {i + 1}. {deck.Name} ({deck.ActiveCards} active cards)");
                }
                
                Console.WriteLine();
                Console.WriteLine("    ESC. Cancel");
                Console.WriteLine();
                ShowInputPrompt("Enter your choice");
                
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return null; // ESC pressed - return null to indicate cancellation
                }
                
                Console.Write(keyInfo.KeyChar);
                var input = keyInfo.KeyChar + Console.ReadLine();
                
                if (int.TryParse(input, out int choice) && choice > 0 && choice <= decks.Count)
                {
                    return decks[choice - 1];
                }
                
                // Invalid selection - show error and loop again
                ShowErrorMessage("Invalid selection. Please try again.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private StudyMode SelectStudyMode()
        {
            var config = _configService.GetConfiguration();
            
            Console.Clear();
            ShowSectionHeader("SELECT STUDY MODE", ConsoleColor.Blue);
            Console.WriteLine();
            
            Console.WriteLine("    1. Front to Back (default) - Show question first, then answer");
            Console.WriteLine("    2. Back to Front - Show answer first, then question");
            Console.WriteLine("    3. Mixed - Randomly switch between modes");
            Console.WriteLine();
            ShowInputPrompt($"Enter your choice (default: {config.StudySession.DefaultStudyMode})");
            
            var input = GetImmediateChoice();
            if (input == "ESC")
            {
                return config.StudySession.DefaultStudyMode; // Return default on ESC
            }
            
            return input switch
            {
                "2" => StudyMode.BackToFront,
                "3" => StudyMode.Mixed,
                _ => config.StudySession.DefaultStudyMode
            };
        }

        private int GetMaxCardsForSession()
        {
            var config = _configService.GetConfiguration();
            
            ShowInputPrompt($"Maximum cards for this session (default: {config.DailyLimits.MaxCardsPerDay})");
            
            // Handle input properly for numeric input with Enter support
            var keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return config.DailyLimits.MaxCardsPerDay; // Return default on ESC
            }
            
            // If Enter was pressed immediately, use default
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                return config.DailyLimits.MaxCardsPerDay;
            }
            
            // Otherwise, read the full input line
            Console.Write(keyInfo.KeyChar);
            var input = keyInfo.KeyChar + Console.ReadLine();
            
            if (int.TryParse(input, out int maxCards) && maxCards > 0)
            {
                return Math.Min(maxCards, config.DailyLimits.MaxCardsPerDay);
            }
            
            return config.DailyLimits.MaxCardsPerDay;
        }

        private void ShowSessionResults(StudySessionResult result)
        {
            var stats = result.SessionStatistics;
            var config = _configService.GetConfiguration();
            
            Console.Clear();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            Console.WriteLine();
            Console.WriteLine("    SESSION COMPLETED!");
            Console.WriteLine();
            
            Console.ResetColor();
            Console.WriteLine();
            
            // Beautiful statistics display
            ShowStatCard("", "Session Time", $"{stats.SessionTime:mm\\:ss}");
            ShowStatCard("", "Cards Studied", stats.TotalCards.ToString());
            ShowStatCard("", "Total Reviews", stats.TotalReviews.ToString());
            ShowStatCard("", "Correct Answers", stats.CorrectAnswers.ToString());
            ShowStatCard("", "Incorrect Answers", stats.IncorrectAnswers.ToString());
            ShowStatCard("", "Success Rate", $"{stats.SuccessRate:F1}%");
            
            if (stats.TotalReviews > 0)
            {
                ShowStatCard("", "Avg Response Time", $"{stats.AverageResponseTime:F1}s");
            }
            
            Console.WriteLine();
            
            // Motivational message based on performance
            if (stats.SuccessRate >= 80)
            {
                ShowSuccessMessage("Excellent work! You're mastering this material!");
            }
            else if (stats.SuccessRate >= 60)
            {
                ShowInfoMessage("Good progress! Keep practicing to improve further!");
            }
            else
            {
                ShowInfoMessage("Keep studying! Every mistake is a learning opportunity!");
            }
        }

        private void ShowStatCard(string icon, string label, string value)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine($"    {icon} {label.PadRight(20)}: {value}");
            Console.ResetColor();
        }

        private void ShowSectionHeader(string title, ConsoleColor color)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = color;
            }
            
            Console.WriteLine();
            Console.WriteLine($"    {title}");
            Console.WriteLine();
            
            Console.ResetColor();
        }

        private void ShowSuccessMessage(string message)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            Console.WriteLine($"    {message}");
            Console.ResetColor();
        }

        private void ShowErrorMessage(string message)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            
            Console.WriteLine($"    {message}");
            Console.ResetColor();
        }

        private void ShowInfoMessage(string message)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.WriteLine($"    {message}");
            Console.ResetColor();
        }

        private void ShowInvalidChoice()
        {
            ShowErrorMessage("Invalid choice. Please try again.");
        }

        private void ShowExitMessage()
        {
            var config = _configService.GetConfiguration();
            
            Console.Clear();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine();
            Console.WriteLine("    THANK YOU FOR LEARNING!");
            Console.WriteLine();
            Console.WriteLine("    Remember: Every study session brings you closer to mastery!");
            Console.WriteLine("    Keep up the great work and never stop learning!");
            Console.WriteLine();
            
            Console.ResetColor();
            Console.WriteLine();
        }

        private void ShowPressAnyKey()
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            
            Console.WriteLine();
            Console.WriteLine("    Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
        }

        private void ShowInputPrompt(string prompt)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.Write($"    {prompt}: ");
            Console.ResetColor();
        }

        private string GetUserChoice()
        {
            var keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return "ESC";
            }
            Console.Write(keyInfo.KeyChar);
            var input = keyInfo.KeyChar + Console.ReadLine();
            return input;
        }

        private string GetImmediateChoice()
        {
            var keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return "ESC";
            }
            Console.Write(keyInfo.KeyChar);
            return keyInfo.KeyChar.ToString();
        }

        private void ShowHelp()
        {
            Console.Clear();
            ShowSectionHeader("HELP & GUIDE", ConsoleColor.Cyan);
            Console.WriteLine();
            
            Console.WriteLine("    This application uses the scientifically-proven Leitner Box system for");
            Console.WriteLine("    spaced repetition learning, helping you retain information long-term.");
            Console.WriteLine();
            
            Console.WriteLine("    How the Leitner Box System Works:");
            Console.WriteLine("    • Cards start in Box 0 (review daily)");
            Console.WriteLine("    • Correct answers move cards to higher boxes");
            Console.WriteLine("    • Incorrect answers move cards back to lower boxes");
            Console.WriteLine("    • Higher boxes have longer review intervals");
            Console.WriteLine("    • This optimizes your study time and retention");
            Console.WriteLine();
            
            Console.WriteLine("    Study Session Controls:");
            Console.WriteLine("    • Any Key - Show answer (space, enter, letters, numbers)");
            Console.WriteLine("    • 1 - Mark as correct (move to higher box)");
            Console.WriteLine("    • 2 - Mark as incorrect (move to lower box)");
            Console.WriteLine("    • S - Skip card (no box change)");
            Console.WriteLine("    • Q or ESC - Quit session (saves progress)");
            Console.WriteLine("    • H - Show this help");
            Console.WriteLine();
            
            Console.WriteLine("    Pro Tips:");
            Console.WriteLine("    • Study regularly for best results");
            Console.WriteLine("    • Don't worry about mistakes - they're part of learning!");
            Console.WriteLine("    • Use the statistics to track your progress");
            Console.WriteLine("    • Create focused decks for different subjects");
        }

        private void ShowStatisticsHeader()
        {
            ShowSectionHeader("LEARNING STATISTICS", ConsoleColor.Magenta);
        }

        private void ShowDeckStatistics(Deck deck)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.WriteLine($"    {deck.Name}");
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine($"    Total Cards: {deck.TotalCards} | Active: {deck.ActiveCards}");
            Console.WriteLine($"    Study Sessions: {deck.Statistics.TotalStudySessions}");
            Console.WriteLine($"    Total Study Time: {deck.Statistics.TotalStudyTime:hh\\:mm\\:ss}");
            Console.WriteLine($"    Overall Success Rate: {deck.Statistics.OverallSuccessRate:F1}%");
            Console.WriteLine($"    Study Streak: {deck.Statistics.StudyStreak} days");
            
            // Show box distribution
            var boxStats = _leitnerBoxService.GetBoxStatistics(deck);
            Console.WriteLine("    Box Distribution:");
            for (int i = 0; i < boxStats.Count; i++)
            {
                Console.WriteLine($"    Box {i}: {boxStats[i]} cards");
            }
            Console.ResetColor();
        }

        private void ShowOverallStatistics(Dictionary<string, object> stats)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine();
            Console.WriteLine("    T  OVERALL STATISTICS");
            Console.WriteLine();
            
            Console.ResetColor();
            Console.WriteLine();
            
            ShowStatCard("", "Total Decks", stats["TotalDecks"].ToString());
            ShowStatCard("", "Total Cards", stats["TotalCards"].ToString());
            ShowStatCard("", "Total Study Time", stats["TotalStudyTime"].ToString());
            ShowStatCard("", "Average Success Rate", $"{stats["AverageSuccessRate"]}%");
        }

        private Dictionary<string, object> CalculateOverallStatistics(List<Deck> decks)
        {
            var totalDecks = decks.Count;
            var totalCards = decks.Sum(d => d.TotalCards);
            var totalStudyTime = TimeSpan.FromTicks(decks.Sum(d => d.Statistics.TotalStudyTime.Ticks));
            var averageSuccessRate = decks.Any() ? decks.Average(d => d.Statistics.OverallSuccessRate) : 0;

            return new Dictionary<string, object>
            {
                ["TotalDecks"] = totalDecks,
                ["TotalCards"] = totalCards,
                ["TotalStudyTime"] = totalStudyTime.ToString(@"hh\:mm\:ss"),
                ["AverageSuccessRate"] = averageSuccessRate.ToString("F1")
            };
        }

        private bool EditDeck()
        {
            var decks = _deckService.LoadAllDecks();
            
            if (!decks.Any())
            {
                ShowErrorMessage("No decks available to edit.");
                return true; // Show pause for error
            }

            Console.Clear();
            ShowSectionHeader("EDIT DECK", ConsoleColor.Yellow);
            Console.WriteLine();
            
            var selectedDeck = SelectDeck(decks, "Select a deck to edit:");
            if (selectedDeck == null) 
            {
                ShowInfoMessage("Edit cancelled.");
                return false; // No pause needed for ESC
            }

            Console.Clear();
            ShowSectionHeader($"EDITING: {selectedDeck.Name}", ConsoleColor.Yellow);
            Console.WriteLine();
            
            Console.WriteLine("What would you like to edit?");
            Console.WriteLine("1. Deck Name");
            Console.WriteLine("2. Description");
            Console.WriteLine("3. Tags");
            Console.WriteLine("4. Add Flashcard");
            Console.WriteLine("5. Edit Flashcard");
            Console.WriteLine("6. Delete Flashcard");
            Console.WriteLine("7. Reset Study Dates");
            Console.WriteLine("ESC. Back");
            Console.WriteLine();
            ShowInputPrompt("Enter your choice");
            
            var keyInfo = Console.ReadKey(true);
            string choice;
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                choice = "ESC";
            }
            else
            {
                Console.Write(keyInfo.KeyChar);
                choice = keyInfo.KeyChar + Console.ReadLine();
            }
            switch (choice)
            {
                case "1":
                    EditDeckName(selectedDeck);
                    break;
                case "2":
                    EditDeckDescription(selectedDeck);
                    break;
                case "3":
                    EditDeckTags(selectedDeck);
                    break;
                case "4":
                    AddFlashcardToDeck(selectedDeck);
                    break;
                case "5":
                    EditFlashcard(selectedDeck);
                    break;
                case "6":
                    DeleteFlashcard(selectedDeck);
                    break;
                case "7":
                    ResetStudyDates(selectedDeck);
                    break;
                case "ESC":
                    return false; // No pause needed for ESC
                default:
                    ShowInvalidChoice();
                    break;
            }
            
            return true; // Show pause for normal completion
        }
        private void DeleteDeck()
        {
            var decks = _deckService.LoadAllDecks();
            
            if (!decks.Any())
            {
                ShowErrorMessage("No decks available to delete.");
                return;
            }

            Console.Clear();
            ShowSectionHeader("DELETE DECK", ConsoleColor.Red);
            Console.WriteLine();
            
            var selectedDeck = SelectDeck(decks, "Select a deck to delete:");
            if (selectedDeck == null) 
            {
                ShowInfoMessage("Delete cancelled.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"WARNING: You are about to delete the deck '{selectedDeck.Name}'");
            Console.WriteLine($"   This will permanently remove {selectedDeck.TotalCards} cards and all statistics.");
            Console.WriteLine();
            ShowInputPrompt("Type 'DELETE' to confirm deletion (or anything else to cancel)");
            
            var confirmation = Console.ReadLine();
            if (confirmation?.ToUpper() == "DELETE")
            {
                if (_deckService.DeleteDeck(selectedDeck.Id))
                {
                    ShowSuccessMessage($"Deck '{selectedDeck.Name}' deleted successfully!");
                }
                else
                {
                    ShowErrorMessage("Failed to delete deck.");
                }
            }
            else
            {
                ShowInfoMessage("Deletion cancelled.");
            }
        }
        private void ImportDeck()
        {
            Console.Clear();
            ShowSectionHeader("IMPORT DECK", ConsoleColor.Green);
            Console.WriteLine();
            
            Console.WriteLine("Import a deck from a CSV, XLSX, or JSON file.");
            Console.WriteLine("Supported formats: .csv, .xlsx, .json");
            Console.WriteLine();
            ShowInputPrompt("Enter the full path to the deck file (or press Enter to browse current directory)");
            
            var input = GetUserChoice();
            if (input == "ESC")
            {
                ShowInfoMessage("Import cancelled.");
                return;
            }
            
            var filePath = input;
            
            if (string.IsNullOrWhiteSpace(filePath))
            {
                // Show files in current directory
                var currentDir = Directory.GetCurrentDirectory();
                var supportedExtensions = new[] { "*.csv", "*.xlsx", "*.json" };
                var files = new List<string>();
                
                foreach (var extension in supportedExtensions)
                {
                    var foundFiles = Directory.GetFiles(currentDir, extension);
                    if (extension == "*.json")
                    {
                        // Filter out config.json
                        foundFiles = foundFiles.Where(f => !f.EndsWith("config.json")).ToArray();
                    }
                    files.AddRange(foundFiles);
                }
                
                if (!files.Any())
                {
                    ShowErrorMessage("No supported files found in current directory.");
                    return;
                }
                
                Console.WriteLine();
                Console.WriteLine("Available files:");
                for (int i = 0; i < files.Count; i++)
                {
                    var fileName = Path.GetFileName(files[i]);
                    var extension = Path.GetExtension(files[i]).ToUpper();
                    Console.WriteLine($"  {i + 1}. {fileName} ({extension})");
                }
                Console.WriteLine();
                ShowInputPrompt("Enter the number of the file to import (or ESC to cancel)");
                
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    ShowInfoMessage("Import cancelled.");
                    return;
                }
                
                var choice = keyInfo.KeyChar.ToString();
                if (int.TryParse(choice, out int fileIndex) && fileIndex > 0 && fileIndex <= files.Count)
                {
                    filePath = files[fileIndex - 1];
                }
                else
                {
                    ShowErrorMessage("Invalid file selection.");
                    return;
                }
            }
            
            if (!File.Exists(filePath))
            {
                ShowErrorMessage($"File not found: {filePath}");
                return;
            }
            
            try
            {
                var importedDeck = _deckService.ImportDeck(filePath);
                if (importedDeck != null)
                {
                    if (_deckService.SaveDeck(importedDeck))
                    {
                        ShowSuccessMessage($"Deck '{importedDeck.Name}' imported successfully!");
                        Console.WriteLine($"   T  {importedDeck.TotalCards} cards imported");
                        Console.WriteLine($"   Format: {Path.GetExtension(filePath).ToUpper()}");
                    }
                    else
                    {
                        ShowErrorMessage("Failed to save imported deck.");
                    }
                }
                else
                {
                    ShowErrorMessage("Failed to import deck. Please check the file format.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error importing deck: {ex.Message}");
            }
        }
        private void ExportDeck()
        {
            var decks = _deckService.LoadAllDecks();
            
            if (!decks.Any())
            {
                ShowErrorMessage("No decks available to export.");
                return;
            }

            Console.Clear();
            ShowSectionHeader("EXPORT DECK", ConsoleColor.Blue);
            Console.WriteLine();
            
            var selectedDeck = SelectDeck(decks, "Select a deck to export:");
            if (selectedDeck == null) 
            {
                ShowInfoMessage("Export cancelled.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Supported export formats:");
            Console.WriteLine("  .csv  - Comma-separated values");
            Console.WriteLine("  T  .xlsx - Excel spreadsheet");
            Console.WriteLine("  L  .json - JSON format");
            Console.WriteLine();
            ShowInputPrompt("Enter the file path to save the deck (or press Enter for default JSON)");
            
            var input = GetUserChoice();
            if (input == "ESC")
            {
                ShowInfoMessage("Export cancelled.");
                return;
            }
            
            var filePath = input;
            if (string.IsNullOrWhiteSpace(filePath))
            {
                var currentDir = Directory.GetCurrentDirectory();
                var safeFileName = selectedDeck.Name.Replace(" ", "_").Replace(":", "").Replace("/", "").Replace("\\", "");
                filePath = Path.Combine(currentDir, $"{safeFileName}_export.json");
            }
            
            try
            {
                if (_deckService.ExportDeck(selectedDeck, filePath))
                {
                    ShowSuccessMessage($"Deck '{selectedDeck.Name}' exported successfully!");
                    Console.WriteLine($"   Saved to: {filePath}");
                    Console.WriteLine($"   T  {selectedDeck.TotalCards} cards exported");
                    Console.WriteLine($"   Format: {Path.GetExtension(filePath).ToUpper()}");
                }
                else
                {
                    ShowErrorMessage("Failed to export deck.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error exporting deck: {ex.Message}");
            }
        }
        private void ShowConfigurationMenu()
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine();
            Console.WriteLine("    CONFIGURATION");
            Console.WriteLine();
            
            Console.ResetColor();
            Console.WriteLine();
            
            ShowMenuOption("1", "View Current Settings", "Display all current configuration settings");
            ShowMenuOption("2", "Leitner Box Settings", "Configure box promotion and demotion rules");
            ShowMenuOption("3", "Study Session Settings", "Customize study session behavior");
            ShowMenuOption("4", "Daily Limits", "Set maximum and minimum study limits");
            ShowMenuOption("5", "UI Settings", "Customize colors, icons, and display options");
            ShowMenuOption("6", "Reset to Defaults", "Restore all settings to default values");
            ShowMenuOption("ESC", "Back to Main Menu", "Return to the main menu");
            
            Console.WriteLine();
            ShowInputPrompt("Enter your choice");
        }
        private bool ViewCurrentConfiguration()
        {
            Console.Clear();
            ShowSectionHeader("CURRENT CONFIGURATION", ConsoleColor.Cyan);
            Console.WriteLine();
            
            var config = _configService.GetConfiguration();
            
            Console.WriteLine("B  LEITNER BOX SETTINGS:");
            Console.WriteLine($"   Number of Boxes: {config.LeitnerBoxes.NumberOfBoxes}");
            Console.WriteLine($"   Promotion Rules: {config.LeitnerBoxes.PromotionRules.Count} rules");
            Console.WriteLine($"   Demotion Rules: {config.LeitnerBoxes.DemotionRules.Count} rules");
            Console.WriteLine();
            
            Console.WriteLine("=  STUDY SESSION SETTINGS:");
            Console.WriteLine($"   Default Study Mode: {config.StudySession.DefaultStudyMode}");
            Console.WriteLine($"   Show Statistics: {config.StudySession.ShowStatistics}");
            Console.WriteLine($"   Auto Advance: {config.StudySession.AutoAdvance}");
            Console.WriteLine($"   Shuffle Cards: {config.StudySession.ShuffleCards}");
            Console.WriteLine($"   Show Progress: {config.StudySession.ShowProgress}");
            Console.WriteLine();
            
            Console.WriteLine("T  DAILY LIMITS:");
            Console.WriteLine($"   Max Cards Per Day: {config.DailyLimits.MaxCardsPerDay}");
            Console.WriteLine($"   Min Cards Per Day: {config.DailyLimits.MinCardsPerDay}");
            Console.WriteLine($"   Max Study Time: {config.DailyLimits.MaxStudyTimePerDay}");
            Console.WriteLine($"   Min Study Time: {config.DailyLimits.MinStudyTimePerDay}");
            Console.WriteLine();
            
            Console.WriteLine("@  UI SETTINGS:");
            Console.WriteLine($"   Use Colors: {config.UI.UseColors}");
            Console.WriteLine($"   Use Icons: {config.UI.UseIcons}");
            Console.WriteLine($"   Show Welcome Message: {config.UI.ShowWelcomeMessage}");
            Console.WriteLine($"   Clear Screen on Menu Change: {config.UI.ClearScreenOnMenuChange}");
            Console.WriteLine($"   Show Detailed Statistics: {config.UI.ShowDetailedStatistics}");
            Console.WriteLine();
            
            Console.WriteLine("FILE PATHS:");
            Console.WriteLine($"   Decks Directory: {config.FilePaths.DecksDirectory}");
            Console.WriteLine($"   Config File: {config.FilePaths.ConfigFileName}");
            Console.WriteLine($"   Deck File Extension: {config.FilePaths.DeckFileExtension}");
            Console.WriteLine($"   Backup Directory: {config.FilePaths.BackupDirectory}");
            Console.WriteLine($"   Export Directory: {config.FilePaths.ExportDirectory}");
            Console.WriteLine();
            
            ShowInputPrompt("Press Enter to continue or ESC to go back...");
            var input = GetUserChoice();
            return input != "ESC"; // Return false if ESC pressed (no pause needed)
        }
        private bool EditLeitnerBoxSettings()
        {
            while (true)
            {
                Console.Clear();
                ShowSectionHeader("LEITNER BOX SETTINGS", ConsoleColor.Yellow);
                Console.WriteLine();
                
                var config = _configService.GetConfiguration();
                
                Console.WriteLine("Current Settings:");
                Console.WriteLine($"   Number of Boxes: {config.LeitnerBoxes.NumberOfBoxes}");
                Console.WriteLine($"   Promotion Rules: {config.LeitnerBoxes.PromotionRules.Count} rules");
                Console.WriteLine($"   Demotion Rules: {config.LeitnerBoxes.DemotionRules.Count} rules");
                Console.WriteLine();
                
                Console.WriteLine("B  PROMOTION RULES (Correct answers):");
                for (int i = 0; i < config.LeitnerBoxes.PromotionRules.Count; i++)
                {
                    var rule = config.LeitnerBoxes.PromotionRules[i];
                    Console.WriteLine($"   Box {i} → Box {i + 1}: {rule.CorrectAnswersNeeded} correct answers");
                }
                Console.WriteLine();
                
                Console.WriteLine("📉  DEMOTION RULES (Incorrect answers):");
                for (int i = 0; i < config.LeitnerBoxes.DemotionRules.Count; i++)
                {
                    var rule = config.LeitnerBoxes.DemotionRules[i];
                    Console.WriteLine($"   Box {rule.BoxNumber} → Box {rule.DemoteToBox}: {rule.IncorrectAnswersNeeded} incorrect answers");
                }
                Console.WriteLine();
                
                Console.WriteLine("EDITING OPTIONS:");
                Console.WriteLine("   1. T  Change Number of Boxes");
                Console.WriteLine("   2. Edit Promotion Rules");
                Console.WriteLine("   3. 📉  Edit Demotion Rules");
                Console.WriteLine("   ESC. Back to Configuration Menu");
                Console.WriteLine();
                
                ShowInputPrompt("Enter your choice");
                var choice = GetImmediateChoice();
                
                switch (choice)
                {
                    case "1":
                        EditNumberOfBoxes();
                        break;
                    case "2":
                        EditPromotionRules();
                        break;
                    case "3":
                        EditDemotionRules();
                        break;
                    case "ESC":
                        return false; // No pause needed for ESC
                    default:
                        ShowInvalidChoice();
                        ShowPressAnyKey();
                        break;
                }
            }
        }
        private bool EditStudySessionSettings()
        {
            while (true)
            {
                Console.Clear();
                ShowSectionHeader("STUDY SESSION SETTINGS", ConsoleColor.Yellow);
                Console.WriteLine();
                
                var config = _configService.GetConfiguration();
                
                Console.WriteLine("    Current Settings:");
                Console.WriteLine($"    Default Study Mode: {config.StudySession.DefaultStudyMode}");
                Console.WriteLine($"    Show Statistics: {config.StudySession.ShowStatistics}");
                Console.WriteLine($"    Auto Advance: {config.StudySession.AutoAdvance}");
                Console.WriteLine($"    Shuffle Cards: {config.StudySession.ShuffleCards}");
                Console.WriteLine($"    Show Progress: {config.StudySession.ShowProgress}");
                Console.WriteLine();
                
                Console.WriteLine("    KEYBOARD SHORTCUTS:");
                Console.WriteLine($"    Correct Answer: {config.StudySession.KeyboardShortcuts.CorrectAnswer}");
                Console.WriteLine($"    Incorrect Answer: {config.StudySession.KeyboardShortcuts.IncorrectAnswer}");
                Console.WriteLine($"    Show Answer: {config.StudySession.KeyboardShortcuts.ShowAnswer}");
                Console.WriteLine($"    Skip Card: {config.StudySession.KeyboardShortcuts.Skip}");
                Console.WriteLine($"    Help: {config.StudySession.KeyboardShortcuts.Help}");
                Console.WriteLine($"    Quit: {config.StudySession.KeyboardShortcuts.Quit}");
                Console.WriteLine();
                
                Console.WriteLine("    TIMING SETTINGS:");
                Console.WriteLine($"    Auto Advance Delay: {config.StudySession.AutoAdvanceDelay} seconds");
                Console.WriteLine();
                
                Console.WriteLine("    EDITING OPTIONS:");
                Console.WriteLine("    1. Change Default Study Mode");
                Console.WriteLine("    2. Toggle Show Statistics");
                Console.WriteLine("    3. Toggle Auto Advance");
                Console.WriteLine("    4. Toggle Shuffle Cards");
                Console.WriteLine("    5. Toggle Show Progress");
                Console.WriteLine("    6. Change Auto Advance Delay");
                Console.WriteLine("    7. Edit Keyboard Shortcuts");
                Console.WriteLine("    ESC. Back to Configuration Menu");
                Console.WriteLine();
                
                ShowInputPrompt("Enter your choice");
                var choice = GetImmediateChoice();
                
                switch (choice)
                {
                    case "1":
                        EditDefaultStudyMode();
                        break;
                    case "2":
                        ToggleShowStatistics();
                        break;
                    case "3":
                        ToggleAutoAdvance();
                        break;
                    case "4":
                        ToggleShuffleCards();
                        break;
                    case "5":
                        ToggleShowProgress();
                        break;
                    case "6":
                        EditAutoAdvanceDelay();
                        break;
                    case "7":
                        EditKeyboardShortcuts();
                        break;
                    case "ESC":
                        return false; // No pause needed for ESC
                    default:
                        ShowInvalidChoice();
                        ShowPressAnyKey();
                        break;
                }
            }
        }
        private bool EditDailyLimits()
        {
            while (true)
            {
                Console.Clear();
                ShowSectionHeader("DAILY LIMITS", ConsoleColor.Yellow);
                Console.WriteLine();
                
                var config = _configService.GetConfiguration();
                
                Console.WriteLine("Current Daily Limits:");
                Console.WriteLine($"   Max Cards Per Day: {config.DailyLimits.MaxCardsPerDay}");
                Console.WriteLine($"   Min Cards Per Day: {config.DailyLimits.MinCardsPerDay}");
                Console.WriteLine($"   Max Study Time: {config.DailyLimits.MaxStudyTimePerDay}");
                Console.WriteLine($"   Min Study Time: {config.DailyLimits.MinStudyTimePerDay}");
                Console.WriteLine();
                
                Console.WriteLine("T  LIMIT DESCRIPTIONS:");
                Console.WriteLine("   • Max Cards Per Day: Maximum number of cards to study in one day");
                Console.WriteLine("   • Min Cards Per Day: Minimum number of cards to study in one day");
                Console.WriteLine("   • Max Study Time: Maximum time to spend studying per day");
                Console.WriteLine("   • Min Study Time: Minimum time to spend studying per day");
                Console.WriteLine();
                
                Console.WriteLine("EDITING OPTIONS:");
                Console.WriteLine("   1. Change Max Cards Per Day");
                Console.WriteLine("   2. Change Min Cards Per Day");
                Console.WriteLine("   3. Change Max Study Time");
                Console.WriteLine("   4. Change Min Study Time");
                Console.WriteLine("   ESC. Back to Configuration Menu");
                Console.WriteLine();
                
                ShowInputPrompt("Enter your choice");
                var choice = GetImmediateChoice();
                
                switch (choice)
                {
                    case "1":
                        EditMaxCardsPerDay();
                        break;
                    case "2":
                        EditMinCardsPerDay();
                        break;
                    case "3":
                        EditMaxStudyTime();
                        break;
                    case "4":
                        EditMinStudyTime();
                        break;
                    case "ESC":
                        return false; // No pause needed for ESC
                    default:
                        ShowInvalidChoice();
                        ShowPressAnyKey();
                        break;
                }
            }
        }
        private bool EditUISettings()
        {
            while (true)
            {
                Console.Clear();
                ShowSectionHeader("UI SETTINGS", ConsoleColor.Yellow);
                Console.WriteLine();
                
                var config = _configService.GetConfiguration();
                
                Console.WriteLine("Current UI Settings:");
                Console.WriteLine($"   Use Colors: {config.UI.UseColors}");
                Console.WriteLine($"   Use Icons: {config.UI.UseIcons}");
                Console.WriteLine($"   Show Welcome Message: {config.UI.ShowWelcomeMessage}");
                Console.WriteLine($"   Clear Screen on Menu Change: {config.UI.ClearScreenOnMenuChange}");
                Console.WriteLine($"   Show Detailed Statistics: {config.UI.ShowDetailedStatistics}");
                Console.WriteLine();
                
                Console.WriteLine("@  UI FEATURE DESCRIPTIONS:");
                Console.WriteLine("   • Use Colors: Enable colored text and backgrounds");
                Console.WriteLine("   • Use Icons: Display emoji icons in menus and messages");
                Console.WriteLine("     (If you see boxes instead of emojis, disable this option)");
                Console.WriteLine("   • Show Welcome Message: Display welcome message on startup");
                Console.WriteLine("   • Clear Screen on Menu Change: Clear screen when switching menus");
                Console.WriteLine("   • Show Detailed Statistics: Display comprehensive statistics");
                Console.WriteLine();
                
                Console.WriteLine("EDITING OPTIONS:");
                Console.WriteLine("   1. Toggle Use Colors");
                Console.WriteLine("   2. Toggle Use Icons");
                Console.WriteLine("   3. Toggle Show Welcome Message");
                Console.WriteLine("   4. Toggle Clear Screen on Menu Change");
                Console.WriteLine("   5. Toggle Show Detailed Statistics");
                Console.WriteLine("   ESC. Back to Configuration Menu");
                Console.WriteLine();
                
                ShowInputPrompt("Enter your choice");
                var choice = GetImmediateChoice();
                
                switch (choice)
                {
                    case "1":
                        ToggleUseColors();
                        break;
                    case "2":
                        ToggleUseIcons();
                        break;
                    case "3":
                        ToggleShowWelcomeMessage();
                        break;
                    case "4":
                        ToggleClearScreenOnMenuChange();
                        break;
                    case "5":
                        ToggleShowDetailedStatistics();
                        break;
                    case "ESC":
                        return false; // No pause needed for ESC
                    default:
                        ShowInvalidChoice();
                        ShowPressAnyKey();
                        break;
                }
            }
        }
        private bool ResetToDefaults()
        {
            Console.Clear();
            ShowSectionHeader("RESET TO DEFAULTS", ConsoleColor.Red);
            Console.WriteLine();
            
            Console.WriteLine("WARNING: This will reset ALL configuration settings to their default values!");
            Console.WriteLine();
            Console.WriteLine("This includes:");
            Console.WriteLine("   • Leitner Box settings");
            Console.WriteLine("   • Study session preferences");
            Console.WriteLine("   • Daily limits");
            Console.WriteLine("   • UI settings");
            Console.WriteLine("   • Keyboard shortcuts");
            Console.WriteLine();
            Console.WriteLine("Your flashcard decks and study progress will NOT be affected.");
            Console.WriteLine();
            
            ShowInputPrompt("Are you sure you want to reset to defaults? (y/N)");
            var confirmation = Console.ReadLine()?.ToLower();
            
            if (confirmation == "y" || confirmation == "yes")
            {
                try
                {
                    // Reset to defaults by creating a new default configuration
                    var defaultConfig = new AppConfiguration();
                    _configService.UpdateConfiguration(config => 
                    {
                        config.LeitnerBoxes = defaultConfig.LeitnerBoxes;
                        config.StudySession = defaultConfig.StudySession;
                        config.DailyLimits = defaultConfig.DailyLimits;
                        config.UI = defaultConfig.UI;
                        config.FilePaths = defaultConfig.FilePaths;
                        config.ReviewScheduling = defaultConfig.ReviewScheduling;
                    });
                    ShowSuccessMessage("Configuration has been reset to defaults!");
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Failed to reset configuration: {ex.Message}");
                }
            }
            else
            {
                ShowInfoMessage("Reset cancelled.");
            }
            
            ShowInputPrompt("Press Enter to continue or ESC to go back...");
            var input = GetUserChoice();
            return input != "ESC"; // Return false if ESC pressed (no pause needed)
        }

        // Helper methods for Configuration editing
        private void EditNumberOfBoxes()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine($"Current number of boxes: {config.LeitnerBoxes.NumberOfBoxes}");
            ShowInputPrompt("Enter new number of boxes (3-10)");
            
            var input = Console.ReadLine();
            if (int.TryParse(input, out int newCount) && newCount >= 3 && newCount <= 10)
            {
                _configService.UpdateConfiguration(cfg => 
                {
                    cfg.LeitnerBoxes.NumberOfBoxes = newCount;
                    // Regenerate rules for new box count
                    cfg.LeitnerBoxes.PromotionRules = GeneratePromotionRules(newCount);
                    cfg.LeitnerBoxes.DemotionRules = GenerateDemotionRules(newCount);
                });
                ShowSuccessMessage($"Number of boxes updated to {newCount}!");
            }
            else
            {
                ShowErrorMessage("Invalid input. Please enter a number between 3 and 10.");
            }
            
            ShowPressAnyKey();
        }

        private void EditPromotionRules()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine("PROMOTION RULES EDITOR");
            Console.WriteLine();
            
            for (int i = 0; i < config.LeitnerBoxes.PromotionRules.Count; i++)
            {
                var rule = config.LeitnerBoxes.PromotionRules[i];
                Console.WriteLine($"Box {i} → Box {i + 1}: Currently requires {rule.CorrectAnswersNeeded} correct answers");
                ShowInputPrompt($"Enter new number of correct answers needed (1-10) or press Enter to keep current");
                
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int newCount) && newCount >= 1 && newCount <= 10)
                {
                    _configService.UpdateConfiguration(cfg => 
                    {
                        cfg.LeitnerBoxes.PromotionRules[i].CorrectAnswersNeeded = newCount;
                    });
                    ShowSuccessMessage($"Box {i} promotion rule updated!");
                }
                else if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("   Keeping current value.");
                }
                else
                {
                    ShowErrorMessage("Invalid input. Keeping current value.");
                }
            }
            
            ShowPressAnyKey();
        }

        private void EditDemotionRules()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine("DEMOTION RULES EDITOR");
            Console.WriteLine();
            
            for (int i = 0; i < config.LeitnerBoxes.DemotionRules.Count; i++)
            {
                var rule = config.LeitnerBoxes.DemotionRules[i];
                Console.WriteLine($"Box {rule.BoxNumber}: Currently demotes to Box {rule.DemoteToBox} after {rule.IncorrectAnswersNeeded} incorrect answers");
                ShowInputPrompt($"Enter new number of incorrect answers needed (1-5) or press Enter to keep current");
                
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int newCount) && newCount >= 1 && newCount <= 5)
                {
                    _configService.UpdateConfiguration(cfg => 
                    {
                        cfg.LeitnerBoxes.DemotionRules[i].IncorrectAnswersNeeded = newCount;
                    });
                    ShowSuccessMessage($"Box {rule.BoxNumber} demotion rule updated!");
                }
                else if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("   Keeping current value.");
                }
                else
                {
                    ShowErrorMessage("Invalid input. Keeping current value.");
                }
            }
            
            ShowPressAnyKey();
        }

        private List<PromotionRule> GeneratePromotionRules(int boxCount)
        {
            var rules = new List<PromotionRule>();
            for (int i = 0; i < boxCount; i++)
            {
                rules.Add(new PromotionRule { BoxNumber = i, CorrectAnswersNeeded = Math.Min(i + 1, 5) });
            }
            return rules;
        }

        private List<DemotionRule> GenerateDemotionRules(int boxCount)
        {
            var rules = new List<DemotionRule>();
            for (int i = 1; i < boxCount; i++)
            {
                int demoteTo = Math.Max(0, i - 2);
                rules.Add(new DemotionRule { BoxNumber = i, IncorrectAnswersNeeded = 1, DemoteToBox = demoteTo });
            }
            return rules;
        }

        // Helper methods for Study Session Settings editing
        private void EditDefaultStudyMode()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine($"Current default study mode: {config.StudySession.DefaultStudyMode}");
            Console.WriteLine();
            Console.WriteLine("Available study modes:");
            Console.WriteLine("   1. FrontToBack - Show front side first");
            Console.WriteLine("   2. BackToFront - Show back side first");
            Console.WriteLine("   3. Mixed - Randomly choose which side to show first");
            Console.WriteLine();
            
            ShowInputPrompt("Enter new study mode (1-3) or press Enter to keep current");
            var input = Console.ReadLine();
            
            if (!string.IsNullOrWhiteSpace(input))
            {
                StudyMode newMode = input switch
                {
                    "1" => StudyMode.FrontToBack,
                    "2" => StudyMode.BackToFront,
                    "3" => StudyMode.Mixed,
                    _ => config.StudySession.DefaultStudyMode
                };
                
                if (newMode != config.StudySession.DefaultStudyMode)
                {
                    _configService.UpdateConfiguration(cfg => cfg.StudySession.DefaultStudyMode = newMode);
                    ShowSuccessMessage($"Default study mode updated to {newMode}!");
                }
                else
                {
                    ShowErrorMessage("Invalid input. Keeping current value.");
                }
            }
            else
            {
                Console.WriteLine("   Keeping current value.");
            }
            
            ShowPressAnyKey();
        }

        private void ToggleShowStatistics()
        {
            var config = _configService.GetConfiguration();
            var newValue = !config.StudySession.ShowStatistics;
            _configService.UpdateConfiguration(cfg => cfg.StudySession.ShowStatistics = newValue);
            ShowSuccessMessage($"Show Statistics set to {newValue}!");
            ShowPressAnyKey();
        }

        private void ToggleAutoAdvance()
        {
            var config = _configService.GetConfiguration();
            var newValue = !config.StudySession.AutoAdvance;
            _configService.UpdateConfiguration(cfg => cfg.StudySession.AutoAdvance = newValue);
            ShowSuccessMessage($"Auto Advance set to {newValue}!");
            ShowPressAnyKey();
        }

        private void ToggleShuffleCards()
        {
            var config = _configService.GetConfiguration();
            var newValue = !config.StudySession.ShuffleCards;
            _configService.UpdateConfiguration(cfg => cfg.StudySession.ShuffleCards = newValue);
            ShowSuccessMessage($"Shuffle Cards set to {newValue}!");
            ShowPressAnyKey();
        }

        private void ToggleShowProgress()
        {
            var config = _configService.GetConfiguration();
            var newValue = !config.StudySession.ShowProgress;
            _configService.UpdateConfiguration(cfg => cfg.StudySession.ShowProgress = newValue);
            ShowSuccessMessage($"Show Progress set to {newValue}!");
            ShowPressAnyKey();
        }

        private void EditAutoAdvanceDelay()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine($"Current auto advance delay: {config.StudySession.AutoAdvanceDelay} seconds");
            ShowInputPrompt("Enter new delay in seconds (1-30) or press Enter to keep current");
            
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int newDelay) && newDelay >= 1 && newDelay <= 30)
            {
                _configService.UpdateConfiguration(cfg => cfg.StudySession.AutoAdvanceDelay = newDelay);
                ShowSuccessMessage($"Auto advance delay updated to {newDelay} seconds!");
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("   Keeping current value.");
            }
            else
            {
                ShowErrorMessage("Invalid input. Please enter a number between 1 and 30.");
            }
            
            ShowPressAnyKey();
        }

        private void EditKeyboardShortcuts()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine("KEYBOARD SHORTCUTS EDITOR");
            Console.WriteLine();
            
            var shortcuts = config.StudySession.KeyboardShortcuts;
            
            // Edit each shortcut
            EditShortcut("Correct Answer", shortcuts.CorrectAnswer, (cfg, value) => cfg.StudySession.KeyboardShortcuts.CorrectAnswer = value);
            EditShortcut("Incorrect Answer", shortcuts.IncorrectAnswer, (cfg, value) => cfg.StudySession.KeyboardShortcuts.IncorrectAnswer = value);
            EditShortcut("Show Answer", shortcuts.ShowAnswer, (cfg, value) => cfg.StudySession.KeyboardShortcuts.ShowAnswer = value);
            EditShortcut("Skip Card", shortcuts.Skip, (cfg, value) => cfg.StudySession.KeyboardShortcuts.Skip = value);
            EditShortcut("Help", shortcuts.Help, (cfg, value) => cfg.StudySession.KeyboardShortcuts.Help = value);
            EditShortcut("Quit", shortcuts.Quit, (cfg, value) => cfg.StudySession.KeyboardShortcuts.Quit = value);
            
            ShowSuccessMessage("Keyboard shortcuts updated!");
            ShowPressAnyKey();
        }

        private void EditShortcut(string name, string currentValue, Action<AppConfiguration, string> updateAction)
        {
            Console.WriteLine($"{name}: Currently '{currentValue}'");
            ShowInputPrompt($"Enter new shortcut key or press Enter to keep current");
            
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                _configService.UpdateConfiguration(cfg => updateAction(cfg, input));
                Console.WriteLine($"   {name} updated to '{input}'");
            }
            else
            {
                Console.WriteLine($"   Keeping current value '{currentValue}'");
            }
        }

        // Helper methods for Daily Limits editing
        private void EditMaxCardsPerDay()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine($"Current max cards per day: {config.DailyLimits.MaxCardsPerDay}");
            ShowInputPrompt("Enter new max cards per day (10-1000) or press Enter to keep current");
            
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int newMax) && newMax >= 10 && newMax <= 1000)
            {
                _configService.UpdateConfiguration(cfg => cfg.DailyLimits.MaxCardsPerDay = newMax);
                ShowSuccessMessage($"Max cards per day updated to {newMax}!");
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("   Keeping current value.");
            }
            else
            {
                ShowErrorMessage("Invalid input. Please enter a number between 10 and 1000.");
            }
            
            ShowPressAnyKey();
        }

        private void EditMinCardsPerDay()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine($"Current min cards per day: {config.DailyLimits.MinCardsPerDay}");
            ShowInputPrompt("Enter new min cards per day (1-100) or press Enter to keep current");
            
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int newMin) && newMin >= 1 && newMin <= 100)
            {
                _configService.UpdateConfiguration(cfg => cfg.DailyLimits.MinCardsPerDay = newMin);
                ShowSuccessMessage($"Min cards per day updated to {newMin}!");
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("   Keeping current value.");
            }
            else
            {
                ShowErrorMessage("Invalid input. Please enter a number between 1 and 100.");
            }
            
            ShowPressAnyKey();
        }

        private void EditMaxStudyTime()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine($"Current max study time: {config.DailyLimits.MaxStudyTimePerDay}");
            Console.WriteLine("Enter new max study time in format HH:MM:SS (e.g., 02:30:00) or press Enter to keep current");
            ShowInputPrompt("Max study time");
            
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && TimeSpan.TryParse(input, out TimeSpan newTime) && newTime.TotalHours <= 24)
            {
                _configService.UpdateConfiguration(cfg => cfg.DailyLimits.MaxStudyTimePerDay = newTime);
                ShowSuccessMessage($"Max study time updated to {newTime}!");
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("   Keeping current value.");
            }
            else
            {
                ShowErrorMessage("Invalid input. Please enter time in HH:MM:SS format (max 24 hours).");
            }
            
            ShowPressAnyKey();
        }

        private void EditMinStudyTime()
        {
            var config = _configService.GetConfiguration();
            Console.WriteLine();
            Console.WriteLine($"Current min study time: {config.DailyLimits.MinStudyTimePerDay}");
            Console.WriteLine("Enter new min study time in format HH:MM:SS (e.g., 00:05:00) or press Enter to keep current");
            ShowInputPrompt("Min study time");
            
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input) && TimeSpan.TryParse(input, out TimeSpan newTime) && newTime.TotalHours <= 24)
            {
                _configService.UpdateConfiguration(cfg => cfg.DailyLimits.MinStudyTimePerDay = newTime);
                ShowSuccessMessage($"Min study time updated to {newTime}!");
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("   Keeping current value.");
            }
            else
            {
                ShowErrorMessage("nvalid input. Please enter time in HH:MM:SS format (max 24 hours).");
            }
            
            ShowPressAnyKey();
        }

        // Helper methods for UI Settings editing
        private void ToggleUseColors()
        {
            var config = _configService.GetConfiguration();
            var newValue = !config.UI.UseColors;
            _configService.UpdateConfiguration(cfg => cfg.UI.UseColors = newValue);
            ShowSuccessMessage($"Use Colors set to {newValue}!");
            ShowPressAnyKey();
        }

        private void ToggleUseIcons()
        {
            var config = _configService.GetConfiguration();
            var newValue = !config.UI.UseIcons;
            _configService.UpdateConfiguration(cfg => cfg.UI.UseIcons = newValue);
            ShowSuccessMessage($"Use Icons set to {newValue}!");
            ShowPressAnyKey();
        }

        private void ToggleShowWelcomeMessage()
        {
            var config = _configService.GetConfiguration();
            var newValue = !config.UI.ShowWelcomeMessage;
            _configService.UpdateConfiguration(cfg => cfg.UI.ShowWelcomeMessage = newValue);
            ShowSuccessMessage($"Show Welcome Message set to {newValue}!");
            ShowPressAnyKey();
        }

        private void ToggleClearScreenOnMenuChange()
        {
            var config = _configService.GetConfiguration();
            var newValue = !config.UI.ClearScreenOnMenuChange;
            _configService.UpdateConfiguration(cfg => cfg.UI.ClearScreenOnMenuChange = newValue);
            ShowSuccessMessage($"Clear Screen on Menu Change set to {newValue}!");
            ShowPressAnyKey();
        }

        private void ToggleShowDetailedStatistics()
        {
            var config = _configService.GetConfiguration();
            var newValue = !config.UI.ShowDetailedStatistics;
            _configService.UpdateConfiguration(cfg => cfg.UI.ShowDetailedStatistics = newValue);
            ShowSuccessMessage($"Show Detailed Statistics set to {newValue}!");
            ShowPressAnyKey();
        }

        // Helper methods for EditDeck functionality
        private void EditDeckName(Deck deck)
        {
            Console.WriteLine();
            Console.WriteLine($"Current name: {deck.Name}");
            ShowInputPrompt("Enter new deck name (or press Enter to keep current)");
            
            var newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                deck.Name = newName;
                if (_deckService.SaveDeck(deck))
                {
                    ShowSuccessMessage("Deck name updated successfully!");
                }
                else
                {
                    ShowErrorMessage("Failed to update deck name.");
                }
            }
        }

        private void EditDeckDescription(Deck deck)
        {
            Console.WriteLine();
            Console.WriteLine($"Current description: {deck.Description ?? "None"}");
            ShowInputPrompt("Enter new description (or press Enter to keep current)");
            
            var newDescription = Console.ReadLine();
            deck.Description = newDescription ?? "";
            
            if (_deckService.SaveDeck(deck))
            {
                ShowSuccessMessage("Deck description updated successfully!");
            }
            else
            {
                ShowErrorMessage("Failed to update deck description.");
            }
        }

        private void EditDeckTags(Deck deck)
        {
            Console.WriteLine();
            Console.WriteLine($"Current tags: {string.Join(", ", deck.Tags) ?? "None"}");
            ShowInputPrompt("Enter new tags (comma-separated, or press Enter to keep current)");
            
            var newTagsInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTagsInput))
            {
                deck.Tags = newTagsInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(t => t.Trim())
                                      .ToList();
            }
            
            if (_deckService.SaveDeck(deck))
            {
                ShowSuccessMessage("Deck tags updated successfully!");
            }
            else
            {
                ShowErrorMessage("Failed to update deck tags.");
            }
        }

        private void AddFlashcardToDeck(Deck deck)
        {
            Console.WriteLine();
            ShowSectionHeader("ADD FLASHCARD", ConsoleColor.Green);
            Console.WriteLine();
            
            ShowInputPrompt("Enter the front side of the card (question)");
            var front = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(front))
            {
                ShowErrorMessage("Front side cannot be empty.");
                return;
            }
            
            ShowInputPrompt("Enter the back side of the card (answer)");
            var back = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(back))
            {
                ShowErrorMessage("Back side cannot be empty.");
                return;
            }
            
            ShowInputPrompt("Enter tags (comma-separated, optional)");
            var tagsInput = Console.ReadLine() ?? "";
            var tags = tagsInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .ToList();
            
            var flashcard = new Flashcard 
            {
                Id = Guid.NewGuid().ToString(),
                Front = front,
                Back = back,
                Tags = tags,
                CreatedDate = DateTime.Now,
                CurrentBox = 0,
                Statistics = new FlashcardStatistics()
            };
            
            if (_deckService.AddFlashcardToDeck(deck, flashcard))
            {
                ShowSuccessMessage("Flashcard added successfully!");
            }
            else
            {
                ShowErrorMessage("Failed to add flashcard.");
            }
        }

        private void EditFlashcard(Deck deck)
        {
            if (!deck.Flashcards.Any())
            {
                ShowErrorMessage("No flashcards in this deck to edit.");
                return;
            }
            
            Console.WriteLine();
            Console.WriteLine("Select a flashcard to edit:");
            for (int i = 0; i < deck.Flashcards.Count; i++)
            {
                var card = deck.Flashcards[i];
                Console.WriteLine($"  {i + 1}. {card.Front} → {card.Back}");
            }
            Console.WriteLine();
            ShowInputPrompt("Enter the number of the flashcard to edit");
            
            var choice = Console.ReadLine();
            if (int.TryParse(choice, out int cardIndex) && cardIndex > 0 && cardIndex <= deck.Flashcards.Count)
            {
                var flashcard = deck.Flashcards[cardIndex - 1];
                EditFlashcardDetails(flashcard, deck);
            }
            else
            {
                ShowErrorMessage("Invalid flashcard selection.");
            }
        }

        private void EditFlashcardDetails(Flashcard flashcard, Deck deck)
        {
            Console.WriteLine();
            Console.WriteLine($"Current front: {flashcard.Front}");
            ShowInputPrompt("Enter new front side (or press Enter to keep current)");
            
            var newFront = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newFront))
            {
                flashcard.Front = newFront;
            }
            
            Console.WriteLine();
            Console.WriteLine($"Current back: {flashcard.Back}");
            ShowInputPrompt("Enter new back side (or press Enter to keep current)");
            
            var newBack = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newBack))
            {
                flashcard.Back = newBack;
            }
            
            Console.WriteLine();
            Console.WriteLine($"Current tags: {string.Join(", ", flashcard.Tags)}");
            ShowInputPrompt("Enter new tags (comma-separated, or press Enter to keep current)");
            
            var newTagsInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTagsInput))
            {
                flashcard.Tags = newTagsInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(t => t.Trim())
                                           .ToList();
            }
            
            // Update the deck's last modified time instead
            deck.LastModified = DateTime.Now;
            
            if (_deckService.SaveDeck(deck))
            {
                ShowSuccessMessage("Flashcard updated successfully!");
            }
            else
            {
                ShowErrorMessage("Failed to update flashcard.");
            }
        }

        private void DeleteFlashcard(Deck deck)
        {
            if (!deck.Flashcards.Any())
            {
                ShowErrorMessage("No flashcards in this deck to delete.");
                return;
            }
            
            Console.WriteLine();
            Console.WriteLine("Select a flashcard to delete:");
            for (int i = 0; i < deck.Flashcards.Count; i++)
            {
                var card = deck.Flashcards[i];
                Console.WriteLine($"  {i + 1}. {card.Front} → {card.Back}");
            }
            Console.WriteLine();
            ShowInputPrompt("Enter the number of the flashcard to delete");
            
            var choice = Console.ReadLine();
            if (int.TryParse(choice, out int cardIndex) && cardIndex > 0 && cardIndex <= deck.Flashcards.Count)
            {
                var flashcard = deck.Flashcards[cardIndex - 1];
                Console.WriteLine();
                Console.WriteLine($"Are you sure you want to delete this flashcard?");
                Console.WriteLine($"   Front: {flashcard.Front}");
                Console.WriteLine($"   Back: {flashcard.Back}");
                Console.WriteLine();
                ShowInputPrompt("Type 'DELETE' to confirm (or anything else to cancel)");
                
                var confirmation = Console.ReadLine();
                if (confirmation?.ToUpper() == "DELETE")
                {
                    if (_deckService.RemoveFlashcardFromDeck(deck, flashcard.Id))
                    {
                        ShowSuccessMessage("Flashcard deleted successfully!");
                    }
                    else
                    {
                        ShowErrorMessage("Failed to delete flashcard.");
                    }
                }
                else
                {
                    ShowInfoMessage("Deletion cancelled.");
                }
            }
            else
            {
                ShowErrorMessage("Invalid flashcard selection.");
            }
        }

        private void ResetStudyDates(Deck deck)
        {
            if (!deck.Flashcards.Any())
            {
                ShowErrorMessage("No flashcards in this deck to reset.");
                return;
            }
            
            Console.WriteLine();
            Console.WriteLine("🔄  RESET STUDY DATES");
            Console.WriteLine();
            Console.WriteLine("This will reset all study dates for cards in this deck to today.");
            Console.WriteLine("This means all cards will be available for study immediately.");
            Console.WriteLine();
            ShowInputPrompt("Type 'RESET' to confirm (or anything else to cancel)");
            
            var confirmation = Console.ReadLine();
            if (confirmation?.ToUpper() == "RESET")
            {
                var today = DateTime.Today;
                var yesterday = today.AddDays(-1); // Set to yesterday to ensure cards are due today
                var resetCount = 0;
                
                foreach (var flashcard in deck.Flashcards)
                {
                    flashcard.LastReviewed = yesterday;
                    flashcard.NextReviewDate = yesterday; // Set to yesterday so they're due today
                    flashcard.CurrentBox = 0; // Reset to box 0 for fresh start
                    resetCount++;
                }
                
                deck.LastModified = DateTime.Now;
                
                if (_deckService.SaveDeck(deck))
                {
                    ShowSuccessMessage($"Study dates reset for {resetCount} cards!");
                    Console.WriteLine("   All cards are now available for study today.");
                    Console.WriteLine($"   Reset date: {yesterday:yyyy-MM-dd}");
                    Console.WriteLine($"   Current date: {DateTime.Now:yyyy-MM-dd}");
                }
                else
                {
                    ShowErrorMessage("Failed to reset study dates.");
                }
            }
            else
            {
                ShowInfoMessage("Reset cancelled.");
            }
        }
    }
}