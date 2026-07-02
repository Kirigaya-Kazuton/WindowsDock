# WindowsDock

> A macOS-style dock for Windows 10 and Windows 11.

WindowsDock brings the elegant macOS dock experience to your Windows desktop. It sits at the bottom of your screen, provides quick access to your favorite applications, and features the iconic glass/acrylic blur effect, icon magnification, running app indicators, and dark/light theme support.

![WindowsDock Preview](https://raw.githubusercontent.com/anomalyco/WindowsDock/feat/macos-dock-redesign/assets/preview.png)

---

## ✨ Features

| Feature | macOS Dock | WindowsDock |
|---|---|---|
| Acrylic blur background | ✅ | ✅ via `SetWindowCompositionAttribute` |
| Icon magnification on hover | ✅ | ✅ smooth `CircleEase` animation |
| Reflection effect | ✅ | ✅ gradient opacity mirror |
| Running app indicators | ✅ | ✅ blue dot + glow |
| Dark / Light mode | ✅ | ✅ auto-detects Windows theme |
| Trash / Finder icons | ✅ | ✅ |
| Drag & drop to add apps | ✅ | ✅ |
| Auto-hide | ✅ | ✅ spring animation |
| Global hotkey (Win+W) | ✅ | ✅ via `RegisterHotKey` |
| Win11 rounded corners | ✅ | ✅ `DWMWA_WINDOW_CORNER_PREFERENCE` |
| Per-monitor DPI | ✅ | ✅ `PerMonitorV2` |
| Customizable icon size | ✅ | ✅ |
| Configuration file | ❌ | `%APPDATA%/WindowsDock/config.json` |
| Multi-language | ❌ | English + Portuguese (extensible) |

### Planned
- [ ] Configuration window (GUI settings editor)
- [ ] TextNotes extension with alarms
- [ ] Script launcher
- [ ] Quick folder browser
- [ ] Desktop items browser
- [ ] Direct download installer

---

## 📋 System Requirements

| Requirement | Minimum | Recommended |
|---|---|---|
| **OS** | Windows 10 build 1809 | Windows 11 |
| **.NET** | .NET 8 Runtime | .NET 8 SDK |
| **RAM** | 256 MB | 512 MB |
| **Disk** | 10 MB | 20 MB |
| **Display** | 1366×768 | 1920×1080+ |

**Compatibility:**
- ✅ Windows 10 (build 1809+) — Acrylic via blur behind
- ✅ Windows 11 — Acrylic + native rounded corners + Mica
- ✅ Multi-monitor (DPI-aware)

---

## 🚀 Installation

### Option 1: Download (pre-built)
> Coming soon — an automated installer is being prepared.

### Option 2: Build from source

**Prerequisites:**
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (or newer)
- Visual Studio 2022 (optional, recommended)

**Steps:**

```powershell
# Clone the repository
git clone https://github.com/anomalyco/WindowsDock.git
cd WindowsDock

# Switch to the redesign branch
git checkout feat/macos-dock-redesign

# Restore dependencies
dotnet restore src/WindowsDock.GUI/WindowsDock.GUI.csproj

# Build
dotnet build src/WindowsDock.GUI/WindowsDock.GUI.csproj -c Release

# Output will be at:
# src/WindowsDock.GUI/bin/Release/net8.0-windows/WindowsDock.GUI.exe
```

**Using Visual Studio 2022:**
1. Open `WindowsDock.sln`
2. Set `WindowsDock.GUI` as startup project
3. Build & Run (F5)

### Option 3: One-click publish
```powershell
dotnet publish src/WindowsDock.GUI/WindowsDock.GUI.csproj -c Release -o ./publish
```

---

## 🎮 Usage

### First Run
1. Launch `WindowsDock.GUI.exe`
2. The dock appears at the **bottom center** of your screen
3. Toggle visibility with **`Win + W`**

### Adding Shortcuts
- **Drag & drop** any file, folder, or `.exe` onto the dock
- The icon is automatically extracted from the file

### Running Applications
- **Left-click** any icon to launch the application
- A **blue dot** below the icon indicates the app is currently running

### Auto-Hide
- Enable auto-hide in config: the dock slides away when not in use
- Hover near the bottom of the screen to reveal it

### Built-in Dock Icons
| Icon | Action |
|---|---|
| **Finder** (left) | Opens File Explorer |
| **Trash** (right) | Opens Recycle Bin |

---

## ⌨️ Keyboard Shortcuts

| Shortcut | Action |
|---|---|
| `Win + W` | Toggle dock visibility |
| `Win + Z` | Open configuration (coming soon) |
| `Win + X` | Close WindowsDock |

*(Custom key bindings are stored in `config.json`)*

---

## ⚙️ Configuration

Settings are stored in `%APPDATA%/WindowsDock/config.json`.

### Example config:
```json
{
  "shortcutIconSize": 48,
  "enableMagnification": true,
  "magnificationFactor": 1.3,
  "enableReflection": true,
  "reflectionOpacity": 0.2,
  "runIndicatorsEnabled": true,
  "enableAcrylic": true,
  "autoTheme": true,
  "themeMode": "Auto",
  "autoHiding": false,
  "opacity": 0.85,
  "activationKey": "W"
}
```

### Key settings:
| Property | Default | Description |
|---|---|---|
| `shortcutIconSize` | `48` | Icon size in pixels (24–128) |
| `enableMagnification` | `true` | Icon scale-up on hover |
| `magnificationFactor` | `1.3` | Scale multiplier (1.0 = off) |
| `enableReflection` | `true` | Reflection below icons |
| `reflectionOpacity` | `0.2` | Reflection strength |
| `runIndicatorsEnabled` | `true` | Show running app dots |
| `enableAcrylic` | `true` | Translucent blur background |
| `autoTheme` | `true` | Follow Windows theme |
| `themeMode` | `"Auto"` | `"Light"`, `"Dark"`, or `"Auto"` |
| `autoHiding` | `false` | Auto-hide when not used |
| `opacity` | `0.85` | Dock opacity (0–1) |
| `activationKey` | `"W"` | Toggle hotkey (combined with Win) |

---

## 🌐 Localization

WindowsDock supports multiple languages. To add your language:

1. Copy `src/WindowsDock.GUI/Resources/Resources.txt` as `Resources_[code].txt`
   (e.g. `Resources_fr-FR.txt` or `Resources_de.txt`)
2. Translate the values after `=` in each line
3. Rebuild the application

**Currently supported:**
| Language | File |
|---|---|
| English | `Resources.txt` |
| Portuguese (BR) | `Resources_pt-BR.txt` |

---

## 🏗️ Project Architecture

```
src/
├── WindowsDock.DesktopCore/       # Base framework (replaces old DesktopCore.dll)
│   ├── NotifyPropertyChanged.cs   # INotifyPropertyChanged base class
│   ├── Resource.cs                # Localization engine
│   ├── HotkeyHelper.cs            # Global hotkeys via P/Invoke
│   ├── DockBarHelper.cs           # Screen edge docking
│   ├── WindowHelper.cs            # Window management utilities
│   └── Converters.cs              # WPF value converters
│
├── WindowsDock.Core/              # Business logic & data models
│   ├── Models/                    # Shortcut, TextNote, Script, etc.
│   ├── Helpers/                   # Position, Icon, Browse, Desktop helpers
│   ├── Services/                  # Config, Hotkey services
│   └── Manager.cs                 # Central state management
│
└── WindowsDock.GUI/               # WPF user interface
    ├── MainWindow.xaml/.cs        # The macOS-style dock window
    ├── Controls/DockIconControl   # Custom render: magnification + reflection + indicator
    ├── Effects/AcrylicHelper.cs   # Acrylic blur (Win10/11)
    ├── Win32/NativeMethods.cs     # P/Invoke declarations
    ├── Themes/                    # Light and Dark theme resources
    └── Resources/                 # Localization files
```

---

## ❓ FAQ / Troubleshooting

**Q: The acrylic/blur effect doesn't work.**
A: Ensure you're on Windows 10 build 1809+ or Windows 11. The effect uses `SetWindowCompositionAttribute` which requires these versions.

**Q: The dock doesn't appear after launch.**
A: Press `Win + W` to toggle visibility. Check if auto-hide is enabled in config.

**Q: Shortcut icons are not showing.**
A: WindowsDock extracts icons using `System.Drawing.Icon.ExtractAssociatedIcon`. Some file types may not have embedded icons.

**Q: Running app indicators are wrong.**
A: The indicator polls processes by matching the file name (without extension) against running processes. Some apps use different process names than their file names.

**Q: How do I reset all settings?**
A: Close WindowsDock and delete `%APPDATA%/WindowsDock/config.json`. It will be recreated with defaults on next launch.

---

## 📝 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- Original project by [neptuo](https://github.com/neptuo)
- macOS dock design by Apple
- Built with .NET 8 WPF
