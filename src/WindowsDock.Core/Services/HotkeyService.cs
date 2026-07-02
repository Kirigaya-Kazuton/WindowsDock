using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace WindowsDock.Core.Services;

public class HotkeyService
{
    private readonly Dictionary<string, HotkeyEntry> _registrations = new();
    private int _currentId;
    private HwndSource? _source;

    private const int WM_HOTKEY = 0x0312;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    public void Initialize(Window window)
    {
        _source = (HwndSource)PresentationSource.FromVisual(window)!;
        _source.AddHook(WndProc);
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_HOTKEY)
        {
            var id = wParam.ToInt32();
            var entry = _registrations.Values.FirstOrDefault(r => r.Id == id);
            if (entry != null)
            {
                entry.Action?.Invoke();
                handled = true;
            }
        }
        return IntPtr.Zero;
    }

    public bool Register(Key key, ModifierKeys modifiers, Action action, string identifier)
    {
        UnRegister(identifier);

        if (_source == null) return false;
        var vk = (uint)KeyInterop.VirtualKeyFromKey(key);
        var mod = (uint)modifiers;
        var id = ++_currentId;

        if (!RegisterHotKey(_source.Handle, id, mod, vk))
            return false;

        _registrations[identifier] = new HotkeyEntry { Id = id, Action = action };
        return true;
    }

    public void UnRegister(string identifier)
    {
        if (_registrations.TryGetValue(identifier, out var entry) && _source != null)
        {
            UnregisterHotKey(_source.Handle, entry.Id);
            _registrations.Remove(identifier);
        }
    }

    public void UnRegisterAll()
    {
        foreach (var (_, entry) in _registrations)
        {
            if (_source != null)
                UnregisterHotKey(_source.Handle, entry.Id);
        }
        _registrations.Clear();
    }

    private class HotkeyEntry
    {
        public int Id { get; set; }
        public Action? Action { get; set; }
    }
}
