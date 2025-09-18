using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using FlashcardApp.UI.Abstractions;
using System;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// WinUI 3 implementation of IUIOutput for Windows Native applications
    /// </summary>
    public class WinUIOutput : IUIOutput
    {
        private readonly TextBlock _outputTextBlock;
        private readonly ScrollViewer _scrollViewer;
        private readonly Window _mainWindow;

        public WinUIOutput(TextBlock outputTextBlock, ScrollViewer scrollViewer, Window mainWindow)
        {
            _outputTextBlock = outputTextBlock ?? throw new ArgumentNullException(nameof(outputTextBlock));
            _scrollViewer = scrollViewer ?? throw new ArgumentNullException(nameof(scrollViewer));
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        }

        public void Write(string value)
        {
            if (value != null)
            {
                _outputTextBlock.Text += value;
                _outputTextBlock.DispatcherQueue.TryEnqueue(() =>
                {
                    _scrollViewer.ChangeView(null, _scrollViewer.ScrollableHeight, null);
                });
            }
        }

        public void WriteLine(string value)
        {
            Write(value + Environment.NewLine);
        }

        public void Clear()
        {
            _outputTextBlock.Text = string.Empty;
        }

        public void SetForegroundColor(UIColor color)
        {
            _outputTextBlock.Foreground = MapUIColorToBrush(color);
        }

        public void ResetColor()
        {
            _outputTextBlock.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
        }

        public void SetTitle(string title)
        {
            _mainWindow.Title = title;
        }

        public void SetCursorVisible(bool visible)
        {
            // In WinUI, cursor visibility is handled by the input controls
            // This method is kept for interface compatibility
        }

        private Brush MapUIColorToBrush(UIColor color)
        {
            return color switch
            {
                UIColor.Black => new SolidColorBrush(Microsoft.UI.Colors.Black),
                UIColor.DarkBlue => new SolidColorBrush(Microsoft.UI.Colors.DarkBlue),
                UIColor.DarkGreen => new SolidColorBrush(Microsoft.UI.Colors.DarkGreen),
                UIColor.DarkCyan => new SolidColorBrush(Microsoft.UI.Colors.DarkCyan),
                UIColor.DarkRed => new SolidColorBrush(Microsoft.UI.Colors.DarkRed),
                UIColor.DarkMagenta => new SolidColorBrush(Microsoft.UI.Colors.DarkMagenta),
                UIColor.DarkYellow => new SolidColorBrush(Microsoft.UI.Colors.Orange),
                UIColor.Gray => new SolidColorBrush(Microsoft.UI.Colors.Gray),
                UIColor.DarkGray => new SolidColorBrush(Microsoft.UI.Colors.DarkGray),
                UIColor.Blue => new SolidColorBrush(Microsoft.UI.Colors.Blue),
                UIColor.Green => new SolidColorBrush(Microsoft.UI.Colors.Green),
                UIColor.Cyan => new SolidColorBrush(Microsoft.UI.Colors.Cyan),
                UIColor.Red => new SolidColorBrush(Microsoft.UI.Colors.Red),
                UIColor.Magenta => new SolidColorBrush(Microsoft.UI.Colors.Magenta),
                UIColor.Yellow => new SolidColorBrush(Microsoft.UI.Colors.Yellow),
                UIColor.White => new SolidColorBrush(Microsoft.UI.Colors.White),
                _ => new SolidColorBrush(Microsoft.UI.Colors.White), // Default color
            };
        }
    }
}
