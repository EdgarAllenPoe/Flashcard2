using FluentAssertions;
using FlashcardApp.Models;
using FlashcardApp.Services;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.Services
{
    public class DeckServiceImportExportTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly string _testConfigPath;
        private readonly ConfigurationService _configService;
        private readonly DeckService _deckService;

        public DeckServiceImportExportTests()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), $"import_export_test_{Guid.NewGuid()}");
            Directory.CreateDirectory(_testDirectory);

            _testConfigPath = Path.Combine(_testDirectory, "config.json");
            var config = new AppConfiguration
            {
                FilePaths = new FilePathConfiguration
                {
                    DecksDirectory = _testDirectory,
                    DeckFileExtension = ".json"
                }
            };

            File.WriteAllText(_testConfigPath, Newtonsoft.Json.JsonConvert.SerializeObject(config));
            _configService = new ConfigurationService(_testConfigPath);
            _deckService = new DeckService(_configService);
        }

        public void Dispose()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }

        private Deck CreateTestDeck()
        {
            var deck = new Deck
            {
                Name = "Test Export Deck",
                Description = "A deck for testing export functionality",
                Tags = new List<string> { "test", "export" }
            };

            deck.Flashcards.Add(new Flashcard
            {
                Front = "What is the capital of France?",
                Back = "Paris",
                Tags = new List<string> { "geography", "capitals" },
                CurrentBox = 1,
                Statistics = new FlashcardStatistics
                {
                    TotalReviews = 5,
                    CorrectAnswers = 4,
                    IncorrectAnswers = 1,
                    AverageResponseTime = 2.5,
                    Streak = 2
                }
            });

            deck.Flashcards.Add(new Flashcard
            {
                Front = "What is 2 + 2?",
                Back = "4",
                Tags = new List<string> { "math", "arithmetic" },
                CurrentBox = 0,
                Statistics = new FlashcardStatistics
                {
                    TotalReviews = 3,
                    CorrectAnswers = 3,
                    IncorrectAnswers = 0,
                    AverageResponseTime = 1.2,
                    Streak = 3
                }
            });

            return deck;
        }

        #region Export Tests

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportDeck_ToJson_ShouldCreateValidFile()
        {
            // Arrange
            var deck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "test_export.json");

            // Act
            var result = _deckService.ExportDeck(deck, exportPath);

            // Assert
            result.Should().BeTrue();
            File.Exists(exportPath).Should().BeTrue();

            var exportedContent = File.ReadAllText(exportPath);
            exportedContent.Should().Contain("Test Export Deck");
            exportedContent.Should().Contain("What is the capital of France?");
            exportedContent.Should().Contain("Paris");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportDeck_ToCsv_ShouldCreateValidFile()
        {
            // Arrange
            var deck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "test_export.csv");

            // Act
            var result = _deckService.ExportDeck(deck, exportPath);

            // Assert
            result.Should().BeTrue();
            File.Exists(exportPath).Should().BeTrue();

            var exportedContent = File.ReadAllText(exportPath);
            exportedContent.Should().Contain("Front,Back,Tags");
            exportedContent.Should().Contain("What is the capital of France?");
            exportedContent.Should().Contain("Paris");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportDeck_ToXlsx_ShouldCreateValidFile()
        {
            // Arrange
            var deck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "test_export.xlsx");

            // Act
            var result = _deckService.ExportDeck(deck, exportPath);

            // Assert
            result.Should().BeTrue();
            File.Exists(exportPath).Should().BeTrue();

            // Verify it's a valid Excel file by checking file size
            var fileInfo = new FileInfo(exportPath);
            fileInfo.Length.Should().BeGreaterThan(0);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportDeck_UnsupportedFormat_ShouldReturnFalse()
        {
            // Arrange
            var deck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "test_export.txt");

            // Act
            var result = _deckService.ExportDeck(deck, exportPath);

            // Assert
            result.Should().BeFalse();
            File.Exists(exportPath).Should().BeFalse();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportDeck_InvalidPath_ShouldReturnFalse()
        {
            // Arrange
            var deck = CreateTestDeck();
            var exportPath = Path.Combine("invalid", "path", "test_export.json");

            // Act
            var result = _deckService.ExportDeck(deck, exportPath);

            // Assert
            result.Should().BeFalse();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportDeck_NullDeck_ShouldReturnFalse()
        {
            // Arrange
            var exportPath = Path.Combine(_testDirectory, "test_export.json");

            // Act
            var result = _deckService.ExportDeck(null!, exportPath);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region Import Tests

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_FileNotFound_ShouldReturnNull()
        {
            // Arrange
            var importPath = Path.Combine(_testDirectory, "nonexistent.json");

            // Act
            var result = _deckService.ImportDeck(importPath);

            // Assert
            result.Should().BeNull();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_UnsupportedFormat_ShouldReturnNull()
        {
            // Arrange
            var importPath = Path.Combine(_testDirectory, "test.txt");
            File.WriteAllText(importPath, "This is not a supported format");

            // Act
            var result = _deckService.ImportDeck(importPath);

            // Assert
            result.Should().BeNull();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_NullPath_ShouldReturnNull()
        {
            // Act
            var result = _deckService.ImportDeck(null!);

            // Assert
            result.Should().BeNull();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_FromJson_ShouldLoadCorrectly()
        {
            // Arrange
            var originalDeck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "test_import.json");
            var importPath = exportPath; // Same file for import

            // Export first
            _deckService.ExportDeck(originalDeck, exportPath);

            // Act
            var importedDeck = _deckService.ImportDeck(importPath);

            // Assert
            importedDeck.Should().NotBeNull();
            // JSON import preserves the original deck name but makes it unique
            importedDeck!.Name.Should().StartWith(originalDeck.Name);
            importedDeck.Description.Should().Be(originalDeck.Description);
            importedDeck.Flashcards.Should().HaveCount(originalDeck.Flashcards.Count);
            importedDeck.Flashcards.Should().Contain(f => f.Front == "What is the capital of France?");
            importedDeck.Flashcards.Should().Contain(f => f.Back == "Paris");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_FromCsv_ShouldLoadCorrectly()
        {
            // Arrange
            var originalDeck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "test_import.csv");
            var importPath = exportPath; // Same file for import

            // Export first
            _deckService.ExportDeck(originalDeck, exportPath);

            // Act
            var importedDeck = _deckService.ImportDeck(importPath);

            // Assert
            importedDeck.Should().NotBeNull();
            // CSV import uses filename as deck name
            importedDeck!.Name.Should().Be("test_import");
            importedDeck.Description.Should().Be("Imported from CSV");
            importedDeck.Flashcards.Should().HaveCount(originalDeck.Flashcards.Count);
            importedDeck.Flashcards.Should().Contain(f => f.Front == "What is the capital of France?");
            importedDeck.Flashcards.Should().Contain(f => f.Back == "Paris");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_FromXlsx_ShouldLoadCorrectly()
        {
            // Arrange
            var originalDeck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "test_import.xlsx");
            var importPath = exportPath; // Same file for import

            // Export first
            _deckService.ExportDeck(originalDeck, exportPath);

            // Act
            var importedDeck = _deckService.ImportDeck(importPath);

            // Assert
            importedDeck.Should().NotBeNull();
            // XLSX import uses filename as deck name
            importedDeck!.Name.Should().Be("test_import");
            importedDeck.Description.Should().Be("Imported from XLSX");
            importedDeck.Flashcards.Should().HaveCount(originalDeck.Flashcards.Count);
            importedDeck.Flashcards.Should().Contain(f => f.Front == "What is the capital of France?");
            importedDeck.Flashcards.Should().Contain(f => f.Back == "Paris");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_CorruptedJsonFile_ShouldReturnNull()
        {
            // Arrange
            var importPath = Path.Combine(_testDirectory, "corrupted.json");
            File.WriteAllText(importPath, "{ invalid json content");

            // Act
            var result = _deckService.ImportDeck(importPath);

            // Assert
            result.Should().BeNull();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_EmptyFile_ShouldReturnNull()
        {
            // Arrange
            var importPath = Path.Combine(_testDirectory, "empty.json");
            File.WriteAllText(importPath, "");

            // Act
            var result = _deckService.ImportDeck(importPath);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region Round-trip Tests

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportThenImport_Json_ShouldPreserveAllData()
        {
            // Arrange
            var originalDeck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "roundtrip_test.json");

            // Act - Export then Import
            var exportResult = _deckService.ExportDeck(originalDeck, exportPath);
            var importedDeck = _deckService.ImportDeck(exportPath);

            // Assert
            exportResult.Should().BeTrue();
            importedDeck.Should().NotBeNull();

            // Verify deck structure
            importedDeck!.Name.Should().StartWith(originalDeck.Name);
            importedDeck.Description.Should().Be(originalDeck.Description);
            importedDeck.Flashcards.Should().HaveCount(originalDeck.Flashcards.Count);

            // Verify flashcard content
            var originalCard1 = originalDeck.Flashcards.First(f => f.Front == "What is the capital of France?");
            var importedCard1 = importedDeck.Flashcards.First(f => f.Front == "What is the capital of France?");

            importedCard1.Back.Should().Be(originalCard1.Back);
            importedCard1.Tags.Should().BeEquivalentTo(originalCard1.Tags);
            importedCard1.CurrentBox.Should().Be(0); // Reset during import
            importedCard1.Statistics.TotalReviews.Should().Be(0); // Reset during import
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportThenImport_Csv_ShouldPreserveFlashcardContent()
        {
            // Arrange
            var originalDeck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "roundtrip_test.csv");

            // Act - Export then Import
            var exportResult = _deckService.ExportDeck(originalDeck, exportPath);
            var importedDeck = _deckService.ImportDeck(exportPath);

            // Assert
            exportResult.Should().BeTrue();
            importedDeck.Should().NotBeNull();

            // Verify flashcard content is preserved
            importedDeck!.Flashcards.Should().HaveCount(originalDeck.Flashcards.Count);
            importedDeck.Flashcards.Should().Contain(f => f.Front == "What is the capital of France?");
            importedDeck.Flashcards.Should().Contain(f => f.Back == "Paris");
            importedDeck.Flashcards.Should().Contain(f => f.Front == "What is 2 + 2?");
            importedDeck.Flashcards.Should().Contain(f => f.Back == "4");

            // Verify tags are preserved
            var geographyCard = importedDeck.Flashcards.First(f => f.Front == "What is the capital of France?");
            geographyCard.Tags.Should().Contain("geography");
            geographyCard.Tags.Should().Contain("capitals");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportThenImport_Xlsx_ShouldPreserveFlashcardContent()
        {
            // Arrange
            var originalDeck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "roundtrip_test.xlsx");

            // Act - Export then Import
            var exportResult = _deckService.ExportDeck(originalDeck, exportPath);
            var importedDeck = _deckService.ImportDeck(exportPath);

            // Assert
            exportResult.Should().BeTrue();
            importedDeck.Should().NotBeNull();

            // Verify flashcard content is preserved
            importedDeck!.Flashcards.Should().HaveCount(originalDeck.Flashcards.Count);
            importedDeck.Flashcards.Should().Contain(f => f.Front == "What is the capital of France?");
            importedDeck.Flashcards.Should().Contain(f => f.Back == "Paris");
            importedDeck.Flashcards.Should().Contain(f => f.Front == "What is 2 + 2?");
            importedDeck.Flashcards.Should().Contain(f => f.Back == "4");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportThenImport_ShouldResetStatisticsAndBoxes()
        {
            // Arrange
            var originalDeck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "statistics_test.json");

            // Act - Export then Import
            _deckService.ExportDeck(originalDeck, exportPath);
            var importedDeck = _deckService.ImportDeck(exportPath);

            // Assert
            importedDeck.Should().NotBeNull();

            // All flashcards should be reset to initial state
            foreach (var card in importedDeck!.Flashcards)
            {
                card.CurrentBox.Should().Be(0);
                card.Statistics.TotalReviews.Should().Be(0);
                card.Statistics.CorrectAnswers.Should().Be(0);
                card.Statistics.IncorrectAnswers.Should().Be(0);
                card.Statistics.Streak.Should().Be(0);
                card.LastReviewed.Should().BeNull();
                card.NextReviewDate.Should().BeNull();
            }
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportThenImport_ShouldGenerateNewIds()
        {
            // Arrange
            var originalDeck = CreateTestDeck();
            var exportPath = Path.Combine(_testDirectory, "ids_test.json");

            // Act - Export then Import
            _deckService.ExportDeck(originalDeck, exportPath);
            var importedDeck = _deckService.ImportDeck(exportPath);

            // Assert
            importedDeck.Should().NotBeNull();

            // Deck should have new ID
            importedDeck!.Id.Should().NotBe(originalDeck.Id);

            // All flashcards should have new IDs
            var originalIds = originalDeck.Flashcards.Select(f => f.Id).ToHashSet();
            var importedIds = importedDeck.Flashcards.Select(f => f.Id).ToHashSet();

            originalIds.Should().NotIntersectWith(importedIds);
        }

        #endregion

        #region Error Handling and Edge Cases

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportDeck_EmptyDeck_ShouldCreateValidFile()
        {
            // Arrange
            var emptyDeck = new Deck
            {
                Name = "Empty Deck",
                Description = "A deck with no flashcards"
            };
            var exportPath = Path.Combine(_testDirectory, "empty_deck.json");

            // Act
            var result = _deckService.ExportDeck(emptyDeck, exportPath);

            // Assert
            result.Should().BeTrue();
            File.Exists(exportPath).Should().BeTrue();

            var importedDeck = _deckService.ImportDeck(exportPath);
            importedDeck.Should().NotBeNull();
            importedDeck!.Flashcards.Should().BeEmpty();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_CsvWithMissingColumns_ShouldReturnNull()
        {
            // Arrange
            var csvPath = Path.Combine(_testDirectory, "incomplete.csv");
            var csvContent = "Front\nQuestion without answer";
            File.WriteAllText(csvPath, csvContent);

            // Act
            var result = _deckService.ImportDeck(csvPath);

            // Assert
            result.Should().BeNull(); // Should fail when required columns are missing
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_CsvWithExtraColumns_ShouldIgnoreExtraColumns()
        {
            // Arrange
            var csvPath = Path.Combine(_testDirectory, "extra_columns.csv");
            var csvContent = "Front,Back,Tags,ExtraColumn\nQuestion,Answer,tag1,ignored";
            File.WriteAllText(csvPath, csvContent);

            // Act
            var result = _deckService.ImportDeck(csvPath);

            // Assert
            result.Should().NotBeNull();
            result!.Flashcards.Should().HaveCount(1);
            result.Flashcards.First().Front.Should().Be("Question");
            result.Flashcards.First().Back.Should().Be("Answer");
            result.Flashcards.First().Tags.Should().Contain("tag1");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_CsvWithEmptyRows_ShouldSkipEmptyRows()
        {
            // Arrange
            var csvPath = Path.Combine(_testDirectory, "empty_rows.csv");
            var csvContent = "Front,Back,Tags\nQuestion1,Answer1,tag1\n\nQuestion2,Answer2,tag2\n";
            File.WriteAllText(csvPath, csvContent);

            // Act
            var result = _deckService.ImportDeck(csvPath);

            // Assert
            result.Should().NotBeNull();
            result!.Flashcards.Should().HaveCount(2);
            result.Flashcards.Should().Contain(f => f.Front == "Question1");
            result.Flashcards.Should().Contain(f => f.Front == "Question2");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_JsonWithInvalidStructure_ShouldReturnNull()
        {
            // Arrange
            var jsonPath = Path.Combine(_testDirectory, "invalid_structure.json");
            var jsonContent = "{\"invalid\": \"structure\", \"notADeck\": true}";
            File.WriteAllText(jsonPath, jsonContent);

            // Act
            var result = _deckService.ImportDeck(jsonPath);

            // Assert
            result.Should().BeNull();
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ExportDeck_DeckWithSpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var specialDeck = new Deck
            {
                Name = "Special Characters Deck",
                Description = "Testing émojis and spëcial chars: 🎯"
            };
            specialDeck.Flashcards.Add(new Flashcard
            {
                Front = "What is café? ☕",
                Back = "A coffee shop with émojis 🎉",
                Tags = new List<string> { "café", "émoji", "special" }
            });

            var exportPath = Path.Combine(_testDirectory, "special_chars.json");

            // Act
            var exportResult = _deckService.ExportDeck(specialDeck, exportPath);
            var importedDeck = _deckService.ImportDeck(exportPath);

            // Assert
            exportResult.Should().BeTrue();
            importedDeck.Should().NotBeNull();
            importedDeck!.Description.Should().Contain("émojis");
            importedDeck.Flashcards.First().Front.Should().Contain("café");
            importedDeck.Flashcards.First().Back.Should().Contain("émojis");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_FileWithVeryLongContent_ShouldHandleCorrectly()
        {
            // Arrange
            var longContent = new string('A', 10000); // 10KB of 'A's
            var deck = new Deck
            {
                Name = "Long Content Deck",
                Description = "Testing very long content"
            };
            deck.Flashcards.Add(new Flashcard
            {
                Front = longContent,
                Back = "Short answer"
            });

            var exportPath = Path.Combine(_testDirectory, "long_content.json");

            // Act
            var exportResult = _deckService.ExportDeck(deck, exportPath);
            var importedDeck = _deckService.ImportDeck(exportPath);

            // Assert
            exportResult.Should().BeTrue();
            importedDeck.Should().NotBeNull();
            importedDeck!.Flashcards.First().Front.Should().Be(longContent);
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_CsvWithCommasInContent_ShouldHandleCorrectly()
        {
            // Arrange
            var csvPath = Path.Combine(_testDirectory, "commas.csv");
            var csvContent = "Front,Back,Tags\n\"Question, with comma\",\"Answer, also with comma\",\"tag1;tag2\"";
            File.WriteAllText(csvPath, csvContent);

            // Act
            var result = _deckService.ImportDeck(csvPath);

            // Assert
            result.Should().NotBeNull();
            result!.Flashcards.Should().HaveCount(1);
            result.Flashcards.First().Front.Should().Be("Question, with comma");
            result.Flashcards.First().Back.Should().Be("Answer, also with comma");
            result.Flashcards.First().Tags.Should().Contain("tag1");
            result.Flashcards.First().Tags.Should().Contain("tag2");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void ImportDeck_CsvWithQuotesInContent_ShouldHandleCorrectly()
        {
            // Arrange
            var csvPath = Path.Combine(_testDirectory, "quotes.csv");
            var csvContent = "Front,Back,Tags\n\"Question with \"\"quotes\"\"\",\"Answer with 'single quotes'\",\"tag1\"";
            File.WriteAllText(csvPath, csvContent);

            // Act
            var result = _deckService.ImportDeck(csvPath);

            // Assert
            result.Should().NotBeNull();
            result!.Flashcards.Should().HaveCount(1);
            result.Flashcards.First().Front.Should().Contain("quotes");
            result.Flashcards.First().Back.Should().Contain("single quotes");
        }

        #endregion
    }
}
