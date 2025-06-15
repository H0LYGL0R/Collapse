using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Collapse.Code.Model;
using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.View.BothAdversariesObjectsViews.Card;

namespace Collapse.Code.Model
{
    public static class GameBalancer
    {
        private const ushort TotalPoints = 1000;
        private const double CostMultiplier = 0.85;
        private const byte MinCardCount = 18;
        private const byte MaxCardCount = 22;
        private const byte BaseStartPower = 25;
        private const byte BaseStartMoney = 8;
        private const byte MinCardCost = 1;

        private const byte PowerDistributionDivisor = 3;
        private const byte MoneyDistributionDivisor = 5;
        private const double MinPowerModifier = 0.8;
        private const double MaxPowerModifier = 1.2;
        private const double MinCostModifier = 0.8;
        private const double MaxCostModifier = 1.2;

        private const byte PointsVariance = 50;
        private const double StatVariance = 0.15;
        private const byte MinPowerAdd = 1;
        private const byte MaxPowerAdd = 5;
        private const byte MinCostAdd = 1;
        private const byte MaxCostAdd = 4;
        private const byte CostRedistributeChance = 7;
        private const byte CostRedistributeThreshold = 10;

        private static readonly Dictionary<StatType, double> StatWeights = new Dictionary<StatType, double>
        {
            { StatType.StartPower, 0.20 },
            { StatType.StartMoney, 0.15 },
            { StatType.CardPower, 0.35 },
            { StatType.CardCost, 0.30 }
        };

        private enum StatType
        {
            StartPower,
            StartMoney,
            CardPower,
            CardCost
        }

        private class CardParameters
        {
            public byte Power { get; }
            public byte Cost { get; }
            public string Title { get; }

            public CardParameters(byte power, byte cost, string title)
            {
                Power = power;
                Cost = cost;
                Title = title;
            }
        }

        public static void GenerateDecks(AdversaryModel player, AdversaryModel enemy,
                                       GraphicsDevice graphicsDevice)
        {
            var playerPoints = TotalPoints / 2 + GameModel.Random.Next(-PointsVariance, PointsVariance);
            var enemyPoints = TotalPoints - playerPoints;

            var playerStats = GenerateStats(playerPoints);
            var enemyStats = GenerateStats(enemyPoints);

            player.Power = (byte)(BaseStartPower + playerStats[StatType.StartPower] / PowerDistributionDivisor);
            player.Money = (byte)(BaseStartMoney + playerStats[StatType.StartMoney] / MoneyDistributionDivisor);
            enemy.Power = (byte)(BaseStartPower + enemyStats[StatType.StartPower] / PowerDistributionDivisor);
            enemy.Money = (byte)(BaseStartMoney + enemyStats[StatType.StartMoney] / MoneyDistributionDivisor);

            GenerateAndShuffleDeck(player, playerStats, graphicsDevice);
            GenerateAndShuffleDeck(enemy, enemyStats, graphicsDevice);
        }

        private static void GenerateAndShuffleDeck(AdversaryModel adversary, Dictionary<StatType, int> stats,
                                           GraphicsDevice graphicsDevice)
        {
            var cards = GenerateCardList(stats);
            var cardsToCreate = new List<CardParameters>(cards);

            for (var i = 0; i < cardsToCreate.Count; i++)
            {
                var card = cardsToCreate[i];
                var generatedImage = CardPictureGenerator.GenerateCardPicture(
                    CardModel.Width,
                    CardModel.Height);

                adversary.DrawPileModel.CreateNewCard(
                    graphicsDevice,
                    card.Cost,
                    card.Power,
                    generatedImage,
                    card.Title);
            }
        }

        private static List<CardParameters> GenerateCardList(Dictionary<StatType, int> stats)
        {
            var cardCount = GameModel.Random.Next(MinCardCount, MaxCardCount + 1);
            var cards = new List<CardParameters>();

            var totalCardPower = stats[StatType.CardPower];
            var totalCardCost = (int)(stats[StatType.CardCost] * CostMultiplier);
            totalCardCost = Math.Max(totalCardCost, cardCount * MinCardCost);

            for (var i = 0; i < cardCount; i++)
            {
                var power = Math.Max(1, totalCardPower / (cardCount - i + 2));
                power = (int)(power * (MinPowerModifier + GameModel.Random.NextDouble() * (MaxPowerModifier - MinPowerModifier)));
                power = Math.Min(power, totalCardPower);

                var cost = Math.Max(MinCardCost, (int)(totalCardCost * 1.8 / (cardCount - i + 1)));
                cost = (int)(cost * (MinCostModifier + GameModel.Random.NextDouble() * (MaxCostModifier - MinCostModifier)));
                cost = Math.Min(cost, totalCardCost);

                totalCardPower -= power;
                totalCardCost -= cost;

                cards.Add(new CardParameters((byte)power, (byte)cost, CardTitleGenerator.Generate()));
            }

            RedistributeRemainingPoints(cards, ref totalCardPower, ref totalCardCost);
            return cards;
        }

        private static void RedistributeRemainingPoints(List<CardParameters> cards,
            ref int remainingPower, ref int remainingCost)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                if (card.Cost < MinCardCost && remainingCost > 0)
                {
                    var needed = MinCardCost - card.Cost;
                    var add = Math.Min(needed, remainingCost);
                    remainingCost -= add;
                    cards[i] = new CardParameters(card.Power, (byte)(card.Cost + add), card.Title);
                }
            }

            while (remainingPower > 0 || remainingCost > 0)
            {
                var index = GameModel.Random.Next(cards.Count);
                var card = cards[index];

                if (remainingPower > 0)
                {
                    var addPower = Math.Min(remainingPower, GameModel.Random.Next(MinPowerAdd, MaxPowerAdd));
                    remainingPower -= addPower;
                    cards[index] = new CardParameters((byte)(card.Power + addPower), card.Cost, card.Title);
                }

                if (remainingCost > 0)
                {
                    var addCost = Math.Min(remainingCost, GameModel.Random.Next(MinCostAdd, MaxCostAdd));
                    remainingCost -= addCost;
                    cards[index] = new CardParameters(card.Power, (byte)(card.Cost + addCost), card.Title);
                }
            }
        }

        private static Dictionary<StatType, int> GenerateStats(int totalPoints)
        {
            var stats = new Dictionary<StatType, int>();
            var remainingPoints = totalPoints;

            foreach (var stat in StatWeights)
            {
                var basePoints = (int)(totalPoints * stat.Value);
                var variance = (int)(basePoints * StatVariance);
                var points = basePoints + GameModel.Random.Next(-variance, variance);

                points = Math.Clamp(points, 1, remainingPoints - (StatWeights.Count - stats.Count - 1));
                stats.Add(stat.Key, points);
                remainingPoints -= points;
            }

            if (remainingPoints > 0)
            {
                var randomStat = GameModel.Random.Next(CostRedistributeThreshold) < CostRedistributeChance
                    ? StatType.CardCost
                    : StatWeights.Keys.ElementAt(GameModel.Random.Next(StatWeights.Count));
                stats[randomStat] += remainingPoints;
            }

            return stats;
        }
    }
}
