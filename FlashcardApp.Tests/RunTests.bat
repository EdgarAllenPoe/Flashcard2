@echo off
REM Batch file to run tests with different profiles
REM Usage: RunTests.bat [ProfileName]

setlocal enabledelayedexpansion

if "%1"=="" goto :show_help
if "%1"=="help" goto :show_help
if "%1"=="-h" goto :show_help
if "%1"=="--help" goto :show_help

set PROFILE=%1

REM Define test profiles
if "%PROFILE%"=="dev" set PROFILE=Development
if "%PROFILE%"=="ci" set PROFILE=Continuous Integration
if "%PROFILE%"=="full" set PROFILE=Full Validation
if "%PROFILE%"=="perf" set PROFILE=Performance Testing

echo Running tests with profile: %PROFILE%
echo.

if "%PROFILE%"=="Development" (
    echo Running fast tests for development...
    dotnet test --filter "Category=Fast" -v minimal
) else if "%PROFILE%"=="Continuous Integration" (
    echo Running CI/CD tests...
    dotnet test --filter "Category=Fast|Category=Integration" -v minimal
) else if "%PROFILE%"=="Full Validation" (
    echo Running all tests including performance...
    dotnet test -v minimal
) else if "%PROFILE%"=="Performance Testing" (
    echo Running performance benchmarks...
    dotnet test --filter "Category=Performance" -v minimal
) else (
    echo Error: Unknown profile '%PROFILE%'
    goto :show_help
)

goto :end

:show_help
echo FlashcardApp Test Runner
echo =======================
echo.
echo Usage: RunTests.bat [ProfileName]
echo.
echo Available Profiles:
echo   dev, Development          - Fast tests for development
echo   ci, "Continuous Integration" - CI/CD tests
echo   full, "Full Validation"   - All tests including performance
echo   perf, "Performance Testing" - Performance benchmarks only
echo.
echo Examples:
echo   RunTests.bat dev
echo   RunTests.bat ci
echo   RunTests.bat full
echo   RunTests.bat perf
echo.
echo For more options, use: powershell -ExecutionPolicy Bypass -File RunTests.ps1 -Help

:end
