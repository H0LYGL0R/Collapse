using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.View;
using System;


namespace Collapse.Code.Model.BothAdversariesObjectsModels
{
    public abstract class CounterModel : BothAdversariesGameObjectModel
    {

        private byte _quantity;
        public byte Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnQuantityChanged?.Invoke();
                }
            }


        }

        public event Action OnQuantityChanged;
        public static new ushort Width { get; private set; } = 108;
        public static new ushort Height { get; private set; } = 108;
        public static ushort PlayersY { get; protected set; }
        public static ushort EnemiesY { get; protected set; }

        public CounterModel(AdversaryModel owner, int x, int y) :
            base(owner, x, y, Width, Height)
        {
            Quantity = 0;
        }

    }
}
