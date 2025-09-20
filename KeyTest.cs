using System;

namespace FlashcardApp
{
    public class KeyTest
    {
        public static void TestKeyValidation()
        {
            Console.WriteLine("Testing Key Validation Logic");
            Console.WriteLine("============================");

            // Test cases for key validation
            var testCases = new[]
            {
                new { Key = ConsoleKey.Spacebar, KeyChar = ' ', QuitKey = "q", Expected = true, Description = "Space bar should be valid" },
                new { Key = ConsoleKey.A, KeyChar = 'a', QuitKey = "q", Expected = true, Description = "Letter A should be valid" },
                new { Key = ConsoleKey.D1, KeyChar = '1', QuitKey = "q", Expected = true, Description = "Number 1 should be valid" },
                new { Key = ConsoleKey.Enter, KeyChar = '\r', QuitKey = "q", Expected = true, Description = "Enter key should be valid" },
                new { Key = ConsoleKey.Escape, KeyChar = '\0', QuitKey = "q", Expected = false, Description = "Escape key should be invalid" },
                new { Key = ConsoleKey.Q, KeyChar = 'q', QuitKey = "q", Expected = false, Description = "Quit key 'q' should be invalid" },
                new { Key = ConsoleKey.Q, KeyChar = 'Q', QuitKey = "q", Expected = false, Description = "Quit key 'Q' should be invalid" },
            };

            int passed = 0;
            int failed = 0;

            foreach (var testCase in testCases)
            {
                var keyInfo = new ConsoleKeyInfo(testCase.KeyChar, testCase.Key, false, false, false);
                var result = IsValidKeyForAnswerReveal(keyInfo, testCase.QuitKey);

                if (result == testCase.Expected)
                {
                    Console.WriteLine($"PASS: {testCase.Description}");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"FAIL: {testCase.Description} - Expected {testCase.Expected}, got {result}");
                    failed++;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Results: {passed} passed, {failed} failed");

            if (failed == 0)
            {
                Console.WriteLine("All tests passed! The key validation logic is working correctly.");
            }
            else
            {
                Console.WriteLine("Some tests failed. The key validation logic needs fixing.");
            }
        }

        public static bool IsValidKeyForAnswerReveal(ConsoleKeyInfo keyInfo, string quitKey)
        {
            if (quitKey == null)
                throw new ArgumentNullException(nameof(quitKey));

            // Check for escape key
            if (keyInfo.Key == ConsoleKey.Escape)
                return false;

            // Check for quit character key
            if (keyInfo.KeyChar.ToString().ToLower() == quitKey.ToLower())
                return false;

            // Any other key is valid
            return true;
        }
    }
}

