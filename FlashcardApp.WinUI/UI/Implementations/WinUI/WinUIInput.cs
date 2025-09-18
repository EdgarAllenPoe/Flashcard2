using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using FlashcardApp.UI.Abstractions;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace FlashcardApp.WinUI.UI.Implementations.WinUI
{
    /// <summary>
    /// WinUI 3 implementation of IUIInput for Windows Native applications
    /// </summary>
    public class WinUIInput : IUIInput
    {
        private readonly TextBox _inputTextBox;
        private readonly TaskCompletionSource<string> _readLineCompletionSource;
        private readonly TaskCompletionSource<UIKeyInfo> _readKeyCompletionSource;
        private readonly TaskCompletionSource<string> _immediateChoiceCompletionSource;
        private readonly TaskCompletionSource<string> _userChoiceCompletionSource;
        private readonly TaskCompletionSource<bool> _waitForAnyKeyCompletionSource;

        public WinUIInput(TextBox inputTextBox)
        {
            _inputTextBox = inputTextBox ?? throw new ArgumentNullException(nameof(inputTextBox));
            _readLineCompletionSource = new TaskCompletionSource<string>();
            _readKeyCompletionSource = new TaskCompletionSource<UIKeyInfo>();
            _immediateChoiceCompletionSource = new TaskCompletionSource<string>();
            _userChoiceCompletionSource = new TaskCompletionSource<string>();
            _waitForAnyKeyCompletionSource = new TaskCompletionSource<bool>();

            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            _inputTextBox.KeyDown += OnKeyDown;
            _inputTextBox.TextChanged += OnTextChanged;
        }

        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var keyInfo = MapKeyToUIKeyInfo(e);
            
            // Complete any pending read key operation
            if (!_readKeyCompletionSource.Task.IsCompleted)
            {
                _readKeyCompletionSource.SetResult(keyInfo);
            }

            // Complete immediate choice if it's a single character
            if (!_immediateChoiceCompletionSource.Task.IsCompleted && 
                keyInfo.KeyChar != '\0' && char.IsLetterOrDigit(keyInfo.KeyChar))
            {
                _immediateChoiceCompletionSource.SetResult(keyInfo.KeyChar.ToString());
            }

            // Complete wait for any key
            if (!_waitForAnyKeyCompletionSource.Task.IsCompleted)
            {
                _waitForAnyKeyCompletionSource.SetResult(true);
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // This could be used for more complex input handling
        }

        public Task<string> ReadLineAsync()
        {
            // For WinUI, we'll use a different approach - show a dialog or use the text box
            // For now, return a placeholder that would be handled by the UI layer
            return Task.FromResult(_inputTextBox.Text);
        }

        public Task<UIKeyInfo> ReadKeyAsync(bool intercept = false)
        {
            return _readKeyCompletionSource.Task;
        }

        public Task<string> GetImmediateChoiceAsync()
        {
            return _immediateChoiceCompletionSource.Task;
        }

        public Task<string> GetUserChoiceAsync()
        {
            return _userChoiceCompletionSource.Task;
        }

        public Task WaitForAnyKeyAsync()
        {
            return _waitForAnyKeyCompletionSource.Task;
        }

        private UIKeyInfo MapKeyToUIKeyInfo(KeyRoutedEventArgs e)
        {
            UIKey uiKey = e.Key switch
            {
                Windows.System.VirtualKey.Enter => UIKey.Enter,
                Windows.System.VirtualKey.Escape => UIKey.Escape,
                Windows.System.VirtualKey.Up => UIKey.UpArrow,
                Windows.System.VirtualKey.Down => UIKey.DownArrow,
                Windows.System.VirtualKey.Left => UIKey.LeftArrow,
                Windows.System.VirtualKey.Right => UIKey.RightArrow,
                Windows.System.VirtualKey.Back => UIKey.Backspace,
                Windows.System.VirtualKey.Delete => UIKey.Delete,
                Windows.System.VirtualKey.Tab => UIKey.Tab,
                Windows.System.VirtualKey.Home => UIKey.Home,
                Windows.System.VirtualKey.End => UIKey.End,
                Windows.System.VirtualKey.PageUp => UIKey.PageUp,
                Windows.System.VirtualKey.PageDown => UIKey.PageDown,
                Windows.System.VirtualKey.F1 => UIKey.F1,
                Windows.System.VirtualKey.F2 => UIKey.F2,
                Windows.System.VirtualKey.F3 => UIKey.F3,
                Windows.System.VirtualKey.F4 => UIKey.F4,
                Windows.System.VirtualKey.F5 => UIKey.F5,
                Windows.System.VirtualKey.F6 => UIKey.F6,
                Windows.System.VirtualKey.F7 => UIKey.F7,
                Windows.System.VirtualKey.F8 => UIKey.F8,
                Windows.System.VirtualKey.F9 => UIKey.F9,
                Windows.System.VirtualKey.F10 => UIKey.F10,
                Windows.System.VirtualKey.F11 => UIKey.F11,
                Windows.System.VirtualKey.F12 => UIKey.F12,
                Windows.System.VirtualKey.Number0 => UIKey.D0,
                Windows.System.VirtualKey.Number1 => UIKey.D1,
                Windows.System.VirtualKey.Number2 => UIKey.D2,
                Windows.System.VirtualKey.Number3 => UIKey.D3,
                Windows.System.VirtualKey.Number4 => UIKey.D4,
                Windows.System.VirtualKey.Number5 => UIKey.D5,
                Windows.System.VirtualKey.Number6 => UIKey.D6,
                Windows.System.VirtualKey.Number7 => UIKey.D7,
                Windows.System.VirtualKey.Number8 => UIKey.D8,
                Windows.System.VirtualKey.Number9 => UIKey.D9,
                Windows.System.VirtualKey.A => UIKey.A,
                Windows.System.VirtualKey.B => UIKey.B,
                Windows.System.VirtualKey.C => UIKey.C,
                Windows.System.VirtualKey.D => UIKey.D,
                Windows.System.VirtualKey.E => UIKey.E,
                Windows.System.VirtualKey.F => UIKey.F,
                Windows.System.VirtualKey.G => UIKey.G,
                Windows.System.VirtualKey.H => UIKey.H,
                Windows.System.VirtualKey.I => UIKey.I,
                Windows.System.VirtualKey.J => UIKey.J,
                Windows.System.VirtualKey.K => UIKey.K,
                Windows.System.VirtualKey.L => UIKey.L,
                Windows.System.VirtualKey.M => UIKey.M,
                Windows.System.VirtualKey.N => UIKey.N,
                Windows.System.VirtualKey.O => UIKey.O,
                Windows.System.VirtualKey.P => UIKey.P,
                Windows.System.VirtualKey.Q => UIKey.Q,
                Windows.System.VirtualKey.R => UIKey.R,
                Windows.System.VirtualKey.S => UIKey.S,
                Windows.System.VirtualKey.T => UIKey.T,
                Windows.System.VirtualKey.U => UIKey.U,
                Windows.System.VirtualKey.V => UIKey.V,
                Windows.System.VirtualKey.W => UIKey.W,
                Windows.System.VirtualKey.X => UIKey.X,
                Windows.System.VirtualKey.Y => UIKey.Y,
                Windows.System.VirtualKey.Z => UIKey.Z,
                Windows.System.VirtualKey.Space => UIKey.Space,
                _ => UIKey.Unknown
            };

            char keyChar = '\0';
            if (e.Key >= Windows.System.VirtualKey.A && e.Key <= Windows.System.VirtualKey.Z)
            {
                keyChar = (char)('a' + (e.Key - Windows.System.VirtualKey.A));
            }
            else if (e.Key >= Windows.System.VirtualKey.Number0 && e.Key <= Windows.System.VirtualKey.Number9)
            {
                keyChar = (char)('0' + (e.Key - Windows.System.VirtualKey.Number0));
            }
            else if (e.Key == Windows.System.VirtualKey.Space)
            {
                keyChar = ' ';
            }

            return new UIKeyInfo(uiKey, keyChar, keyChar != '\0');
        }
    }
}
