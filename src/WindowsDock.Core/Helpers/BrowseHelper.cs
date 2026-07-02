using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowsDock.Core;

public class RunResult
{
    public bool Handled { get; set; }
    public bool Runnable { get; set; }
    public Process? Process { get; set; }
}

public static class BrowseHelper
{
    public static int LastIndexOfSlash(string path)
    {
        return path.LastIndexOfAny(['/', '\\']);
    }

    public static string? GetParent(string path)
    {
        try { return System.IO.Path.GetDirectoryName(path); }
        catch { return null; }
    }

    public static void SetPath(TextBox box, string path)
    {
        box.Text = path;
        box.CaretIndex = path.Length;
    }

    public static bool OpenFolder(string path, Manager manager)
    {
        try
        {
            manager.BrowseItems.Clear();
            if (!Directory.Exists(path)) return false;

            foreach (var dir in Directory.EnumerateDirectories(path))
            {
                var name = dir.Substring(LastIndexOfSlash(dir) + 1);
                manager.BrowseItems.Add(new BrowseItem(name, dir));
            }
            return manager.Commands.Count > 0;
        }
        catch
        {
            return false;
        }
    }

    public static RunResult TryToRun(Key key, ListView browseView, TextBox pathBox, Manager manager)
    {
        var result = new RunResult();

        if (key == Key.Enter)
        {
            if (browseView.SelectedItem is BrowseItem selected)
            {
                if (Directory.Exists(selected.Path))
                {
                    SetPath(pathBox, selected.Path);
                    result.Runnable = OpenFolder(selected.Path, manager);
                    result.Handled = true;
                }
                else
                {
                    Process.Start(new ProcessStartInfo { FileName = selected.Path, UseShellExecute = true });
                    result.Process = new Process();
                    result.Handled = true;
                }
            }
            else if (manager.Commands.Default != null)
            {
                var cmd = manager.Commands.Default;
                var p = new Process();
                p.StartInfo.FileName = cmd.Path;
                p.StartInfo.Arguments = string.Format(cmd.Args, pathBox.Text);
                p.Start();
                result.Process = p;
                result.Handled = true;
            }
        }
        else if (key == Key.Down)
        {
            if (browseView.Items.Count > 0)
            {
                browseView.SelectedIndex = 0;
                _ = browseView.Focus();
            }
            result.Handled = true;
        }

        return result;
    }

    public static bool HandleBack(TextBox pathBox, Manager manager)
    {
        var parent = GetParent(pathBox.Text);
        if (parent != null)
        {
            SetPath(pathBox, parent);
            manager.BrowseItems.Clear();
            OpenFolder(parent, manager);
            return true;
        }
        return false;
    }

    public static void RunDefault(TextBox pathBox, Manager manager)
    {
        if (manager.Commands.Default != null)
        {
            var cmd = manager.Commands.Default;
            var p = new Process();
            p.StartInfo.FileName = cmd.Path;
            p.StartInfo.Arguments = string.Format(cmd.Args, pathBox.Text);
            p.Start();
        }
    }
}
