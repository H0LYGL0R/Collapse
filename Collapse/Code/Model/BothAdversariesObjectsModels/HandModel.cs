using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.View;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Collapse.Code.View.BothAdversariesObjectsViews.Card;

namespace Collapse.Code.Model.BothAdversariesObjects
{
    public class HandModel
    {
        public List<CardView> Cards { get; private set; }
        public AdversaryModel Owner { get; private set; }
        public const byte Size = 8;
        private const byte Spacing = 15;
        private short YPosition { get; set; }

        public static short PlayersY { get; private set; } =
            (short)(GameView.WindowHeight - CardModel.HiddenModHeight);
        public static short EnemiesY { get; private set; } =
            (short)-CardModel.HiddenModHeight;

        public HandModel(AdversaryModel owner, short y)
        {
            Owner = owner;
            YPosition = y;
            Cards = new List<CardView>();
        }

        public void UpdateHandPositions()
        {
            var totalWidth = Cards.Count * CardModel.Width + (Cards.Count - 1) * Spacing;
            var startX = (GameView.WindowWidth - totalWidth) / 2;

            for (var i = 0; i < Cards.Count; i++)
            {
                var cardModel = Cards[i].Model as CardModel;
                if (!cardModel.IsTaken)
                {
                    var XPosition = startX + i * (CardModel.Width + Spacing);
                    cardModel.TargetPosition = new Vector2(XPosition, YPosition);
                    cardModel.ReplaceToTargetPosition();
                }
            }
        }

        public void AddCards(Vector2 startPosition, params CardView[] cards)
        {
            var addedCards = new List<CardView>();

            foreach (var card in cards)
            {
                if (Cards.Count >= Size) break;
                Cards.Add(card);
                addedCards.Add(card);

                var cardModel = card.Model as CardModel;
                cardModel.Position = startPosition;
                cardModel.OnMoveToField += () => Cards.Remove(card);
            }

            if (addedCards.Count > 0)
            {
                UpdateHandPositions();
                foreach (var card in addedCards)
                {
                    (card.Model as CardModel).Draw();
                }
            }
        }
    }
}