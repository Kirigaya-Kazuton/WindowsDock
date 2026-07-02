namespace WindowsDock.Core;

public class DesktopItems : ObservableCollection<DesktopItem> { }

public class DesktopItem : NotifyPropertyChanged
{
    private string _name = "";
    private ImageSource? _imageSource;

    public DesktopItem() { }

    public DesktopItem(string name, ImageSource? imageSource)
    {
        Name = name;
        ImageSource = imageSource;
    }

    public string Name { get => _name; set { _name = value; FirePropertyChanged(nameof(Name)); } }
    public ImageSource? ImageSource { get => _imageSource; set { _imageSource = value; FirePropertyChanged(nameof(ImageSource)); } }
}
