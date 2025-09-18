using FlashcardApp.UI.Abstractions;
using System;
using System.Threading.Tasks;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// Test-friendly WinUI implementation of IUIInput for unit testing
    /// This version can be instantiated without UI controls
    /// </summary>
    public class TestWinUIInput : IUIInput
    {
        private string _nextLineInput = "";
        private UIKeyInfo _nextKeyInput = new UIKeyInfo(UIKey.Enter, '\r', true);
        private string _nextImmediateChoice = "1";
        private string _nextUserChoice = "1";

        public TestWinUIInput()
        {
            // Parameterless constructor for testing
        }

        public void SetNextLineInput(string input)
        {
            _nextLineInput = input;
        }

        public void SetNextKeyInput(UIKeyInfo keyInfo)
        {
            _nextKeyInput = keyInfo;
        }

        public void SetNextImmediateChoice(string choice)
        {
            _nextImmediateChoice = choice;
        }

        public void SetNextUserChoice(string choice)
        {
            _nextUserChoice = choice;
        }

        public Task<string> ReadLineAsync()
        {
            return Task.FromResult(_nextLineInput);
        }

        public Task<UIKeyInfo> ReadKeyAsync(bool intercept = false)
        {
            return Task.FromResult(_nextKeyInput);
        }

        public Task<string> GetImmediateChoiceAsync()
        {
            return Task.FromResult(_nextImmediateChoice);
        }

        public Task<string> GetUserChoiceAsync()
        {
            return Task.FromResult(_nextUserChoice);
        }

        public Task WaitForAnyKeyAsync()
        {
            return Task.CompletedTask;
        }
    }
}
