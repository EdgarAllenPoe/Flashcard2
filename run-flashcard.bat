@echo off
REM Flashcard App Launcher with Unicode Support
REM This batch file sets up the console for proper emoji display

echo Setting up console for Unicode support...

REM Set console code page to UTF-8
chcp 65001 >nul 2>&1

REM Set console font to one that supports Unicode (if possible)
REM Note: This may not work on all systems, but it's worth trying

REM Run the flashcard application
echo Starting Flashcard App...
echo.

REM Check if the published version exists
if exist "bin\Release\net8.0\win-x64\publish\Flashcard.exe" (
    "bin\Release\net8.0\win-x64\publish\Flashcard.exe"
) else if exist "Flashcard.exe" (
    Flashcard.exe
) else (
    echo Error: Flashcard.exe not found!
    echo Please run 'dotnet publish' first or ensure the executable is in the current directory.
    pause
    exit /b 1
)

REM Keep console open if there was an error
if %ERRORLEVEL% neq 0 (
    echo.
    echo Application exited with error code %ERRORLEVEL%
    pause
)
