namespace WindowsDock.Core;

public class Commands : ObservableCollection<Command>, INotifyPropertyChanged
{
    private int _defaultIndex;

    public new event PropertyChangedEventHandler? PropertyChanged;

    public int DefaultIndex
    {
        get => _defaultIndex;
        set
        {
            _defaultIndex = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DefaultIndex)));
        }
    }

    public Command? Default
    {
        get => _defaultIndex >= 0 && _defaultIndex < Count ? this[_defaultIndex] : null;
        set
        {
            var idx = IndexOf(value!);
            if (idx >= 0) DefaultIndex = idx;
        }
    }
}

public class Command : NotifyPropertyChanged
{
    private string _name = "";
    private string _path = "";
    private string _args = "";

    public Command() { }

    public Command(string name, string path, string args)
    {
        Name = name;
        Path = path;
        Args = args;
    }

    public string Name { get => _name; set { _name = value; FirePropertyChanged(nameof(Name)); } }
    public string Path { get => _path; set { _path = value; FirePropertyChanged(nameof(Path)); } }
    public string Args { get => _args; set { _args = value; FirePropertyChanged(nameof(Args)); } }
}
