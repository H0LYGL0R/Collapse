using Collapse.Code.Model.BothAdversariesObjects;
using Collapse.Code.Model.BothAdversariesObjectsModels.Counters;
using Collapse.Code.View.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Collapse.Code.View.BothAdversariesObjectsViews.Counters
{
    public class PowerCounterView : CounterView
    {
        public PowerCounterView(PowerCounterModel model, GraphicsDevice graphicsDevice) :
            base(model, graphicsDevice, Image.PowerCounter, new Color(163, 74, 164), Text.Font.RoundsBlack)
        { }
    }
}
