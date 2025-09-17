# Emoji Support Diagnostic Script
# This script tests various emoji display methods

Write-Host "Emoji Support Diagnostic Test" -ForegroundColor Cyan
Write-Host "=================================" -ForegroundColor Cyan
Write-Host ""

# Test 1: Basic emoji display
Write-Host "Test 1: Basic emoji display" -ForegroundColor Yellow
Write-Host "Target Rocket Sparkle Books Graduation Folder Chart Gear Question Door" -ForegroundColor White
Write-Host ""

# Test 2: Console encoding
Write-Host "Test 2: Console encoding" -ForegroundColor Yellow
Write-Host "Output Encoding: $([Console]::OutputEncoding.EncodingName)" -ForegroundColor White
Write-Host "Input Encoding: $([Console]::InputEncoding.EncodingName)" -ForegroundColor White
Write-Host "Code Page: $(chcp)" -ForegroundColor White
Write-Host ""

# Test 3: Console font info
Write-Host "Test 3: Console font information" -ForegroundColor Yellow
try {
    $font = $Host.UI.RawUI.Font
    Write-Host "Font: $($font.Name) Size: $($font.Width)x$($font.Height)" -ForegroundColor White
} catch {
    Write-Host "Font information not available" -ForegroundColor Red
}
Write-Host ""

# Test 4: Windows version
Write-Host "Test 4: Windows version" -ForegroundColor Yellow
$os = Get-WmiObject -Class Win32_OperatingSystem
Write-Host "OS: $($os.Caption) $($os.Version)" -ForegroundColor White
Write-Host ""

# Test 5: PowerShell version
Write-Host "Test 5: PowerShell version" -ForegroundColor Yellow
Write-Host "PowerShell: $($PSVersionTable.PSVersion)" -ForegroundColor White
Write-Host ""

# Test 6: Try different emoji display methods
Write-Host "Test 6: Different emoji display methods" -ForegroundColor Yellow
Write-Host "Method 1 (Direct): [TARGET]" -ForegroundColor White
Write-Host "Method 2 (Unicode): $([char]0x1F3AF)" -ForegroundColor White
Write-Host ""

# Test 7: Set UTF-8 and test again
Write-Host "Test 7: After setting UTF-8" -ForegroundColor Yellow
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::InputEncoding = [System.Text.Encoding]::UTF8
chcp 65001 | Out-Null
Write-Host "UTF-8 set, testing emoji display..." -ForegroundColor Green
Write-Host ""

# Test 8: Try to set console font
Write-Host "Test 8: Attempting to set console font" -ForegroundColor Yellow
try {
    # Try to set a font that supports emojis
    $Host.UI.RawUI.Font = New-Object System.Management.Automation.Host.Size(8, 16)
    Write-Host "Font set successfully" -ForegroundColor Green
} catch {
    Write-Host "Could not set font: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "Diagnostic complete!" -ForegroundColor Cyan
Write-Host "If you see question marks, boxes, or missing characters above," -ForegroundColor Yellow
Write-Host "your console font does not support emojis." -ForegroundColor Yellow
Write-Host ""
Read-Host "Press Enter to continue"