# PowerShell script to fork FlashcardApp to a new GitHub repository
# This script will help you set up a new repository with updated information

param(
    [Parameter(Mandatory=$true)]
    [string]$GitHubUsername,
    
    [Parameter(Mandatory=$true)]
    [string]$RepositoryName
)

Write-Host "🚀 Forking FlashcardApp to new repository..." -ForegroundColor Green
Write-Host "GitHub Username: $GitHubUsername" -ForegroundColor Cyan
Write-Host "Repository Name: $RepositoryName" -ForegroundColor Cyan

# Check if we're in a git repository
if (-not (Test-Path ".git")) {
    Write-Error "❌ Not in a Git repository. Please run this script from the project root."
    exit 1
}

# Update README.md with new repository information
Write-Host "📝 Updating README.md..." -ForegroundColor Yellow
$readmePath = "README.md"
if (Test-Path $readmePath) {
    $readmeContent = Get-Content $readmePath -Raw
    $readmeContent = $readmeContent -replace "YOUR_USERNAME", $GitHubUsername
    $readmeContent = $readmeContent -replace "FlashcardApp", $RepositoryName
    Set-Content $readmePath $readmeContent -NoNewline
    Write-Host "✅ README.md updated" -ForegroundColor Green
} else {
    Write-Warning "⚠️ README.md not found"
}

# Update remote URL
$newRemoteUrl = "https://github.com/$GitHubUsername/$RepositoryName.git"
Write-Host "🔗 Updating remote URL to: $newRemoteUrl" -ForegroundColor Yellow

try {
    git remote set-url origin $newRemoteUrl
    Write-Host "✅ Remote URL updated" -ForegroundColor Green
} catch {
    Write-Error "❌ Failed to update remote URL: $_"
    exit 1
}

# Verify remote URL
Write-Host "🔍 Verifying remote configuration..." -ForegroundColor Yellow
git remote -v

# Add and commit changes
Write-Host "💾 Committing changes..." -ForegroundColor Yellow
try {
    git add .
    git commit -m "Update repository information for fork"
    Write-Host "✅ Changes committed" -ForegroundColor Green
} catch {
    Write-Warning "⚠️ No changes to commit or commit failed: $_"
}

# Push to new repository
Write-Host "🚀 Pushing to new repository..." -ForegroundColor Yellow
Write-Host "⚠️  Make sure you have created the repository '$RepositoryName' on GitHub first!" -ForegroundColor Red
Write-Host "Press any key to continue with the push, or Ctrl+C to cancel..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

try {
    git push -u origin master
    Write-Host "✅ Successfully pushed to new repository!" -ForegroundColor Green
} catch {
    Write-Error "❌ Failed to push to repository. Make sure:"
    Write-Error "   1. The repository '$RepositoryName' exists on GitHub"
    Write-Error "   2. You have push access to the repository"
    Write-Error "   3. You're authenticated with GitHub"
    exit 1
}

Write-Host ""
Write-Host "🎉 Fork completed successfully!" -ForegroundColor Green
Write-Host "📋 Next steps:" -ForegroundColor Cyan
Write-Host "   1. Visit your new repository: https://github.com/$GitHubUsername/$RepositoryName" -ForegroundColor White
Write-Host "   2. Update the repository description and topics" -ForegroundColor White
Write-Host "   3. Consider adding a LICENSE file if needed" -ForegroundColor White
Write-Host "   4. Update any CI/CD workflows if present" -ForegroundColor White
Write-Host ""
Write-Host "🔗 Repository URL: https://github.com/$GitHubUsername/$RepositoryName" -ForegroundColor Green
