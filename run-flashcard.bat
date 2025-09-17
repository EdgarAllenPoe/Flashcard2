@echo off
REM Flashcard App Launcher with Unicode Support
REM This batch file launches PowerShell with proper emoji display

echo Starting Flashcard App Launcher...
echo.

REM Check if PowerShell script exists
if exist "run-flashcard.ps1" (
    echo Found PowerShell launcher script, using it...
    REM Launch PowerShell with the script and keep window open
    powershell -ExecutionPolicy Bypass -NoExit -File "run-flashcard.ps1"
) else (
    echo PowerShell script not found, using fallback method...
    REM Fallback to direct execution
    echo Setting up console for Unicode support...
    chcp 65001 >nul 2>&1
    echo Starting Flashcard App...
    echo.
    
    if exist "bin\Release\net8.0\win-x64\publish\Flashcard.exe" (
        echo Found published version, running it...
        "bin\Release\net8.0\win-x64\publish\Flashcard.exe"
    ) else if exist "Flashcard.exe" (
        echo Found Flashcard.exe, running it...
        Flashcard.exe
    ) else (
        echo Error: Flashcard.exe not found!
        echo Please run 'dotnet publish' first or ensure the executable is in the current directory.
        echo.
        echo Current directory: %CD%
        echo Files in current directory:
        dir /b
        echo.
        pause
        exit /b 1
    )
    
    if %ERRORLEVEL% neq 0 (
        echo.
        echo Application exited with error code %ERRORLEVEL%
        pause
    )
)
