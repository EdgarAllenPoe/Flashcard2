# Git Hooks Setup Script for Flashcard App
# This script sets up pre-commit hooks for quality assurance

param(
    [switch]$Force,
    [switch]$Verbose
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

Write-Status "Setting up Git hooks for Flashcard App..." "INFO"

# Check if we're in a Git repository
if (-not (Test-Path ".git")) {
    Write-Status "Not in a Git repository. Please run this script from the project root." "ERROR"
    exit 1
}

# Create hooks directory if it doesn't exist
$hooksDir = ".git\hooks"
if (-not (Test-Path $hooksDir)) {
    New-Item -ItemType Directory -Path $hooksDir -Force | Out-Null
    Write-Status "Created Git hooks directory" "INFO"
}

# Pre-commit hook
$preCommitHook = Join-Path $hooksDir "pre-commit"
$preCommitScript = "scripts\pre-commit.ps1"

if (Test-Path $preCommitScript) {
    if ((Test-Path $preCommitHook) -and -not $Force) {
        Write-Status "Pre-commit hook already exists. Use -Force to overwrite." "WARNING"
    } else {
        # Create the pre-commit hook
        $hookContent = @"
#!/bin/sh
# Pre-commit hook for Flashcard App
# This hook runs quality checks before each commit

echo "Running pre-commit quality checks..."

# Run the PowerShell pre-commit script
powershell.exe -ExecutionPolicy Bypass -File "scripts\pre-commit.ps1"

# Check the exit code
if [ `$? -ne 0 ]; then
    echo "Pre-commit checks failed. Commit aborted."
    exit 1
fi

echo "Pre-commit checks passed. Proceeding with commit."
"@

        $hookContent | Out-File -FilePath $preCommitHook -Encoding ASCII
        Write-Status "Pre-commit hook created successfully" "SUCCESS"
    }
} else {
    Write-Status "Pre-commit script not found: $preCommitScript" "ERROR"
    exit 1
}

# Commit-msg hook (optional - for commit message validation)
$commitMsgHook = Join-Path $hooksDir "commit-msg"
$commitMsgContent = @"
#!/bin/sh
# Commit message validation hook for Flashcard App

commit_regex='^(feat|fix|docs|style|refactor|test|chore|perf|ci|build|revert)(\(.+\))?: .{1,50}'

if ! grep -qE "`$commit_regex" "`$1"; then
    echo "Invalid commit message format!"
    echo "Format: type(scope): description"
    echo "Types: feat, fix, docs, style, refactor, test, chore, perf, ci, build, revert"
    echo "Example: feat(config): add configuration page"
    exit 1
fi
"@

if ((Test-Path $commitMsgHook) -and -not $Force) {
    Write-Status "Commit-msg hook already exists. Use -Force to overwrite." "WARNING"
} else {
    $commitMsgContent | Out-File -FilePath $commitMsgHook -Encoding ASCII
    Write-Status "Commit-msg hook created successfully" "SUCCESS"
}

# Post-commit hook (optional - for notifications)
$postCommitHook = Join-Path $hooksDir "post-commit"
$postCommitContent = @"
#!/bin/sh
# Post-commit hook for Flashcard App
# This hook runs after each successful commit

echo "Commit completed successfully!"
echo "Consider running: .\scripts\dev-workflow.ps1 -Action quality"
"@

if ((Test-Path $postCommitHook) -and -not $Force) {
    Write-Status "Post-commit hook already exists. Use -Force to overwrite." "WARNING"
} else {
    $postCommitContent | Out-File -FilePath $postCommitHook -Encoding ASCII
    Write-Status "Post-commit hook created successfully" "SUCCESS"
}

# Make hooks executable (on Unix-like systems)
if ($IsLinux -or $IsMacOS) {
    chmod +x $preCommitHook
    chmod +x $commitMsgHook
    chmod +x $postCommitHook
    Write-Status "Made hooks executable" "INFO"
}

Write-Status "Git hooks setup completed!" "SUCCESS"
Write-Host ""
Write-Host "Hooks installed:" -ForegroundColor Cyan
Write-Host "  - pre-commit: Runs quality checks before each commit" -ForegroundColor White
Write-Host "  - commit-msg: Validates commit message format" -ForegroundColor White
Write-Host "  - post-commit: Shows success message after commit" -ForegroundColor White
Write-Host ""
Write-Host "To test the hooks:" -ForegroundColor Yellow
Write-Host "  git add ." -ForegroundColor White
Write-Host "  git commit -m 'test: test commit message'" -ForegroundColor White
Write-Host ""
Write-Host "To bypass hooks (not recommended):" -ForegroundColor Yellow
Write-Host "  git commit --no-verify -m 'message'" -ForegroundColor White
