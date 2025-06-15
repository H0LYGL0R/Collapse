using System;

namespace Collapse.Code.Model.GameObjects
{
    public abstract class ClickableGameObjectModel: GameObjectModel
    {
        public event Action OnClick;
        public ClickableGameObjectModel(uint width, uint height) : 
            base(width, height){ }

        protected void ClickIfCollides(int x, int y)
        {
            if (DoesCollidePoint(x, y)) Click();
        }


        protected void Click() => OnClick?.Invoke();
    }
}
