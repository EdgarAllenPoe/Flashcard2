# Flashcard App

A beautiful, feature-rich console-based flashcard application with spaced repetition learning using the Leitner Box system.

## Features

### **Core Learning System**
- **Leitner Box System**: Intelligent spaced repetition for optimal learning
- **Card Retention**: Missed cards stay in study deck until answered correctly
- **Session State**: Resume study sessions anytime with graceful exit
- **Statistics Tracking**: Comprehensive learning analytics (hidden during study)

### 🎨 **Modern UI**
- **Beautiful Console Interface**: Colorful, emoji-rich design
- **ESC Key Navigation**: Intuitive menu navigation throughout
- **Word Wrapping**: Clean text display for long content
- **No ASCII Boxes**: Clean, modern appearance

### ⚙️ **Configuration & Customization**
- **Full Configuration Editing**: Interactive settings for all options
- **Leitner Box Settings**: Customizable promotion/demotion rules
- **Study Session Settings**: Flexible study modes and shortcuts
- **Daily Limits**: Control study intensity and time
- **UI Preferences**: Colors, icons, and display options

### 📊 **Import/Export**
- **Multiple Formats**: JSON, CSV, and XLSX support
- **Deck Management**: Create, edit, delete, and organize decks
- **Data Portability**: Easy backup and sharing

### **Study Features**
- **Any Key Reveal**: Press any key to show answers
- **In-Session Editing**: Edit cards during study sessions
- **Keyboard Shortcuts**: Quick access to skip, edit, help, quit
- **Progress Tracking**: Visual study progress indicators

## Quick Start

### Prerequisites
- .NET 8.0 SDK or later
- Windows, macOS, or Linux

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/FlashcardApp.git
   cd FlashcardApp
   ```

2. **Build the application**
   ```bash
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Or run the published executable**
   ```bash
   dotnet publish -c Release
   ./bin/Release/net8.0/FlashcardApp.exe
   ```

## 📖 Usage

### First Time Setup
1. Run the application
2. Go to **Configuration** to customize settings
3. Create your first deck or import sample data

### Study Session
1. Select **Study Session** from main menu
2. Choose a deck
3. Use keyboard shortcuts:
   - **Any key**: Reveal answer
   - **1/2**: Rate your performance
   - **S**: Skip card
   - **E**: Edit current card
   - **H**: Show help
   - **ESC**: Exit gracefully (saves progress)

### Deck Management
- **Create**: Add new flashcard decks
- **Edit**: Modify existing cards and settings
- **Import/Export**: Backup and share decks
- **Delete**: Remove unwanted decks

## ⚙️ Configuration

### Leitner Box Settings
- **Number of Boxes**: Configure spaced repetition intervals
- **Promotion Rules**: Define when cards advance
- **Demotion Rules**: Set when cards move back

### Study Session Settings
- **Study Mode**: Choose between different learning approaches
- **Keyboard Shortcuts**: Customize hotkeys
- **Auto-advance**: Control card progression
- **Shuffle**: Randomize card order

### Daily Limits
- **Max/Min Cards**: Control study intensity
- **Study Time**: Set session duration limits

### UI Settings
- **Colors**: Enable/disable colored output
- **Icons**: Toggle emoji display
- **Welcome Message**: Customize startup experience
- **Screen Clearing**: Control menu transitions

## 📁 Project Structure

```
FlashcardApp/
├── Models/                 # Data models
│   ├── Flashcard.cs       # Individual card structure
│   ├── Deck.cs           # Deck container
│   ├── AppConfiguration.cs # App settings
│   └── SessionState.cs   # Study session state
├── Services/              # Business logic
│   ├── DeckService.cs    # Deck management
│   ├── StudySessionService.cs # Study logic
│   └── ConfigurationService.cs # Settings management
├── UI/                    # User interface
│   └── ConsoleUI.cs      # Console interface
├── Program.cs            # Application entry point
├── KeyTest.cs           # Key validation testing
└── copy-files-to-build.bat # Build automation
```

## 🔧 Development

### Building
```bash
# Debug build
dotnet build

# Release build
dotnet build --configuration Release

# Publish for distribution
dotnet publish -c Release -r win-x64 --self-contained
```

### Testing
```bash
# Run key validation tests
dotnet run test
```

### File Management
The `copy-files-to-build.bat` script automatically copies configuration and sample files to build output directories for easy testing.

## 📋 Keyboard Shortcuts

### Main Navigation
- **ESC**: Go back/Quit application
- **Number keys**: Select menu options

### Study Session
- **Any key**: Reveal answer
- **1**: Correct answer
- **2**: Incorrect answer
- **S**: Skip card
- **E**: Edit current card
- **H**: Show help
- **ESC**: Exit gracefully

### Configuration
- **ESC**: Return to previous menu
- **Enter**: Edit current setting
- **Number keys**: Select options

## Leitner Box System

The app uses a scientifically-proven spaced repetition system:

1. **Box 1**: New cards (review daily)
2. **Box 2**: Review every 2 days
3. **Box 3**: Review every 4 days
4. **Box 4**: Review every 8 days
5. **Box 5**: Review every 16 days

Cards advance when answered correctly and move back when missed.

## 📊 Data Formats

### JSON Format
```json
{
  "Name": "Sample Deck",
  "Description": "Example flashcards",
  "Cards": [
    {
      "Front": "Question",
      "Back": "Answer",
      "Tags": ["tag1", "tag2"],
      "CurrentBox": 1,
      "LastReviewed": "2024-01-01T00:00:00",
      "NextReviewDate": "2024-01-02T00:00:00"
    }
  ]
}
```

### CSV Format
```csv
Front,Back,Tags,CurrentBox,LastReviewed,NextReviewDate
Question,Answer,"tag1,tag2",1,2024-01-01T00:00:00,2024-01-02T00:00:00
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Leitner Box system for spaced repetition
- .NET community for excellent tooling
- Contributors and testers

## 📞 Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/yourusername/FlashcardApp/issues) page
2. Create a new issue with detailed information
3. Include your operating system and .NET version

---

**Happy Learning!**