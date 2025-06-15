namespace Collapse.Code.Controller.Cursors.EnemiesCursorCommands
{
    public abstract class EnemiesCursorCommand
    {
        public bool IsCompleted { get; protected set; }
        public virtual void Update() =>
            _cursor.Model.Update(_cursor.IsClicked, (int)_cursor.Model.Position.X, (int)_cursor.Model.Position.Y);

        protected readonly EnemiesCursorController _cursor;

        public EnemiesCursorCommand(EnemiesCursorController cursor)
        {
            _cursor = cursor;
        }
    }
}