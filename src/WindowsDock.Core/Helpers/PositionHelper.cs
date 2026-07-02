using System.Windows;
using System.Windows.Controls;

namespace WindowsDock.Core;

public static class PositionHelper
{
    public static bool IsShown(Window window, WindowPosition position, double edgeValue, double hiddenOffset)
    {
        return position switch
        {
            WindowPosition.Top => window.Top == edgeValue,
            WindowPosition.Left => window.Left == edgeValue,
            WindowPosition.Right => GetEdgeValue(window, position) != edgeValue - hiddenOffset,
            WindowPosition.Bottom => GetEdgeValue(window, position) != edgeValue - hiddenOffset,
            _ => false
        };
    }

    public static double GetEdgeValue(Window window, WindowPosition position)
    {
        return position switch
        {
            WindowPosition.Top => window.Top,
            WindowPosition.Left => window.Left,
            WindowPosition.Right => SystemParameters.PrimaryScreenWidth - window.Left,
            WindowPosition.Bottom => SystemParameters.PrimaryScreenHeight - window.Top,
            _ => double.NaN
        };
    }

    public static double GetComputedEdgeValue(WindowPosition position, double normalEdge, double hiddenEdge, bool addTaskbarHeight, double taskbarHeight)
    {
        if (!addTaskbarHeight) taskbarHeight = 0;
        return position switch
        {
            WindowPosition.Top or WindowPosition.Left => normalEdge,
            WindowPosition.Right => SystemParameters.PrimaryScreenWidth + hiddenEdge,
            WindowPosition.Bottom => SystemParameters.PrimaryScreenHeight + hiddenEdge - taskbarHeight,
            _ => double.NaN
        };
    }

    public static DependencyProperty GetEdgeProperty(WindowPosition position)
    {
        return position switch
        {
            WindowPosition.Top or WindowPosition.Bottom => Window.TopProperty,
            WindowPosition.Left or WindowPosition.Right => Window.LeftProperty,
            _ => null!
        };
    }

    public static void SetPosition(double position, double height, WindowPosition windowPosition, Window window, StackPanel panel)
    {
        switch (windowPosition)
        {
            case WindowPosition.Top:
            case WindowPosition.Bottom:
                window.Top = position;
                panel.Height = height;
                break;
            case WindowPosition.Left:
            case WindowPosition.Right:
                window.Left = position;
                panel.Width = height;
                break;
        }
    }

    public static void SetToCenter(WindowPosition windowPosition, Window window)
    {
        var screenValue = windowPosition is WindowPosition.Top or WindowPosition.Bottom
            ? SystemParameters.PrimaryScreenWidth : SystemParameters.PrimaryScreenHeight;
        var thisValue = windowPosition is WindowPosition.Top or WindowPosition.Bottom
            ? window.ActualWidth : window.ActualHeight;
        var position = (screenValue - thisValue) / 2;

        switch (windowPosition)
        {
            case WindowPosition.Top:
            case WindowPosition.Bottom:
                window.Left = position;
                break;
            case WindowPosition.Left:
            case WindowPosition.Right:
                window.Top = position;
                break;
        }
    }

    public static void SetAlign(WindowPosition position, WindowAlign align, int offset, Window window)
    {
        if (position is WindowPosition.Top or WindowPosition.Bottom)
        {
            window.Left = align == WindowAlign.Left ? offset : SystemParameters.PrimaryScreenWidth - window.ActualWidth - offset;
        }
        else
        {
            window.Top = align == WindowAlign.Left ? offset : SystemParameters.PrimaryScreenHeight - window.ActualHeight - offset;
        }
    }

    public static void DockWindow(WindowPosition position, DockBarHelper dockBar, FrameworkElement element, int offset)
    {
        if (position is WindowPosition.Top or WindowPosition.Bottom)
        {
            element.Width = SystemParameters.PrimaryScreenWidth;
            dockBar.Window.Left = 0;
            dockBar.Register(position == WindowPosition.Top ? DockEdge.Top : DockEdge.Bottom);
        }
        else
        {
            element.Height = SystemParameters.PrimaryScreenHeight - offset;
            dockBar.Window.Top = 0;
            dockBar.Register(position == WindowPosition.Left ? DockEdge.Left : DockEdge.Right);
        }
    }

    public static void UnDockWindow(DockBarHelper dockBar, FrameworkElement element)
    {
        dockBar.UnRegister();
        element.Width = double.NaN;
        element.Height = double.NaN;
    }
}
