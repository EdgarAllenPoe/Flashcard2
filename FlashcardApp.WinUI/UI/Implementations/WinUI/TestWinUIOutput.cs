using FlashcardApp.UI.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// Test-friendly WinUI implementation of IUIOutput for unit testing
    /// This version can be instantiated without WinUI dependencies
    /// </summary>
    public class TestWinUIOutput : IUIOutput
    {
        public List<string> OutputLines { get; } = new List<string>();
        public List<string> ErrorLines { get; } = new List<string>();
        public UIColor CurrentColor { get; private set; } = UIColor.Default;
        public string? CurrentTitle { get; private set; }
        public bool CursorVisible { get; private set; } = true;

        public void WriteLine(string text)
        {
            OutputLines.Add(text);
        }

        public void Write(string text)
        {
            OutputLines.Add(text);
        }

        public void Clear()
        {
            OutputLines.Clear();
            ErrorLines.Clear();
        }

        public void SetForegroundColor(UIColor color)
        {
            CurrentColor = color;
        }

        public void ResetColor()
        {
            CurrentColor = UIColor.Default;
        }

        public void SetTitle(string title)
        {
            CurrentTitle = title;
        }

        public void SetCursorVisible(bool visible)
        {
            CursorVisible = visible;
        }
    }
}
