# Flashcard App Launcher
# This PowerShell script launches the Flashcard App

try {
    # Set console encoding to UTF-8
    [Console]::OutputEncoding = [System.Text.Encoding]::UTF8
    [Console]::InputEncoding = [System.Text.Encoding]::UTF8

    # Set code page to UTF-8
    chcp 65001 | Out-Null

# Set console title
$Host.UI.RawUI.WindowTitle = "* Flashcard App"

    Write-Host "> Starting Flashcard App..." -ForegroundColor Green
    Write-Host ""

    # Get the directory where this script is located
    $ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
    Set-Location $ScriptDir
    
    Write-Host "Current directory: $ScriptDir" -ForegroundColor Cyan
    Write-Host "Files in directory:" -ForegroundColor Cyan
    Get-ChildItem | ForEach-Object { Write-Host "  $($_.Name)" -ForegroundColor Gray }
    Write-Host ""

    # Check if the published version exists
    if (Test-Path "bin\Release\net8.0\win-x64\publish\Flashcard.exe") {
        Write-Host "+ Found published version, running it..." -ForegroundColor Green
        $exePath = Join-Path $ScriptDir "bin\Release\net8.0\win-x64\publish\Flashcard.exe"
        & $exePath
    } elseif (Test-Path "Flashcard.exe") {
        Write-Host "+ Found Flashcard.exe, running it..." -ForegroundColor Green
        $exePath = Join-Path $ScriptDir "Flashcard.exe"
        & $exePath
    } else {
        Write-Host "X Error: Flashcard.exe not found!" -ForegroundColor Red
        Write-Host "Please run 'dotnet publish' first or ensure the executable is in the current directory." -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Current directory: $ScriptDir" -ForegroundColor Yellow
        Write-Host "Files in current directory:" -ForegroundColor Yellow
        Get-ChildItem | ForEach-Object { Write-Host "  $($_.Name)" -ForegroundColor Gray }
        Write-Host ""
        Read-Host "Press Enter to continue"
        exit 1
    }

    # Keep console open if there was an error
    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Host "Application exited with error code $LASTEXITCODE" -ForegroundColor Red
        Read-Host "Press Enter to continue"
    }
} catch {
    Write-Host "X An error occurred: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Stack trace: $($_.ScriptStackTrace)" -ForegroundColor Red
    Read-Host "Press Enter to continue"
    exit 1
}
