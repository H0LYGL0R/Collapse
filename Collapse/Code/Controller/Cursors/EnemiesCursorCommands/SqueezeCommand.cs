using Collapse.Code.Controller.Cursors.EnemiesCursorCommands;
using Collapse.Code.Controller.Cursors;
using Microsoft.Xna.Framework;
using Collapse.Code.Model;

namespace Collapse.Code.Controller.Cursors.EnemiesCursorCommands
{
    public class SqueezeCommand : EnemiesCursorCommand
    {

        public SqueezeCommand(EnemiesCursorController cursor) : base(cursor) { }

        public override void Update()
        {
            _cursor.IsClicked = true;
            IsCompleted = true;
            base.Update();
        }
    }
}
