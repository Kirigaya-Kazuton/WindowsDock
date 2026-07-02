using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using DesktopCore;
using WindowsDock.Core;
using WindowsDock.Core.Services;
using WindowsDock.GUI.Effects;
using WindowsDock.GUI.Win32;

namespace WindowsDock.GUI;

public partial class MainWindow : Window
{
    private readonly Manager _manager;
    private readonly HotkeyService _hotkeyService;
    private bool _acrylicEnabled;

    public MainWindow()
    {
        InitializeComponent();

        _manager = new Manager();
        _manager.Load();
        _hotkeyService = new HotkeyService();

        DataContext = _manager;

        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Initialize hotkey service
        _hotkeyService.Initialize(this);

        // Register activation hotkey (Win+W)
        _hotkeyService.Register(_manager.ActivationKey, ModifierKeys.Windows, ToggleDock, "Activation");

        // Register app hotkeys
        _hotkeyService.Register(_manager.ConfigKey, ModifierKeys.Windows, OpenConfig, "Config");
        _hotkeyService.Register(_manager.CloseKey, ModifierKeys.Windows, () => Close(), "Close");

        // Register shortcut global hotkeys
        foreach (var shortcut in _manager.Shortcuts)
        {
            if (shortcut.GlobalKey != Key.None)
            {
                _hotkeyService.Register(shortcut.GlobalKey, shortcut.GlobalModifiers,
                    () => RunShortcut(shortcut), shortcut.Path);
            }
        }

        // Apply acrylic effect
        ApplyAcrylic();

        // Apply theme
        ApplyTheme();

        // Set corner preference for Windows 11
        AcrylicHelper.SetCornerPreference(this, true);

        // Position the dock at bottom center
        PositionDock();

        // Start hidden if configured
        if (_manager.StartHidden)
            HideDock();
    }

    private void ApplyAcrylic()
    {
        if (_manager.EnableAcrylic)
        {
            var bgColor = (Color)ColorConverter.ConvertFromString(_manager.Background);
            AcrylicHelper.EnableAcrylic(this, bgColor, _manager.Opacity);
            _acrylicEnabled = true;
        }
    }

    private void ApplyTheme()
    {
        bool isDark;
        if (_manager.AutoTheme)
        {
            // Detect Windows theme
            try
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                var value = key?.GetValue("AppsUseLightTheme");
                isDark = value is int v && v == 0;
            }
            catch
            {
                isDark = false;
            }
        }
        else
        {
            isDark = _manager.ThemeMode == "Dark";
        }

        // Apply dark mode to title bar
        AcrylicHelper.SetDarkMode(this, isDark);

        // Switch resource dictionary
        var uri = isDark
            ? new Uri("Themes/DarkTheme.xaml", UriKind.Relative)
            : new Uri("Themes/LightTheme.xaml", UriKind.Relative);

        var dict = new ResourceDictionary { Source = uri };
        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(dict);
    }

    private void PositionDock()
    {
        // Always position at bottom center (macOS style)
        this.Top = SystemParameters.PrimaryScreenHeight - ActualHeight - _manager.TaskbarHeight - 10;
        var left = (SystemParameters.PrimaryScreenWidth - ActualWidth) / 2;
        if (left < 0) left = 0;
        this.Left = left;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        WindowHelper.HideFromWindowList(this);
    }

    private void ToggleDock()
    {
        if (Opacity > 0.5)
            HideDock();
        else
            ShowDock();
    }

    private void ShowDock()
    {
        var anim = new DoubleAnimation(1.0, TimeSpan.FromMilliseconds(_manager.HideDuration.TotalMilliseconds))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };
        BeginAnimation(OpacityProperty, anim);

        var translateAnim = new DoubleAnimation(0, TimeSpan.FromMilliseconds(_manager.HideDuration.TotalMilliseconds))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };
        // TODO: Use RenderTransform for slide-up effect
    }

    private void HideDock()
    {
        var anim = new DoubleAnimation(0.0, TimeSpan.FromMilliseconds(_manager.HideDuration.TotalMilliseconds))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
        };
        BeginAnimation(OpacityProperty, anim);
    }

    private void Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (_manager.AutoHiding)
            ShowDock();
    }

    private void Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (_manager.AutoHiding)
            HideDock();
    }

    private void Window_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                _manager.Shortcuts.Add(new Shortcut(file));
            }
            _manager.Save();
        }
    }

    private void RunShortcut(Shortcut shortcut)
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

            if (_manager.AutoHiding)
                HideDock();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to run {shortcut.Path}: {ex.Message}", "WindowsDock");
        }
    }

    private void btnFinder_Click(object sender, RoutedEventArgs e)
    {
        Process.Start("explorer.exe");
    }

    private void btnTrash_Click(object sender, RoutedEventArgs e)
    {
        // Open Recycle Bin
        Process.Start("explorer.exe", "shell:RecycleBinFolder");
    }

    private void OpenConfig()
    {
        // Open configuration window
        // TODO: Implement configuration window
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _hotkeyService.UnRegisterAll();
        _manager.Save();
    }

    // Adjust position when size changes
    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        PositionDock();
    }
}
