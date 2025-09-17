# Flashcard App Deploy Script
param(
    [string]$TargetPath = "$env:USERPROFILE\Documents\Flashcard"
)

Write-Host "Flashcard App Deploy Script" -ForegroundColor Cyan
Write-Host "==============================" -ForegroundColor Cyan

# Step 1: Create target directory
Write-Host "Creating target directory: $TargetPath" -ForegroundColor Yellow
if (Test-Path $TargetPath) {
    Write-Host "Directory already exists, cleaning it..." -ForegroundColor Yellow
    Remove-Item -Path $TargetPath -Recurse -Force
}
New-Item -ItemType Directory -Path $TargetPath -Force | Out-Null
Write-Host "Target directory created!" -ForegroundColor Green

# Step 2: Copy executable
$exeSource = "bin\Release\net8.0\win-x64\publish\Flashcard.exe"
$exeTarget = Join-Path $TargetPath "Flashcard.exe"

Write-Host "Copying executable..." -ForegroundColor Yellow
if (Test-Path $exeSource) {
    Copy-Item -Path $exeSource -Destination $exeTarget -Force
    Write-Host "Executable copied!" -ForegroundColor Green
} else {
    Write-Host "Executable not found at: $exeSource" -ForegroundColor Red
    Write-Host "Please run 'dotnet publish' first!" -ForegroundColor Red
    exit 1
}

# Step 3: Copy config file
Write-Host "Setting up configuration..." -ForegroundColor Yellow
$configSource = "config.json"
$configTarget = Join-Path $TargetPath "config.json"

if (Test-Path $configSource) {
    Copy-Item -Path $configSource -Destination $configTarget -Force
    Write-Host "Configuration file copied!" -ForegroundColor Green
} else {
    Write-Host "Config file not found, creating default..." -ForegroundColor Yellow
    $defaultConfig = @{
        leitnerBoxes = @{
            numberOfBoxes = 5
            promotionRules = @(
                @{boxNumber = 0; correctAnswersNeeded = 1}
                @{boxNumber = 1; correctAnswersNeeded = 2}
                @{boxNumber = 2; correctAnswersNeeded = 3}
                @{boxNumber = 3; correctAnswersNeeded = 4}
                @{boxNumber = 4; correctAnswersNeeded = 5}
            )
            demotionRules = @(
                @{boxNumber = 1; incorrectAnswersNeeded = 1; demoteToBox = 0}
                @{boxNumber = 2; incorrectAnswersNeeded = 1; demoteToBox = 0}
                @{boxNumber = 3; incorrectAnswersNeeded = 1; demoteToBox = 1}
                @{boxNumber = 4; incorrectAnswersNeeded = 1; demoteToBox = 2}
            )
        }
        studySession = @{
            defaultStudyMode = 0
            showStatistics = $true
            autoAdvance = $false
            autoAdvanceDelay = 3
            shuffleCards = $true
            showProgress = $true
            keyboardShortcuts = @{
                correctAnswer = "1"
                incorrectAnswer = "2"
                showAnswer = "space"
                quit = "q"
                skip = "s"
                flipCard = "f"
                showStatistics = "t"
                help = "h"
            }
        }
        filePaths = @{
            decksDirectory = "decks"
            configFileName = "config.json"
            deckFileExtension = ".json"
            backupDirectory = "backups"
            exportDirectory = "exports"
        }
        reviewScheduling = @{
            boxIntervals = @(
                @{boxNumber = 0; intervalDays = 1}
                @{boxNumber = 1; intervalDays = 3}
                @{boxNumber = 2; intervalDays = 7}
                @{boxNumber = 3; intervalDays = 14}
                @{boxNumber = 4; intervalDays = 30}
            )
            newCardInterval = 1
            maxNewCardsPerDay = 20
        }
        dailyLimits = @{
            maxCardsPerDay = 100
            minCardsPerDay = 5
            maxStudyTimePerDay = "02:00:00"
            minStudyTimePerDay = "00:05:00"
        }
        ui = @{
            useColors = $true
            useIcons = $false
            showWelcomeMessage = $true
            clearScreenOnMenuChange = $true
            showDetailedStatistics = $true
        }
    }
    
    $defaultConfig | ConvertTo-Json -Depth 10 | Out-File -FilePath $configTarget -Encoding UTF8
    Write-Host "Default configuration created!" -ForegroundColor Green
}

# Step 4: Create directories
Write-Host "Creating directories..." -ForegroundColor Yellow
New-Item -ItemType Directory -Path (Join-Path $TargetPath "decks") -Force | Out-Null
New-Item -ItemType Directory -Path (Join-Path $TargetPath "backups") -Force | Out-Null
New-Item -ItemType Directory -Path (Join-Path $TargetPath "exports") -Force | Out-Null
Write-Host "Directories created!" -ForegroundColor Green

# Step 5: Copy example deck
Write-Host "Setting up example deck..." -ForegroundColor Yellow
$deckSource = "sample-vocabulary-deck.json"
$deckTarget = Join-Path $TargetPath "decks\sample-vocabulary-deck.json"

if (Test-Path $deckSource) {
    Copy-Item -Path $deckSource -Destination $deckTarget -Force
    Write-Host "Example deck copied!" -ForegroundColor Green
} else {
    Write-Host "Example deck not found, skipping..." -ForegroundColor Yellow
}

# Step 6: Create README
Write-Host "Creating README..." -ForegroundColor Yellow
$readmeContent = @"
# Flashcard App

A portable console application for spaced repetition learning using the Leitner box system.

## Quick Start

1. Double-click Flashcard.exe to run the application
2. The app will automatically create necessary directories
3. Start with the included sample deck or create your own

## Features

- Leitner Box System: Spaced repetition with 5 configurable boxes
- Multiple Study Modes: Front-to-back, back-to-front, or mixed
- Statistics Tracking: Monitor your progress and success rates
- Keyboard Shortcuts: Efficient studying with hotkeys
- Portable: No installation required, runs from any folder

## Study Session Controls

- SPACE - Show answer
- 1 - Mark as correct
- 2 - Mark as incorrect
- S - Skip card
- Q - Quit session
- H - Show help

## Getting Started

1. Run the application
2. Select "Manage Decks" to create your first deck
3. Add flashcards with questions and answers
4. Start studying with "Start Study Session"

Happy learning!
"@

$readmeContent | Out-File -FilePath (Join-Path $TargetPath "README.txt") -Encoding UTF8
Write-Host "README created!" -ForegroundColor Green

# Summary
Write-Host ""
Write-Host "Deployment completed successfully!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host "Location: $TargetPath" -ForegroundColor Cyan
Write-Host "To run: Double-click Flashcard.exe" -ForegroundColor Cyan
Write-Host ""
Write-Host "Contents:" -ForegroundColor Yellow
Write-Host "  - Flashcard.exe (main application)" -ForegroundColor White
Write-Host "  - config.json (configuration)" -ForegroundColor White
Write-Host "  - decks/ (flashcard decks)" -ForegroundColor White
Write-Host "  - backups/ (automatic backups)" -ForegroundColor White
Write-Host "  - exports/ (exported decks)" -ForegroundColor White
Write-Host "  - README.txt (instructions)" -ForegroundColor White
Write-Host ""
Write-Host "Ready to start learning!" -ForegroundColor Green