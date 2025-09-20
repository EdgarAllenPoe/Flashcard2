using FlashcardApp.Services;
using FlashcardApp.UI;
using FlashcardApp.UI.Abstractions;
using FlashcardApp.UI.Implementations.Console;

namespace FlashcardApp
{
    /// <summary>
    /// Alternative Program entry point that demonstrates the new UI abstraction architecture
    /// This shows how the application can be wired up with the new abstraction layer
    /// </summary>
    class ProgramNew
    {
        static async Task Main(string[] args)
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

                // Initialize UI abstractions
                var output = new ConsoleOutput();
                var input = new ConsoleInput();
                var theme = new ConsoleTheme();
                var renderer = new ConsoleRenderer(output, theme);
                var userInteraction = new ConsoleUserInteractionService(input, output, theme, renderer);

                // Initialize application service
                var applicationService = new ConsoleApplicationService(
                    configService, deckService, studySessionService, leitnerBoxService);

                // For now, use the bridge to maintain compatibility
                // In the future, this could be replaced with a fully refactored UI
                var ui = new ConsoleUIBridge(configService, deckService, studySessionService, leitnerBoxService);
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
