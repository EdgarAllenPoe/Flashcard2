# PowerShell script to run tests with different profiles
param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("Development", "Continuous Integration", "Full Validation", "Performance Testing", "List")]
    [string]$Profile = "List",
    
    [Parameter(Mandatory=$false)]
    [switch]$ShowVerbose,
    
    [Parameter(Mandatory=$false)]
    [switch]$Help
)

if ($Help) {
    Write-Host "FlashcardApp Test Runner" -ForegroundColor Green
    Write-Host "=======================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Usage: .\RunTests.ps1 [-Profile <ProfileName>] [-Verbose] [-Help]" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Available Profiles:" -ForegroundColor Cyan
    Write-Host "  Development          - Fast tests for development (2 min timeout)" -ForegroundColor White
    Write-Host "  'Continuous Integration' - CI/CD tests (10 min timeout)" -ForegroundColor White
    Write-Host "  'Full Validation'    - All tests including performance (30 min timeout)" -ForegroundColor White
    Write-Host "  'Performance Testing' - Performance benchmarks only (15 min timeout)" -ForegroundColor White
    Write-Host "  List                 - Show all profiles and exit" -ForegroundColor White
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor Cyan
    Write-Host "  .\RunTests.ps1 -Profile Development" -ForegroundColor White
    Write-Host "  .\RunTests.ps1 -Profile 'Continuous Integration' -ShowVerbose" -ForegroundColor White
    Write-Host "  .\RunTests.ps1 -Profile List" -ForegroundColor White
    exit 0
}

# Define test profiles
$profiles = @{
    "Development" = @{
        Filter = "Category=Fast"
        Timeout = 120000
        Description = "Fast tests for development workflow"
    }
    "Continuous Integration" = @{
        Filter = "Category=Fast|Category=Integration"
        Timeout = 600000
        Description = "Comprehensive tests for CI/CD pipeline"
    }
    "Full Validation" = @{
        Filter = ""
        Timeout = 1800000
        Description = "All tests including performance benchmarks"
    }
    "Performance Testing" = @{
        Filter = "Category=Performance"
        Timeout = 900000
        Description = "Performance benchmarks and critical path tests"
    }
}

if ($Profile -eq "List") {
    Write-Host "Available Test Profiles:" -ForegroundColor Green
    Write-Host "=======================" -ForegroundColor Green
    Write-Host ""
    
    foreach ($profileName in $profiles.Keys) {
        $profile = $profiles[$profileName]
        Write-Host "Profile: $profileName" -ForegroundColor Yellow
        Write-Host "  Description: $($profile.Description)" -ForegroundColor White
        Write-Host "  Filter: $($profile.Filter)" -ForegroundColor White
        Write-Host "  Timeout: $([math]::Round($profile.Timeout / 60000, 1)) minutes" -ForegroundColor White
        Write-Host ""
    }
    
    Write-Host "Usage Examples:" -ForegroundColor Cyan
    Write-Host "  .\RunTests.ps1 -Profile Development" -ForegroundColor White
    Write-Host "  .\RunTests.ps1 -Profile 'Continuous Integration'" -ForegroundColor White
    Write-Host "  .\RunTests.ps1 -Profile 'Full Validation'" -ForegroundColor White
    Write-Host "  .\RunTests.ps1 -Profile 'Performance Testing'" -ForegroundColor White
    exit 0
}

if (-not $profiles.ContainsKey($Profile)) {
    Write-Host "Error: Profile '$Profile' not found!" -ForegroundColor Red
    Write-Host "Use -Help to see available profiles" -ForegroundColor Yellow
    exit 1
}

$selectedProfile = $profiles[$Profile]
Write-Host "Running tests with profile: $Profile" -ForegroundColor Green
Write-Host "Description: $($selectedProfile.Description)" -ForegroundColor White
Write-Host "Filter: $($selectedProfile.Filter)" -ForegroundColor White
Write-Host "Timeout: $([math]::Round($selectedProfile.Timeout / 60000, 1)) minutes" -ForegroundColor White
Write-Host ""

# Build the dotnet test command
$testArgs = @("test")

if ($selectedProfile.Filter) {
    $testArgs += "--filter"
    $testArgs += $selectedProfile.Filter
}

# Note: Timeout is handled by VSTest, not dotnet test directly
# $testArgs += "--timeout"
# $testArgs += $selectedProfile.Timeout.ToString()

if ($ShowVerbose) {
    $testArgs += "-v"
    $testArgs += "normal"
} else {
    $testArgs += "-v"
    $testArgs += "minimal"
}

# Add performance optimizations
$testArgs += "--logger"
$testArgs += "console;verbosity=minimal"

Write-Host "Command: dotnet $($testArgs -join ' ')" -ForegroundColor Cyan
Write-Host ""

# Run the tests
$startTime = Get-Date
try {
    & dotnet $testArgs
    $exitCode = $LASTEXITCODE
} catch {
    Write-Host "Error running tests: $($_.Exception.Message)" -ForegroundColor Red
    $exitCode = 1
}
$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host ""
Write-Host "Test execution completed in $([math]::Round($duration.TotalSeconds, 1)) seconds" -ForegroundColor Green

if ($exitCode -eq 0) {
    Write-Host "All tests passed! ✅" -ForegroundColor Green
} else {
    Write-Host "Some tests failed! ❌" -ForegroundColor Red
}

exit $exitCode
