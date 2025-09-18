using System;
using FlashcardApp.UI.Abstractions;

namespace FlashcardApp.UI.Implementations.Console
{
    /// <summary>
    /// Console-specific implementation of IUIOutput
    /// </summary>
    public class ConsoleOutput : IUIOutput
    {
        public void WriteLine(string text)
        {
            System.Console.WriteLine(text);
        }

        public void Write(string text)
        {
            System.Console.Write(text);
        }

        public void Clear()
        {
            System.Console.Clear();
        }

        public void SetForegroundColor(UIColor color)
        {
            System.Console.ForegroundColor = ConvertToConsoleColor(color);
        }

        public void ResetColor()
        {
            System.Console.ResetColor();
        }

        public void SetTitle(string title)
        {
            System.Console.Title = title;
        }

        public void SetCursorVisible(bool visible)
        {
            System.Console.CursorVisible = visible;
        }

        private static System.ConsoleColor ConvertToConsoleColor(UIColor color)
        {
            return color switch
            {
                UIColor.Default => System.ConsoleColor.Gray,
                UIColor.Black => System.ConsoleColor.Black,
                UIColor.White => System.ConsoleColor.White,
                UIColor.Red => System.ConsoleColor.Red,
                UIColor.Green => System.ConsoleColor.Green,
                UIColor.Blue => System.ConsoleColor.Blue,
                UIColor.Yellow => System.ConsoleColor.Yellow,
                UIColor.Cyan => System.ConsoleColor.Cyan,
                UIColor.Magenta => System.ConsoleColor.Magenta,
                UIColor.DarkGray => System.ConsoleColor.DarkGray,
                UIColor.Gray => System.ConsoleColor.Gray,
                UIColor.DarkRed => System.ConsoleColor.DarkRed,
                UIColor.DarkGreen => System.ConsoleColor.DarkGreen,
                UIColor.DarkBlue => System.ConsoleColor.DarkBlue,
                UIColor.DarkYellow => System.ConsoleColor.DarkYellow,
                UIColor.DarkCyan => System.ConsoleColor.DarkCyan,
                UIColor.DarkMagenta => System.ConsoleColor.DarkMagenta,
                _ => System.ConsoleColor.Gray
            };
        }
    }
}
