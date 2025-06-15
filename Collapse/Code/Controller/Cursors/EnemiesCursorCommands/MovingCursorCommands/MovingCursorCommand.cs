using Collapse.Code.Controller.Cursors;
using Microsoft.Xna.Framework;
using System;

namespace Collapse.Code.Controller.Cursors.EnemiesCursorCommands
{
    public abstract class MovingCursorCommand : EnemiesCursorCommand
    {
        protected Vector2 _targetPosition;
        private byte MoveSpeed { get; set; }
        private const byte OriginMoveSpeed = 5;
        private const byte MoveIncreaseFrequency = 25;
        private byte _framesUntilMoveIncrease;

        protected MovingCursorCommand(EnemiesCursorController cursor) : base(cursor)
        {
            MoveSpeed = OriginMoveSpeed;
            _framesUntilMoveIncrease = MoveIncreaseFrequency;
        }

        public override void Update()
        {
            var direction = _targetPosition - _cursor.Model.Position;
            var distance = direction.Length();

            if (distance <= MoveSpeed)
            {
                OnTargetReached();
                return;
            }

            direction.Normalize();
            var newPosition = _cursor.Model.Position + direction * MoveSpeed;
            _cursor.Model.Update(_cursor.IsClicked, (int)newPosition.X, (int)newPosition.Y);

            _framesUntilMoveIncrease--;
            if (_framesUntilMoveIncrease == 0)
            {
                MoveSpeed++;
                _framesUntilMoveIncrease = MoveIncreaseFrequency;
            }
        }

        protected virtual void OnTargetReached()
        {
            _cursor.Model.Update(_cursor.IsClicked, (int)_targetPosition.X, (int)_targetPosition.Y);
            IsCompleted = true;
        }

        protected void IncreaseSpeed()
        {
            _framesUntilMoveIncrease--;
            if (_framesUntilMoveIncrease == 0)
            {
                MoveSpeed++;
                _framesUntilMoveIncrease = MoveIncreaseFrequency;
            }
        }
    }
}