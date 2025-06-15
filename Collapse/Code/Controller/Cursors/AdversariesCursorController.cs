using Microsoft.Xna.Framework.Input;
using System;
using Collapse.Code.Model.GameObjects;
using Collapse.Code.Model;

namespace Collapse.Code.Controller.Cursors
{
    public abstract class AdversariesCursorController : IUpdatable
    {
        public CursorModel Model { get; protected set; }
        protected MouseState MouseState { get; set; }
        public bool IsClicked { get; set; }

        protected int XPosition { get; set; }
        protected int YPosition { get; set; }

        protected bool WasClicked { get; set; }

        public event Action OnUpdate;

        public AdversariesCursorController(CursorModel cursorModel)
        {
            Model = cursorModel;
            OnUpdate = UpdateCursorState;
            Model.IsStageCorrect += IsStageCorrectForPlacingCard;
        }

        protected abstract void UpdateCursorState();

        protected abstract bool IsStageCorrectForPlacingCard();

        public void Update() => OnUpdate.Invoke();
    }
}