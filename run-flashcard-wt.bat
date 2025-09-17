@echo off
REM Flashcard App Launcher for Windows Terminal (Best Emoji Support)
REM This launcher tries to use Windows Terminal for better emoji support

echo Starting Flashcard App with best emoji support...

REM Get the current directory
set "CURRENT_DIR=%~dp0"

REM Try Windows Terminal first (best emoji support)
where wt.exe >nul 2>&1
if %ERRORLEVEL% equ 0 (
    echo Found Windows Terminal, using it for best emoji support...
    wt.exe -d "%CURRENT_DIR%" powershell -ExecutionPolicy Bypass -Command "& {[Console]::OutputEncoding = [System.Text.Encoding]::UTF8; [Console]::InputEncoding = [System.Text.Encoding]::UTF8; chcp 65001 | Out-Null; if (Test-Path 'Flashcard.exe') { & 'Flashcard.exe' } else { Write-Host 'Flashcard.exe not found!' -ForegroundColor Red; Read-Host 'Press Enter to continue' }}"
    goto :end
)

REM Try PowerShell ISE (better emoji support than regular PowerShell)
where powershell_ise.exe >nul 2>&1
if %ERRORLEVEL% equ 0 (
    echo Found PowerShell ISE, using it...
    powershell_ise.exe -File "%CURRENT_DIR%run-flashcard.ps1"
    goto :end
)

REM Fallback to regular PowerShell
echo Using regular PowerShell...
powershell -ExecutionPolicy Bypass -NoExit -File "%CURRENT_DIR%run-flashcard.ps1"

:end
