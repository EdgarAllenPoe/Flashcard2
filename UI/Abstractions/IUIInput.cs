using System;
using System.Threading.Tasks;

namespace FlashcardApp.UI.Abstractions
{
    /// <summary>
    /// Abstraction for UI input operations, enabling platform-specific implementations
    /// </summary>
    public interface IUIInput
    {
        /// <summary>
        /// Reads a line of input from the user
        /// </summary>
        /// <returns>The input string</returns>
        Task<string> ReadLineAsync();

        /// <summary>
        /// Reads a single key from the user
        /// </summary>
        /// <param name="intercept">Whether to intercept the key (not display it)</param>
        /// <returns>Information about the key pressed</returns>
        Task<UIKeyInfo> ReadKeyAsync(bool intercept = false);

        /// <summary>
        /// Gets a user choice with immediate key handling
        /// </summary>
        /// <returns>The user's choice</returns>
        Task<string> GetImmediateChoiceAsync();

        /// <summary>
        /// Gets a user choice with full input handling
        /// </summary>
        /// <returns>The user's choice</returns>
        Task<string> GetUserChoiceAsync();

        /// <summary>
        /// Waits for any key press
        /// </summary>
        Task WaitForAnyKeyAsync();
    }

    /// <summary>
    /// Platform-agnostic key information
    /// </summary>
    public struct UIKeyInfo
    {
        public UIKey Key { get; set; }
        public char KeyChar { get; set; }
        public bool HasKeyChar { get; set; }

        public UIKeyInfo(UIKey key, char keyChar, bool hasKeyChar = true)
        {
            Key = key;
            KeyChar = keyChar;
            HasKeyChar = hasKeyChar;
        }
    }

    /// <summary>
    /// Platform-agnostic key enumeration
    /// </summary>
    public enum UIKey
    {
        // Letters
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,

        // Numbers
        D0, D1, D2, D3, D4, D5, D6, D7, D8, D9,

        // Special keys
        Enter,
        Escape,
        Space,
        Backspace,
        Delete,
        Tab,

        // Arrow keys
        UpArrow,
        DownArrow,
        LeftArrow,
        RightArrow,

        // Function keys
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,

        // Navigation keys
        Home,
        End,
        PageUp,
        PageDown,

        // Other
        Unknown
    }
}
