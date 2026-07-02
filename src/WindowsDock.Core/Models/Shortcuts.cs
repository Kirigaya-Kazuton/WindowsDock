using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace WindowsDock.Core;

public class Shortcuts : ObservableCollection<Shortcut>
{
    public static readonly Key[] PermittedKeys = [
        Key.None,
        Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0,
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J,
        Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T,
        Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z, Key.Space
    ];

    public Shortcut Swap(Shortcut item1, Shortcut item2)
    {
        var temp = new Shortcut(item1.Path, item1.ImageSource, item1.Args, item1.WorkingDirectory, item1.Key, item1.GlobalKey, item1.GlobalModifiers);
        item1.CopyFrom(item2);
        item2.CopyFrom(temp);
        return item2;
    }
}

public class Shortcut : NotifyPropertyChanged
{
    private string _path = "";
    private string _args = "";
    private string _workingDirectory = "";
    private ImageSource? _imageSource;
    private Key _key = Key.None;
    private Key _globalKey = Key.None;
    private ModifierKeys _globalModifiers = ModifierKeys.Windows;

    public Shortcut() { }

    public Shortcut(string path)
    {
        Path = path;
        WorkingDirectory = System.IO.Path.GetDirectoryName(Path) ?? "";
    }

    public Shortcut(string path, ImageSource? icon, string args, string workingDirectory, Key key, Key globalKey, ModifierKeys globalModifiers)
    {
        Path = path;
        ImageSource = icon;
        Args = args;
        WorkingDirectory = workingDirectory;
        Key = key;
        GlobalKey = globalKey;
        GlobalModifiers = globalModifiers;
    }

    public string Path
    {
        get => _path;
        set
        {
            _path = value;
            ImageSource = IconHelper.GetIcon(value);
            FirePropertyChanged(nameof(Path));
        }
    }

    public string Args { get => _args; set { _args = value; FirePropertyChanged(nameof(Args)); } }
    public ImageSource? ImageSource { get => _imageSource; set { _imageSource = value; FirePropertyChanged(nameof(ImageSource)); } }
    public Key Key { get => _key; set { _key = value; FirePropertyChanged(nameof(Key)); } }
    public Key GlobalKey { get => _globalKey; set { _globalKey = value; FirePropertyChanged(nameof(GlobalKey)); } }
    public ModifierKeys GlobalModifiers { get => _globalModifiers; set { _globalModifiers = value; FirePropertyChanged(nameof(GlobalModifiers)); } }
    public string WorkingDirectory { get => _workingDirectory; set { if (!string.IsNullOrWhiteSpace(value)) { _workingDirectory = value; FirePropertyChanged(nameof(WorkingDirectory)); } } }

    public void CopyFrom(Shortcut other)
    {
        Path = other.Path;
        ImageSource = other.ImageSource;
        Args = other.Args;
        WorkingDirectory = other.WorkingDirectory;
        Key = other.Key;
        GlobalKey = other.GlobalKey;
        GlobalModifiers = other.GlobalModifiers;
    }
}
