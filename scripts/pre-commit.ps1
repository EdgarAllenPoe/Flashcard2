# Pre-commit hook script for Flashcard App
# This script runs quality checks before each commit

param(
    [switch]$SkipTests,
    [switch]$SkipFormatting,
    [switch]$ShowVerbose
)

$ErrorActionPreference = "Stop"

Write-Host "🔍 Running pre-commit quality checks..." -ForegroundColor Cyan

# Function to write colored output
function Write-Status {
    param([string]$Message, [string]$Status = "INFO")
    $color = switch ($Status) {
        "SUCCESS" { "Green" }
        "ERROR" { "Red" }
        "WARNING" { "Yellow" }
        default { "White" }
    }
    Write-Host "[$Status] $Message" -ForegroundColor $color
}

# Function to check if a command exists
function Test-Command {
    param([string]$Command)
    try {
        Get-Command $Command -ErrorAction Stop | Out-Null
        return $true
    }
    catch {
        return $false
    }
}

# Check if required tools are installed
Write-Status "Checking required tools..." "INFO"
$requiredTools = @("dotnet", "dotnet-format", "xstyler")
$missingTools = @()

foreach ($tool in $requiredTools) {
    if (-not (Test-Command $tool)) {
        $missingTools += $tool
    }
}

if ($missingTools.Count -gt 0) {
    Write-Status "Missing required tools: $($missingTools -join ', ')" "ERROR"
    Write-Status "Please install missing tools and try again." "ERROR"
    exit 1
}

Write-Status "All required tools are available" "SUCCESS"

# Get the solution file
$solutionFile = "FlashcardApp.sln"
if (-not (Test-Path $solutionFile)) {
    Write-Status "Solution file not found: $solutionFile" "ERROR"
    exit 1
}

# 1. Code Formatting Check
if (-not $SkipFormatting) {
    Write-Status "Running code formatting check..." "INFO"
    
    try {
        $formatResult = dotnet-format FlashcardApp.sln --check --verbosity minimal 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Status "Code formatting check passed" "SUCCESS"
        } else {
            Write-Status "Code formatting issues found. Please run 'dotnet-format' to fix them." "ERROR"
            if ($ShowVerbose) {
                Write-Host $formatResult
            }
            exit 1
        }
    }
    catch {
        Write-Status "Error running code formatting check: $($_.Exception.Message)" "ERROR"
        exit 1
    }
} else {
    Write-Status "Skipping code formatting check" "WARNING"
}

# 2. XAML Formatting Check
if (-not $SkipFormatting) {
    Write-Status "Running XAML formatting check..." "INFO"
    
    try {
        if (Test-Path "Views") {
            $xamlResult = xstyler --directory Views --passive 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Status "XAML formatting check passed" "SUCCESS"
            } else {
                Write-Status "XAML formatting issues found. Please run 'xstyler --directory Views' to fix them." "ERROR"
                if ($ShowVerbose) {
                    Write-Host $xamlResult
                }
                exit 1
            }
        } else {
            Write-Status "Views directory not found, skipping XAML check" "WARNING"
        }
    }
    catch {
        Write-Status "Error running XAML formatting check: $($_.Exception.Message)" "ERROR"
        exit 1
    }
} else {
    Write-Status "Skipping XAML formatting check" "WARNING"
}

# 3. Build Check
Write-Status "Running build check..." "INFO"

try {
    $buildResult = dotnet build $solutionFile --configuration Release --no-restore --verbosity minimal 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Status "Build check passed" "SUCCESS"
    } else {
        Write-Status "Build failed. Please fix build errors before committing." "ERROR"
        if ($ShowVerbose) {
            Write-Host $buildResult
        }
        exit 1
    }
}
catch {
    Write-Status "Error running build check: $($_.Exception.Message)" "ERROR"
    exit 1
}

# 4. Fast Tests
if (-not $SkipTests) {
    Write-Status "Running fast tests..." "INFO"
    
    try {
        $testResult = dotnet test FlashcardApp.Tests/FlashcardApp.Tests.csproj --configuration Release --filter "Category=Fast" --verbosity minimal --no-build 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Status "Fast tests passed" "SUCCESS"
        } else {
            Write-Status "Fast tests failed. Please fix test failures before committing." "ERROR"
            if ($ShowVerbose) {
                Write-Host $testResult
            }
            exit 1
        }
    }
    catch {
        Write-Status "Error running fast tests: $($_.Exception.Message)" "ERROR"
        exit 1
    }
} else {
    Write-Status "Skipping tests" "WARNING"
}

# 5. Code Analysis
Write-Status "Running code analysis..." "INFO"

try {
    $analysisResult = dotnet build $solutionFile --configuration Release --verbosity minimal /p:RunAnalyzersDuringBuild=true /p:EnableNETAnalyzers=true --no-restore 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Status "Code analysis passed" "SUCCESS"
    } else {
        Write-Status "Code analysis found issues. Please review and fix them." "WARNING"
        if ($ShowVerbose) {
            Write-Host $analysisResult
        }
        # Don't fail the commit for analysis warnings, just warn
    }
}
catch {
    Write-Status "Error running code analysis: $($_.Exception.Message)" "WARNING"
    # Don't fail the commit for analysis errors, just warn
}

# 6. Security Check
Write-Status "Running security check..." "INFO"

try {
    $securityResult = dotnet list FlashcardApp.sln package --vulnerable --include-transitive 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Status "Security check passed" "SUCCESS"
    } else {
        Write-Status "Security vulnerabilities found. Please update vulnerable packages." "WARNING"
        if ($ShowVerbose) {
            Write-Host $securityResult
        }
        # Don't fail the commit for security warnings, just warn
    }
}
catch {
    Write-Status "Error running security check: $($_.Exception.Message)" "WARNING"
    # Don't fail the commit for security errors, just warn
}

Write-Status "All pre-commit checks completed successfully!" "SUCCESS"
Write-Host "✅ Ready to commit!" -ForegroundColor Green
