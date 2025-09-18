using System.Diagnostics;
using Newtonsoft.Json;
using FlashcardApp.Models;

namespace FlashcardApp.Services
{
    public class StudySessionService
    {
        private readonly ConfigurationService _configService;
        private readonly LeitnerBoxService _leitnerBoxService;
        private readonly DeckService _deckService;
        private readonly string _sessionStatePath = "session_state.json";

        public StudySessionService(ConfigurationService configService, LeitnerBoxService leitnerBoxService, DeckService deckService)
        {
            _configService = configService;
            _leitnerBoxService = leitnerBoxService;
            _deckService = deckService;
        }

        public StudySessionResult StartStudySession(Deck deck, StudyMode studyMode, int maxCards = 50)
        {
            var config = _configService.GetConfiguration();
            var sessionStartTime = DateTime.Now;
            var stopwatch = Stopwatch.StartNew();

            // Check for existing session state
            var sessionState = LoadSessionState(deck.Id);
            var isResuming = sessionState != null && sessionState.IsActive;

            if (isResuming)
            {
                try { Console.Clear(); } catch { /* Ignore console errors in test environment */ }
                Console.WriteLine("Resuming previous study session...");
                Console.WriteLine($"Progress: {sessionState.StudiedCards.Count} cards studied, {sessionState.IncorrectCards.Count} incorrect cards remaining");
                Console.WriteLine("Press any key to continue or ESC to start fresh...");
                
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    sessionState = null; // Start fresh
                }
            }

            // Initialize or restore session state
            if (sessionState == null)
            {
                var cardsToStudy = _leitnerBoxService.GetCardsForStudySession(deck, studyMode, maxCards);
                
                if (!cardsToStudy.Any())
                {
                    return new StudySessionResult
                    {
                        Success = false,
                        Message = "No cards available for study at this time.",
                        SessionStatistics = new StudySessionStatistics()
                    };
                }

                sessionState = new SessionState
                {
                    DeckId = deck.Id,
                    StudyMode = studyMode,
                    CardsToStudy = cardsToStudy.Select(c => c.Id).ToList(),
                    CurrentCardIndex = 0,
                    StudiedCards = new List<string>(),
                    IncorrectCards = new List<string>(),
                    SessionStartTime = sessionStartTime,
                    LastSaveTime = sessionStartTime,
                    IsActive = true,
                    SessionStatistics = new SessionStatistics
                    {
                        TotalCards = cardsToStudy.Count,
                        SessionStartTime = sessionStartTime,
                        LastActivityTime = sessionStartTime
                    }
                };
            }

            var sessionStats = sessionState.SessionStatistics;
            var studiedCards = new List<Flashcard>();

            try { Console.Clear(); } catch { /* Ignore console errors in test environment */ }
            DisplaySessionHeader(deck, sessionState.CardsToStudy.Count, studyMode, isResuming);

            // Main study loop
            while (sessionState.CurrentCardIndex < sessionState.CardsToStudy.Count || sessionState.IncorrectCards.Any())
            {
                Flashcard? currentCard = null;
                int cardNumber = 0;
                int totalCards = 0;

                // Get next card to study
                if (sessionState.CurrentCardIndex < sessionState.CardsToStudy.Count)
                {
                    // Study new cards
                    var cardId = sessionState.CardsToStudy[sessionState.CurrentCardIndex];
                    currentCard = deck.Flashcards.FirstOrDefault(f => f.Id == cardId);
                    cardNumber = sessionState.CurrentCardIndex + 1;
                    totalCards = sessionState.CardsToStudy.Count;
                }
                else if (sessionState.IncorrectCards.Any())
                {
                    // Study incorrect cards
                    var cardId = sessionState.IncorrectCards[0];
                    currentCard = deck.Flashcards.FirstOrDefault(f => f.Id == cardId);
                    cardNumber = sessionState.StudiedCards.Count + 1;
                    totalCards = sessionState.CardsToStudy.Count + sessionState.IncorrectCards.Count;
                }

                if (currentCard == null)
                {
                    break;
                }

                var cardResult = StudyCard(currentCard, studyMode, cardNumber, totalCards);
                
                if (cardResult.WasStudied)
                {
                    studiedCards.Add(currentCard);
                    sessionStats.CardsStudied++;
                    sessionStats.LastActivityTime = DateTime.Now;
                    
                    if (cardResult.WasCorrect)
                    {
                        sessionStats.CorrectAnswers++;
                        _leitnerBoxService.ProcessCorrectAnswer(currentCard);
                        
                        // Remove from incorrect cards if it was there
                        sessionState.IncorrectCards.Remove(currentCard.Id);
                        
                        // Move to next card
                        if (sessionState.CurrentCardIndex < sessionState.CardsToStudy.Count)
                        {
                            sessionState.StudiedCards.Add(currentCard.Id);
                            sessionState.CurrentCardIndex++;
                        }
                    }
                    else
                    {
                        sessionStats.IncorrectAnswers++;
                        _leitnerBoxService.ProcessIncorrectAnswer(currentCard);
                        
                        // Add to incorrect cards if not already there
                        if (!sessionState.IncorrectCards.Contains(currentCard.Id))
                        {
                            sessionState.IncorrectCards.Add(currentCard.Id);
                        }
                        
                        // Move to next card only if we're studying new cards
                        if (sessionState.CurrentCardIndex < sessionState.CardsToStudy.Count)
                        {
                            sessionState.StudiedCards.Add(currentCard.Id);
                            sessionState.CurrentCardIndex++;
                        }
                        else
                        {
                            // Remove from incorrect cards list since we just studied it
                            sessionState.IncorrectCards.RemoveAt(0);
                        }
                    }

                    // Update flashcard statistics
                    _leitnerBoxService.UpdateFlashcardStatistics(currentCard, cardResult.ResponseTime, cardResult.WasCorrect);
                }

                if (cardResult.ShouldEditCard)
                {
                    // Edit the current card and continue the session
                    EditCurrentCard(currentCard, deck);
                    // Continue with the same card (don't advance)
                    continue;
                }

                if (cardResult.ShouldShowHelp)
                {
                    // Show help and continue the session
                    DisplayKeyboardShortcuts();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    // Continue with the same card (don't advance)
                    continue;
                }

                if (cardResult.ShouldQuit)
                {
                    // Save session state for resuming later
                    sessionState.IsActive = true;
                    sessionState.LastSaveTime = DateTime.Now;
                    SaveSessionState(sessionState);
                    
                    // Save deck with updated statistics
                    _deckService.SaveDeck(deck);
                    
                    stopwatch.Stop();
                    sessionStats.TotalStudyTime = stopwatch.Elapsed;
                    
                    return new StudySessionResult
                    {
                        Success = true,
                        Message = $"Session paused. Studied {studiedCards.Count} cards. You can resume later.",
                        SessionStatistics = ConvertToStudySessionStatistics(sessionStats),
                        StudiedCards = studiedCards
                    };
                }

                // Save progress periodically
                if (studiedCards.Count % 5 == 0)
                {
                    sessionState.LastSaveTime = DateTime.Now;
                    SaveSessionState(sessionState);
                    _deckService.SaveDeck(deck);
                }
            }

            // Session completed
            sessionState.IsActive = false;
            sessionState.LastSaveTime = DateTime.Now;
            SaveSessionState(sessionState);
            
            stopwatch.Stop();
            sessionStats.TotalStudyTime = stopwatch.Elapsed;

            // Save deck with updated statistics
            _deckService.SaveDeck(deck);

            return new StudySessionResult
            {
                Success = true,
                Message = $"Study session completed! Studied {studiedCards.Count} cards.",
                SessionStatistics = ConvertToStudySessionStatistics(sessionStats),
                StudiedCards = studiedCards
            };
        }

        private CardStudyResult StudyCard(Flashcard card, StudyMode studyMode, int cardNumber, int totalCards)
        {
            var config = _configService.GetConfiguration();
            var stopwatch = Stopwatch.StartNew();
            var result = new CardStudyResult();

            // Determine which side to show first
            var showFrontFirst = studyMode switch
            {
                StudyMode.FrontToBack => true,
                StudyMode.BackToFront => false,
                StudyMode.Mixed => new Random().Next(2) == 0,
                _ => true
            };

            // Display card
            DisplayCard(card, showFrontFirst, cardNumber, totalCards);
            
            // Wait for user to show answer (any key except quit keys)
            var keyInfo = WaitForAnyKeyExceptQuit();
            if (keyInfo.Key == ConsoleKey.Escape || 
                keyInfo.KeyChar.ToString().ToLower() == config.StudySession.KeyboardShortcuts.Quit.ToLower())
            {
                result.ShouldQuit = true;
                return result;
            }

            // Show answer
            DisplayAnswer(card, showFrontFirst);
            
            // Wait for user response
            keyInfo = WaitForKeyPress($"{config.StudySession.KeyboardShortcuts.CorrectAnswer}/{config.StudySession.KeyboardShortcuts.IncorrectAnswer}");
            
            stopwatch.Stop();
            result.ResponseTime = stopwatch.Elapsed;

            // Check for special keys first
            if (keyInfo.Key == ConsoleKey.Escape || 
                keyInfo.KeyChar.ToString().ToLower() == config.StudySession.KeyboardShortcuts.Quit.ToLower())
            {
                result.ShouldQuit = true;
            }
            else
            {
                switch (keyInfo.KeyChar.ToString())
                {
                    case var key when key == config.StudySession.KeyboardShortcuts.CorrectAnswer:
                        result.WasCorrect = true;
                        result.WasStudied = true;
                        DisplayFeedback("Correct!", ConsoleColor.Green);
                        break;
                    case var key when key == config.StudySession.KeyboardShortcuts.IncorrectAnswer:
                        result.WasCorrect = false;
                        result.WasStudied = true;
                        DisplayFeedback("Incorrect", ConsoleColor.Red);
                        break;
                    case var key when key.ToLower() == config.StudySession.KeyboardShortcuts.Skip.ToLower():
                        result.WasStudied = false;
                        DisplayFeedback("Skipped", ConsoleColor.Yellow);
                        break;
                    case var key when key.ToLower() == "e":
                        result.WasStudied = false;
                        result.ShouldEditCard = true;
                        DisplayFeedback("Editing card...", ConsoleColor.Cyan);
                        break;
                    case var key when key.ToLower() == config.StudySession.KeyboardShortcuts.Help.ToLower():
                        result.WasStudied = false;
                        result.ShouldShowHelp = true;
                        DisplayFeedback("Showing help...", ConsoleColor.Blue);
                        break;
                    default:
                        result.WasStudied = false;
                        DisplayFeedback("Skipped", ConsoleColor.Yellow);
                        break;
                }
            }

            // Statistics are tracked but not displayed during study session
            // to keep the interface clean and focused

            // Auto-advance if configured
            if (config.StudySession.AutoAdvance && !result.ShouldQuit)
            {
                Thread.Sleep(config.StudySession.AutoAdvanceDelay * 1000);
            }
            else if (!result.ShouldQuit)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

            return result;
        }

        private void DisplaySessionHeader(Deck deck, int cardCount, StudyMode studyMode, bool isResuming = false)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine("==================================================================");
            Console.WriteLine("|                        STUDY SESSION                        |");
            Console.WriteLine("==================================================================");
            
            Console.ResetColor();
            Console.WriteLine($"Deck: {deck.Name}");
            Console.WriteLine($"Cards to study: {cardCount}");
            Console.WriteLine($"Study mode: {studyMode}");
            Console.WriteLine($"Start time: {DateTime.Now:HH:mm:ss}");
            
            if (isResuming)
            {
                Console.WriteLine("Resuming previous session");
            }
            
            Console.WriteLine();
            
            DisplayKeyboardShortcuts();
            Console.WriteLine();
        }

        private void DisplayCard(Flashcard card, bool showFrontFirst, int cardNumber, int totalCards)
        {
            var config = _configService.GetConfiguration();
            
            try { Console.Clear(); } catch { /* Ignore console errors in test environment */ }
            
            // Beautiful card display with modern design
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            
            Console.WriteLine();
            Console.WriteLine("    FLASHCARD");
            Console.WriteLine();
            
            Console.ResetColor();
            Console.WriteLine();
            
            // Card info header
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine($"    Card {cardNumber} of {totalCards}");
            Console.ResetColor();
            Console.WriteLine();
            
            // Display the question/answer with beautiful formatting
            var content = showFrontFirst ? card.Front : card.Back;
            var side = showFrontFirst ? "QUESTION" : "ANSWER";
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.WriteLine($"    {side}:");
            Console.ResetColor();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            // Word wrap the text for better readability
            var wrappedText = WordWrap(content, 70);
            foreach (var line in wrappedText)
            {
                Console.WriteLine($"    {line}");
            }
            
            Console.ResetColor();
            Console.WriteLine();
            
            // Beautiful prompt for revealing answer
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            Console.WriteLine("    Press any key to reveal the answer...");
            
            Console.ResetColor();
        }

        private void DisplayAnswer(Flashcard card, bool showedFrontFirst)
        {
            var config = _configService.GetConfiguration();
            
            // Beautiful answer display
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            Console.WriteLine("    ANSWER REVEALED!");
            
            Console.ResetColor();
            Console.WriteLine();
            
            var answerSide = showedFrontFirst ? "ANSWER" : "QUESTION";
            var answerContent = showedFrontFirst ? card.Back : card.Front;
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.WriteLine($"    {answerSide}:");
            Console.ResetColor();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            // Word wrap the answer for better readability
            var wrappedText = WordWrap(answerContent, 70);
            foreach (var line in wrappedText)
            {
                Console.WriteLine($"    {line}");
            }
            
            Console.ResetColor();
            
            // Beautiful keyboard shortcuts display
            DisplayKeyboardShortcuts();
        }

        private void DisplayFeedback(string message, ConsoleColor color)
        {
            var config = _configService.GetConfiguration();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = color;
            }
            
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        private void DisplayCardStatistics(Flashcard card)
        {
            var config = _configService.GetConfiguration();
            
            if (!config.UI.ShowDetailedStatistics)
                return;
                
            Console.WriteLine("\n" + "-".PadRight(40, '-'));
            Console.WriteLine("Card Statistics:");
            Console.WriteLine($"  Success Rate: {card.Statistics.SuccessRate:F1}%");
            Console.WriteLine($"  Total Reviews: {card.Statistics.TotalReviews}");
            Console.WriteLine($"  Current Streak: {card.Statistics.Streak}");
            Console.WriteLine($"  Longest Streak: {card.Statistics.LongestStreak}");
            Console.WriteLine($"  Average Response Time: {card.Statistics.AverageResponseTime:F1}s");
        }

        private void EditCurrentCard(Flashcard flashcard, Deck deck)
        {
            var config = _configService.GetConfiguration();
            
            try { Console.Clear(); } catch { /* Ignore console errors in test environment */ }
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine();
            Console.WriteLine("       EDIT CURRENT CARD");
            Console.WriteLine();
            
            Console.ResetColor();
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
            
            // Update the deck's last modified time
            deck.LastModified = DateTime.Now;
            
            if (_deckService.SaveDeck(deck))
            {
                Console.WriteLine();
                if (config.UI.UseColors)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine("Card updated successfully!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine();
                if (config.UI.UseColors)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine("Failed to update card.");
                Console.ResetColor();
            }
            
            Console.WriteLine();
            ShowPressAnyKey();
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

        private void DisplayKeyboardShortcuts()
        {
            var config = _configService.GetConfiguration();
            var shortcuts = config.StudySession.KeyboardShortcuts;
            
            Console.WriteLine();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine("    KEYBOARD SHORTCUTS");
            
            Console.ResetColor();
            Console.WriteLine();
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            Console.WriteLine($"    {shortcuts.CorrectAnswer} - Mark as CORRECT (move to higher box)");
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            
            Console.WriteLine($"    {shortcuts.IncorrectAnswer} - Mark as INCORRECT (move to lower box)");
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            
            Console.WriteLine($"    {shortcuts.Skip} - SKIP card (no box change)");
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            
            Console.WriteLine($"    E - EDIT current card");
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            
            Console.WriteLine($"    {shortcuts.Quit} or ESC - QUIT session (saves progress)");
            
            if (config.UI.UseColors)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            
            Console.WriteLine($"    {shortcuts.Help} - Show HELP");
            
            Console.ResetColor();
        }

        private ConsoleKeyInfo WaitForKeyPress(string validKeys)
        {
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                var config = _configService.GetConfiguration();
                
                // Check for space key specifically
                if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    return keyInfo;
                }
                
                // Check for other character keys
                if (validKeys.Contains(keyInfo.KeyChar.ToString().ToLower()))
                {
                    return keyInfo;
                }
                
                // Check for special keys like Escape
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return keyInfo;
                }
                
                // Check for special study session keys
                var keyChar = keyInfo.KeyChar.ToString().ToLower();
                if (keyChar == config.StudySession.KeyboardShortcuts.Skip.ToLower() ||
                    keyChar == "e" || // Edit current card
                    keyChar == config.StudySession.KeyboardShortcuts.Help.ToLower() ||
                    keyChar == config.StudySession.KeyboardShortcuts.Quit.ToLower())
                {
                    return keyInfo;
                }
            }
        }

        private ConsoleKeyInfo WaitForAnyKeyExceptQuit()
        {
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                var config = _configService.GetConfiguration();
                
                // Use the testable validation logic
                if (IsValidKeyForAnswerReveal(keyInfo, config.StudySession.KeyboardShortcuts.Quit))
                {
                    return keyInfo;
                }
                
                // If it's a quit key, return it so the caller can handle it
                return keyInfo;
            }
        }

        private bool IsValidKeyForAnswerReveal(ConsoleKeyInfo keyInfo, string quitKey)
        {
            // Check for escape key
            if (keyInfo.Key == ConsoleKey.Escape)
                return false;

            // Check for quit character key
            if (keyInfo.KeyChar.ToString().ToLower() == quitKey.ToLower())
                return false;

            // Any other key is valid
            return true;
        }

        private List<string> WordWrap(string text, int maxWidth)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string> { "" };

            var words = text.Split(' ');
            var lines = new List<string>();
            var currentLine = "";

            foreach (var word in words)
            {
                if ((currentLine + word).Length <= maxWidth)
                {
                    currentLine += (currentLine == "" ? "" : " ") + word;
                }
                else
                {
                    if (currentLine != "")
                    {
                        lines.Add(currentLine);
                        currentLine = word;
                    }
                    else
                    {
                        // Word is longer than maxWidth, add it anyway
                        lines.Add(word);
                    }
                }
            }

            if (currentLine != "")
            {
                lines.Add(currentLine);
            }

            return lines;
        }

        private string GetIcon(string type)
        {
            var config = _configService.GetConfiguration();
            if (!config.UI.UseIcons)
                return "";

            return type switch
            {
                "correct" => "[CORRECT]",
                "incorrect" => "[INCORRECT]",
                "skip" => "[SKIP]",
                "quit" => "[QUIT]",
                "space" => "[SPACE]",
                "help" => "[HELP]",
                _ => ""
            };
        }

        private SessionState? LoadSessionState(string deckId)
        {
            try
            {
                if (File.Exists(_sessionStatePath))
                {
                    var json = File.ReadAllText(_sessionStatePath);
                    var sessionState = JsonConvert.DeserializeObject<SessionState>(json);
                    
                    // Only return if it's for the same deck
                    if (sessionState?.DeckId == deckId)
                    {
                        return sessionState;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading session state: {ex.Message}");
            }
            
            return null;
        }

        private void SaveSessionState(SessionState sessionState)
        {
            try
            {
                var json = JsonConvert.SerializeObject(sessionState, Formatting.Indented);
                File.WriteAllText(_sessionStatePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving session state: {ex.Message}");
            }
        }

        public void ClearSessionState(string deckId)
        {
            try
            {
                if (File.Exists(_sessionStatePath))
                {
                    var json = File.ReadAllText(_sessionStatePath);
                    var sessionState = JsonConvert.DeserializeObject<SessionState>(json);
                    
                    // Only clear if it's for the same deck
                    if (sessionState?.DeckId == deckId)
                    {
                        File.Delete(_sessionStatePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing session state: {ex.Message}");
            }
        }

        private StudySessionStatistics ConvertToStudySessionStatistics(SessionStatistics sessionStats)
        {
            return new StudySessionStatistics
            {
                TotalCards = sessionStats.TotalCards,
                TotalReviews = sessionStats.CardsStudied,
                CorrectAnswers = sessionStats.CorrectAnswers,
                IncorrectAnswers = sessionStats.IncorrectAnswers,
                SuccessRate = sessionStats.SuccessRate,
                SessionTime = sessionStats.TotalStudyTime,
                TotalStudyTime = sessionStats.TotalStudyTime,
                AverageResponseTime = sessionStats.CardsStudied > 0 ? sessionStats.TotalStudyTime.TotalSeconds / sessionStats.CardsStudied : 0,
                StartTime = sessionStats.SessionStartTime,
                EndTime = DateTime.Now
            };
        }
    }

    public class StudySessionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public StudySessionStatistics SessionStatistics { get; set; } = new StudySessionStatistics();
        public List<Flashcard> StudiedCards { get; set; } = new List<Flashcard>();
    }

    public class CardStudyResult
    {
        public bool WasStudied { get; set; }
        public bool WasCorrect { get; set; }
        public bool ShouldQuit { get; set; }
        public bool ShouldEditCard { get; set; }
        public bool ShouldShowHelp { get; set; }
        public TimeSpan ResponseTime { get; set; }
    }
}
