# Flashcard App Launcher with Emoji Support
# This PowerShell script ensures proper emoji display

# Set console encoding to UTF-8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::InputEncoding = [System.Text.Encoding]::UTF8

# Set code page to UTF-8
chcp 65001 | Out-Null

# Set console title
$Host.UI.RawUI.WindowTitle = "🎯 Flashcard App - Emoji Support"

Write-Host "🚀 Starting Flashcard App with emoji support..." -ForegroundColor Green
Write-Host ""

# Get the directory where this script is located
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $ScriptDir

# Check if the published version exists
if (Test-Path "bin\Release\net8.0\win-x64\publish\Flashcard.exe") {
    & "bin\Release\net8.0\win-x64\publish\Flashcard.exe"
} elseif (Test-Path "Flashcard.exe") {
    & "Flashcard.exe"
} else {
    Write-Host "❌ Error: Flashcard.exe not found!" -ForegroundColor Red
    Write-Host "Please run 'dotnet publish' first or ensure the executable is in the current directory." -ForegroundColor Yellow
    Read-Host "Press Enter to continue"
    exit 1
}

# Keep console open if there was an error
if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "Application exited with error code $LASTEXITCODE" -ForegroundColor Red
    Read-Host "Press Enter to continue"
}
