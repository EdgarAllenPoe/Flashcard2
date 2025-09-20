using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlashcardApp.Tests.TestProfiles
{
    /// <summary>
    /// Manages test profiles and provides easy access to different test scenarios
    /// </summary>
    public static class TestProfileManager
    {
        private static readonly Dictionary<string, TestProfileBase> _profiles = new();

        static TestProfileManager()
        {
            LoadProfiles();
        }

        private static void LoadProfiles()
        {
            var profileTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(TestProfileBase).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var type in profileTypes)
            {
                var profile = (TestProfileBase)Activator.CreateInstance(type)!;
                _profiles[profile.Name] = profile;
            }
        }

        /// <summary>
        /// Gets all available test profiles
        /// </summary>
        public static IEnumerable<TestProfileBase> GetAllProfiles() => _profiles.Values;

        /// <summary>
        /// Gets a test profile by name
        /// </summary>
        public static TestProfileBase? GetProfile(string name)
        {
            _profiles.TryGetValue(name, out var profile);
            return profile;
        }

        /// <summary>
        /// Gets the development test profile
        /// </summary>
        public static TestProfileBase Development => GetProfile("Development")!;

        /// <summary>
        /// Gets the continuous integration test profile
        /// </summary>
        public static TestProfileBase ContinuousIntegration => GetProfile("Continuous Integration")!;

        /// <summary>
        /// Gets the full test profile
        /// </summary>
        public static TestProfileBase Full => GetProfile("Full Validation")!;

        /// <summary>
        /// Gets the performance test profile
        /// </summary>
        public static TestProfileBase Performance => GetProfile("Performance Testing")!;

        /// <summary>
        /// Prints all available profiles to console
        /// </summary>
        public static void PrintProfiles()
        {
            Console.WriteLine("Available Test Profiles:");
            Console.WriteLine("=======================");

            foreach (var profile in _profiles.Values)
            {
                Console.WriteLine($"Name: {profile.Name}");
                Console.WriteLine($"Description: {profile.Description}");
                Console.WriteLine($"Categories: {string.Join(", ", profile.Categories)}");
                Console.WriteLine($"Excluded: {string.Join(", ", profile.ExcludedCategories)}");
                Console.WriteLine($"Filter: {profile.GetFilterExpression()}");
                Console.WriteLine($"Timeout: {profile.Timeout?.TotalMinutes} minutes");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Gets the command line for running tests with a specific profile
        /// </summary>
        public static string GetCommandLine(string profileName)
        {
            var profile = GetProfile(profileName);
            if (profile == null)
            {
                throw new ArgumentException($"Profile '{profileName}' not found");
            }

            var args = profile.GetCommandLineArguments();
            return $"dotnet test {string.Join(" ", args)}";
        }
    }
}
