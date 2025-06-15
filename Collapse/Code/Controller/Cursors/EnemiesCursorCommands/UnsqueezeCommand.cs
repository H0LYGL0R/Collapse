
using Collapse.Code.Model;

namespace Collapse.Code.Controller.Cursors.EnemiesCursorCommands
{
    public class UnsqueezeCommand : EnemiesCursorCommand
    {
        public UnsqueezeCommand(EnemiesCursorController cursor) : base(cursor) { }
        public override void Update()
        {
            _cursor.IsClicked = false;
            IsCompleted = true;
            base.Update();
        }
    }
}
