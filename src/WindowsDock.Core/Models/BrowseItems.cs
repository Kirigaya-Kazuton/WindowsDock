namespace WindowsDock.Core;

public class BrowseItems : ObservableCollection<BrowseItem> { }

public class BrowseItem : NotifyPropertyChanged
{
    private string _name = "";
    private string _path = "";

    public BrowseItem() { }

    public BrowseItem(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public string Name { get => _name; set { _name = value; FirePropertyChanged(nameof(Name)); } }
    public string Path { get => _path; set { _path = value; FirePropertyChanged(nameof(Path)); } }
}
