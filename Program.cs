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
                // Configure console for Unicode/emoji support
                ConfigureConsoleForUnicode();
                
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

        private static void ConfigureConsoleForUnicode()
        {
            try
            {
                // Set console output encoding to UTF-8
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.InputEncoding = System.Text.Encoding.UTF8;
                
                // For Windows, also try to set the console code page
                if (OperatingSystem.IsWindows())
                {
                    // Set console code page to UTF-8 (65001)
                    var handle = GetStdHandle(-11); // STD_OUTPUT_HANDLE
                    if (handle != IntPtr.Zero)
                    {
                        SetConsoleOutputCP(65001); // UTF-8 code page
                    }
                }
            }
            catch
            {
                // If encoding setup fails, continue without it
                // The app will still work, just emojis might not display properly
            }
        }

        // Windows API imports for console configuration
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);
    }
}
