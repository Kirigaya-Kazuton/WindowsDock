using System.Runtime.InteropServices;

namespace WindowsDock.Core;

public static class DesktopHelper
{
    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll")]
    private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

    public static void ShowIcons(bool visible)
    {
        var hWnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);
        ShowWindow(hWnd, visible ? SW_SHOW : SW_HIDE);
    }
}
