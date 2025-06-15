using Collapse.Code.Controller.Cursors;
using Collapse.Code.Model;
using Microsoft.Xna.Framework.Input;

public class PlayersCursorController : AdversariesCursorController
{
    public PlayersCursorController(CursorModel cursorModel) : base(cursorModel) {}
    
    protected override void UpdateCursorState()
    {
        MouseState = Mouse.GetState();
        IsClicked = MouseState.LeftButton == ButtonState.Pressed;
        XPosition = MouseState.X;
        YPosition = MouseState.Y;

        Model.Update(IsClicked, XPosition, YPosition);
    }
    protected override bool IsStageCorrectForPlacingCard() => GameModel.CurrentState == GameStage.PlayerTurn;
}

