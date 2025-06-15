using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.View;
using Collapse.Code.View.BothAdversariesObjectsViews.Card;
using Microsoft.Xna.Framework.Media;

namespace Collapse.Code.Model.BothAdversariesObjects.Piles
{
    public class DiscardPileModel : PileModel
    {
        public DiscardPileModel(AdversaryModel owner, int x, int y, GameView.GameObjectsManager objectsManager) : 
            base(owner, x, y, objectsManager) { }

        public void AddCard(CardView card)
        {
            Cards.Add(card);
            NotifyCardsQuantityChange();

        }
    }
}