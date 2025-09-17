@echo off
echo Flashcard App Build and Deploy Script
echo =========================================

REM Run the PowerShell script
powershell.exe -ExecutionPolicy Bypass -File "build-and-deploy.ps1"

REM Pause to see the results
pause

