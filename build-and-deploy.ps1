# Flashcard App Build and Deploy Script
# This script builds the application and deploys it to the Documents folder

param(
    [switch]$SkipBuild = $false,
    [string]$TargetPath = "$env:USERPROFILE\Documents\Flashcard"
)

Write-Host "Flashcard App Build and Deploy Script" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan

# Step 1: Build the application (unless skipped)
if (-not $SkipBuild) {
    Write-Host "Building application..." -ForegroundColor Yellow
    dotnet build --configuration Release
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Build failed!" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "Publishing application..." -ForegroundColor Yellow
    dotnet publish --configuration Release --runtime win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Publish failed!" -ForegroundColor Red
        exit 1
    }
    Write-Host "Build completed successfully!" -ForegroundColor Green
}

# Step 2: Create target directory
Write-Host "Creating target directory: $TargetPath" -ForegroundColor Yellow
if (Test-Path $TargetPath) {
    Write-Host "Directory already exists, cleaning it..." -ForegroundColor Yellow
    Remove-Item -Path $TargetPath -Recurse -Force
}
New-Item -ItemType Directory -Path $TargetPath -Force | Out-Null
Write-Host "Target directory created!" -ForegroundColor Green

# Step 3: Copy executable
$exeSource = "bin\Release\net8.0\win-x64\publish\Flashcard.exe"
$exeTarget = Join-Path $TargetPath "Flashcard.exe"

Write-Host "Copying executable..." -ForegroundColor Yellow
if (Test-Path $exeSource) {
    Copy-Item -Path $exeSource -Destination $exeTarget -Force
    Write-Host "Executable copied!" -ForegroundColor Green
} else {
    Write-Host "Executable not found at: $exeSource" -ForegroundColor Red
    exit 1
}

# Step 4: Copy or create config file
$configSource = "config.json"
$configTarget = Join-Path $TargetPath "config.json"

Write-Host "Setting up configuration..." -ForegroundColor Yellow
if (Test-Path $configSource) {
    Copy-Item -Path $configSource -Destination $configTarget -Force
    Write-Host "Configuration file copied!" -ForegroundColor Green
} else {
    Write-Host "Config file not found, creating default..." -ForegroundColor Yellow
    # Create default config if it doesn't exist
    $defaultConfig = @"
{
  "leitnerBoxes": {
    "numberOfBoxes": 5,
    "promotionRules": [
      {"boxNumber": 0, "correctAnswersNeeded": 1},
      {"boxNumber": 1, "correctAnswersNeeded": 2},
      {"boxNumber": 2, "correctAnswersNeeded": 3},
      {"boxNumber": 3, "correctAnswersNeeded": 4},
      {"boxNumber": 4, "correctAnswersNeeded": 5}
    ],
    "demotionRules": [
      {"boxNumber": 1, "incorrectAnswersNeeded": 1, "demoteToBox": 0},
      {"boxNumber": 2, "incorrectAnswersNeeded": 1, "demoteToBox": 0},
      {"boxNumber": 3, "incorrectAnswersNeeded": 1, "demoteToBox": 1},
      {"boxNumber": 4, "incorrectAnswersNeeded": 1, "demoteToBox": 2}
    ]
  },
  "studySession": {
    "defaultStudyMode": 0,
    "showStatistics": true,
    "autoAdvance": false,
    "autoAdvanceDelay": 3,
    "shuffleCards": true,
    "showProgress": true,
    "keyboardShortcuts": {
      "correctAnswer": "1",
      "incorrectAnswer": "2",
      "showAnswer": "space",
      "quit": "q",
      "skip": "s",
      "flipCard": "f",
      "showStatistics": "t",
      "help": "h"
    }
  },
  "filePaths": {
    "decksDirectory": "decks",
    "configFileName": "config.json",
    "deckFileExtension": ".json",
    "backupDirectory": "backups",
    "exportDirectory": "exports"
  },
  "reviewScheduling": {
    "boxIntervals": [
      {"boxNumber": 0, "intervalDays": 1},
      {"boxNumber": 1, "intervalDays": 3},
      {"boxNumber": 2, "intervalDays": 7},
      {"boxNumber": 3, "intervalDays": 14},
      {"boxNumber": 4, "intervalDays": 30}
    ],
    "newCardInterval": 1,
    "maxNewCardsPerDay": 20
  },
  "dailyLimits": {
    "maxCardsPerDay": 100,
    "minCardsPerDay": 5,
    "maxStudyTimePerDay": "02:00:00",
    "minStudyTimePerDay": "00:05:00"
  },
  "ui": {
    "useColors": true,
    "useIcons": true,
    "showWelcomeMessage": true,
    "clearScreenOnMenuChange": true,
    "showDetailedStatistics": true
  }
}
"@
    $defaultConfig | Out-File -FilePath $configTarget -Encoding UTF8
    Write-Host "Default configuration created!" -ForegroundColor Green
}

# Step 5: Copy launcher scripts
Write-Host "Setting up launcher scripts..." -ForegroundColor Yellow

# Copy batch launcher
$batchLauncherSource = "run-flashcard.bat"
$batchLauncherTarget = Join-Path $TargetPath "run-flashcard.bat"
if (Test-Path $batchLauncherSource) {
    Copy-Item -Path $batchLauncherSource -Destination $batchLauncherTarget -Force
    Write-Host "Batch launcher script copied!" -ForegroundColor Green
} else {
    Write-Host "Batch launcher script not found, skipping..." -ForegroundColor Yellow
}

# Copy PowerShell launcher
$psLauncherSource = "run-flashcard.ps1"
$psLauncherTarget = Join-Path $TargetPath "run-flashcard.ps1"
if (Test-Path $psLauncherSource) {
    Copy-Item -Path $psLauncherSource -Destination $psLauncherTarget -Force
    Write-Host "PowerShell launcher script copied!" -ForegroundColor Green
} else {
    Write-Host "PowerShell launcher script not found, skipping..." -ForegroundColor Yellow
}

# Step 6: Create example deck
Write-Host "Creating example deck..." -ForegroundColor Yellow
$decksDir = Join-Path $TargetPath "decks"
New-Item -ItemType Directory -Path $decksDir -Force | Out-Null

$currentDate = Get-Date -Format 'yyyy-MM-ddTHH:mm:ss'
$exampleDeck = @"
{
  "id": "example-deck-001",
  "name": "Sample Vocabulary Deck",
  "description": "A sample deck to get you started with the flashcard app",
  "flashcards": [
    {
      "id": "card-001",
      "front": "What is the capital of France?",
      "back": "Paris",
      "tags": ["geography", "capitals"],
      "statistics": {
        "totalReviews": 0,
        "correctAnswers": 0,
        "incorrectAnswers": 0,
        "averageResponseTime": 0.0,
        "totalStudyTime": "00:00:00",
        "lastStudySession": null,
        "streak": 0,
        "longestStreak": 0
      },
      "createdDate": "$currentDate",
      "lastReviewed": null,
      "nextReviewDate": null,
      "currentBox": 0,
      "isActive": true
    },
    {
      "id": "card-002",
      "front": "What is 2 + 2?",
      "back": "4",
      "tags": ["math", "basic"],
      "statistics": {
        "totalReviews": 0,
        "correctAnswers": 0,
        "incorrectAnswers": 0,
        "averageResponseTime": 0.0,
        "totalStudyTime": "00:00:00",
        "lastStudySession": null,
        "streak": 0,
        "longestStreak": 0
      },
      "createdDate": "$currentDate",
      "lastReviewed": null,
      "nextReviewDate": null,
      "currentBox": 0,
      "isActive": true
    },
    {
      "id": "card-003",
      "front": "What is the largest planet in our solar system?",
      "back": "Jupiter",
      "tags": ["science", "astronomy"],
      "statistics": {
        "totalReviews": 0,
        "correctAnswers": 0,
        "incorrectAnswers": 0,
        "averageResponseTime": 0.0,
        "totalStudyTime": "00:00:00",
        "lastStudySession": null,
        "streak": 0,
        "longestStreak": 0
      },
      "createdDate": "$currentDate",
      "lastReviewed": null,
      "nextReviewDate": null,
      "currentBox": 0,
      "isActive": true
    }
  ],
  "createdDate": "$currentDate",
  "lastModified": "$currentDate",
  "tags": ["example", "sample", "getting-started"],
  "statistics": {
    "totalStudySessions": 0,
    "totalStudyTime": "00:00:00",
    "averageSessionTime": "00:00:00",
    "lastStudySession": null,
    "cardsMastered": 0,
    "overallSuccessRate": 0.0,
    "studyStreak": 0,
    "longestStudyStreak": 0
  }
}
"@

$exampleDeckPath = Join-Path $decksDir "sample-vocabulary-deck.json"
$exampleDeck | Out-File -FilePath $exampleDeckPath -Encoding UTF8
Write-Host "Example deck created!" -ForegroundColor Green

# Step 7: Create additional directories
Write-Host "Creating additional directories..." -ForegroundColor Yellow
$backupsDir = Join-Path $TargetPath "backups"
$exportsDir = Join-Path $TargetPath "exports"
New-Item -ItemType Directory -Path $backupsDir -Force | Out-Null
New-Item -ItemType Directory -Path $exportsDir -Force | Out-Null
Write-Host "Additional directories created!" -ForegroundColor Green

# Step 8: Create a README for the deployed version
Write-Host "Creating README..." -ForegroundColor Yellow
$readmeContent = @"
# Flashcard App

A portable console application for spaced repetition learning using the Leitner box system.

## Quick Start

1. Double-click `run-flashcard.bat` for best emoji support (recommended)
   OR double-click `Flashcard.exe` to run the application directly
2. The app will automatically create necessary directories
3. Start with the included sample deck or create your own

## Features

- **Leitner Box System**: Spaced repetition with 5 configurable boxes
- **Multiple Study Modes**: Front-to-back, back-to-front, or mixed
- **Statistics Tracking**: Monitor your progress and success rates
- **Keyboard Shortcuts**: Efficient studying with hotkeys
- **Portable**: No installation required, runs from any folder

## Study Session Controls

- `SPACE` - Show answer
- `1` - Mark as correct
- `2` - Mark as incorrect
- `S` - Skip card
- `Q` - Quit session
- `H` - Show help

## File Structure

- `run-flashcard.bat` - Launcher with emoji support (recommended)
- `Flashcard.exe` - Main application
- `config.json` - Configuration settings
- `decks/` - Your flashcard decks
- `backups/` - Automatic backups
- `exports/` - Exported decks

## Getting Started

1. Run the application
2. Select "Manage Decks" to create your first deck
3. Add flashcards with questions and answers
4. Start studying with "Start Study Session"

Happy learning!
"@

$readmePath = Join-Path $TargetPath "README.txt"
$readmeContent | Out-File -FilePath $readmePath -Encoding UTF8
Write-Host "README created!" -ForegroundColor Green

# Summary
Write-Host ""
Write-Host "Deployment completed successfully!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host "Location: $TargetPath" -ForegroundColor Cyan
Write-Host "To run: Double-click Flashcard.exe" -ForegroundColor Cyan
Write-Host ""
Write-Host "Contents:" -ForegroundColor Yellow
Write-Host "  • Flashcard.exe (main application)" -ForegroundColor White
Write-Host "  • config.json (configuration)" -ForegroundColor White
Write-Host "  • decks/ (flashcard decks)" -ForegroundColor White
Write-Host "  • backups/ (automatic backups)" -ForegroundColor White
Write-Host "  • exports/ (exported decks)" -ForegroundColor White
Write-Host "  • README.txt (instructions)" -ForegroundColor White
Write-Host ""
Write-Host "Ready to start learning!" -ForegroundColor Green
