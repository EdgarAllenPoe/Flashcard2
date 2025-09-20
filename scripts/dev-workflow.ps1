# Development Workflow Script for Flashcard App
# This script provides various development tasks and quality checks

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("format", "test", "build", "coverage", "quality", "clean", "setup", "help")]
    [string]$Action = "help",
    
    [Parameter(Mandatory=$false)]
    [switch]$ShowVerbose,
    
    [Parameter(Mandatory=$false)]
    [switch]$All
)

$ErrorActionPreference = "Stop"

# Function to write colored output
function Write-Status {
    param([string]$Message, [string]$Status = "INFO")
    $color = switch ($Status) {
        "SUCCESS" { "Green" }
        "ERROR" { "Red" }
        "WARNING" { "Yellow" }
        "INFO" { "Cyan" }
        default { "White" }
    }
    Write-Host "[$Status] $Message" -ForegroundColor $color
}

# Function to run command with error handling
function Invoke-Command {
    param(
        [string]$Command,
        [string]$Description,
        [switch]$ContinueOnError
    )
    
    Write-Status $Description "INFO"
    
    try {
        if ($ShowVerbose) {
            Invoke-Expression $Command
        } else {
            Invoke-Expression $Command | Out-Null
        }
        
        if ($LASTEXITCODE -eq 0) {
            Write-Status "$Description completed successfully" "SUCCESS"
            return $true
        } else {
            if ($ContinueOnError) {
                Write-Status "$Description failed but continuing" "WARNING"
                return $false
            } else {
                Write-Status "$Description failed" "ERROR"
                return $false
            }
        }
    }
    catch {
        if ($ContinueOnError) {
            Write-Status "$Description failed with exception: $($_.Exception.Message)" "WARNING"
            return $false
        } else {
            Write-Status "$Description failed with exception: $($_.Exception.Message)" "ERROR"
            return $false
        }
    }
}

# Format code
function Format-Code {
    Write-Status "Starting code formatting..." "INFO"
    
    $success = $true
    
    # Format C# code
    if (-not (Invoke-Command "dotnet-format FlashcardApp.sln" "Formatting C# code" -ContinueOnError)) {
        $success = $false
    }
    
    # Format XAML code
    if (Test-Path "Views") {
        if (-not (Invoke-Command "xstyler --directory Views" "Formatting XAML code" -ContinueOnError)) {
            $success = $false
        }
    }
    
    if ($success) {
        Write-Status "Code formatting completed successfully" "SUCCESS"
    } else {
        Write-Status "Code formatting completed with some issues" "WARNING"
    }
}

# Run tests
function Test-Code {
    param([string]$Filter = "")
    
    Write-Status "Starting test execution..." "INFO"
    
    $testCommand = "dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release"
    
    if ($Filter) {
        $testCommand += " --filter `"$Filter`""
    }
    
    if ($ShowVerbose) {
        $testCommand += " --verbosity normal"
    } else {
        $testCommand += " --verbosity minimal"
    }
    
    if (Invoke-Command $testCommand "Running tests") {
        Write-Status "All tests passed" "SUCCESS"
    } else {
        Write-Status "Some tests failed" "ERROR"
        exit 1
    }
}

# Build solution
function Build-Solution {
    Write-Status "Starting build..." "INFO"
    
    if (Invoke-Command "dotnet build FlashcardApp.sln --configuration Release" "Building solution") {
        Write-Status "Build completed successfully" "SUCCESS"
    } else {
        Write-Status "Build failed" "ERROR"
        exit 1
    }
}

# Generate coverage report
function Get-Coverage {
    Write-Status "Starting coverage analysis..." "INFO"
    
    # Run tests with coverage
    $coverageCommand = "dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --collect:`"XPlat Code Coverage`" --results-directory ./TestResults"
    
    if (-not (Invoke-Command $coverageCommand "Running tests with coverage collection" -ContinueOnError)) {
        Write-Status "Coverage collection failed" "ERROR"
        return
    }
    
    # Generate coverage report
    if (Test-Command "reportgenerator") {
        if (Invoke-Command "reportgenerator -reports:`"TestResults/**/coverage.cobertura.xml`" -targetdir:`"coverage-report`" -reporttypes:`"Html;JsonSummary`"" "Generating coverage report" -ContinueOnError) {
            Write-Status "Coverage report generated in coverage-report/ directory" "SUCCESS"
        } else {
            Write-Status "Coverage report generation failed" "WARNING"
        }
    } else {
        Write-Status "reportgenerator tool not found. Please install it with: dotnet tool install --global dotnet-reportgenerator-globaltool" "WARNING"
    }
}

# Run quality checks
function Test-Quality {
    Write-Status "Starting quality checks..." "INFO"
    
    $success = $true
    
    # Code formatting check
    if (-not (Invoke-Command "dotnet-format FlashcardApp.sln --check" "Checking code formatting" -ContinueOnError)) {
        $success = $false
    }
    
    # XAML formatting check
    if (Test-Path "Views") {
        if (-not (Invoke-Command "xstyler --directory Views --passive" "Checking XAML formatting" -ContinueOnError)) {
            $success = $false
        }
    }
    
    # Build check
    if (-not (Invoke-Command "dotnet build FlashcardApp.sln --configuration Release --no-restore" "Checking build" -ContinueOnError)) {
        $success = $false
    }
    
    # Fast tests
    if (-not (Invoke-Command "dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter `"Category=Fast`" --no-build" "Running fast tests" -ContinueOnError)) {
        $success = $false
    }
    
    # Security check
    if (-not (Invoke-Command "dotnet list FlashcardApp.sln package --vulnerable --include-transitive" "Checking for vulnerable packages" -ContinueOnError)) {
        $success = $false
    }
    
    if ($success) {
        Write-Status "All quality checks passed" "SUCCESS"
    } else {
        Write-Status "Some quality checks failed" "WARNING"
    }
}

# Clean build artifacts
function Clear-Build {
    Write-Status "Cleaning build artifacts..." "INFO"
    
    $directories = @("bin", "obj", "TestResults", "coverage-report", "publish")
    
    foreach ($dir in $directories) {
        if (Test-Path $dir) {
            Remove-Item -Recurse -Force $dir
            Write-Status "Removed $dir" "INFO"
        }
    }
    
    Write-Status "Build artifacts cleaned" "SUCCESS"
}

# Setup development environment
function Initialize-Environment {
    Write-Status "Setting up development environment..." "INFO"
    
    # Restore packages
    if (Invoke-Command "dotnet restore FlashcardApp.sln" "Restoring NuGet packages") {
        Write-Status "Packages restored successfully" "SUCCESS"
    } else {
        Write-Status "Package restoration failed" "ERROR"
        return
    }
    
    # Build solution
    if (Invoke-Command "dotnet build FlashcardApp.sln --configuration Release" "Building solution") {
        Write-Status "Solution built successfully" "SUCCESS"
    } else {
        Write-Status "Solution build failed" "ERROR"
        return
    }
    
    # Run fast tests
    if (Invoke-Command "dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter `"Category=Fast`" --no-build" "Running fast tests") {
        Write-Status "Fast tests passed" "SUCCESS"
    } else {
        Write-Status "Fast tests failed" "WARNING"
    }
    
    Write-Status "Development environment setup completed" "SUCCESS"
}

# Show help
function Show-Help {
    Write-Host "Flashcard App Development Workflow" -ForegroundColor Cyan
    Write-Host "=================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage: .\scripts\dev-workflow.ps1 -Action <action> [-Verbose] [-All]" -ForegroundColor White
    Write-Host ""
    Write-Host "Actions:" -ForegroundColor Yellow
    Write-Host "  format    - Format C# and XAML code" -ForegroundColor White
    Write-Host "  test      - Run tests (use -All for all tests)" -ForegroundColor White
    Write-Host "  build     - Build the solution" -ForegroundColor White
    Write-Host "  coverage  - Generate code coverage report" -ForegroundColor White
    Write-Host "  quality   - Run all quality checks" -ForegroundColor White
    Write-Host "  clean     - Clean build artifacts" -ForegroundColor White
    Write-Host "  setup     - Setup development environment" -ForegroundColor White
    Write-Host "  help      - Show this help message" -ForegroundColor White
    Write-Host ""
Write-Host "Options:" -ForegroundColor Yellow
Write-Host "  -ShowVerbose  - Show detailed output" -ForegroundColor White
Write-Host "  -All      - Run all tests (not just fast ones)" -ForegroundColor White
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor Yellow
    Write-Host "  .\scripts\dev-workflow.ps1 -Action format" -ForegroundColor White
    Write-Host "  .\scripts\dev-workflow.ps1 -Action test -All" -ForegroundColor White
    Write-Host "  .\scripts\dev-workflow.ps1 -Action quality -ShowVerbose" -ForegroundColor White
    Write-Host ""
}

# Main execution
switch ($Action) {
    "format" { Format-Code }
    "test" { 
        if ($All) {
            Test-Code
        } else {
            Test-Code -Filter "Category=Fast"
        }
    }
    "build" { Build-Solution }
    "coverage" { Get-Coverage }
    "quality" { Test-Quality }
    "clean" { Clear-Build }
    "setup" { Initialize-Environment }
    "help" { Show-Help }
    default { Show-Help }
}
