namespace FlashcardApp.Tests
{
    /// <summary>
    /// Test categories for performance optimization and selective test execution
    /// </summary>
    public static class TestCategories
    {
        /// <summary>
        /// Fast unit tests that run in &lt; 10ms each
        /// </summary>
        public const string Fast = "Fast";

        /// <summary>
        /// Slower tests that take 10-100ms each (file I/O, console operations)
        /// </summary>
        public const string Slow = "Slow";

        /// <summary>
        /// Integration tests that test multiple components together
        /// </summary>
        public const string Integration = "Integration";

        /// <summary>
        /// End-to-end tests that test complete workflows
        /// </summary>
        public const string E2E = "E2E";

        /// <summary>
        /// Performance benchmark tests
        /// </summary>
        public const string Performance = "Performance";
    }
}
