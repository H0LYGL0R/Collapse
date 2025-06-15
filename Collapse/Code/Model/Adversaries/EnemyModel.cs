using System.Collections.Generic;
using System.Linq;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Microsoft.Xna.Framework;
using Collapse.Code.Controller.Cursors.EnemiesCursorCommands;
using Collapse.Code.View;
using System;
using Collapse.Code.Controller.Cursors.EnemiesCursorCommands.MovingCursorCommands;

namespace Collapse.Code.Model.AdversariesModels
{
    public class EnemyModel : AdversaryModel
    {
        private const int MaxCardsPerTurn = 3;

        public EnemyModel(ushort pilesX, ushort fieldY, short handY, ushort moneyCounterY,
            ushort powerCounterY, ushort moneyCounterX, ushort powerCounterX,
            GameView.GameObjectsManager objectsManager) :
            base(pilesX, fieldY, handY, moneyCounterY, powerCounterY, moneyCounterX, powerCounterX, objectsManager)
        { }

        public override void CreateOwnModels()
        {
            base.CreateOwnModels();
            CursorController = new EnemiesCursorController(CursorModel);
        }

        public override void StartTurn()
        {
            base.StartTurn();
            MakeStrategicTurn();
        }

        private void MakeStrategicTurn()
        {
            var cursor = (CursorController as EnemiesCursorController);
            cursor.ClearCommands();

            var emptySlots = FieldModel.EmptySlotsNumbers.ToList();
            if (emptySlots.Count == 0)
            {
                CompleteTurn(cursor);
                return;
            }

            var currentMoney = Money;

            var cardModels = HandModel.Cards
                .Select(card => card.Model as CardModel)
                .ToList();

            var affordableCards = cardModels
                .Where(c => c.Cost <= currentMoney)
                .OrderBy(c => c.Cost)
                .ToList();

            if (affordableCards.Count == 0)
            {
                CompleteTurn(cursor);
                return;
            }

            var cardsToPlay = Math.Min(MaxCardsPerTurn, Math.Min(affordableCards.Count, emptySlots.Count));

            for (var i = 0; i < cardsToPlay; i++)
            {
                var card = affordableCards[i];

                if (card.Cost > currentMoney)
                    break;

                var slotNumber = emptySlots[i]; 
                PlayCard(cursor, card, slotNumber);
                currentMoney -= card.Cost;
            }

            CompleteTurn(cursor);
        }

        private void PlayCard(EnemiesCursorController cursor, CardModel card, byte slotNumber)
        {
            var selectedSlot = FieldModel.Slots[slotNumber];

            var cardPosition = card.Position + new Vector2(
                GameModel.Random.Next(CardModel.Width - 1),
                CardModel.HiddenModHeight +
                CardModel.Height - 1 - CardModel.HiddenModHeight);

            var slotPosition = new Vector2(
                selectedSlot.Position.X + CardSlotModel.Width / 2,
                selectedSlot.Position.Y + CardSlotModel.Height / 2);

            cursor.AddCommand(new FollowToCommand(cursor, cardPosition, card));
            cursor.AddCommand(new SqueezeCommand(cursor));
            cursor.AddCommand(new MoveToCommand(cursor, slotPosition));
            cursor.AddCommand(new UnsqueezeCommand(cursor));
        }

        private void CompleteTurn(EnemiesCursorController cursor)
        {
            var buttonPosition = new Vector2(StageButtonModel.XPosition, StageButtonModel.YPosition) +
                new Vector2(
                    GameModel.Random.Next(StageButtonModel.Width - 1 - CursorModel.Width),
                    GameModel.Random.Next(StageButtonModel.Height - 1 - CursorModel.Height));

            cursor.AddCommand(new MoveToCommand(cursor, buttonPosition));
            cursor.AddCommand(new SqueezeCommand(cursor));
            cursor.AddCommand(new UnsqueezeCommand(cursor));
            cursor.AddCommand(new MoveToCommand(cursor, CursorModel.EnemiesCursorBasicPosition));
        }
    }
}