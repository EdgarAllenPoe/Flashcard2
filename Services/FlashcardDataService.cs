using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlashcardApp.WinUI.Services
{
    /// <summary>
    /// Data service for managing flashcard decks and providing data binding support.
    /// </summary>
    public class FlashcardDataService : INotifyPropertyChanged
    {
        private readonly ObservableCollection<FlashcardDeck> _decks;
        private FlashcardDeck? _selectedDeck;
        private StudySession? _currentSession;

        public FlashcardDataService()
        {
            _decks = new ObservableCollection<FlashcardDeck>();
            InitializeSampleData();
        }

        public ObservableCollection<FlashcardDeck> Decks => _decks;

        public FlashcardDeck? SelectedDeck
        {
            get => _selectedDeck;
            set
            {
                if (_selectedDeck != value)
                {
                    _selectedDeck = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedDeckCards));
                }
            }
        }

        public ObservableCollection<Flashcard> SelectedDeckCards
        {
            get
            {
                if (_selectedDeck?.Cards != null)
                {
                    return new ObservableCollection<Flashcard>(_selectedDeck.Cards);
                }
                return new ObservableCollection<Flashcard>();
            }
        }

        public StudySession? CurrentSession
        {
            get => _currentSession;
            set
            {
                if (_currentSession != value)
                {
                    _currentSession = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void InitializeSampleData()
        {
            var spanishDeck = new FlashcardDeck
            {
                Id = 1,
                Name = "Spanish Vocabulary",
                Description = "Basic Spanish words and phrases",
                Cards = new List<Flashcard>
                {
                    new Flashcard { Front = "Hello", Back = "Hola" },
                    new Flashcard { Front = "Goodbye", Back = "Adiós" },
                    new Flashcard { Front = "Thank you", Back = "Gracias" },
                    new Flashcard { Front = "Please", Back = "Por favor" },
                    new Flashcard { Front = "Yes", Back = "Sí" }
                }
            };

            var mathDeck = new FlashcardDeck
            {
                Id = 2,
                Name = "Math Formulas",
                Description = "Essential mathematical formulas",
                Cards = new List<Flashcard>
                {
                    new Flashcard { Front = "Area of circle", Back = "πr²" },
                    new Flashcard { Front = "Pythagorean theorem", Back = "a² + b² = c²" },
                    new Flashcard { Front = "Quadratic formula", Back = "x = (-b ± √(b²-4ac)) / 2a" }
                }
            };

            var scienceDeck = new FlashcardDeck
            {
                Id = 3,
                Name = "Science Facts",
                Description = "General science knowledge",
                Cards = new List<Flashcard>
                {
                    new Flashcard { Front = "What is the capital of France?", Back = "Paris" },
                    new Flashcard { Front = "What is 2 + 2?", Back = "4" },
                    new Flashcard { Front = "What is the largest planet in our solar system?", Back = "Jupiter" },
                    new Flashcard { Front = "What is the chemical symbol for gold?", Back = "Au" },
                    new Flashcard { Front = "What year did World War II end?", Back = "1945" }
                }
            };

            _decks.Add(spanishDeck);
            _decks.Add(mathDeck);
            _decks.Add(scienceDeck);
        }

        public void AddDeck(FlashcardDeck deck)
        {
            deck.Id = _decks.Count > 0 ? _decks.Max(d => d.Id) + 1 : 1;
            _decks.Add(deck);
        }

        public void UpdateDeck(FlashcardDeck deck)
        {
            var existingDeck = _decks.FirstOrDefault(d => d.Id == deck.Id);
            if (existingDeck != null)
            {
                var index = _decks.IndexOf(existingDeck);
                _decks[index] = deck;
                if (_selectedDeck?.Id == deck.Id)
                {
                    SelectedDeck = deck;
                }
            }
        }

        public void DeleteDeck(int deckId)
        {
            var deck = _decks.FirstOrDefault(d => d.Id == deckId);
            if (deck != null)
            {
                _decks.Remove(deck);
                if (_selectedDeck?.Id == deckId)
                {
                    SelectedDeck = null;
                }
            }
        }

        public void AddCardToDeck(int deckId, Flashcard card)
        {
            var deck = _decks.FirstOrDefault(d => d.Id == deckId);
            if (deck != null)
            {
                deck.Cards.Add(card);
                if (_selectedDeck?.Id == deckId)
                {
                    OnPropertyChanged(nameof(SelectedDeckCards));
                }
            }
        }

        public void UpdateCardInDeck(int deckId, int cardIndex, Flashcard card)
        {
            var deck = _decks.FirstOrDefault(d => d.Id == deckId);
            if (deck != null && cardIndex >= 0 && cardIndex < deck.Cards.Count)
            {
                deck.Cards[cardIndex] = card;
                if (_selectedDeck?.Id == deckId)
                {
                    OnPropertyChanged(nameof(SelectedDeckCards));
                }
            }
        }

        public void DeleteCardFromDeck(int deckId, int cardIndex)
        {
            var deck = _decks.FirstOrDefault(d => d.Id == deckId);
            if (deck != null && cardIndex >= 0 && cardIndex < deck.Cards.Count)
            {
                deck.Cards.RemoveAt(cardIndex);
                if (_selectedDeck?.Id == deckId)
                {
                    OnPropertyChanged(nameof(SelectedDeckCards));
                }
            }
        }

        public StudySession StartStudySession(FlashcardDeck deck)
        {
            var session = new StudySession
            {
                Deck = deck,
                CurrentCardIndex = 0,
                CorrectCount = 0,
                IncorrectCount = 0,
                StartTime = DateTime.Now
            };
            CurrentSession = session;
            return session;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Data Models
    public class FlashcardDeck : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _description = string.Empty;
        private List<Flashcard> _cards = new();

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Flashcard> Cards
        {
            get => _cards;
            set
            {
                if (_cards != value)
                {
                    _cards = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Flashcard : INotifyPropertyChanged
    {
        private string _front = string.Empty;
        private string _back = string.Empty;

        public string Front
        {
            get => _front;
            set
            {
                if (_front != value)
                {
                    _front = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Back
        {
            get => _back;
            set
            {
                if (_back != value)
                {
                    _back = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class StudySession : INotifyPropertyChanged
    {
        private int _currentCardIndex;
        private int _correctCount;
        private int _incorrectCount;
        private bool _answerRevealed;

        public FlashcardDeck Deck { get; set; } = new();
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int CurrentCardIndex
        {
            get => _currentCardIndex;
            set
            {
                if (_currentCardIndex != value)
                {
                    _currentCardIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentCard));
                    OnPropertyChanged(nameof(Progress));
                    OnPropertyChanged(nameof(IsComplete));
                }
            }
        }

        public int CorrectCount
        {
            get => _correctCount;
            set
            {
                if (_correctCount != value)
                {
                    _correctCount = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Accuracy));
                }
            }
        }

        public int IncorrectCount
        {
            get => _incorrectCount;
            set
            {
                if (_incorrectCount != value)
                {
                    _incorrectCount = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Accuracy));
                }
            }
        }

        public bool AnswerRevealed
        {
            get => _answerRevealed;
            set
            {
                if (_answerRevealed != value)
                {
                    _answerRevealed = value;
                    OnPropertyChanged();
                }
            }
        }

        public Flashcard? CurrentCard =>
            CurrentCardIndex >= 0 && CurrentCardIndex < Deck.Cards.Count ? Deck.Cards[CurrentCardIndex] : null;

        public double Progress =>
            Deck.Cards.Count > 0 ? (double)CurrentCardIndex / Deck.Cards.Count : 0;

        public bool IsComplete => CurrentCardIndex >= Deck.Cards.Count;

        public double Accuracy =>
            (CorrectCount + IncorrectCount) > 0 ? (double)CorrectCount / (CorrectCount + IncorrectCount) : 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

