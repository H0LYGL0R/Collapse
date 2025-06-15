using System;

namespace Collapse.Code.Model.GameObjects
{
    public interface IUpdatable
    {
        public event Action OnUpdate;
        public void Update();
    }
}
