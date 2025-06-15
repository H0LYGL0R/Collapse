using System.Collections.Generic;
using System.Linq;
using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.View;
using System;
using Collapse.Code.View.BothAdversariesObjectsViews.Card;


namespace Collapse.Code.Model.BothAdversariesObjects.Piles
{
    public class DrawPileModel : PileModel
    {
        public DrawPileModel(AdversaryModel owner, int x, int y, GameView.GameObjectsManager objectsManager) :
            base(owner, x, y, objectsManager) { }

        public void DrawTopCards(byte count = 1)
        {
            if (count == 0) return;

            var drawnCards = new List<CardView>();

            for (var i = 0; i < count; i++)
            {
                if (Cards.Count == 0)
                {
                    if (OwnerModel.DiscardPileModel.Cards.Count == 0) return;
                    ShuffleDiscardPileIntoDrawPile();
                    return;
                }

                var drawnCard = Cards[0];
                Cards.RemoveAt(0);
                drawnCards.Add(drawnCard);
            }

            if (drawnCards.Count > 0)
            {
                OwnerModel.HandModel.AddCards(Position, drawnCards.ToArray());
            }

            NotifyCardsQuantityChange();
        }

        public void ShuffleDiscardPileIntoDrawPile()
        {
            NotifyShuffle();
            var discardPile = OwnerModel.DiscardPileModel as PileModel;
            if (discardPile.Cards.Count == 0)
                return;
            var shuffledCards = discardPile.Cards
                .OrderBy(card => GameModel.Random.Next())
                .ToList();

            foreach (var card in shuffledCards)
            {
                (card.Model as CardModel).ShuffleIntoDrawPileFromDiscardPile();
                Cards.Add(card);
                discardPile.Cards.RemoveAt(0);
            }
            discardPile.NotifyCardsQuantityChange();
        }
    }
}