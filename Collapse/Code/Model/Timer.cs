using Collapse.Code.Model.GameObjects;
using Collapse.Code.View;
using System;


namespace Collapse.Code.Model
{
    public class Timer : IUpdatable
    {
        public event Action OnUpdate;
        public event Action OnTimerTicks;
        private event Action OnTimerIsOver;

        public uint FramesQuantity { get; private set; }
        private readonly GameView.GameObjectsManager _objectsManager;

        public Timer(uint framesQuantity, GameView.GameObjectsManager objectsManager, Action onTimerIsOver)
        {
            FramesQuantity = framesQuantity;
            _objectsManager = objectsManager;
            OnUpdate = Tick;
            OnTimerIsOver = onTimerIsOver;
            _objectsManager.AddUpdatable(this);
        }

        private void Tick()
        {
            OnTimerTicks?.Invoke();
            FramesQuantity -= 1;
            if (FramesQuantity == 0)
            {
                _objectsManager.RemoveUpdatable(this);
                OnTimerIsOver.Invoke();
            }
        }

        public void Update() => OnUpdate?.Invoke();
    }
}
