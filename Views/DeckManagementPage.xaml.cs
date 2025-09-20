using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace FlashcardApp.WinUI.Views
{
    /// <summary>
    /// Deck management page with full CRUD operations for flashcard decks.
    /// </summary>
    public partial class DeckManagementPage : Page
    {
        private readonly List<FlashcardDeck> _decks = new();
        private FlashcardDeck? _selectedDeck = null;

        public DeckManagementPage()
        {
            this.InitializeComponent();
            InitializeDeckManagement();
        }

        private void InitializeDeckManagement()
        {
            // Initialize with sample decks
            _decks.AddRange(new[]
            {
                new FlashcardDeck 
                { 
                    Id = 1, 
                    Name = "Spanish Vocabulary", 
                    Description = "Basic Spanish words and phrases",
                    Cards = new List<Flashcard>
                    {
                        new Flashcard { Front = "Hello", Back = "Hola" },
                        new Flashcard { Front = "Goodbye", Back = "Adiós" },
                        new Flashcard { Front = "Thank you", Back = "Gracias" }
                    }
                },
                new FlashcardDeck 
                { 
                    Id = 2, 
                    Name = "Math Formulas", 
                    Description = "Essential mathematical formulas",
                    Cards = new List<Flashcard>
                    {
                        new Flashcard { Front = "Area of circle", Back = "πr²" },
                        new Flashcard { Front = "Pythagorean theorem", Back = "a² + b² = c²" }
                    }
                }
            });

            UpdateDeckList();
            UpdateButtonStates();
            UpdateStatus("Deck management initialized");
        }

        private void UpdateDeckList()
        {
            DeckListPanel.Children.Clear();
            
            foreach (var deck in _decks)
            {
                var deckButton = new Button
                {
                    Content = $"{deck.Name} ({deck.Cards.Count} cards)",
                    Margin = new Thickness(0, 5, 0, 5),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Tag = deck
                };
                deckButton.Click += DeckButton_Click;
                DeckListPanel.Children.Add(deckButton);
            }
        }

        private void DeckButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is FlashcardDeck deck)
            {
                _selectedDeck = deck;
                UpdateDeckDetails();
                UpdateButtonStates();
                UpdateStatus($"Selected deck: {deck.Name}");
            }
        }

        private void UpdateDeckDetails()
        {
            if (_selectedDeck != null)
            {
                DeckTitleText.Text = _selectedDeck.Name;
                UpdateDeckContent();
            }
            else
            {
                DeckTitleText.Text = "Select a deck to view details";
                DeckContentPanel.Children.Clear();
            }
        }

        private void UpdateDeckContent()
        {
            if (_selectedDeck == null) return;

            DeckContentPanel.Children.Clear();

            // Add description
            var descriptionText = new TextBlock
            {
                Text = _selectedDeck.Description,
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 10),
                TextWrapping = TextWrapping.Wrap
            };
            DeckContentPanel.Children.Add(descriptionText);

            // Add card count
            var cardCountText = new TextBlock
            {
                Text = $"Cards: {_selectedDeck.Cards.Count}",
                FontSize = 12,
                Margin = new Thickness(0, 0, 0, 15),
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gray)
            };
            DeckContentPanel.Children.Add(cardCountText);

            // Add cards list
            if (_selectedDeck.Cards.Count > 0)
            {
                var cardsHeader = new TextBlock
                {
                    Text = "Cards:",
                    FontSize = 14,
                    FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                DeckContentPanel.Children.Add(cardsHeader);

                foreach (var card in _selectedDeck.Cards)
                {
                    var cardButton = new Button
                    {
                        Margin = new Thickness(0, 0, 0, 8),
                        Background = new SolidColorBrush(Microsoft.UI.Colors.LightGray),
                        Padding = new Thickness(10),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Tag = card
                    };

                    var cardPanel = new StackPanel();

                    var frontText = new TextBlock
                    {
                        Text = $"Q: {card.Front}",
                        FontSize = 12,
                        FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                        TextWrapping = TextWrapping.Wrap
                    };
                    cardPanel.Children.Add(frontText);

                    var backText = new TextBlock
                    {
                        Text = $"A: {card.Back}",
                        FontSize = 12,
                        Margin = new Thickness(0, 2, 0, 0),
                        TextWrapping = TextWrapping.Wrap
                    };
                    cardPanel.Children.Add(backText);

                    cardButton.Content = cardPanel;
                    cardButton.Click += CardButton_Click;
                    DeckContentPanel.Children.Add(cardButton);
                }
            }
            else
            {
                var noCardsText = new TextBlock
                {
                    Text = "No cards in this deck yet. Click 'Add Card' to get started!",
                    FontSize = 12,
                    // FontStyle property not available in WinUI TextBlock
                    Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gray)
                };
                DeckContentPanel.Children.Add(noCardsText);
            }
        }

        private void UpdateButtonStates()
        {
            bool hasSelectedDeck = _selectedDeck != null;
            
            EditDeckButton.IsEnabled = hasSelectedDeck;
            DeleteDeckButton.IsEnabled = hasSelectedDeck;
            AddCardButton.IsEnabled = hasSelectedDeck;
            StudyDeckButton.IsEnabled = hasSelectedDeck;
        }

        private void UpdateStatus(string message)
        {
            StatusText.Text = message;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to main page
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void CreateDeckButton_Click(object sender, RoutedEventArgs e)
        {
            var newDeck = new FlashcardDeck
            {
                Id = _decks.Count > 0 ? _decks.Max(d => d.Id) + 1 : 1,
                Name = "New Deck",
                Description = "A new flashcard deck",
                Cards = new List<Flashcard>()
            };
            
            _decks.Add(newDeck);
            UpdateDeckList();
            UpdateButtonStates();
            UpdateStatus($"Created new deck: {newDeck.Name}");
        }

        private async void EditDeckButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDeck != null)
            {
                // Create a simple dialog for editing deck properties
                var dialog = new ContentDialog
                {
                    Title = "Edit Deck",
                    PrimaryButtonText = "Save",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot
                };

                var panel = new StackPanel { Spacing = 10 };

                var nameTextBox = new TextBox
                {
                    Header = "Deck Name",
                    Text = _selectedDeck.Name,
                    PlaceholderText = "Enter deck name"
                };

                var descriptionTextBox = new TextBox
                {
                    Header = "Description",
                    Text = _selectedDeck.Description,
                    PlaceholderText = "Enter deck description",
                    AcceptsReturn = true,
                    Height = 100
                };

                panel.Children.Add(nameTextBox);
                panel.Children.Add(descriptionTextBox);
                dialog.Content = panel;

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    var newName = nameTextBox.Text.Trim();
                    var newDescription = descriptionTextBox.Text.Trim();
                    
                    if (string.IsNullOrEmpty(newName))
                    {
                        UpdateStatus("Deck name cannot be empty");
                        return;
                    }
                    
                    _selectedDeck.Name = newName;
                    _selectedDeck.Description = newDescription;
                    UpdateDeckList();
                    UpdateDeckDetails();
                    UpdateStatus($"Updated deck: {_selectedDeck.Name}");
                }
            }
            else
            {
                UpdateStatus("Please select a deck to edit");
            }
        }

        private async void DeleteDeckButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDeck != null)
            {
                var dialog = new ContentDialog
                {
                    Title = "Delete Deck",
                    Content = $"Are you sure you want to delete '{_selectedDeck.Name}'? This action cannot be undone.",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot
                };

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
            {
                _decks.Remove(_selectedDeck);
                _selectedDeck = null;
                UpdateDeckList();
                UpdateDeckDetails();
                    UpdateButtonStates();
                UpdateStatus("Deck deleted successfully");
                }
            }
            else
            {
                UpdateStatus("Please select a deck to delete");
            }
        }

        private async void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDeck != null)
            {
                var newCard = await ShowCardEditDialog(null);
                if (newCard != null)
                {
                _selectedDeck.Cards.Add(newCard);
                UpdateDeckDetails();
                UpdateDeckList();
                UpdateStatus($"Added new card to {_selectedDeck.Name}");
                }
            }
            else
            {
                UpdateStatus("Please select a deck to add cards to");
            }
        }

        private async void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Flashcard card)
            {
                var updatedCard = await ShowCardEditDialog(card);
                if (updatedCard != null)
                {
                    UpdateDeckDetails();
                    UpdateStatus($"Updated card in {_selectedDeck?.Name}");
                }
            }
        }

        private async Task<Flashcard?> ShowCardEditDialog(Flashcard? existingCard)
        {
            var dialog = new ContentDialog
            {
                Title = existingCard == null ? "Add New Card" : "Edit Card",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            var panel = new StackPanel { Spacing = 10 };

            var frontTextBox = new TextBox
            {
                Header = "Question",
                Text = existingCard?.Front ?? "",
                PlaceholderText = "Enter the question",
                AcceptsReturn = true,
                Height = 80
            };

            var backTextBox = new TextBox
            {
                Header = "Answer",
                Text = existingCard?.Back ?? "",
                PlaceholderText = "Enter the answer",
                AcceptsReturn = true,
                Height = 80
            };

            panel.Children.Add(frontTextBox);
            panel.Children.Add(backTextBox);
            dialog.Content = panel;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var front = frontTextBox.Text.Trim();
                var back = backTextBox.Text.Trim();
                
                if (!string.IsNullOrEmpty(front) && !string.IsNullOrEmpty(back))
                {
                    if (existingCard != null)
                    {
                        existingCard.Front = front;
                        existingCard.Back = back;
                        return existingCard;
                    }
                    else
                    {
                        return new Flashcard { Front = front, Back = back };
                    }
                }
            }
            
            return null;
        }

        private void StudyDeckButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedDeck != null)
            {
                if (_selectedDeck.Cards.Count == 0)
                {
                    UpdateStatus($"Cannot study '{_selectedDeck.Name}' - no cards in deck");
                    return;
                }

                // Navigate to study session with selected deck
                if (this.Frame != null)
                {
                    UpdateStatus($"Starting study session for '{_selectedDeck.Name}' with {_selectedDeck.Cards.Count} cards");
                    
                    // Navigate to StudySessionPage with deck data
                    this.Frame.Navigate(typeof(StudySessionPage), _selectedDeck);
                }
            }
            else
            {
                UpdateStatus("Please select a deck to study");
            }
        }

        private async void ImportDeckButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateStatus("Starting import process...");
                
                var picker = new FileOpenPicker();
                picker.ViewMode = PickerViewMode.List;
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeFilter.Add(".json");
                
                UpdateStatus("File picker created, initializing window handle...");
                
                // Initialize the picker with the window handle
                var window = App.MainWindow;
                if (window != null)
                {
                    UpdateStatus($"Window found: {window.GetType().Name}");
                    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    UpdateStatus($"Window handle obtained: {hwnd}");
                    WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
                    UpdateStatus("File picker initialized with window handle");
                }
                else
                {
                    UpdateStatus("ERROR: App.MainWindow is null!");
                    return;
                }

                UpdateStatus("Opening file picker dialog...");
                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    UpdateStatus($"File selected: {file.Name}");
                    var content = await FileIO.ReadTextAsync(file);
                    var importedDeck = System.Text.Json.JsonSerializer.Deserialize<FlashcardDeck>(content);
                    
                    if (importedDeck != null)
                    {
                        // Validate imported deck
                        if (string.IsNullOrEmpty(importedDeck.Name))
                        {
                            importedDeck.Name = "Imported Deck";
                        }
                        
                        if (importedDeck.Cards == null)
                        {
                            importedDeck.Cards = new List<Flashcard>();
                        }
                        
                        importedDeck.Id = _decks.Count > 0 ? _decks.Max(d => d.Id) + 1 : 1;
                        _decks.Add(importedDeck);
                        UpdateDeckList();
                        UpdateStatus($"Imported deck: {importedDeck.Name} with {importedDeck.Cards.Count} cards");
                    }
                    else
                    {
                        UpdateStatus("Failed to import deck - invalid file format");
                    }
                }
                else
                {
                    UpdateStatus("File selection cancelled");
                }
            }
            catch (Exception ex)
            {
                var errorDetails = $"Import failed: {ex.Message}\n" +
                                 $"Exception Type: {ex.GetType().Name}\n" +
                                 $"Stack Trace: {ex.StackTrace}";
                UpdateStatus(errorDetails);
                System.Diagnostics.Debug.WriteLine($"IMPORT ERROR: {errorDetails}");
            }
        }

        private async void ExportAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (_decks.Count == 0)
            {
                UpdateStatus("No decks to export");
                return;
            }

            try
            {
                UpdateStatus("Starting export process...");
                
                var picker = new FileSavePicker();
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeChoices.Add("JSON files", new List<string>() { ".json" });
                picker.SuggestedFileName = "flashcard_decks_export";
                
                UpdateStatus("File save picker created, initializing window handle...");
                
                // Initialize the picker with the window handle
                var window = App.MainWindow;
                if (window != null)
                {
                    UpdateStatus($"Window found: {window.GetType().Name}");
                    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    UpdateStatus($"Window handle obtained: {hwnd}");
                    WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
                    UpdateStatus("File save picker initialized with window handle");
                }
                else
                {
                    UpdateStatus("ERROR: App.MainWindow is null!");
                    return;
                }

                UpdateStatus("Opening file save picker dialog...");
                var file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    UpdateStatus($"File save location selected: {file.Name}");
                    var json = System.Text.Json.JsonSerializer.Serialize(_decks, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    await FileIO.WriteTextAsync(file, json);
                    UpdateStatus($"Exported {_decks.Count} decks successfully");
                }
                else
                {
                    UpdateStatus("File save cancelled");
                }
            }
            catch (Exception ex)
            {
                var errorDetails = $"Export failed: {ex.Message}\n" +
                                 $"Exception Type: {ex.GetType().Name}\n" +
                                 $"Stack Trace: {ex.StackTrace}";
                UpdateStatus(errorDetails);
                System.Diagnostics.Debug.WriteLine($"EXPORT ERROR: {errorDetails}");
            }
        }
    }

    // Data Models
    public class FlashcardDeck
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Flashcard> Cards { get; set; } = new();
    }

    public class Flashcard
    {
        public string Front { get; set; } = string.Empty;
        public string Back { get; set; } = string.Empty;
    }
}
