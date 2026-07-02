using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WindowsDock.Core.Services;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class ConfigAttribute : Attribute
{
    public string Name { get; }
    public ConfigAttribute(string name) => Name = name;
}

public class ConfigInfo
{
    public ConfigAttribute Attribute { get; set; } = null!;
    public PropertyInfo Property { get; set; } = null!;
}

public static class ConfigHelper
{
    public static IEnumerable<ConfigInfo> GetProperties(Type type)
    {
        foreach (var prop in type.GetProperties())
        {
            var attr = prop.GetCustomAttribute<ConfigAttribute>(true);
            if (attr != null)
                yield return new ConfigInfo { Attribute = attr, Property = prop };
        }
    }
}

public class AppConfig
{
    // Shortcuts, TextNotes, Scripts, Commands stored as serializable DTOs
    public List<ShortcutDto> Shortcuts { get; set; } = [];
    public List<TextNoteDto> TextNotes { get; set; } = [];
    public List<ScriptDto> Scripts { get; set; } = [];
    public List<CommandDto> Commands { get; set; } = [];
    public int CommandDefaultIndex { get; set; }

    // Settings
    public WindowPosition Position { get; set; } = WindowPosition.Bottom;
    public bool DockWindow { get; set; } = false;
    public WindowAlign Align { get; set; } = WindowAlign.Center;
    public int AlignOffset { get; set; } = 0;
    public int CornerRadius { get; set; } = 12;
    public int HideDurationMs { get; set; } = 125;
    public int HideDelayMs { get; set; } = 250;
    public double Opacity { get; set; } = 0.85;
    public string Background { get; set; } = "#CCF0F0F0";
    public int BorderThickness { get; set; } = 0;
    public string BorderColor { get; set; } = "#00FFFFFF";
    public string AppButtonColor { get; set; } = "#80000000";
    public bool AutoHiding { get; set; } = false;
    public bool StartHidden { get; set; } = false;
    public int TaskbarHeight { get; set; } = 40;
    public bool UseTaskBarHeightWhenBottom { get; set; } = true;
    public int HiddenOffset { get; set; } = 2;
    public bool TextNotesEnabled { get; set; } = true;
    public bool ScriptsEnabled { get; set; } = true;
    public bool BrowserEnabled { get; set; } = true;
    public bool DesktopEnabled { get; set; } = true;
    public string AlarmSound { get; set; } = "Sounds/alarm.wav";
    public bool DesktopIconsEnabled { get; set; } = true;
    public string Locale { get; set; } = "en-US";
    public bool ShowConfigButton { get; set; } = true;
    public bool ShowCloseButton { get; set; } = true;
    public bool ShowShortcutBubble { get; set; } = true;
    public int ShortcutBubbleFontSize { get; set; } = 8;
    public int ShortcutIconSize { get; set; } = 48;

    // Keys
    public string ActivationKey { get; set; } = "W";
    public string ConfigKey { get; set; } = "Z";
    public string CloseKey { get; set; } = "X";
    public string TextNotesKey { get; set; } = "T";
    public string ScriptsKey { get; set; } = "S";
    public string FolderBrowserKey { get; set; } = "B";
    public string DesktopBrowserKey { get; set; } = "D";
    public string DesktopExplorerKey { get; set; } = "None";

    // macOS dock features
    public bool EnableMagnification { get; set; } = true;
    public double MagnificationFactor { get; set; } = 1.3;
    public bool EnableReflection { get; set; } = true;
    public double ReflectionOpacity { get; set; } = 0.2;
    public bool RunIndicatorsEnabled { get; set; } = true;
    public bool EnableAcrylic { get; set; } = true;
    public bool AutoTheme { get; set; } = true;
    public string ThemeMode { get; set; } = "Auto"; // Light, Dark, Auto
}

public class ShortcutDto
{
    public string Path { get; set; } = "";
    public string Args { get; set; } = "";
    public string WorkingDirectory { get; set; } = "";
    public string Key { get; set; } = "None";
    public string GlobalKey { get; set; } = "None";
    public string GlobalModifiers { get; set; } = "Windows";
}

public class TextNoteDto
{
    public string Header { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime Modified { get; set; }
    public DateTime? Alarm { get; set; }
}

public class ScriptDto
{
    public string Header { get; set; } = "";
    public string Path { get; set; } = "";
    public string WorkingDirectory { get; set; } = "";
}

public class CommandDto
{
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public string Args { get; set; } = "";
}
