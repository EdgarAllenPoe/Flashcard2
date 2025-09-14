using FlashcardApp.Services;
using FlashcardApp.UI;

namespace FlashcardApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Check if we should run tests
                if (args.Length > 0 && args[0].ToLower() == "test")
                {
                    KeyTest.TestKeyValidation();
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadKey();
                    return;
                }

                // Initialize services
                var configService = new ConfigurationService();
                var deckService = new DeckService(configService);
                var leitnerBoxService = new LeitnerBoxService(configService);
                var studySessionService = new StudySessionService(configService, leitnerBoxService, deckService);

                // Ensure directories exist
                configService.EnsureDirectoriesExist();

                // Initialize and run the UI
                var ui = new ConsoleUI(configService, deckService, studySessionService, leitnerBoxService);
                ui.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
