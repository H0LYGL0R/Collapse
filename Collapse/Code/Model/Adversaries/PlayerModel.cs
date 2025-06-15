using Collapse.Code.View;

namespace Collapse.Code.Model.AdversariesModels
{
    public class PlayerModel : AdversaryModel
    {

        public PlayerModel(ushort pilesX, ushort fieldY, short handY, ushort moneyCounterY,
                           ushort powerCounterY, ushort moneyCounterX, ushort powerCounterX,
                           GameView.GameObjectsManager objectsManager) : 
            base(pilesX, fieldY, handY, moneyCounterY, powerCounterY, 
                moneyCounterX, powerCounterX, objectsManager) { }

        public override void CreateOwnModels()
        {
            base.CreateOwnModels();
            CursorController = new PlayersCursorController(CursorModel);
        }
    }
}
