using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.View.Assets;

namespace Collapse.Code.View.BothAdversariesObjectsViews.Field
{
    public class CardSlotView : GameObjectView
    {

        public CardSlotView(CardSlotModel model) : base(RenderLayer.Background)
        {
            _texture = Image.CardSlot;
            Model = model;
        }
    }
}
