using System;
using System.Collections.Generic;
using System.Linq;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.TestProfiles
{
    /// <summary>
    /// Test profile for full validation - all tests including performance
    /// </summary>
    public class FullTestProfile : TestProfileBase
    {
        public override string Name => "Full Validation";
        public override string Description => "All tests including performance benchmarks";
        public override IEnumerable<string> Categories => new[] { TestCategories.Fast, TestCategories.Slow, TestCategories.Integration, TestCategories.Performance };
        public override IEnumerable<string> ExcludedCategories => Array.Empty<string>();
        public override bool IncludePerformanceTests => true;
        public override bool IncludeIntegrationTests => true;
        public override TimeSpan? Timeout => TimeSpan.FromMinutes(30);
    }
}
