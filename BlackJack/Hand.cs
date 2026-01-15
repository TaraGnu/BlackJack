
namespace BlackJack
{
    public class Hand
    {
        private readonly List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        public IReadOnlyList<Card> Cards => cards.AsReadOnly();

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public void AddCards(IEnumerable<Card> newCards)
        {
            cards.AddRange(newCards);
        }

        public void Clear()
        {
            cards.Clear();
        }

        public int CalculateScore()
        {
            int score = 0;
            int aceCount = 0;

            foreach (var card in cards)
            {
                score += card.Value;
                if (card.IsAce)
                    aceCount++;
            }

            while (score > 21 && aceCount > 0)
            {
                score -= 10; 
                aceCount--;
            }

            return score;
        }

        public bool IsBusted => CalculateScore() > 21;

        public bool HasBlackjack => CalculateScore() == 21 && cards.Count == 2;

        public bool IsSoftHand
        {
            get
            {
                int scoreWithoutAceAdjustment = cards.Sum(c => c.Value);
                return scoreWithoutAceAdjustment <= 21 && cards.Any(c => c.IsAce);
            }
        }

        public override string ToString()
        {
            return string.Join(", ", cards.Select(c => c.ToString()));
        }
    }
}
