namespace BlackJack.Tests
{
    public class CardTests
    {
        [Theory]
        [InlineData("A", 11)]
        [InlineData("2", 2)]
        [InlineData("10", 10)]
        [InlineData("J", 10)]
        [InlineData("Q", 10)]
        [InlineData("K", 10)]
        public void Card_Value_ReturnsCorrectValue(string rank, int expectedValue)
        {
            var card = new Card(Suit.Hearts, rank);

            var value = card.Value;

            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void Card_Constructor_ThrowsExceptionForInvalidRank()
        {
            Assert.Throws<ArgumentException>(() => new Card(Suit.Hearts, "Invalid"));
        }

        [Fact]
        public void Card_IsAce_ReturnsTrueOnlyForAce()
        {
            var ace = new Card(Suit.Hearts, "A");
            var king = new Card(Suit.Diamonds, "K");
            var seven = new Card(Suit.Clubs, "7");

            Assert.True(ace.IsAce);
            Assert.False(king.IsAce);
            Assert.False(seven.IsAce);
        }

        [Fact]
        public void Card_GetAllRanks_ReturnsAll13Ranks()
        {
            var allRanks = Card.GetAllRanks();

            Assert.Equal(13, allRanks.Count());
            Assert.Contains("A", allRanks);
            Assert.Contains("K", allRanks);
            Assert.Contains("Q", allRanks);
            Assert.Contains("J", allRanks);
            Assert.Contains("10", allRanks);
        }
    }

    public class HandTests
    {
        [Fact]
        public void Hand_CalculateScore_SimpleCards()
        {
            var hand = new Hand();
            hand.AddCard(new Card(Suit.Hearts, "7"));
            hand.AddCard(new Card(Suit.Diamonds, "8"));

            var score = hand.CalculateScore();

            Assert.Equal(15, score);
        }

        [Theory]
        [InlineData(new string[] { "A", "K" }, 21)]      
        [InlineData(new string[] { "A", "5", "7" }, 13)] 
        [InlineData(new string[] { "A", "A", "9" }, 21)] 
        [InlineData(new string[] { "A", "A", "A", "A" }, 14)] 
        [InlineData(new string[] { "10", "J", "Q" }, 30)] 
        public void Hand_CalculateScore_VariousCombinations(string[] ranks, int expectedScore)
        {

            var hand = new Hand();
            foreach (var rank in ranks)
            {
                hand.AddCard(new Card(Suit.Hearts, rank));
            }

            var score = hand.CalculateScore();

            Assert.Equal(expectedScore, score);
        }

        [Fact]
        public void Hand_HasBlackjack_ReturnsTrueForAceAndTenValueCard()
        {
            var hand1 = new Hand();
            hand1.AddCard(new Card(Suit.Hearts, "A"));
            hand1.AddCard(new Card(Suit.Diamonds, "K"));

            var hand2 = new Hand();
            hand2.AddCard(new Card(Suit.Clubs, "A"));
            hand2.AddCard(new Card(Suit.Spades, "Q"));

            var hand3 = new Hand();
            hand3.AddCard(new Card(Suit.Hearts, "A"));
            hand3.AddCard(new Card(Suit.Diamonds, "5"));
            hand3.AddCard(new Card(Suit.Clubs, "5"));

            Assert.True(hand1.HasBlackjack);
            Assert.True(hand2.HasBlackjack);
            Assert.False(hand3.HasBlackjack);
        }

        [Fact]
        public void Hand_IsSoftHand_ReturnsTrueWhenAceCanBe11WithoutBust()
        {
            var softHand = new Hand(); 
            softHand.AddCard(new Card(Suit.Hearts, "A"));
            softHand.AddCard(new Card(Suit.Diamonds, "6"));

            var hardHand = new Hand(); 
            hardHand.AddCard(new Card(Suit.Hearts, "A"));
            hardHand.AddCard(new Card(Suit.Diamonds, "6"));
            hardHand.AddCard(new Card(Suit.Clubs, "10"));

            var bustHand = new Hand(); 
            bustHand.AddCard(new Card(Suit.Hearts, "10"));
            bustHand.AddCard(new Card(Suit.Diamonds, "10"));
            bustHand.AddCard(new Card(Suit.Clubs, "5"));

            Assert.True(softHand.IsSoftHand);
            Assert.False(hardHand.IsSoftHand);
            Assert.False(bustHand.IsSoftHand);
        }
    }

    public class DeckTests
    {
        [Fact]
        public void Deck_Initialization_Creates52Cards()
        {
            var deck = new Deck();

            Assert.Equal(52, deck.CardsRemaining);
        }

        [Fact]
        public void Deck_Shuffle_RandomizesCards()
        {
            var deck1 = new Deck();
            var deck2 = new Deck();

            var originalOrder = new List<Card>();
            while (deck1.CardsRemaining > 0)
            {
                originalOrder.Add(deck1.DrawCard());
            }

            deck2.Shuffle();
            var shuffledOrder = new List<Card>();
            while (deck2.CardsRemaining > 0)
            {
                shuffledOrder.Add(deck2.DrawCard());
            }


            Assert.Equal(52, originalOrder.Count);
            Assert.Equal(52, shuffledOrder.Count);

            var allCards = originalOrder.Concat(shuffledOrder).ToList();
            var distinctCards = allCards.Select(c => $"{c.Rank}{c.Suit}").Distinct();
            Assert.Equal(52, distinctCards.Count());
        }

        [Fact]
        public void Deck_DrawMultiple_ReturnsCorrectNumberOfCards()
        {

            var deck = new Deck();
            var initialCount = deck.CardsRemaining;

            var drawnCards = deck.DrawMultiple(5);

            Assert.Equal(5, drawnCards.Count);
            Assert.Equal(initialCount - 5, deck.CardsRemaining);
        }
    }
}