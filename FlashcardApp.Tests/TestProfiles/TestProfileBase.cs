using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.Tests.TestProfiles
{
    /// <summary>
    /// Base class for test profiles that define different test execution scenarios
    /// </summary>
    public abstract class TestProfileBase
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract IEnumerable<string> Categories { get; }
        public abstract IEnumerable<string> ExcludedCategories { get; }
        public abstract bool IncludePerformanceTests { get; }
        public abstract bool IncludeIntegrationTests { get; }
        public abstract TimeSpan? Timeout { get; }

        /// <summary>
        /// Gets the filter expression for this test profile
        /// </summary>
        public string GetFilterExpression()
        {
            var filters = new List<string>();

            // Include categories
            if (Categories.Any())
            {
                var categoryFilter = string.Join("|", Categories.Select(c => $"Category={c}"));
                filters.Add($"({categoryFilter})");
            }

            // Exclude categories
            if (ExcludedCategories.Any())
            {
                var excludeFilter = string.Join("&", ExcludedCategories.Select(c => $"Category!={c}"));
                filters.Add($"({excludeFilter})");
            }

            return filters.Any() ? string.Join("&", filters) : "";
        }

        /// <summary>
        /// Gets the command line arguments for this test profile
        /// </summary>
        public IEnumerable<string> GetCommandLineArguments()
        {
            var args = new List<string>();

            var filter = GetFilterExpression();
            if (!string.IsNullOrEmpty(filter))
            {
                args.Add("--filter");
                args.Add($"\"{filter}\"");
            }

            if (Timeout.HasValue)
            {
                args.Add("--timeout");
                args.Add(Timeout.Value.TotalMilliseconds.ToString("F0"));
            }

            return args;
        }
    }
}
