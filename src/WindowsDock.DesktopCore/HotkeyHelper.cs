using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace DesktopCore
{
    public class HotkeyHelper
    {
        private readonly Window _window;
        private readonly Dictionary<string, HotkeyRegistration> _registrations = new();
        private int _currentId = 0;

        private const int WM_HOTKEY = 0x0312;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public HotkeyHelper(Window window)
        {
            _window = window;
            var source = PresentationSource.FromVisual(window) as HwndSource;
            if (source != null)
                source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY)
            {
                var id = wParam.ToInt32();
                if (_registrations.Values.FirstOrDefault(r => r.Id == id) is HotkeyRegistration reg)
                {
                    reg.Action?.Invoke();
                    handled = true;
                }
            }
            return IntPtr.Zero;
        }

        public bool Register(Key key, ModifierKeys modifiers, Action action, string identifier)
        {
            UnRegister(identifier);

            var hwnd = ((HwndSource)PresentationSource.FromVisual(_window)!).Handle;
            var vk = (uint)KeyInterop.VirtualKeyFromKey(key);
            var mod = (uint)modifiers;
            var id = ++_currentId;

            if (!RegisterHotKey(hwnd, id, mod, vk))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            _registrations[identifier] = new HotkeyRegistration { Id = id, Action = action };
            return true;
        }

        public void UnRegister(string identifier)
        {
            if (_registrations.TryGetValue(identifier, out var reg))
            {
                var hwnd = ((HwndSource)PresentationSource.FromVisual(_window)!).Handle;
                UnregisterHotKey(hwnd, reg.Id);
                _registrations.Remove(identifier);
            }
        }

        private class HotkeyRegistration
        {
            public int Id { get; set; }
            public Action? Action { get; set; }
        }
    }
}
