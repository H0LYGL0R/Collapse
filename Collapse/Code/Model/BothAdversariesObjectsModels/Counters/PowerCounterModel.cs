using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjects.Piles;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.View;

namespace Collapse.Code.Model.BothAdversariesObjectsModels.Counters
{
    public class PowerCounterModel : CounterModel
    {
        public static ushort PositionY { get; private set; } = 
            (ushort)(GameView.WindowHeight / 2 - Height / 2);

        public static ushort PlayersX { get; private set; } =
            (ushort)(PileModel.PlayersX + PileModel.Width / 2 - Width / 2);

        public static ushort EnemiesX { get; private set; } =
                (ushort)(PileModel.PlayersX + PileModel.Width + PileModel.HorizontalIndentation +
            (CardSlotModel.Width + CardSlotModel.HorizontalIndentation) * (FieldModel.Size - 1) +
            +CardSlotModel.Width + PileModel.HorizontalIndentation + PileModel.Width/2 - Width/2);

        public PowerCounterModel(AdversaryModel owner, int x, int y) :
            base(owner, x, y) { }

    }
}
