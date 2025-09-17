@echo off
echo Flashcard App Deploy Script
echo ==============================

REM Set target path
set TARGET_PATH=%USERPROFILE%\Documents\Flashcard

REM Step 1: Create target directory
echo Creating target directory: %TARGET_PATH%
if exist "%TARGET_PATH%" (
    echo Directory already exists, cleaning it...
    rmdir /s /q "%TARGET_PATH%"
)
mkdir "%TARGET_PATH%"
echo Target directory created!

REM Step 2: Copy executable
echo Copying executable...
if exist "bin\Release\net8.0\win-x64\publish\Flashcard.exe" (
    copy "bin\Release\net8.0\win-x64\publish\Flashcard.exe" "%TARGET_PATH%\Flashcard.exe"
    echo Executable copied!
) else (
    echo Executable not found! Please run 'dotnet publish' first!
    pause
    exit /b 1
)

REM Step 3: Copy config file
echo Setting up configuration...
if exist "config.json" (
    copy "config.json" "%TARGET_PATH%\config.json"
    echo Configuration file copied!
) else (
    echo Config file not found, creating default...
    echo {> "%TARGET_PATH%\config.json"
    echo   "leitnerBoxes": {>> "%TARGET_PATH%\config.json"
    echo     "numberOfBoxes": 5,>> "%TARGET_PATH%\config.json"
    echo     "promotionRules": [>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 0, "correctAnswersNeeded": 1},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 1, "correctAnswersNeeded": 2},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 2, "correctAnswersNeeded": 3},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 3, "correctAnswersNeeded": 4},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 4, "correctAnswersNeeded": 5}>> "%TARGET_PATH%\config.json"
    echo     ],>> "%TARGET_PATH%\config.json"
    echo     "demotionRules": [>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 1, "incorrectAnswersNeeded": 1, "demoteToBox": 0},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 2, "incorrectAnswersNeeded": 1, "demoteToBox": 0},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 3, "incorrectAnswersNeeded": 1, "demoteToBox": 1},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 4, "incorrectAnswersNeeded": 1, "demoteToBox": 2}>> "%TARGET_PATH%\config.json"
    echo     ]>> "%TARGET_PATH%\config.json"
    echo   },>> "%TARGET_PATH%\config.json"
    echo   "studySession": {>> "%TARGET_PATH%\config.json"
    echo     "defaultStudyMode": 0,>> "%TARGET_PATH%\config.json"
    echo     "showStatistics": true,>> "%TARGET_PATH%\config.json"
    echo     "autoAdvance": false,>> "%TARGET_PATH%\config.json"
    echo     "autoAdvanceDelay": 3,>> "%TARGET_PATH%\config.json"
    echo     "shuffleCards": true,>> "%TARGET_PATH%\config.json"
    echo     "showProgress": true,>> "%TARGET_PATH%\config.json"
    echo     "keyboardShortcuts": {>> "%TARGET_PATH%\config.json"
    echo       "correctAnswer": "1",>> "%TARGET_PATH%\config.json"
    echo       "incorrectAnswer": "2",>> "%TARGET_PATH%\config.json"
    echo       "showAnswer": "space",>> "%TARGET_PATH%\config.json"
    echo       "quit": "q",>> "%TARGET_PATH%\config.json"
    echo       "skip": "s",>> "%TARGET_PATH%\config.json"
    echo       "flipCard": "f",>> "%TARGET_PATH%\config.json"
    echo       "showStatistics": "t",>> "%TARGET_PATH%\config.json"
    echo       "help": "h">> "%TARGET_PATH%\config.json"
    echo     }>> "%TARGET_PATH%\config.json"
    echo   },>> "%TARGET_PATH%\config.json"
    echo   "filePaths": {>> "%TARGET_PATH%\config.json"
    echo     "decksDirectory": "decks",>> "%TARGET_PATH%\config.json"
    echo     "configFileName": "config.json",>> "%TARGET_PATH%\config.json"
    echo     "deckFileExtension": ".json",>> "%TARGET_PATH%\config.json"
    echo     "backupDirectory": "backups",>> "%TARGET_PATH%\config.json"
    echo     "exportDirectory": "exports">> "%TARGET_PATH%\config.json"
    echo   },>> "%TARGET_PATH%\config.json"
    echo   "reviewScheduling": {>> "%TARGET_PATH%\config.json"
    echo     "boxIntervals": [>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 0, "intervalDays": 1},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 1, "intervalDays": 3},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 2, "intervalDays": 7},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 3, "intervalDays": 14},>> "%TARGET_PATH%\config.json"
    echo       {"boxNumber": 4, "intervalDays": 30}>> "%TARGET_PATH%\config.json"
    echo     ],>> "%TARGET_PATH%\config.json"
    echo     "newCardInterval": 1,>> "%TARGET_PATH%\config.json"
    echo     "maxNewCardsPerDay": 20>> "%TARGET_PATH%\config.json"
    echo   },>> "%TARGET_PATH%\config.json"
    echo   "dailyLimits": {>> "%TARGET_PATH%\config.json"
    echo     "maxCardsPerDay": 100,>> "%TARGET_PATH%\config.json"
    echo     "minCardsPerDay": 5,>> "%TARGET_PATH%\config.json"
    echo     "maxStudyTimePerDay": "02:00:00",>> "%TARGET_PATH%\config.json"
    echo     "minStudyTimePerDay": "00:05:00">> "%TARGET_PATH%\config.json"
    echo   },>> "%TARGET_PATH%\config.json"
    echo   "ui": {>> "%TARGET_PATH%\config.json"
    echo     "useColors": true,>> "%TARGET_PATH%\config.json"
    echo     "useIcons": false,>> "%TARGET_PATH%\config.json"
    echo     "showWelcomeMessage": true,>> "%TARGET_PATH%\config.json"
    echo     "clearScreenOnMenuChange": true,>> "%TARGET_PATH%\config.json"
    echo     "showDetailedStatistics": true>> "%TARGET_PATH%\config.json"
    echo   }>> "%TARGET_PATH%\config.json"
    echo }>> "%TARGET_PATH%\config.json"
    echo Default configuration created!
)

REM Step 4: Create directories
echo Creating directories...
mkdir "%TARGET_PATH%\decks"
mkdir "%TARGET_PATH%\backups"
mkdir "%TARGET_PATH%\exports"
echo Directories created!

REM Step 5: Copy example deck
echo Setting up example deck...
if exist "sample-vocabulary-deck.json" (
    copy "sample-vocabulary-deck.json" "%TARGET_PATH%\decks\sample-vocabulary-deck.json"
    echo Example deck copied!
) else (
    echo Example deck not found, skipping...
)

REM Step 6: Create README
echo Creating README...
echo # Flashcard App> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo A portable console application for spaced repetition learning using the Leitner box system.>> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo ## Quick Start>> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo 1. Double-click Flashcard.exe to run the application>> "%TARGET_PATH%\README.txt"
echo 2. The app will automatically create necessary directories>> "%TARGET_PATH%\README.txt"
echo 3. Start with the included sample deck or create your own>> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo ## Features>> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo - Leitner Box System: Spaced repetition with 5 configurable boxes>> "%TARGET_PATH%\README.txt"
echo - Multiple Study Modes: Front-to-back, back-to-front, or mixed>> "%TARGET_PATH%\README.txt"
echo - Statistics Tracking: Monitor your progress and success rates>> "%TARGET_PATH%\README.txt"
echo - Keyboard Shortcuts: Efficient studying with hotkeys>> "%TARGET_PATH%\README.txt"
echo - Portable: No installation required, runs from any folder>> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo ## Study Session Controls>> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo - SPACE - Show answer>> "%TARGET_PATH%\README.txt"
echo - 1 - Mark as correct>> "%TARGET_PATH%\README.txt"
echo - 2 - Mark as incorrect>> "%TARGET_PATH%\README.txt"
echo - S - Skip card>> "%TARGET_PATH%\README.txt"
echo - Q - Quit session>> "%TARGET_PATH%\README.txt"
echo - H - Show help>> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo ## Getting Started>> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo 1. Run the application>> "%TARGET_PATH%\README.txt"
echo 2. Select "Manage Decks" to create your first deck>> "%TARGET_PATH%\README.txt"
echo 3. Add flashcards with questions and answers>> "%TARGET_PATH%\README.txt"
echo 4. Start studying with "Start Study Session">> "%TARGET_PATH%\README.txt"
echo.>> "%TARGET_PATH%\README.txt"
echo Happy learning!>> "%TARGET_PATH%\README.txt"
echo README created!

REM Summary
echo.
echo Deployment completed successfully!
echo =====================================
echo Location: %TARGET_PATH%
echo To run: Double-click Flashcard.exe
echo.
echo Contents:
echo   - Flashcard.exe (main application)
echo   - config.json (configuration)
echo   - decks/ (flashcard decks)
echo   - backups/ (automatic backups)
echo   - exports/ (exported decks)
echo   - README.txt (instructions)
echo.
echo Ready to start learning!
echo.
pause