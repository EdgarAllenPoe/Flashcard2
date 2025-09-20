@echo off
echo 🔍 Checking XAML files for formatting issues...

xstyler --passive --directory Views
if %ERRORLEVEL% EQU 0 (
    echo ✅ All XAML files are properly formatted!
) else (
    echo ❌ Some XAML files have formatting issues.
    echo 🔧 Fixing formatting issues...
    xstyler --directory Views
    echo ✅ XAML files have been reformatted!
)

echo.
echo 📋 XAML Tools Available:
echo   • xstyler --passive --directory Views    # Check formatting
echo   • xstyler --directory Views              # Fix formatting
echo   • format-xaml Views                      # Alternative formatter
echo   • xstyler --write-to-stdout --file ^<file^> # Preview changes

echo.
echo 💡 Tips:
echo   • Run this script before committing XAML changes
echo   • Use 'xstyler --passive' in CI/CD pipelines
echo   • Configure .xamlstyler file for custom formatting rules
