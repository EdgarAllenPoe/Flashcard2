using System;
using System.Collections.Generic;
using System.Linq;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.TestProfiles
{
    /// <summary>
    /// Test profile for performance testing - benchmarks only
    /// </summary>
    public class PerformanceTestProfile : TestProfileBase
    {
        public override string Name => "Performance Testing";
        public override string Description => "Performance benchmarks and critical path tests";
        public override IEnumerable<string> Categories => new[] { TestCategories.Performance };
        public override IEnumerable<string> ExcludedCategories => new[] { TestCategories.Fast, TestCategories.Slow, TestCategories.Integration };
        public override bool IncludePerformanceTests => true;
        public override bool IncludeIntegrationTests => false;
        public override TimeSpan? Timeout => TimeSpan.FromMinutes(15);
    }
}
