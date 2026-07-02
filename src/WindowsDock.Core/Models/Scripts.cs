namespace WindowsDock.Core;

public class Scripts : ObservableCollection<Script> { }

public class Script : NotifyPropertyChanged
{
    private string _header = "";
    private string _path = "";
    private string _workingDirectory = "";

    public Script() { }

    public Script(string header, string path, string workingDirectory)
    {
        Header = header;
        Path = path;
        WorkingDirectory = workingDirectory;
    }

    public string Header { get => _header; set { _header = value; FirePropertyChanged(nameof(Header)); } }
    public string Path { get => _path; set { _path = value; FirePropertyChanged(nameof(Path)); } }
    public string WorkingDirectory { get => _workingDirectory; set { _workingDirectory = value; FirePropertyChanged(nameof(WorkingDirectory)); } }
}
