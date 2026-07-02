namespace WindowsDock.Core;

public enum ExtensionType { None, TextNotes, Scripts, Browser, Desktop }

public enum WindowPosition
{
    [Resource("WindowPosition.Top")] Top,
    [Resource("WindowPosition.Left")] Left,
    [Resource("WindowPosition.Right")] Right,
    [Resource("WindowPosition.Bottom")] Bottom
}

public enum WindowAlign
{
    [Resource("WindowAlign.Left")] Left,
    [Resource("WindowAlign.Center")] Center,
    [Resource("WindowAlign.Right")] Right
}
