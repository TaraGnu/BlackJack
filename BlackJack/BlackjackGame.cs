namespace BlackJack
{
    public class BlackjackGame
    {
        private Deck deck;
        private Hand playerHand;
        private Hand dealerHand;
        private bool gameEnded;

        public void StartGame()
        {
            InitializeGame();

            Console.WriteLine("Начало игры в Blackjack!");
            Console.WriteLine("=======================\n");

            DealInitialCards();

            if (CheckForNaturalBlackjack())
            {
                EndGame();
                return;
            }

            PlayerTurn();
        }

        private void InitializeGame()
        {
            deck = new Deck();
            playerHand = new Hand();
            dealerHand = new Hand();
            gameEnded = false;

            deck.Shuffle();
        }

        private void DealInitialCards()
        {
            playerHand.AddCards(deck.DrawMultiple(2));

            dealerHand.AddCards(deck.DrawMultiple(2));
        }

        private bool CheckForNaturalBlackjack()
        {
            bool playerHasBlackjack = playerHand.HasBlackjack;
            bool dealerHasBlackjack = dealerHand.HasBlackjack;

            if (playerHasBlackjack || dealerHasBlackjack)
            {
                Console.WriteLine("\n=== НАТИВНЫЙ БЛЭКДЖЕК ===");
                return true;
            }

            return false;
        }

        private void PlayerTurn()
        {
            while (!gameEnded && !playerHand.IsBusted)
            {
                DisplayGameState(false);

                Console.WriteLine("\nДоступные действия:");
                Console.WriteLine("H - Взять карту (Hit)");
                Console.WriteLine("S - Остановиться (Stand)");
                Console.WriteLine("R - Сбросить и начать заново");
                Console.Write("Ваш выбор: ");

                var choice = Console.ReadLine()?.Trim().ToUpper();

                switch (choice)
                {
                    case "H":
                        playerHand.AddCard(deck.DrawCard());
                        Console.WriteLine($"\nВы взяли: {playerHand.Cards.Last()}");

                        if (playerHand.IsBusted)
                        {
                            Console.WriteLine("\n ПЕРЕБОР! У вас больше 21.");
                            EndGame();
                        }
                        else if (playerHand.CalculateScore() == 21)
                        {
                            Console.WriteLine("\n У вас 21!");
                            DealerTurn();
                        }
                        break;

                    case "S":
                        DealerTurn();
                        break;

                    case "R":
                        Console.WriteLine("\nИгра сброшена. Начинаем заново!\n");
                        StartGame();
                        return;

                    default:
                        Console.WriteLine("Неверный ввод. Попробуйте снова.");
                        break;
                }
            }
        }

        private void DealerTurn()
        {
            Console.WriteLine("\nХод дилера...");
            DisplayGameState(true);

            while (dealerHand.CalculateScore() < 17 && !dealerHand.IsBusted)
            {
                var newCard = deck.DrawCard();
                dealerHand.AddCard(newCard);
                Console.WriteLine($"Дилер берет: {newCard}");
            }

            EndGame();
        }

        private void DisplayGameState(bool revealDealerHand)
        {
            Console.Clear();
            Console.WriteLine("=== BLACKJACK 21 ===");
            Console.WriteLine($"Карт в колоде: {deck.CardsRemaining}\n");

            Console.WriteLine("ДИЛЕР:");
            if (revealDealerHand)
            {
                Console.WriteLine($"  Карты: {dealerHand}");
                Console.WriteLine($"  Счет: {dealerHand.CalculateScore()}");
            }
            else
            {
                var dealerCards = dealerHand.Cards;
                Console.WriteLine($"  Карты: {dealerCards[0]}, [скрытая карта]");
                Console.WriteLine($"  Счет: {dealerCards[0].Value} + ?");
            }

            Console.WriteLine("\nИГРОК:");
            Console.WriteLine($"  Карты: {playerHand}");
            Console.WriteLine($"  Счет: {playerHand.CalculateScore()}");
            Console.WriteLine($"  Состояние: {(playerHand.IsBusted ? "ПЕРЕБОР" : playerHand.HasBlackjack ? "БЛЭКДЖЕК" : "В игре")}");
        }

        private void EndGame()
        {
            gameEnded = true;

            Console.Clear();
            DisplayGameState(true);

            DetermineWinner();

            AskForNewGame();
        }

        private void DetermineWinner()
        {
            int playerScore = playerHand.CalculateScore();
            int dealerScore = dealerHand.CalculateScore();

            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("РЕЗУЛЬТАТ ИГРЫ:");
            Console.WriteLine($"Ваш счет: {playerScore}");
            Console.WriteLine($"Счет дилера: {dealerScore}");
            Console.WriteLine(new string('=', 40));

            if (playerHand.IsBusted)
            {
                Console.WriteLine("ВЫ ПРОИГРАЛИ: Перебор!");
            }
            else if (dealerHand.IsBusted)
            {
                Console.WriteLine("ВЫ ВЫИГРАЛИ: У дилера перебор!");
            }
            else if (playerHand.HasBlackjack && !dealerHand.HasBlackjack)
            {
                Console.WriteLine("ВЫ ВЫИГРАЛИ: Blackjack!");
            }
            else if (playerScore > dealerScore)
            {
                Console.WriteLine("ВЫ ВЫИГРАЛИ!");
            }
            else if (playerScore < dealerScore)
            {
                Console.WriteLine("ВЫ ПРОИГРАЛИ!");
            }
            else
            {
                Console.WriteLine("НИЧЬЯ!");
            }
        }

        private void AskForNewGame()
        {
            Console.WriteLine("\nХотите сыграть еще раз? (Y/N)");
            var response = Console.ReadLine()?.Trim().ToUpper();

            if (response == "Y" || response == "Д")
            {
                Console.Clear();
                StartGame();
            }
            else
            {
                Console.WriteLine("\nСпасибо за игру! До свидания!");
            }
        }
    }
}
