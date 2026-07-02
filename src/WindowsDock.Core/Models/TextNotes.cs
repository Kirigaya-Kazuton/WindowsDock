namespace WindowsDock.Core;

public class TextNotes : ObservableCollection<TextNote> { }

public class TextNote : NotifyPropertyChanged
{
    private string _header = "";
    private string _content = "";
    private DateTime _modified;
    private DateTime? _alarm;
    private bool _isAlarming;

    public TextNote() { Modified = DateTime.Now; }

    public TextNote(string header) : this() { Header = header; }

    public TextNote(string header, string content, DateTime modified, DateTime? alarm = null)
    {
        Header = header;
        Content = content;
        Modified = modified;
        Alarm = alarm;
    }

    public string Header { get => _header; set { _header = value; FirePropertyChanged(nameof(Header)); } }
    public string Content { get => _content; set { _content = value; FirePropertyChanged(nameof(Content)); } }
    public DateTime Modified { get => _modified; set { _modified = value; FirePropertyChanged(nameof(Modified)); } }
    public DateTime? Alarm { get => _alarm; set { _alarm = value; FirePropertyChanged(nameof(Alarm)); } }
    public bool IsAlarming { get => _isAlarming; set { _isAlarming = value; FirePropertyChanged(nameof(IsAlarming)); } }
}
