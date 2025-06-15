using Collapse.Code.Controller.Cursors.EnemiesCursorCommands;
using Microsoft.Xna.Framework;
using System;


public class MoveToCommand : MovingCursorCommand
{
    public MoveToCommand(EnemiesCursorController cursor, Vector2 targetPosition) : base(cursor)
    {
        _targetPosition = targetPosition;
    }
}