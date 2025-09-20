@echo off
setlocal

:: Development Workflow Batch File for Flashcard App
:: This provides easy access to development tasks

if "%1"=="" goto show_help
if /i "%1"=="format" goto format_code
if /i "%1"=="test" goto run_tests
if /i "%1"=="build" goto build_solution
if /i "%1"=="coverage" goto get_coverage
if /i "%1"=="quality" goto run_quality
if /i "%1"=="clean" goto clean_build
if /i "%1"=="setup" goto setup_env
if /i "%1"=="hooks" goto setup_hooks
if /i "%1"=="help" goto show_help

echo Error: Unknown action '%1'
goto show_help

:format_code
echo Formatting code...
powershell.exe -ExecutionPolicy Bypass -File "scripts\dev-workflow.ps1" -Action format
goto end

:run_tests
echo Running tests...
if "%2"=="all" (
    powershell.exe -ExecutionPolicy Bypass -File "scripts\dev-workflow.ps1" -Action test -All
) else (
    powershell.exe -ExecutionPolicy Bypass -File "scripts\dev-workflow.ps1" -Action test
)
goto end

:build_solution
echo Building solution...
powershell.exe -ExecutionPolicy Bypass -File "scripts\dev-workflow.ps1" -Action build
goto end

:get_coverage
echo Generating coverage report...
powershell.exe -ExecutionPolicy Bypass -File "scripts\dev-workflow.ps1" -Action coverage
goto end

:run_quality
echo Running quality checks...
powershell.exe -ExecutionPolicy Bypass -File "scripts\dev-workflow.ps1" -Action quality
goto end

:clean_build
echo Cleaning build artifacts...
powershell.exe -ExecutionPolicy Bypass -File "scripts\dev-workflow.ps1" -Action clean
goto end

:setup_env
echo Setting up development environment...
powershell.exe -ExecutionPolicy Bypass -File "scripts\dev-workflow.ps1" -Action setup
goto end

:setup_hooks
echo Setting up Git hooks...
powershell.exe -ExecutionPolicy Bypass -File "scripts\setup-git-hooks.ps1"
goto end

:show_help
echo.
echo Flashcard App Development Workflow
echo =================================
echo.
echo Usage: dev.bat [action] [options]
echo.
echo Actions:
echo   format    - Format C# and XAML code
echo   test      - Run fast tests
echo   test all  - Run all tests
echo   build     - Build the solution
echo   coverage  - Generate code coverage report
echo   quality   - Run all quality checks
echo   clean     - Clean build artifacts
echo   setup     - Setup development environment
echo   hooks     - Setup Git hooks
echo   help      - Show this help message
echo.
echo Examples:
echo   dev.bat format
echo   dev.bat test all
echo   dev.bat quality
echo   dev.bat setup
echo.

:end
endlocal
