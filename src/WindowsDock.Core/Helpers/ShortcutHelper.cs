using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsDock.Core;

public static class ShortcutHelper
{
    [DllImport("kernel32.dll")]
    private static extern uint GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, int cchBuffer);

    public static string? GetShortPath(string longPath)
    {
        var sb = new StringBuilder(260);
        var result = GetShortPathName(longPath, sb, sb.Capacity);
        return result > 0 ? sb.ToString() : null;
    }

    public static void Run(Shortcut shortcut)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = shortcut.Path,
                WorkingDirectory = shortcut.WorkingDirectory,
                UseShellExecute = true
            };
            if (!string.IsNullOrEmpty(shortcut.Args))
                psi.Arguments = shortcut.Args;

            Process.Start(psi);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to run {shortcut.Path}: {ex.Message}", "WindowsDock");
        }
    }
}
