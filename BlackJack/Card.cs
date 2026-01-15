namespace BlackJack
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public class Card
    {
        private static readonly Dictionary<string, int> _cardValues = new Dictionary<string, int>
        {
            { "A", 11 },  
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "10", 10 },
            { "J", 10 }, 
            { "Q", 10 }, 
            { "K", 10 }  
        };

        private static readonly Dictionary<string, string> _cardDisplayNames = new Dictionary<string, string>
        {
            { "A", "Туз" },
            { "J", "Валет" },
            { "Q", "Дама" },
            { "K", "Король" }
        };

        public Suit Suit { get; }
        public string Rank { get; }
        public int Value => _cardValues[Rank];

        public Card(Suit suit, string rank)
        {
            if (!_cardValues.ContainsKey(rank))
                throw new ArgumentException($"Недопустимый ранг карты: {rank}");

            Suit = suit;
            Rank = rank;
        }

        public bool IsAce => Rank == "A";
        public bool IsFaceCard => Rank == "J" || Rank == "Q" || Rank == "K";

        public override string ToString()
        {
            string rankName = _cardDisplayNames.ContainsKey(Rank)
                ? _cardDisplayNames[Rank]
                : Rank;

            string suitSymbol = Suit switch
            {
                Suit.Hearts => "♥",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣",
                Suit.Spades => "♠",
                _ => "?"
            };

            return $"{rankName}{suitSymbol}";
        }

        public static bool IsValidRank(string rank) => _cardValues.ContainsKey(rank);
        public static IEnumerable<string> GetAllRanks() => _cardValues.Keys;
    }
}
