# XAML Validation and Formatting Script
# This script checks and formats XAML files in the Views directory

Write-Host "🔍 Checking XAML files for formatting issues..." -ForegroundColor Cyan

# Check formatting with XAML Styler
xstyler --passive --directory Views
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ All XAML files are properly formatted!" -ForegroundColor Green
} else {
    Write-Host "❌ Some XAML files have formatting issues." -ForegroundColor Red
    Write-Host "🔧 Fixing formatting issues..." -ForegroundColor Yellow
    
    # Fix formatting
    xstyler --directory Views
    Write-Host "✅ XAML files have been reformatted!" -ForegroundColor Green
}

Write-Host ""
Write-Host "📋 XAML Tools Available:" -ForegroundColor Cyan
Write-Host "  • xstyler --passive --directory Views    # Check formatting"
Write-Host "  • xstyler --directory Views              # Fix formatting"
Write-Host "  • format-xaml Views                      # Alternative formatter"
Write-Host "  • xstyler --write-to-stdout --file <file> # Preview changes"

Write-Host ""
Write-Host "💡 Tips:" -ForegroundColor Cyan
Write-Host "  • Run this script before committing XAML changes"
Write-Host "  • Use 'xstyler --passive' in CI/CD pipelines"
Write-Host "  • Configure .xamlstyler file for custom formatting rules"
