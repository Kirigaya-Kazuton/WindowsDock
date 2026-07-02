using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using WindowsDock.Core;

namespace WindowsDock.GUI.Controls;

public class DockIconControl : FrameworkElement
{
    public static readonly DependencyProperty ShortcutProperty =
        DependencyProperty.Register(nameof(Shortcut), typeof(Shortcut), typeof(DockIconControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertyChanged));

    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(nameof(IconSize), typeof(int), typeof(DockIconControl),
            new FrameworkPropertyMetadata(48, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty MagnificationEnabledProperty =
        DependencyProperty.Register(nameof(MagnificationEnabled), typeof(bool), typeof(DockIconControl),
            new FrameworkPropertyMetadata(true));

    public static readonly DependencyProperty MagnificationFactorProperty =
        DependencyProperty.Register(nameof(MagnificationFactor), typeof(double), typeof(DockIconControl),
            new FrameworkPropertyMetadata(1.3));

    public static readonly DependencyProperty ReflectionEnabledProperty =
        DependencyProperty.Register(nameof(ReflectionEnabled), typeof(bool), typeof(DockIconControl),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ReflectionOpacityProperty =
        DependencyProperty.Register(nameof(ReflectionOpacity), typeof(double), typeof(DockIconControl),
            new FrameworkPropertyMetadata(0.2, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty ShowIndicatorProperty =
        DependencyProperty.Register(nameof(ShowIndicator), typeof(bool), typeof(DockIconControl),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

    public static readonly DependencyProperty IsRunningProperty =
        DependencyProperty.Register(nameof(IsRunning), typeof(bool), typeof(DockIconControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public Shortcut? Shortcut
    {
        get => (Shortcut?)GetValue(ShortcutProperty);
        set => SetValue(ShortcutProperty, value);
    }

    public int IconSize
    {
        get => (int)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public bool MagnificationEnabled
    {
        get => (bool)GetValue(MagnificationEnabledProperty);
        set => SetValue(MagnificationEnabledProperty, value);
    }

    public double MagnificationFactor
    {
        get => (double)GetValue(MagnificationFactorProperty);
        set => SetValue(MagnificationFactorProperty, value);
    }

    public bool ReflectionEnabled
    {
        get => (bool)GetValue(ReflectionEnabledProperty);
        set => SetValue(ReflectionEnabledProperty, value);
    }

    public double ReflectionOpacity
    {
        get => (double)GetValue(ReflectionOpacityProperty);
        set => SetValue(ReflectionOpacityProperty, value);
    }

    public bool ShowIndicator
    {
        get => (bool)GetValue(ShowIndicatorProperty);
        set => SetValue(ShowIndicatorProperty, value);
    }

    public bool IsRunning
    {
        get => (bool)GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }

    private double _currentScale = 1.0;
    private string? _processName;
    private readonly DispatcherTimer? _runningTimer;

    public DockIconControl()
    {
        Width = 64;
        Height = 80;
        ClipToBounds = false;
        Cursor = Cursors.Hand;

        MouseEnter += OnMouseEnter;
        MouseLeave += OnMouseLeave;
        MouseLeftButtonUp += OnClick;

        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            _runningTimer = new DispatcherTimer(TimeSpan.FromSeconds(2), DispatcherPriority.Background, CheckIfRunning, Dispatcher);
            _runningTimer.Start();
        }
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DockIconControl ctrl && e.Property == ShortcutProperty && ctrl.Shortcut != null)
        {
            ctrl._processName = Path.GetFileNameWithoutExtension(ctrl.Shortcut.Path);
            ctrl.InvalidateVisual();
        }
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        if (MagnificationEnabled)
            AnimateScale(MagnificationFactor);
        InvalidateVisual();
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (MagnificationEnabled)
            AnimateScale(1.0);
        InvalidateVisual();
    }

    private void OnClick(object sender, MouseButtonEventArgs e)
    {
        if (Shortcut == null) return;
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = Shortcut.Path,
                WorkingDirectory = Shortcut.WorkingDirectory,
                UseShellExecute = true
            };
            if (!string.IsNullOrEmpty(Shortcut.Args))
                psi.Arguments = Shortcut.Args;
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to run {Shortcut.Path}: {ex.Message}", "WindowsDock");
        }
    }

    private void AnimateScale(double targetScale)
    {
        _currentScale = targetScale;
        InvalidateVisual();
    }

    private void CheckIfRunning(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_processName) || !ShowIndicator)
        {
            IsRunning = false;
            return;
        }

        try
        {
            var processes = Process.GetProcessesByName(_processName);
            IsRunning = processes.Length > 0;
            foreach (var p in processes) p.Dispose();
        }
        catch
        {
            IsRunning = false;
        }
    }

    protected override void OnRender(DrawingContext dc)
    {
        if (Shortcut?.ImageSource == null) return;

        var iconSize = IconSize;
        var scaledSize = (int)(iconSize * _currentScale);
        var indent = (scaledSize - iconSize) / 2;

        var x = (ActualWidth - scaledSize) / 2;
        var y = 6 - indent;

        // Draw icon
        dc.DrawImage(Shortcut.ImageSource, new Rect(x, y, scaledSize, scaledSize));

        // Draw reflection (flipped below icon with gradient fade)
        if (ReflectionEnabled && _currentScale > 0.5)
        {
            var refY = y + scaledSize + 2;
            var refHeight = scaledSize * 0.35;
            var opacity = (byte)(ReflectionOpacity * 255);

            var gradient = new LinearGradientBrush(
                Color.FromArgb(opacity, 255, 255, 255),
                Color.FromArgb(0, 255, 255, 255),
                90);
            gradient.Freeze();

            dc.PushOpacityMask(gradient);

            // Scale transform for flip
            var flipScale = new ScaleTransform(1, -1);
            // We need to push a transform that flips the image vertically
            // and then draws it below the original
            dc.PushTransform(new TranslateTransform(0, refY * 2 + scaledSize));
            dc.PushTransform(new ScaleTransform(1, -1, 0, refY));
            dc.DrawImage(Shortcut.ImageSource, new Rect(x, refY, scaledSize, scaledSize));
            dc.Pop();
            dc.Pop();
            dc.Pop();
        }

        // Draw running indicator dot
        if (ShowIndicator && IsRunning)
        {
            var dotSize = 4.0;
            var dotX = (ActualWidth - dotSize) / 2;
            var dotY = ActualHeight - 8;

            // Glow
            dc.DrawEllipse(
                new SolidColorBrush(Color.FromArgb(50, 0, 122, 255)),
                null,
                new Point(dotX + dotSize, dotY + dotSize),
                dotSize * 1.5, dotSize * 1.5);

            // Dot
            dc.DrawEllipse(
                new SolidColorBrush(Color.FromRgb(0, 122, 255)),
                null,
                new Point(dotX + dotSize, dotY + dotSize),
                dotSize / 2, dotSize / 2);
        }
    }
}
