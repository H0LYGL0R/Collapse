using Collapse.Code.Controller.Cursors;
using Collapse.Code.Model.Battle;
using Collapse.Code.Model.BothAdversariesObjects;
using Collapse.Code.Model.BothAdversariesObjects.Piles;
using Collapse.Code.Model.BothAdversariesObjectsModels.Counters;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.View;
using System;

namespace Collapse.Code.Model.AdversariesModels
{
    public abstract class AdversaryModel : IBattler
    {

        protected const byte StartHandSize = 3;
        public const byte MoneyPerTurn = 3;

        public short Power
        {
            get
            {
                return PowerCounterModel.Quantity;
            }

            set
            {
                var powerQuantity = PowerCounterModel.Quantity;
                if ((this as IBattler).NeedToStopPowerGet(ref powerQuantity, value)) return;
                PowerCounterModel.Quantity = powerQuantity;
                if (PowerCounterModel.Quantity == 0)
                {
                    OnDead.Invoke();
                }
            }
        }

        public byte Money
        {
            get
            {
                return MoneyCounterModel.Quantity;
            }

            set
            {
                if (value == Money) return;
                MoneyCounterModel.Quantity = value;
                foreach (var card in HandModel.Cards)
                    card.ConstructImage();
            }
        }

        public FieldModel FieldModel { get; protected set; }
        public HandModel HandModel { get; protected set; }
        public DrawPileModel DrawPileModel { get; protected set; }
        public DiscardPileModel DiscardPileModel { get; protected set; }
        public AdversariesCursorController CursorController { get; protected set; }
        public MoneyCounterModel MoneyCounterModel { get; protected set; }
        public PowerCounterModel PowerCounterModel { get; protected set; }
        public PileModel.AnimationMovingPileModel AnimationMovingPileModel { get; protected set; }
        public CursorModel CursorModel { get; protected set; }

        protected readonly GameView.GameObjectsManager _objectsManager;

        public ushort PilesX { get; protected set; }
        public ushort FieldY { get; protected set; }
        public short HandY { get; protected set; }
        public ushort MoneyCounterY { get; protected set; }
        public ushort PowerCounterY { get; protected set; }
        public ushort MoneyCounterX { get; protected set; }
        public ushort PowerCounterX { get; protected set; }

        public event Action OnDead;

        protected AdversaryModel(ushort pilesX, ushort fieldY, short handY, ushort moneyCounterY,
                               ushort powerCounterY, ushort moneyCounterX, ushort powerCounterX,
                               GameView.GameObjectsManager objectsManager)
        {
            PilesX = pilesX;
            FieldY = fieldY;
            HandY = handY;
            MoneyCounterY = moneyCounterY;
            PowerCounterY = powerCounterY;
            MoneyCounterX = moneyCounterX;
            PowerCounterX = powerCounterX;
            _objectsManager = objectsManager;
        }

        public virtual void CreateOwnModels()
        {
            FieldModel = new FieldModel(this, FieldY);
            HandModel = new HandModel(this, HandY);

            DrawPileModel = new DrawPileModel(this, PilesX, PileModel.DrawPilesY, _objectsManager);
            DiscardPileModel = new DiscardPileModel(this, PilesX, PileModel.DiscardPilesY, _objectsManager);

            AnimationMovingPileModel = 
                new PileModel.AnimationMovingPileModel(this, (ushort)DiscardPileModel.Position.X, 
                (ushort)DiscardPileModel.Position.Y,
                (ushort)DrawPileModel.Position.Y, _objectsManager);

            CursorModel = new CursorModel();

            MoneyCounterModel = new MoneyCounterModel(this, MoneyCounterX, MoneyCounterY);
            PowerCounterModel = new PowerCounterModel(this, PowerCounterX, PowerCounterY);

            CursorModel.OnUnsqueeze += HandModel.UpdateHandPositions;
            DrawPileModel.OnShuffled += AnimationMovingPileModel.StartMoving;
        }

        public void DealStartingHand() =>
            DrawPileModel.DrawTopCards(StartHandSize);

        public virtual void StartTurn()
        {
            Money += MoneyPerTurn;
            DrawPileModel.DrawTopCards();
        }

    }
}