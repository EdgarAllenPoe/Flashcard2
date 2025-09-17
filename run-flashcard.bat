@echo off
REM Flashcard App Launcher with Unicode Support
REM This batch file opens a new console window with proper emoji display

REM Get the current directory
set "CURRENT_DIR=%~dp0"

REM Create a temporary batch file that will run in the new console
echo @echo off > "%TEMP%\flashcard-launcher.bat"
echo REM Set console code page to UTF-8 for emoji support >> "%TEMP%\flashcard-launcher.bat"
echo chcp 65001 ^>nul 2^>^&1 >> "%TEMP%\flashcard-launcher.bat"
echo echo. >> "%TEMP%\flashcard-launcher.bat"
echo echo 🚀 Starting Flashcard App with emoji support... >> "%TEMP%\flashcard-launcher.bat"
echo echo. >> "%TEMP%\flashcard-launcher.bat"
echo cd /d "%CURRENT_DIR%" >> "%TEMP%\flashcard-launcher.bat"
echo. >> "%TEMP%\flashcard-launcher.bat"
echo REM Check if the published version exists >> "%TEMP%\flashcard-launcher.bat"
echo if exist "bin\Release\net8.0\win-x64\publish\Flashcard.exe" ^( >> "%TEMP%\flashcard-launcher.bat"
echo     "bin\Release\net8.0\win-x64\publish\Flashcard.exe" >> "%TEMP%\flashcard-launcher.bat"
echo ^) else if exist "Flashcard.exe" ^( >> "%TEMP%\flashcard-launcher.bat"
echo     Flashcard.exe >> "%TEMP%\flashcard-launcher.bat"
echo ^) else ^( >> "%TEMP%\flashcard-launcher.bat"
echo     echo ❌ Error: Flashcard.exe not found! >> "%TEMP%\flashcard-launcher.bat"
echo     echo Please run 'dotnet publish' first or ensure the executable is in the current directory. >> "%TEMP%\flashcard-launcher.bat"
echo     pause >> "%TEMP%\flashcard-launcher.bat"
echo     exit /b 1 >> "%TEMP%\flashcard-launcher.bat"
echo ^) >> "%TEMP%\flashcard-launcher.bat"
echo. >> "%TEMP%\flashcard-launcher.bat"
echo REM Keep console open if there was an error >> "%TEMP%\flashcard-launcher.bat"
echo if %%ERRORLEVEL%% neq 0 ^( >> "%TEMP%\flashcard-launcher.bat"
echo     echo. >> "%TEMP%\flashcard-launcher.bat"
echo     echo Application exited with error code %%ERRORLEVEL%% >> "%TEMP%\flashcard-launcher.bat"
echo     pause >> "%TEMP%\flashcard-launcher.bat"
echo ^) >> "%TEMP%\flashcard-launcher.bat"

REM Launch the new console window with the temporary batch file
start "Flashcard App" cmd /k "%TEMP%\flashcard-launcher.bat"

REM Clean up the temporary file after a short delay
timeout /t 2 /nobreak >nul
del "%TEMP%\flashcard-launcher.bat" >nul 2>&1
