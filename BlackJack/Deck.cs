
namespace BlackJack
{
    public class Deck
    {
        private List<Card> cards;
        private Random random;

        public Deck()
        {
            cards = new List<Card>();
            random = new Random();
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            cards.Clear();

            var allRanks = Card.GetAllRanks();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (var rank in allRanks)
                {
                    cards.Add(new Card(suit, rank));
                }
            }
        }

        public void Shuffle()
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }
        }

        public Card DrawCard()
        {
            if (cards.Count == 0)
            {
                InitializeDeck();
                Shuffle();
                Console.WriteLine("Колода перетасована заново!");
            }

            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public int CardsRemaining => cards.Count;

        public void Reshuffle()
        {
            InitializeDeck();
            Shuffle();
        }

        public List<Card> DrawMultiple(int count)
        {
            var drawnCards = new List<Card>();
            for (int i = 0; i < count; i++)
            {
                drawnCards.Add(DrawCard());
            }
            return drawnCards;
        }
    }
}
