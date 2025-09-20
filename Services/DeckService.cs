using Newtonsoft.Json;
using FlashcardApp.Models;
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlashcardApp.Services
{
    public class DeckService
    {
        private readonly ConfigurationService _configService;
        private readonly string _decksDirectory;

        public DeckService(ConfigurationService configService)
        {
            _configService = configService;
            _decksDirectory = _configService.GetConfiguration().FilePaths.DecksDirectory;
        }

        public List<Deck> LoadAllDecks()
        {
            var decks = new List<Deck>();

            if (!Directory.Exists(_decksDirectory))
            {
                Directory.CreateDirectory(_decksDirectory);
                return decks;
            }

            var deckFiles = Directory.GetFiles(_decksDirectory, $"*{_configService.GetConfiguration().FilePaths.DeckFileExtension}");

            foreach (var file in deckFiles)
            {
                try
                {
                    var deck = LoadDeck(file);
                    if (deck != null)
                    {
                        decks.Add(deck);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading deck from {file}: {ex.Message}");
                }
            }

            return decks;
        }

        public Deck? LoadDeck(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Deck>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading deck: {ex.Message}");
                return null;
            }
        }

        public Deck? LoadDeckById(string deckId)
        {
            var filePath = Path.Combine(_decksDirectory, $"{deckId}.json");
            return LoadDeck(filePath);
        }

        public bool SaveDeck(Deck deck)
        {
            try
            {
                deck.LastModified = DateTime.Now;
                string json = JsonConvert.SerializeObject(deck, Formatting.Indented);
                string filePath = Path.Combine(_decksDirectory, $"{deck.Id}.json");
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving deck: {ex.Message}");
                return false;
            }
        }

        public bool DeleteDeck(string deckId)
        {
            try
            {
                string filePath = Path.Combine(_decksDirectory, $"{deckId}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting deck: {ex.Message}");
                return false;
            }
        }

        public Deck CreateNewDeck(string name, string description = "", List<string>? tags = null)
        {
            return new Deck
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                Tags = tags ?? new List<string>(),
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now
            };
        }

        public bool DeckNameExists(string name)
        {
            var existingDecks = LoadAllDecks();
            return existingDecks.Any(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public string GetUniqueDeckName(string baseName)
        {
            if (!DeckNameExists(baseName))
            {
                return baseName;
            }

            int counter = 1;
            string uniqueName;
            do
            {
                uniqueName = $"{baseName} ({counter})";
                counter++;
            } while (DeckNameExists(uniqueName));

            return uniqueName;
        }

        public bool AddFlashcardToDeck(Deck deck, Flashcard flashcard)
        {
            try
            {
                deck.Flashcards.Add(flashcard);
                deck.LastModified = DateTime.Now;
                return SaveDeck(deck);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding flashcard to deck: {ex.Message}");
                return false;
            }
        }

        public bool RemoveFlashcardFromDeck(Deck deck, string flashcardId)
        {
            try
            {
                var flashcard = deck.Flashcards.FirstOrDefault(f => f.Id == flashcardId);
                if (flashcard != null)
                {
                    deck.Flashcards.Remove(flashcard);
                    deck.LastModified = DateTime.Now;
                    return SaveDeck(deck);
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing flashcard from deck: {ex.Message}");
                return false;
            }
        }

        public bool ExportDeck(Deck deck, string exportPath)
        {
            try
            {
                if (deck == null)
                {
                    Console.WriteLine("Error: Cannot export null deck");
                    return false;
                }

                var extension = Path.GetExtension(exportPath).ToLower();

                switch (extension)
                {
                    case ".csv":
                        return ExportToCsv(deck, exportPath);
                    case ".xlsx":
                        return ExportToXlsx(deck, exportPath);
                    case ".json":
                        return ExportToJson(deck, exportPath);
                    default:
                        Console.WriteLine($"Unsupported export format: {extension}");
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting deck: {ex.Message}");
                return false;
            }
        }

        public Deck? ImportDeck(string importPath)
        {
            try
            {
                if (!File.Exists(importPath))
                    return null;

                var extension = Path.GetExtension(importPath).ToLower();

                switch (extension)
                {
                    case ".csv":
                        return ImportFromCsv(importPath);
                    case ".xlsx":
                        return ImportFromXlsx(importPath);
                    case ".json":
                        return ImportFromJson(importPath);
                    default:
                        Console.WriteLine($"Unsupported import format: {extension}");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing deck: {ex.Message}");
                return null;
            }
        }

        private bool ExportToJson(Deck deck, string exportPath)
        {
            string json = JsonConvert.SerializeObject(deck, Formatting.Indented);
            File.WriteAllText(exportPath, json);
            return true;
        }

        private Deck? ImportFromJson(string importPath)
        {
            try
            {
                string json = File.ReadAllText(importPath);
                var deck = JsonConvert.DeserializeObject<Deck>(json);

                // Validate that this is actually a valid deck structure
                if (deck != null && !string.IsNullOrEmpty(deck.Name))
                {
                    // Generate new ID to avoid conflicts
                    deck.Id = Guid.NewGuid().ToString();
                    deck.CreatedDate = DateTime.Now;
                    deck.LastModified = DateTime.Now;

                    // Ensure unique deck name
                    deck.Name = GetUniqueDeckName(deck.Name);

                    // Reset all flashcard IDs and statistics
                    foreach (var flashcard in deck.Flashcards)
                    {
                        flashcard.Id = Guid.NewGuid().ToString();
                        flashcard.Statistics = new FlashcardStatistics();
                        flashcard.CurrentBox = 0;
                        flashcard.LastReviewed = null;
                        flashcard.NextReviewDate = null;
                    }

                    return deck;
                }
                else
                {
                    return null; // Invalid deck structure
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing JSON: {ex.Message}");
                return null;
            }
        }

        private bool ExportToCsv(Deck deck, string exportPath)
        {
            using (var writer = new StreamWriter(exportPath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
            {
                // Write deck information as comments
                writer.WriteLine($"# Deck: {deck.Name}");
                writer.WriteLine($"# Description: {deck.Description}");
                writer.WriteLine($"# Created: {deck.CreatedDate:yyyy-MM-dd}");
                writer.WriteLine($"# Total Cards: {deck.Flashcards.Count}");
                writer.WriteLine();

                // Write CSV headers
                csv.WriteField("Front");
                csv.WriteField("Back");
                csv.WriteField("Tags");
                csv.NextRecord();

                // Write flashcard data
                foreach (var card in deck.Flashcards)
                {
                    csv.WriteField(card.Front);
                    csv.WriteField(card.Back);
                    csv.WriteField(string.Join(";", card.Tags));
                    csv.NextRecord();
                }
            }
            return true;
        }

        private Deck? ImportFromCsv(string importPath)
        {
            try
            {
                var baseName = Path.GetFileNameWithoutExtension(importPath);
                var deck = new Deck
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = GetUniqueDeckName(baseName),
                    Description = "Imported from CSV",
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now,
                    Flashcards = new List<Flashcard>()
                };

                using (var reader = new StreamReader(importPath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
                {
                    // Skip comment lines
                    while (reader.Peek() == '#')
                    {
                        reader.ReadLine();
                    }

                    // Read the header row
                    csv.Read();
                    csv.ReadHeader();

                    // Read data rows
                    while (csv.Read())
                    {
                        var front = csv.GetField("Front") ?? "";
                        var back = csv.GetField("Back") ?? "";
                        var tagsString = csv.GetField("Tags") ?? "";

                        // Skip empty rows
                        if (string.IsNullOrWhiteSpace(front) && string.IsNullOrWhiteSpace(back))
                            continue;

                        var tags = string.IsNullOrEmpty(tagsString)
                            ? new List<string>()
                            : tagsString.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                       .Select(t => t.Trim())
                                       .ToList();

                        var flashcard = new Flashcard
                        {
                            Id = Guid.NewGuid().ToString(),
                            Front = front,
                            Back = back,
                            Tags = tags,
                            Statistics = new FlashcardStatistics(),
                            CurrentBox = 0,
                            CreatedDate = DateTime.Now
                        };

                        deck.Flashcards.Add(flashcard);
                    }
                }

                return deck;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing CSV: {ex.Message}");
                return null;
            }
        }

        private bool ExportToXlsx(Deck deck, string exportPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Flashcards");

                // Write deck information
                worksheet.Cells[1, 1].Value = "Deck Name:";
                worksheet.Cells[1, 2].Value = deck.Name;
                worksheet.Cells[2, 1].Value = "Description:";
                worksheet.Cells[2, 2].Value = deck.Description;
                worksheet.Cells[3, 1].Value = "Created:";
                worksheet.Cells[3, 2].Value = deck.CreatedDate.ToString("yyyy-MM-dd");
                worksheet.Cells[4, 1].Value = "Total Cards:";
                worksheet.Cells[4, 2].Value = deck.Flashcards.Count;

                // Write headers
                worksheet.Cells[6, 1].Value = "Front";
                worksheet.Cells[6, 2].Value = "Back";
                worksheet.Cells[6, 3].Value = "Tags";

                // Style headers
                using (var range = worksheet.Cells[6, 1, 6, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Write flashcard data
                for (int i = 0; i < deck.Flashcards.Count; i++)
                {
                    var card = deck.Flashcards[i];
                    var row = i + 7; // Start from row 7 (after headers and deck info)

                    worksheet.Cells[row, 1].Value = card.Front;
                    worksheet.Cells[row, 2].Value = card.Back;
                    worksheet.Cells[row, 3].Value = string.Join("; ", card.Tags);
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                package.SaveAs(new FileInfo(exportPath));
            }
            return true;
        }

        private Deck? ImportFromXlsx(string importPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var baseName = Path.GetFileNameWithoutExtension(importPath);
            var deck = new Deck
            {
                Id = Guid.NewGuid().ToString(),
                Name = GetUniqueDeckName(baseName),
                Description = "Imported from XLSX",
                CreatedDate = DateTime.Now,
                LastModified = DateTime.Now,
                Flashcards = new List<Flashcard>()
            };

            using (var package = new ExcelPackage(new FileInfo(importPath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension?.Rows ?? 0;

                // Find the header row (look for "Front" in column A)
                int headerRow = 1;
                for (int row = 1; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Value?.ToString()?.ToLower() == "front")
                    {
                        headerRow = row;
                        break;
                    }
                }

                // Read data starting from the row after headers
                for (int row = headerRow + 1; row <= rowCount; row++)
                {
                    var front = worksheet.Cells[row, 1].Value?.ToString() ?? "";
                    var back = worksheet.Cells[row, 2].Value?.ToString() ?? "";
                    var tagsString = worksheet.Cells[row, 3].Value?.ToString() ?? "";

                    if (string.IsNullOrEmpty(front) && string.IsNullOrEmpty(back))
                        continue; // Skip empty rows

                    var tags = string.IsNullOrEmpty(tagsString)
                        ? new List<string>()
                        : tagsString.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(t => t.Trim())
                                   .ToList();

                    var flashcard = new Flashcard
                    {
                        Id = Guid.NewGuid().ToString(),
                        Front = front,
                        Back = back,
                        Tags = tags,
                        Statistics = new FlashcardStatistics(),
                        CurrentBox = 0,
                        CreatedDate = DateTime.Now
                    };

                    deck.Flashcards.Add(flashcard);
                }
            }

            return deck;
        }

        public bool BackupDeck(Deck deck)
        {
            try
            {
                var backupDir = _configService.GetConfiguration().FilePaths.BackupDirectory;
                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }

                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var backupPath = Path.Combine(backupDir, $"{deck.Name}_{timestamp}.json");

                return ExportDeck(deck, backupPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error backing up deck: {ex.Message}");
                return false;
            }
        }

        public List<Deck> SearchDecks(string searchTerm)
        {
            var allDecks = LoadAllDecks();
            return allDecks.Where(d =>
                d.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                d.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                d.Tags.Any(t => t.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        public List<Flashcard> SearchFlashcards(Deck deck, string searchTerm)
        {
            if (deck == null)
                return new List<Flashcard>();

            return deck.Flashcards.Where(f =>
                f.Front.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                f.Back.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                f.Tags.Any(t => t.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
    }
}
