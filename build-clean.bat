@echo off
REM Simple batch file to build with suppressed warnings
if "%1"=="" (
    powershell -ExecutionPolicy Bypass -File build-clean.ps1
) else (
    powershell -ExecutionPolicy Bypass -File build-clean.ps1 -Filter "%1"
)

