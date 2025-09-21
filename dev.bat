@echo off
setlocal

:: Development Workflow Batch File for Flashcard App
:: This provides easy access to development tasks

if "%1"=="" goto show_help
if /i "%1"=="format" goto format_code
if /i "%1"=="test" goto run_tests
if /i "%1"=="winui" goto run_winui_tests
if /i "%1"=="integration" goto run_integration_tests
if /i "%1"=="performance" goto run_performance_tests
if /i "%1"=="config" goto run_config_tests
if /i "%1"=="edge" goto run_edge_tests
if /i "%1"=="models" goto run_model_tests
if /i "%1"=="ui" goto run_ui_tests
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

:run_winui_tests
echo Running WinUI tests (fast ones only)...
dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter "Category=Fast&FullyQualifiedName~WinUI" --logger "console;verbosity=normal"
goto end

:run_integration_tests
echo Running integration tests (fixed ones only)...
dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter "FullyQualifiedName~WinUIStudySessionIntegrationTests" --logger "console;verbosity=normal"
goto end

:run_performance_tests
echo Running performance tests...
dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter "Category=Performance" --logger "console;verbosity=normal"
goto end

:run_config_tests
echo Running configuration tests...
dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter "FullyQualifiedName~Configuration" --logger "console;verbosity=normal"
goto end

:run_edge_tests
echo Running edge case tests...
dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter "FullyQualifiedName~EdgeCase" --logger "console;verbosity=normal"
goto end

:run_model_tests
echo Running model tests...
dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter "FullyQualifiedName~Models" --logger "console;verbosity=normal"
goto end

:run_ui_tests
echo Running UI tests (non-WinUI specific)...
dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter "FullyQualifiedName~UI&FullyQualifiedName!~WinUI" --logger "console;verbosity=normal"
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
echo   format      - Format C# and XAML code
echo   test        - Run fast tests (recommended for development)
echo   test all    - Run all tests (may hang due to integration tests)
echo   winui       - Run WinUI fast tests only
echo   integration - Run fixed integration tests only
echo   performance - Run performance benchmark tests
echo   config      - Run configuration-related tests
echo   edge        - Run edge case tests
echo   models      - Run model tests
echo   ui          - Run UI tests (non-WinUI specific)
echo   build       - Build the solution
echo   coverage    - Generate code coverage report
echo   quality     - Run all quality checks
echo   clean       - Clean build artifacts
echo   setup       - Setup development environment
echo   hooks       - Setup Git hooks
echo   help        - Show this help message
echo.
echo Examples:
echo   dev.bat format
echo   dev.bat test
echo   dev.bat winui
echo   dev.bat config
echo   dev.bat edge
echo   dev.bat models
echo   dev.bat integration
echo   dev.bat performance
echo   dev.bat quality
echo   dev.bat setup
echo.

:end
endlocal
