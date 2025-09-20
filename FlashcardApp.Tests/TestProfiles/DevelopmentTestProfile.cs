using System;
using System.Collections.Generic;
using System.Linq;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.TestProfiles
{
    /// <summary>
    /// Test profile for development - fast tests only
    /// </summary>
    public class DevelopmentTestProfile : TestProfileBase
    {
        public override string Name => "Development";
        public override string Description => "Fast tests for development workflow";
        public override IEnumerable<string> Categories => new[] { TestCategories.Fast };
        public override IEnumerable<string> ExcludedCategories => new[] { TestCategories.Slow, TestCategories.Integration, TestCategories.Performance };
        public override bool IncludePerformanceTests => false;
        public override bool IncludeIntegrationTests => false;
        public override TimeSpan? Timeout => TimeSpan.FromMinutes(2);
    }
}
