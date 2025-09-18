using System;
using System.Threading.Tasks;
using FlashcardApp.UI.Abstractions;

namespace FlashcardApp.UI.Implementations.Console
{
    /// <summary>
    /// Console-specific implementation of IUIInput
    /// </summary>
    public class ConsoleInput : IUIInput
    {
        public async Task<string> ReadLineAsync()
        {
            return await Task.Run(() => System.Console.ReadLine() ?? string.Empty);
        }

        public async Task<UIKeyInfo> ReadKeyAsync(bool intercept = false)
        {
            var keyInfo = await Task.Run(() => System.Console.ReadKey(intercept));
            return ConvertToUIKeyInfo(keyInfo);
        }

        public async Task<string> GetImmediateChoiceAsync()
        {
            var keyInfo = await ReadKeyAsync(true);
            return keyInfo.HasKeyChar ? keyInfo.KeyChar.ToString() : keyInfo.Key.ToString();
        }

        public async Task<string> GetUserChoiceAsync()
        {
            var keyInfo = await ReadKeyAsync(false);
            var input = keyInfo.HasKeyChar ? keyInfo.KeyChar.ToString() : keyInfo.Key.ToString();
            var remainingInput = await ReadLineAsync();
            return input + remainingInput;
        }

        public async Task WaitForAnyKeyAsync()
        {
            await Task.Run(() => System.Console.ReadKey(true));
        }

        private static UIKeyInfo ConvertToUIKeyInfo(System.ConsoleKeyInfo keyInfo)
        {
            var uiKey = ConvertToUIKey(keyInfo.Key);
            return new UIKeyInfo(uiKey, keyInfo.KeyChar, keyInfo.KeyChar != '\0');
        }

        private static UIKey ConvertToUIKey(System.ConsoleKey consoleKey)
        {
            return consoleKey switch
            {
                // Letters
                System.ConsoleKey.A => UIKey.A,
                System.ConsoleKey.B => UIKey.B,
                System.ConsoleKey.C => UIKey.C,
                System.ConsoleKey.D => UIKey.D,
                System.ConsoleKey.E => UIKey.E,
                System.ConsoleKey.F => UIKey.F,
                System.ConsoleKey.G => UIKey.G,
                System.ConsoleKey.H => UIKey.H,
                System.ConsoleKey.I => UIKey.I,
                System.ConsoleKey.J => UIKey.J,
                System.ConsoleKey.K => UIKey.K,
                System.ConsoleKey.L => UIKey.L,
                System.ConsoleKey.M => UIKey.M,
                System.ConsoleKey.N => UIKey.N,
                System.ConsoleKey.O => UIKey.O,
                System.ConsoleKey.P => UIKey.P,
                System.ConsoleKey.Q => UIKey.Q,
                System.ConsoleKey.R => UIKey.R,
                System.ConsoleKey.S => UIKey.S,
                System.ConsoleKey.T => UIKey.T,
                System.ConsoleKey.U => UIKey.U,
                System.ConsoleKey.V => UIKey.V,
                System.ConsoleKey.W => UIKey.W,
                System.ConsoleKey.X => UIKey.X,
                System.ConsoleKey.Y => UIKey.Y,
                System.ConsoleKey.Z => UIKey.Z,

                // Numbers
                System.ConsoleKey.D0 => UIKey.D0,
                System.ConsoleKey.D1 => UIKey.D1,
                System.ConsoleKey.D2 => UIKey.D2,
                System.ConsoleKey.D3 => UIKey.D3,
                System.ConsoleKey.D4 => UIKey.D4,
                System.ConsoleKey.D5 => UIKey.D5,
                System.ConsoleKey.D6 => UIKey.D6,
                System.ConsoleKey.D7 => UIKey.D7,
                System.ConsoleKey.D8 => UIKey.D8,
                System.ConsoleKey.D9 => UIKey.D9,

                // Special keys
                System.ConsoleKey.Enter => UIKey.Enter,
                System.ConsoleKey.Escape => UIKey.Escape,
                System.ConsoleKey.Spacebar => UIKey.Space,
                System.ConsoleKey.Backspace => UIKey.Backspace,
                System.ConsoleKey.Delete => UIKey.Delete,
                System.ConsoleKey.Tab => UIKey.Tab,

                // Arrow keys
                System.ConsoleKey.UpArrow => UIKey.UpArrow,
                System.ConsoleKey.DownArrow => UIKey.DownArrow,
                System.ConsoleKey.LeftArrow => UIKey.LeftArrow,
                System.ConsoleKey.RightArrow => UIKey.RightArrow,

                // Function keys
                System.ConsoleKey.F1 => UIKey.F1,
                System.ConsoleKey.F2 => UIKey.F2,
                System.ConsoleKey.F3 => UIKey.F3,
                System.ConsoleKey.F4 => UIKey.F4,
                System.ConsoleKey.F5 => UIKey.F5,
                System.ConsoleKey.F6 => UIKey.F6,
                System.ConsoleKey.F7 => UIKey.F7,
                System.ConsoleKey.F8 => UIKey.F8,
                System.ConsoleKey.F9 => UIKey.F9,
                System.ConsoleKey.F10 => UIKey.F10,
                System.ConsoleKey.F11 => UIKey.F11,
                System.ConsoleKey.F12 => UIKey.F12,

                // Navigation keys
                System.ConsoleKey.Home => UIKey.Home,
                System.ConsoleKey.End => UIKey.End,
                System.ConsoleKey.PageUp => UIKey.PageUp,
                System.ConsoleKey.PageDown => UIKey.PageDown,

                _ => UIKey.Unknown
            };
        }
    }
}
