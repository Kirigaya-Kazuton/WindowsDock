using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using WindowsDock.GUI.Win32;

namespace WindowsDock.GUI.Effects;

public static class AcrylicHelper
{
    public static void EnableAcrylic(Window window, Color tintColor, double opacity = 0.85)
    {
        var hwnd = ((HwndSource)PresentationSource.FromVisual(window)!).Handle;

        // Try DwmExtendFrameIntoClientArea for Windows 10/11 acrylic effect
        var margins = new NativeMethods.MARGINS
        {
            cxLeftWidth = -1,
            cxRightWidth = -1,
            cyTopHeight = -1,
            cyBottomHeight = -1
        };
        NativeMethods.DwmExtendFrameIntoClientArea(hwnd, ref margins);

        // Apply acrylic blur via SetWindowCompositionAttribute
        var accent = new NativeMethods.ACCENTPOLICY
        {
            AccentState = NativeMethods.ACCENT_ENABLE_ACRYLICBLURBEHIND,
            AccentFlags = 2,
            GradientColor = (int)(((uint)(byte)(opacity * 255) << 24) | ((uint)tintColor.R << 16) | ((uint)tintColor.G << 8) | (uint)tintColor.B),
            AnimationId = 0
        };

        var data = new NativeMethods.WINCOMPATTRDATA
        {
            Attribute = NativeMethods.WCA_ACCENT_POLICY,
            SizeOfData = Marshal.SizeOf<NativeMethods.ACCENTPOLICY>(),
            Data = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.ACCENTPOLICY>())
        };

        try
        {
            Marshal.StructureToPtr(accent, data.Data, false);
            SetWindowCompositionAttribute(hwnd, ref data);
        }
        finally
        {
            Marshal.FreeHGlobal(data.Data);
        }
    }

    public static void DisableAcrylic(Window window)
    {
        var hwnd = ((HwndSource)PresentationSource.FromVisual(window)!).Handle;

        var accent = new NativeMethods.ACCENTPOLICY
        {
            AccentState = 0,
            AccentFlags = 0,
            GradientColor = 0,
            AnimationId = 0
        };

        var data = new NativeMethods.WINCOMPATTRDATA
        {
            Attribute = NativeMethods.WCA_ACCENT_POLICY,
            SizeOfData = Marshal.SizeOf<NativeMethods.ACCENTPOLICY>(),
            Data = Marshal.AllocHGlobal(Marshal.SizeOf<NativeMethods.ACCENTPOLICY>())
        };

        try
        {
            Marshal.StructureToPtr(accent, data.Data, false);
            SetWindowCompositionAttribute(hwnd, ref data);
        }
        finally
        {
            Marshal.FreeHGlobal(data.Data);
        }

        var margins = new NativeMethods.MARGINS();
        NativeMethods.DwmExtendFrameIntoClientArea(hwnd, ref margins);
    }

    public static void SetDarkMode(Window window, bool enable)
    {
        if (Environment.OSVersion.Version.Build >= 22000) // Windows 11
        {
            var hwnd = ((HwndSource)PresentationSource.FromVisual(window)!).Handle;
            var value = enable ? 1 : 0;
            NativeMethods.DwmSetWindowAttribute(hwnd, NativeMethods.DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
        }
    }

    public static void SetCornerPreference(Window window, bool preferRound)
    {
        if (Environment.OSVersion.Version.Build >= 22000) // Windows 11 only
        {
            var hwnd = ((HwndSource)PresentationSource.FromVisual(window)!).Handle;
            var value = preferRound ? 2 : 1; // DWMWCP_ROUND = 2, DWMWCP_DONOTROUND = 1
            NativeMethods.DwmSetWindowAttribute(hwnd, NativeMethods.DWM_WINDOW_CORNER_PREFERENCE, ref value, sizeof(int));
        }
    }

    [DllImport("user32.dll")]
    private static extern bool SetWindowCompositionAttribute(IntPtr hwnd, ref NativeMethods.WINCOMPATTRDATA data);
}
