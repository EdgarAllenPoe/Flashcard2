using System;
using System.Collections.Concurrent;
using System.IO;

namespace FlashcardApp.Tests
{
    /// <summary>
    /// Provides cached access to test data files to improve test performance
    /// </summary>
    public static class TestDataProvider
    {
        private static readonly ConcurrentDictionary<string, string> _fileCache = new();
        private static readonly string _projectRoot = Path.Combine("..", "..", "..", "..");

        /// <summary>
        /// Gets the content of a file with caching for performance
        /// </summary>
        /// <param name="relativePath">Path relative to project root</param>
        /// <returns>Cached file content</returns>
        public static string GetFileContent(string relativePath)
        {
            return _fileCache.GetOrAdd(relativePath, path =>
            {
                var fullPath = Path.Combine(_projectRoot, path);
                return File.Exists(fullPath) ? File.ReadAllText(fullPath) : string.Empty;
            });
        }

        /// <summary>
        /// Gets XAML content for UI tests
        /// </summary>
        public static class Xaml
        {
            public static string MainPage => GetFileContent("Views/MainPage.xaml");
            public static string MainPageCodeBehind => GetFileContent("Views/MainPage.xaml.cs");
            public static string StudySessionPage => GetFileContent("Views/StudySessionPage.xaml");
            public static string StudySessionPageCodeBehind => GetFileContent("Views/StudySessionPage.xaml.cs");
            public static string DeckManagementPage => GetFileContent("Views/DeckManagementPage.xaml");
            public static string DeckManagementPageCodeBehind => GetFileContent("Views/DeckManagementPage.xaml.cs");
            public static string StatisticsPage => GetFileContent("Views/StatisticsPage.xaml");
            public static string StatisticsPageCodeBehind => GetFileContent("Views/StatisticsPage.xaml.cs");
            public static string ConfigurationPage => GetFileContent("Views/ConfigurationPage.xaml");
            public static string ConfigurationPageCodeBehind => GetFileContent("Views/ConfigurationPage.xaml.cs");
            public static string AppXaml => GetFileContent("App.xaml");
        }

        /// <summary>
        /// Gets project file content
        /// </summary>
        public static class Project
        {
            public static string WinUIProject => GetFileContent("FlashcardApp.WinUI.csproj");
            public static string MainProject => GetFileContent("FlashcardApp.csproj");
            public static string TestProject => GetFileContent("FlashcardApp.Tests/FlashcardApp.Tests.csproj");
        }

        /// <summary>
        /// Clears the file cache (useful for testing)
        /// </summary>
        public static void ClearCache()
        {
            _fileCache.Clear();
        }

        /// <summary>
        /// Gets cache statistics for monitoring
        /// </summary>
        public static (int Count, long TotalSize) GetCacheStats()
        {
            var count = _fileCache.Count;
            var totalSize = 0L;
            foreach (var content in _fileCache.Values)
            {
                totalSize += content.Length * sizeof(char);
            }
            return (count, totalSize);
        }
    }
}
