using System;
using System.Collections.Generic;
using System.Linq;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.TestProfiles
{
    /// <summary>
    /// Test profile for continuous integration - comprehensive but fast
    /// </summary>
    public class ContinuousIntegrationTestProfile : TestProfileBase
    {
        public override string Name => "Continuous Integration";
        public override string Description => "Comprehensive tests for CI/CD pipeline";
        public override IEnumerable<string> Categories => new[] { TestCategories.Fast, TestCategories.Integration };
        public override IEnumerable<string> ExcludedCategories => new[] { TestCategories.Slow, TestCategories.Performance };
        public override bool IncludePerformanceTests => false;
        public override bool IncludeIntegrationTests => true;
        public override TimeSpan? Timeout => TimeSpan.FromMinutes(10);
    }
}
