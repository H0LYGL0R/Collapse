using System;
using Collapse.Code.Model;
using Collapse.Code.Model.GameObjects;
using Microsoft.Xna.Framework;

namespace Collapse.Code.Controller.Cursors.EnemiesCursorCommands.MovingCursorCommands
{
    public class FollowToCommand : MovingCursorCommand
    {
        private readonly GameObjectModel _followingObject;
        private Vector2 _previousTargetsPosition;

        public FollowToCommand(EnemiesCursorController cursor, Vector2 targetPosition, GameObjectModel objectModel)
            : base(cursor)
        {
            _targetPosition = targetPosition;
            _followingObject = objectModel;
            _previousTargetsPosition = objectModel.Position;
            _followingObject.OnChangePosition += ChangeTargetPosition;
        }

        protected override void OnTargetReached()
        {
            base.OnTargetReached();
            _followingObject.OnChangePosition -= ChangeTargetPosition;
        }

        private void ChangeTargetPosition(Vector2 newTargetsPosition)
        {
            _targetPosition += newTargetsPosition - _previousTargetsPosition;
            _previousTargetsPosition = newTargetsPosition;
        }
    }
}
