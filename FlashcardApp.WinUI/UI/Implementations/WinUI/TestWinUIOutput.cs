using FlashcardApp.UI.Abstractions;
using System;
using System.Collections.Generic;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// Test-friendly WinUI implementation of IUIOutput for unit testing
    /// This version can be instantiated without UI controls
    /// </summary>
    public class TestWinUIOutput : IUIOutput
    {
        private readonly List<string> _output = new List<string>();
        private UIColor _currentForegroundColor = UIColor.White;
        private string _currentTitle = "";
        private bool _cursorVisible = true;

        public TestWinUIOutput()
        {
            // Parameterless constructor for testing
        }

        public IReadOnlyList<string> Output => _output.AsReadOnly();
        public UIColor CurrentForegroundColor => _currentForegroundColor;
        public string CurrentTitle => _currentTitle;
        public bool CursorVisible => _cursorVisible;

        public void Write(string value)
        {
            if (value != null)
            {
                _output.Add(value);
            }
        }

        public void WriteLine(string value)
        {
            Write(value + Environment.NewLine);
        }

        public void Clear()
        {
            _output.Clear();
        }

        public void SetForegroundColor(UIColor color)
        {
            _currentForegroundColor = color;
        }

        public void ResetColor()
        {
            _currentForegroundColor = UIColor.White;
        }

        public void SetTitle(string title)
        {
            _currentTitle = title ?? "";
        }

        public void SetCursorVisible(bool visible)
        {
            _cursorVisible = visible;
        }

        public string GetOutputAsString()
        {
            return string.Join("", _output);
        }
    }
}
