using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DesktopCore
{
    public enum DockEdge
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public class DockBarHelper
    {
        private const int ABM_NEW = 0x00000000;
        private const int ABM_REMOVE = 0x00000001;
        private const int ABM_QUERYPOS = 0x00000002;
        private const int ABM_SETPOS = 0x00000003;
        private const int ABE_LEFT = 0;
        private const int ABE_TOP = 1;
        private const int ABE_RIGHT = 2;
        private const int ABE_BOTTOM = 3;

        [StructLayout(LayoutKind.Sequential)]
        private struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public RECT rc;
            public int lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [DllImport("shell32.dll")]
        private static extern IntPtr SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);

        private bool _registered;

        public Window Window { get; }

        public DockBarHelper(Window window)
        {
            Window = window;
        }

        public void Register(DockEdge edge)
        {
            if (_registered) return;
            var hwnd = ((HwndSource)PresentationSource.FromVisual(Window)!).Handle;
            var abd = new APPBARDATA
            {
                cbSize = Marshal.SizeOf<APPBARDATA>(),
                hWnd = hwnd,
                uEdge = edge switch
                {
                    DockEdge.Top => ABE_TOP,
                    DockEdge.Bottom => ABE_BOTTOM,
                    DockEdge.Left => ABE_LEFT,
                    DockEdge.Right => ABE_RIGHT,
                    _ => ABE_BOTTOM
                }
            };
            SHAppBarMessage(ABM_NEW, ref abd);
            _registered = true;
        }

        public void UnRegister()
        {
            if (!_registered) return;
            var hwnd = ((HwndSource)PresentationSource.FromVisual(Window)!).Handle;
            var abd = new APPBARDATA { cbSize = Marshal.SizeOf<APPBARDATA>(), hWnd = hwnd };
            SHAppBarMessage(ABM_REMOVE, ref abd);
            _registered = false;
        }
    }
}
