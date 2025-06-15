using Collapse.Code.Controller.Cursors.EnemiesCursorCommands;
using Collapse.Code.Controller.Cursors;
using Collapse.Code.Model;
using System.Collections.Generic;

public class EnemiesCursorController : AdversariesCursorController
{
    private Queue<EnemiesCursorCommand> _commandQueue = new Queue<EnemiesCursorCommand>();
    private EnemiesCursorCommand _currentCommand;
    private bool _isAIProcessing;

    public EnemiesCursorController(CursorModel cursorModel) : base(cursorModel)
    {
        IsClicked = false;
    }

    public void AddCommand(EnemiesCursorCommand command)
    {
        _commandQueue.Enqueue(command);
        if (!_isAIProcessing)
        {
            _isAIProcessing = true;
        }
    }

    public void ClearCommands()
    {
        _commandQueue.Clear();
        _currentCommand = null;
        _isAIProcessing = false;
    }

    protected override void UpdateCursorState()
    {
        if (!_isAIProcessing) return;

        if (_currentCommand == null && _commandQueue.Count > 0)
        {
            _currentCommand = _commandQueue.Dequeue();
        }

        if (_currentCommand != null)
        {
            _currentCommand.Update();

            if (_currentCommand.IsCompleted)
            {
                _currentCommand = null;
                _isAIProcessing = _commandQueue.Count > 0;
            }
        }
    }

    protected override bool IsStageCorrectForPlacingCard() => GameModel.CurrentState == GameStage.EnemyTurn;
}