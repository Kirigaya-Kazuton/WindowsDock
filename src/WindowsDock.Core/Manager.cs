using System.Globalization;
using System.Windows;
using System.Windows.Input;
using WindowsDock.Core.Services;

namespace WindowsDock.Core;

public interface IManager
{
    Shortcuts Shortcuts { get; set; }
    TextNotes TextNotes { get; set; }
    Scripts Scripts { get; set; }
    BrowseItems BrowseItems { get; set; }
    Commands Commands { get; set; }
    DesktopItems DesktopItems { get; set; }

    WindowPosition Position { get; set; }
    bool DockWindow { get; set; }
    WindowAlign Align { get; set; }
    int AlignOffset { get; set; }
    int CornerRadius { get; set; }
    TimeSpan HideDuration { get; set; }
    TimeSpan HideDelay { get; set; }
    double Opacity { get; set; }
    string Background { get; set; }
    int BorderThickness { get; set; }
    string BorderColor { get; set; }
    string AppButtonColor { get; set; }
    bool AutoHiding { get; set; }
    bool StartHidden { get; set; }
    int TaskbarHeight { get; set; }
    bool UseTaskBarHeightWhenBottom { get; set; }
    int HiddenOffset { get; set; }
    bool TextNotesEnabled { get; set; }
    bool ScriptsEnabled { get; set; }
    bool BrowserEnabled { get; set; }
    bool DesktopEnabled { get; set; }
    string AlarmSound { get; set; }
    bool DesktopIconsEnabled { get; set; }
    CultureInfo Locale { get; set; }
    bool ShowConfigButton { get; set; }
    bool ShowCloseButton { get; set; }
    bool ShowShortcutBubble { get; set; }
    int ShortcutBubbleFontSize { get; set; }
    int ShortcutIconSize { get; set; }

    Key ActivationKey { get; set; }
    Key ConfigKey { get; set; }
    Key CloseKey { get; set; }
    Key TextNotesKey { get; set; }
    Key ScriptsKey { get; set; }
    Key FolderBrowserKey { get; set; }
    Key DesktopBrowserKey { get; set; }
    Key DesktopExplorerKey { get; set; }

    // macOS dock features
    bool EnableMagnification { get; set; }
    double MagnificationFactor { get; set; }
    bool EnableReflection { get; set; }
    double ReflectionOpacity { get; set; }
    bool RunIndicatorsEnabled { get; set; }
    bool EnableAcrylic { get; set; }
    bool AutoTheme { get; set; }
    string ThemeMode { get; set; }

    void Save();
    void Load();
    void RestoreDefaults(bool shortcuts, bool textNotes, bool scripts, bool settings);
}

public class Manager : NotifyPropertyChanged, IManager
{
    private readonly ConfigurationService _configService = new();

    private Shortcuts _shortcuts = [];
    private TextNotes _textNotes = [];
    private Scripts _scripts = [];
    private BrowseItems _browseItems = [];
    private Commands _commands = [];
    private DesktopItems _desktopItems = [];

    private WindowPosition _position = WindowPosition.Bottom;
    private bool _dockWindow;
    private WindowAlign _align = WindowAlign.Center;
    private int _alignOffset;
    private int _cornerRadius = 12;
    private TimeSpan _hideDuration = TimeSpan.FromMilliseconds(125);
    private TimeSpan _hideDelay = TimeSpan.FromMilliseconds(250);
    private double _opacity = 0.85;
    private string _background = "#CCF0F0F0";
    private int _borderThickness;
    private string _borderColor = "#00FFFFFF";
    private string _appButtonColor = "#80000000";
    private bool _autoHiding;
    private bool _startHidden;
    private int _taskbarHeight = 40;
    private bool _useTaskBarHeightWhenBottom = true;
    private int _hiddenOffset = 2;
    private bool _textNotesEnabled = true;
    private bool _scriptsEnabled = true;
    private bool _browserEnabled = true;
    private bool _desktopEnabled = true;
    private string _alarmSound = "Sounds/alarm.wav";
    private bool _desktopIconsEnabled = true;
    private CultureInfo _locale = CultureInfo.CurrentCulture;
    private bool _showConfigButton = true;
    private bool _showCloseButton = true;
    private bool _showShortcutBubble = true;
    private int _shortcutBubbleFontSize = 8;
    private int _shortcutIconSize = 48;

    private Key _activationKey = Key.W;
    private Key _configKey = Key.Z;
    private Key _closeKey = Key.X;
    private Key _textNotesKey = Key.T;
    private Key _scriptsKey = Key.S;
    private Key _folderBrowserKey = Key.B;
    private Key _desktopBrowserKey = Key.D;
    private Key _desktopExplorerKey = Key.None;

    // macOS dock features
    private bool _enableMagnification = true;
    private double _magnificationFactor = 1.3;
    private bool _enableReflection = true;
    private double _reflectionOpacity = 0.2;
    private bool _runIndicatorsEnabled = true;
    private bool _enableAcrylic = true;
    private bool _autoTheme = true;
    private string _themeMode = "Auto";

    public Shortcuts Shortcuts { get => _shortcuts; set { _shortcuts = value; FirePropertyChanged(nameof(Shortcuts)); } }
    public TextNotes TextNotes { get => _textNotes; set { _textNotes = value; FirePropertyChanged(nameof(TextNotes)); } }
    public Scripts Scripts { get => _scripts; set { _scripts = value; FirePropertyChanged(nameof(Scripts)); } }
    public BrowseItems BrowseItems { get => _browseItems; set { _browseItems = value; FirePropertyChanged(nameof(BrowseItems)); } }
    public Commands Commands { get => _commands; set { _commands = value; FirePropertyChanged(nameof(Commands)); } }
    public DesktopItems DesktopItems { get => _desktopItems; set { _desktopItems = value; FirePropertyChanged(nameof(DesktopItems)); } }

    public WindowPosition Position { get => _position; set { _position = value; FirePropertyChanged(nameof(Position)); } }
    public bool DockWindow { get => _dockWindow; set { _dockWindow = value; FirePropertyChanged(nameof(DockWindow)); } }
    public WindowAlign Align { get => _align; set { _align = value; FirePropertyChanged(nameof(Align)); } }
    public int AlignOffset { get => _alignOffset; set { _alignOffset = value; FirePropertyChanged(nameof(AlignOffset)); } }
    public int CornerRadius { get => _cornerRadius; set { _cornerRadius = value; FirePropertyChanged(nameof(CornerRadius)); } }
    public TimeSpan HideDuration { get => _hideDuration; set { _hideDuration = value; FirePropertyChanged(nameof(HideDuration)); } }
    public TimeSpan HideDelay { get => _hideDelay; set { _hideDelay = value; FirePropertyChanged(nameof(HideDelay)); } }
    public double Opacity { get => _opacity; set { _opacity = value; FirePropertyChanged(nameof(Opacity)); } }
    public string Background { get => _background; set { _background = value; FirePropertyChanged(nameof(Background)); } }
    public int BorderThickness { get => _borderThickness; set { _borderThickness = value; FirePropertyChanged(nameof(BorderThickness)); } }
    public string BorderColor { get => _borderColor; set { _borderColor = value; FirePropertyChanged(nameof(BorderColor)); } }
    public string AppButtonColor { get => _appButtonColor; set { _appButtonColor = value; FirePropertyChanged(nameof(AppButtonColor)); } }
    public bool AutoHiding { get => _autoHiding; set { _autoHiding = value; FirePropertyChanged(nameof(AutoHiding)); } }
    public bool StartHidden { get => _startHidden; set { _startHidden = value; FirePropertyChanged(nameof(StartHidden)); } }
    public int TaskbarHeight { get => _taskbarHeight; set { _taskbarHeight = value; FirePropertyChanged(nameof(TaskbarHeight)); } }
    public bool UseTaskBarHeightWhenBottom { get => _useTaskBarHeightWhenBottom; set { _useTaskBarHeightWhenBottom = value; FirePropertyChanged(nameof(UseTaskBarHeightWhenBottom)); } }
    public int HiddenOffset { get => _hiddenOffset; set { _hiddenOffset = value; FirePropertyChanged(nameof(HiddenOffset)); } }

    public bool TextNotesEnabled { get => _textNotesEnabled; set { _textNotesEnabled = value; FirePropertyChanged(nameof(TextNotesEnabled)); } }
    public bool ScriptsEnabled { get => _scriptsEnabled; set { _scriptsEnabled = value; FirePropertyChanged(nameof(ScriptsEnabled)); } }
    public bool BrowserEnabled { get => _browserEnabled; set { _browserEnabled = value; FirePropertyChanged(nameof(BrowserEnabled)); } }
    public bool DesktopEnabled { get => _desktopEnabled; set { _desktopEnabled = value; FirePropertyChanged(nameof(DesktopEnabled)); } }
    public string AlarmSound { get => _alarmSound; set { _alarmSound = value; FirePropertyChanged(nameof(AlarmSound)); } }
    public bool DesktopIconsEnabled { get => _desktopIconsEnabled; set { _desktopIconsEnabled = value; FirePropertyChanged(nameof(DesktopIconsEnabled)); } }
    public CultureInfo Locale { get => _locale; set { _locale = value; FirePropertyChanged(nameof(Locale)); } }
    public bool ShowConfigButton { get => _showConfigButton; set { _showConfigButton = value; FirePropertyChanged(nameof(ShowConfigButton)); } }
    public bool ShowCloseButton { get => _showCloseButton; set { _showCloseButton = value; FirePropertyChanged(nameof(ShowCloseButton)); } }
    public bool ShowShortcutBubble { get => _showShortcutBubble; set { _showShortcutBubble = value; FirePropertyChanged(nameof(ShowShortcutBubble)); } }
    public int ShortcutBubbleFontSize { get => _shortcutBubbleFontSize; set { _shortcutBubbleFontSize = value; FirePropertyChanged(nameof(ShortcutBubbleFontSize)); } }
    public int ShortcutIconSize { get => _shortcutIconSize; set { _shortcutIconSize = value; FirePropertyChanged(nameof(ShortcutIconSize)); } }

    public Key ActivationKey { get => _activationKey; set { _activationKey = value; FirePropertyChanged(nameof(ActivationKey)); } }
    public Key ConfigKey { get => _configKey; set { _configKey = value; FirePropertyChanged(nameof(ConfigKey)); } }
    public Key CloseKey { get => _closeKey; set { _closeKey = value; FirePropertyChanged(nameof(CloseKey)); } }
    public Key TextNotesKey { get => _textNotesKey; set { _textNotesKey = value; FirePropertyChanged(nameof(TextNotesKey)); } }
    public Key ScriptsKey { get => _scriptsKey; set { _scriptsKey = value; FirePropertyChanged(nameof(ScriptsKey)); } }
    public Key FolderBrowserKey { get => _folderBrowserKey; set { _folderBrowserKey = value; FirePropertyChanged(nameof(FolderBrowserKey)); } }
    public Key DesktopBrowserKey { get => _desktopBrowserKey; set { _desktopBrowserKey = value; FirePropertyChanged(nameof(DesktopBrowserKey)); } }
    public Key DesktopExplorerKey { get => _desktopExplorerKey; set { _desktopExplorerKey = value; FirePropertyChanged(nameof(DesktopExplorerKey)); } }

    public bool EnableMagnification { get => _enableMagnification; set { _enableMagnification = value; FirePropertyChanged(nameof(EnableMagnification)); } }
    public double MagnificationFactor { get => _magnificationFactor; set { _magnificationFactor = value; FirePropertyChanged(nameof(MagnificationFactor)); } }
    public bool EnableReflection { get => _enableReflection; set { _enableReflection = value; FirePropertyChanged(nameof(EnableReflection)); } }
    public double ReflectionOpacity { get => _reflectionOpacity; set { _reflectionOpacity = value; FirePropertyChanged(nameof(ReflectionOpacity)); } }
    public bool RunIndicatorsEnabled { get => _runIndicatorsEnabled; set { _runIndicatorsEnabled = value; FirePropertyChanged(nameof(RunIndicatorsEnabled)); } }
    public bool EnableAcrylic { get => _enableAcrylic; set { _enableAcrylic = value; FirePropertyChanged(nameof(EnableAcrylic)); } }
    public bool AutoTheme { get => _autoTheme; set { _autoTheme = value; FirePropertyChanged(nameof(AutoTheme)); } }
    public string ThemeMode { get => _themeMode; set { _themeMode = value; FirePropertyChanged(nameof(ThemeMode)); } }

    public void Load()
    {
        var cfg = _configService.Load();

        Position = cfg.Position;
        DockWindow = cfg.DockWindow;
        Align = cfg.Align;
        AlignOffset = cfg.AlignOffset;
        CornerRadius = cfg.CornerRadius;
        HideDuration = TimeSpan.FromMilliseconds(cfg.HideDurationMs);
        HideDelay = TimeSpan.FromMilliseconds(cfg.HideDelayMs);
        Opacity = cfg.Opacity;
        Background = cfg.Background;
        BorderThickness = cfg.BorderThickness;
        BorderColor = cfg.BorderColor;
        AppButtonColor = cfg.AppButtonColor;
        AutoHiding = cfg.AutoHiding;
        StartHidden = cfg.StartHidden;
        TaskbarHeight = cfg.TaskbarHeight;
        UseTaskBarHeightWhenBottom = cfg.UseTaskBarHeightWhenBottom;
        HiddenOffset = cfg.HiddenOffset;
        TextNotesEnabled = cfg.TextNotesEnabled;
        ScriptsEnabled = cfg.ScriptsEnabled;
        BrowserEnabled = cfg.BrowserEnabled;
        DesktopEnabled = cfg.DesktopEnabled;
        AlarmSound = cfg.AlarmSound;
        DesktopIconsEnabled = cfg.DesktopIconsEnabled;
        ShowConfigButton = cfg.ShowConfigButton;
        ShowCloseButton = cfg.ShowCloseButton;
        ShowShortcutBubble = cfg.ShowShortcutBubble;
        ShortcutBubbleFontSize = cfg.ShortcutBubbleFontSize;
        ShortcutIconSize = cfg.ShortcutIconSize;

        EnableMagnification = cfg.EnableMagnification;
        MagnificationFactor = cfg.MagnificationFactor;
        EnableReflection = cfg.EnableReflection;
        ReflectionOpacity = cfg.ReflectionOpacity;
        RunIndicatorsEnabled = cfg.RunIndicatorsEnabled;
        EnableAcrylic = cfg.EnableAcrylic;
        AutoTheme = cfg.AutoTheme;
        ThemeMode = cfg.ThemeMode;

        ActivationKey = ParseKey(cfg.ActivationKey, Key.W);
        ConfigKey = ParseKey(cfg.ConfigKey, Key.Z);
        CloseKey = ParseKey(cfg.CloseKey, Key.X);
        TextNotesKey = ParseKey(cfg.TextNotesKey, Key.T);
        ScriptsKey = ParseKey(cfg.ScriptsKey, Key.S);
        FolderBrowserKey = ParseKey(cfg.FolderBrowserKey, Key.B);
        DesktopBrowserKey = ParseKey(cfg.DesktopBrowserKey, Key.D);
        DesktopExplorerKey = ParseKey(cfg.DesktopExplorerKey, Key.None);

        // Restore shortcuts
        Shortcuts.Clear();
        foreach (var s in cfg.Shortcuts)
        {
            if (File.Exists(s.Path))
            {
                var shortcut = new Shortcut(s.Path)
                {
                    Args = s.Args,
                    WorkingDirectory = s.WorkingDirectory,
                    Key = ParseKey(s.Key, Key.None),
                    GlobalKey = ParseKey(s.GlobalKey, Key.None),
                    GlobalModifiers = ParseModifiers(s.GlobalModifiers)
                };
                Shortcuts.Add(shortcut);
            }
        }

        // Restore text notes
        TextNotes.Clear();
        foreach (var n in cfg.TextNotes)
            TextNotes.Add(new TextNote(n.Header, n.Content, n.Modified, n.Alarm));

        // Restore scripts
        Scripts.Clear();
        foreach (var s in cfg.Scripts)
            Scripts.Add(new Script(s.Header, s.Path, s.WorkingDirectory));

        // Restore commands
        Commands.Clear();
        foreach (var c in cfg.Commands)
            Commands.Add(new Command(c.Name, c.Path, c.Args));
        Commands.DefaultIndex = cfg.CommandDefaultIndex;

        // Locale
        try { Locale = new CultureInfo(cfg.Locale); } catch { Locale = CultureInfo.CurrentCulture; }

        Resource.Load("Resources/Resources");
    }

    public void Save()
    {
        var cfg = new AppConfig
        {
            Position = Position,
            DockWindow = DockWindow,
            Align = Align,
            AlignOffset = AlignOffset,
            CornerRadius = CornerRadius,
            HideDurationMs = (int)HideDuration.TotalMilliseconds,
            HideDelayMs = (int)HideDelay.TotalMilliseconds,
            Opacity = Opacity,
            Background = Background,
            BorderThickness = BorderThickness,
            BorderColor = BorderColor,
            AppButtonColor = AppButtonColor,
            AutoHiding = AutoHiding,
            StartHidden = StartHidden,
            TaskbarHeight = TaskbarHeight,
            UseTaskBarHeightWhenBottom = UseTaskBarHeightWhenBottom,
            HiddenOffset = HiddenOffset,
            TextNotesEnabled = TextNotesEnabled,
            ScriptsEnabled = ScriptsEnabled,
            BrowserEnabled = BrowserEnabled,
            DesktopEnabled = DesktopEnabled,
            AlarmSound = AlarmSound,
            DesktopIconsEnabled = DesktopIconsEnabled,
            Locale = Locale.Name,
            ShowConfigButton = ShowConfigButton,
            ShowCloseButton = ShowCloseButton,
            ShowShortcutBubble = ShowShortcutBubble,
            ShortcutBubbleFontSize = ShortcutBubbleFontSize,
            ShortcutIconSize = ShortcutIconSize,
            EnableMagnification = EnableMagnification,
            MagnificationFactor = MagnificationFactor,
            EnableReflection = EnableReflection,
            ReflectionOpacity = ReflectionOpacity,
            RunIndicatorsEnabled = RunIndicatorsEnabled,
            EnableAcrylic = EnableAcrylic,
            AutoTheme = AutoTheme,
            ThemeMode = ThemeMode,
            ActivationKey = ActivationKey.ToString(),
            ConfigKey = ConfigKey.ToString(),
            CloseKey = CloseKey.ToString(),
            TextNotesKey = TextNotesKey.ToString(),
            ScriptsKey = ScriptsKey.ToString(),
            FolderBrowserKey = FolderBrowserKey.ToString(),
            DesktopBrowserKey = DesktopBrowserKey.ToString(),
            DesktopExplorerKey = DesktopExplorerKey.ToString(),
            Shortcuts = Shortcuts.Select(s => new ShortcutDto
            {
                Path = s.Path,
                Args = s.Args,
                WorkingDirectory = s.WorkingDirectory,
                Key = s.Key.ToString(),
                GlobalKey = s.GlobalKey.ToString(),
                GlobalModifiers = s.GlobalModifiers.ToString()
            }).ToList(),
            TextNotes = TextNotes.Select(n => new TextNoteDto
            {
                Header = n.Header,
                Content = n.Content,
                Modified = n.Modified,
                Alarm = n.Alarm
            }).ToList(),
            Scripts = Scripts.Select(s => new ScriptDto
            {
                Header = s.Header,
                Path = s.Path,
                WorkingDirectory = s.WorkingDirectory
            }).ToList(),
            Commands = Commands.Select(c => new CommandDto
            {
                Name = c.Name,
                Path = c.Path,
                Args = c.Args
            }).ToList(),
            CommandDefaultIndex = Commands.DefaultIndex
        };

        _configService.Save(cfg);
    }

    public void RestoreDefaults(bool shortcuts, bool textNotes, bool scripts, bool settings)
    {
        var defaults = new AppConfig();
        if (shortcuts) Shortcuts.Clear();
        if (textNotes) TextNotes.Clear();
        if (scripts) Scripts.Clear();
        if (settings)
        {
            Position = defaults.Position;
            DockWindow = defaults.DockWindow;
            Align = defaults.Align;
            CornerRadius = defaults.CornerRadius;
            HideDuration = TimeSpan.FromMilliseconds(defaults.HideDurationMs);
            HideDelay = TimeSpan.FromMilliseconds(defaults.HideDelayMs);
            Opacity = defaults.Opacity;
            Background = defaults.Background;
            AutoHiding = defaults.AutoHiding;
            StartHidden = defaults.StartHidden;
            HiddenOffset = defaults.HiddenOffset;
            TextNotesEnabled = defaults.TextNotesEnabled;
            ScriptsEnabled = defaults.ScriptsEnabled;
            BrowserEnabled = defaults.BrowserEnabled;
            DesktopEnabled = defaults.DesktopEnabled;
            DesktopIconsEnabled = defaults.DesktopIconsEnabled;
            EnableMagnification = defaults.EnableMagnification;
            MagnificationFactor = defaults.MagnificationFactor;
            EnableReflection = defaults.EnableReflection;
            ReflectionOpacity = defaults.ReflectionOpacity;
            RunIndicatorsEnabled = defaults.RunIndicatorsEnabled;
            EnableAcrylic = defaults.EnableAcrylic;
            AutoTheme = defaults.AutoTheme;
            ThemeMode = defaults.ThemeMode;
        }
    }

    private static Key ParseKey(string value, Key defaultKey)
    {
        if (string.IsNullOrEmpty(value) || value == "None") return Key.None;
        return Enum.TryParse<Key>(value, out var key) ? key : defaultKey;
    }

    private static ModifierKeys ParseModifiers(string value)
    {
        if (string.IsNullOrEmpty(value)) return ModifierKeys.Windows;
        return Enum.TryParse<ModifierKeys>(value, out var mod) ? mod : ModifierKeys.Windows;
    }
}
