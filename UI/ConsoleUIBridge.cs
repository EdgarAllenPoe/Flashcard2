using FlashcardApp.Models;
using FlashcardApp.Services;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.UI.Implementations.Console;

namespace FlashcardApp.UI
{
    /// <summary>
    /// Bridge class that adapts the existing ConsoleUI to use the new abstraction layer
    /// This maintains the same interface while using the new architecture internally
    /// </summary>
    public class ConsoleUIBridge
    {
        private readonly ConsoleUI _originalUI;

        public ConsoleUIBridge(ConfigurationService configService, DeckService deckService,
                             StudySessionService studySessionService, LeitnerBoxService leitnerBoxService)
        {
            _originalUI = new ConsoleUI(configService, deckService, studySessionService, leitnerBoxService);
        }

        public void Run()
        {
            _originalUI.Run();
        }
    }
}
