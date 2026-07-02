<#
.SYNOPSIS
    Builds and publishes WindowsDock for Windows 10/11.
.DESCRIPTION
    Compiles WindowsDock and creates a ready-to-use .exe in the ./publish folder.
    Requires .NET 8 SDK installed on Windows.
.PARAMETER SingleFile
    Publish as a single .exe file (no extra DLLs).
.PARAMETER OutputDir
    Output directory (default: ./publish).
.EXAMPLE
    .\publish.ps1
    .\publish.ps1 -SingleFile -OutputDir "C:\WindowsDock"
#>

param(
    [switch]$SingleFile = $false,
    [string]$OutputDir = "./publish"
)

$ErrorActionPreference = "Stop"
$ProjectFile = "src\WindowsDock.GUI\WindowsDock.GUI.csproj"

# Check OS
if ($env:OS -ne "Windows_NT") {
    Write-Error "WindowsDock can only be built on Windows (WPF requires Windows)."
    exit 1
}

# Check .NET SDK
try {
    $version = dotnet --version
    Write-Host ".NET SDK $version detected" -ForegroundColor Green
} catch {
    Write-Error ".NET SDK not found. Download from: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
}

# Check if project exists
if (-not (Test-Path $ProjectFile)) {
    Write-Error "Project file not found: $ProjectFile`nRun this script from the repository root."
    exit 1
}

Write-Host "`n=== WindowsDock Build Script ===" -ForegroundColor Cyan
Write-Host "Project : $ProjectFile"
Write-Host "Output  : $OutputDir"
Write-Host "Mode    : $('Single-file' -f $SingleFile)"
Write-Host ""

# Restore
Write-Host "`n[1/3] Restoring dependencies..." -ForegroundColor Yellow
dotnet restore $ProjectFile
if ($LASTEXITCODE -ne 0) { throw "Restore failed" }

# Build
Write-Host "`n[2/3] Building (Release)..." -ForegroundColor Yellow
dotnet build $ProjectFile -c Release --no-restore
if ($LASTEXITCODE -ne 0) { throw "Build failed" }

# Publish
Write-Host "`n[3/3] Publishing..." -ForegroundColor Yellow
$publishArgs = @(
    "publish", $ProjectFile,
    "-c", "Release",
    "-o", $OutputDir,
    "--no-build"
)

if ($SingleFile) {
    Write-Host "  -> Single-file mode enabled" -ForegroundColor Gray
    $publishArgs += "/p:PublishSingleFile=true"
    $publishArgs += "/p:IncludeNativeLibrariesForSelfExtract=true"
}

dotnet $publishArgs
if ($LASTEXITCODE -ne 0) { throw "Publish failed" }

# Create run script
$runScript = @"
@echo off
start "" "%~dp0WindowsDock.GUI.exe"
"@
$runScript | Out-File -FilePath "$OutputDir\WindowsDock.bat" -Encoding ASCII

Write-Host "`n=== Build complete! ===" -ForegroundColor Green
Write-Host ""
Write-Host "Output folder: $(Resolve-Path $OutputDir)"
Write-Host "Executable   : $OutputDir\WindowsDock.GUI.exe"
Write-Host "Run script   : $OutputDir\WindowsDock.bat (double-click to start)"
Write-Host ""

if ($SingleFile) {
    $exePath = "$OutputDir\WindowsDock.GUI.exe"
    if (Test-Path $exePath) {
        $size = (Get-Item $exePath).Length / 1MB
        Write-Host "Single-file EXE size: $('{0:N1}' -f $size) MB" -ForegroundColor Cyan
    }
}

Write-Host "Tip: Press Win+W to toggle the dock after launching." -ForegroundColor Gray
Write-Host ""
