using System;

namespace FlashcardApp.UI.Abstractions
{
    /// <summary>
    /// Abstraction for UI output operations, enabling platform-specific implementations
    /// </summary>
    public interface IUIOutput
    {
        /// <summary>
        /// Writes a line of text to the output
        /// </summary>
        /// <param name="text">The text to write</param>
        void WriteLine(string text);

        /// <summary>
        /// Writes text to the output without a newline
        /// </summary>
        /// <param name="text">The text to write</param>
        void Write(string text);

        /// <summary>
        /// Clears the output area
        /// </summary>
        void Clear();

        /// <summary>
        /// Sets the foreground color for subsequent output
        /// </summary>
        /// <param name="color">The color to set</param>
        void SetForegroundColor(UIColor color);

        /// <summary>
        /// Resets the output color to default
        /// </summary>
        void ResetColor();

        /// <summary>
        /// Sets the title of the application window
        /// </summary>
        /// <param name="title">The title to set</param>
        void SetTitle(string title);

        /// <summary>
        /// Sets the cursor visibility
        /// </summary>
        /// <param name="visible">Whether the cursor should be visible</param>
        void SetCursorVisible(bool visible);
    }

    /// <summary>
    /// Platform-agnostic color enumeration
    /// </summary>
    public enum UIColor
    {
        Default,
        Black,
        White,
        Red,
        Green,
        Blue,
        Yellow,
        Cyan,
        Magenta,
        DarkGray,
        Gray,
        DarkRed,
        DarkGreen,
        DarkBlue,
        DarkYellow,
        DarkCyan,
        DarkMagenta
    }
}
