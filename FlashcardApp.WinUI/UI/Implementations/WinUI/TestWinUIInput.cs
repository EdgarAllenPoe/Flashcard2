using FlashcardApp.UI.Abstractions;
using System;
using System.Threading.Tasks;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// Test-friendly WinUI implementation of IUIInput for unit testing
    /// This version can be instantiated without WinUI dependencies
    /// </summary>
    public class TestWinUIInput : IUIInput
    {
        public string? LastInput { get; private set; }
        public bool IsInputAvailable => true;

        public Task<string> ReadLineAsync()
        {
            // For testing, return a default input
            LastInput = "test input";
            return Task.FromResult("test input");
        }

        public Task<UIKeyInfo> ReadKeyAsync(bool intercept = false)
        {
            // For testing, return a default key
            return Task.FromResult(new UIKeyInfo(UIKey.D1, '1', true));
        }

        public Task<string> GetImmediateChoiceAsync()
        {
            // For testing, return a default choice
            return Task.FromResult("1");
        }

        public Task<string> GetUserChoiceAsync()
        {
            // For testing, return a default choice
            return Task.FromResult("1");
        }

        public Task WaitForAnyKeyAsync()
        {
            // For testing, just complete immediately
            return Task.CompletedTask;
        }
    }
}
