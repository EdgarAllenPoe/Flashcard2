# PowerShell script to build with suppressed warnings
param(
    [string]$Project = "FlashcardApp.WinUI",
    [string]$Filter = ""
)

if ($Filter -ne "") {
    dotnet test --filter $Filter --verbosity quiet 2>&1 | Where-Object { $_ -notmatch "warning" }
} else {
    dotnet build $Project --verbosity quiet 2>&1 | Where-Object { $_ -notmatch "warning" }
}

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build/Test completed successfully!" -ForegroundColor Green
} else {
    Write-Host "❌ Build/Test failed!" -ForegroundColor Red
}

