using Collapse.Code.View.Assets;
using Collapse.Code.Model;

namespace Collapse.Code.View
{
    public class StageButtonView : GameObjectView
    {
        public StageButtonView(StageButtonModel model) : base(RenderLayer.Midground) 
        {
            Model = model;
            model.OnPressedChange += (_) => SetTexture();
            model.OnPressedChange += (isPressed) => { if (isPressed) Audio.StageButtonClick.Play(); };
            SetTexture();
        }
        private void SetTexture()
        {
            _texture = (Model as StageButtonModel).Pressed ? Image.PressedButton : Image.UnpressedButton;
        }
    }
}
