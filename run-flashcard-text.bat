@echo off
REM Flashcard App Launcher with Text Alternatives
REM This launcher disables emojis and uses text alternatives

echo Starting Flashcard App with text alternatives...

REM Set environment variable to disable emojis
set FLASHCARD_NO_EMOJIS=1

REM Run the application
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

if %ERRORLEVEL% neq 0 (
    echo.
    echo Application exited with error code %ERRORLEVEL%
    pause
)
