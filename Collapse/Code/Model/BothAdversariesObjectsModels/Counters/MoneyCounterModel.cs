using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.View;


namespace Collapse.Code.Model.BothAdversariesObjectsModels.Counters
{
    public class MoneyCounterModel : CounterModel
    {
        public static ushort PositionX { get; private set; } = (ushort)(GameView.WindowWidth - Width * 1.5);
        public static new ushort PlayersY { get; private set; } = (ushort)(GameView.WindowHeight - 3.5 * Height);
        public static new ushort EnemiesY { get; private set; } =  (ushort)(2.5 * Height);

        public MoneyCounterModel(AdversaryModel owner, int x, int y) :
            base(owner, x, y) { }

    }
}
