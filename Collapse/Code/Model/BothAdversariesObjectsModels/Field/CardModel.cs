using Microsoft.Xna.Framework;
using System;
using Collapse.Code.Model.GameObjects;
using Collapse.Model;
using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.View;
using Collapse.Code.Model.Battle;

namespace Collapse.Code.Model.BothAdversariesObjectsModels.Field
{
    public class CardModel : ClickableGameObjectModel, IBattler, IUpdatable
    {
        private Vector2? _targetPosition;
        public Vector2 TargetPosition
        {
            get
            {
                return _targetPosition ?? throw new Exception("Целевое положение не задано");
            }
            set
            {
                _targetPosition = value;
            }
        }

        private const byte MoveSpeed = 20;

        public Location Location { get; private set; }
        public byte Cost { get; private set; }

        private readonly byte _startPower;
        private byte _power;
        public short Power
        {
            get
            {
                return _power;
            }

            set
            {
                if ((this as IBattler).NeedToStopPowerGet(ref _power, value)) return;

                if (_power == 0)
                    DiscardFromField();
                OnPowerChange.Invoke();
            }
        }

        public AdversaryModel OwnerModel { get; private set; }

        private readonly CursorModel _cursorModel;

        public  CardShadowModel ShadowModel { get; private set; }
        
        public bool IsTaken { get; set; }

        public bool CanBePlayed => OwnerModel.Money >= Cost;

        public static new byte Width { get; private set; } = 175;
        public static new ushort Height { get; private set; } = 275;
        public static byte HiddenModHeight { get; private set; } = 151;

        public event Action OnDrawFromPile;
        public event Action OnDiscard;
        public event Action OnUpdate;
        public event Action<sbyte> OnLayerChange;
        public event Action OnShow;
        public event Action OnMoveToField;
        public event Action OnPowerChange;
        private event Action OnTargetReached;

        public CardModel(byte cost, byte power, AdversaryModel owner) : base(Width, Height)
        {
            Cost = cost;
            _startPower = power;
            _power = _startPower;
            OwnerModel = owner;
            _cursorModel = owner.CursorModel;
            Location = Location.DrawPile;
            OnClick += () => TakeByCursor(_cursorModel);
            ShadowModel = new CardShadowModel();
            OnChangePosition += (newPosition) => ShadowModel.Position = newPosition;
        }

        public void Update() => OnUpdate?.Invoke();

        public void Draw()
        {
            if (Location != Location.DrawPile)
                throw new Exception("Карту, не находящуюся в стопке добора, нельзя вытащить");
            CreateRectangle();
            Location = Location.Hand;
            OnDrawFromPile?.Invoke();
            _cursorModel.OnClick += ClickIfCollides;
        }

        private void DiscardFromField()
        { 
            if (Location != Location.Field)
                throw new Exception("Карту, не находящуюся на поле, нельзя сбросить");
            Location = Location.DiscardPile;
            TargetPosition = OwnerModel.DiscardPileModel.Position;
            RaiseToHigherLayer();
            OnTargetReached = DoOnDiscard;
            ReplaceToTargetPosition();
            ShadowModel.CreateRectangle(Position);
        }

        public void MoveToField(Vector2 position)
        {
            if (Location != Location.Hand)
                throw new Exception("Карту, не находящуюся в руке, нельзя переместить на поле");
            OwnerModel.Money -= Cost;
            Location = Location.Field;
            Position = position;

            _targetPosition = null;
            OnUpdate -= MoveToTargetPosition;

            _cursorModel.OnClick -= ClickIfCollides;
            LowerToBelowLayer();
            ShadowModel.DestroyRectangle();
            OnMoveToField?.Invoke();
        }

        public void ShuffleIntoDrawPileFromDiscardPile()
        {
            if (Location != Location.DiscardPile)
                throw new Exception("Карту, не находящуюся в стопке сброса, " +
                    "нельзя замешать в стопку добора");
            Location = Location.DrawPile;
        }

        public void DragTo(float newX, float newY)
        {
            Position = new Vector2(
                newX >= 0 ?
                newX + Rectangle.Width < GameView.WindowWidth ?
                newX : GameView.WindowWidth - Rectangle.Width : 0,
            newY >= 0 ?
            newY + Rectangle.Height < GameView.WindowHeight ?
            newY : GameView.WindowHeight - Rectangle.Height : 0);
        }

        private void MoveToTargetPosition()
        {
            var direction = TargetPosition - Position;
            var distance = direction.Length();

            if (distance <= MoveSpeed)
            {
                Position = TargetPosition;
                OnUpdate -= MoveToTargetPosition;
                OnTargetReached?.Invoke();
                OnTargetReached = null;
                return;
            }

            direction.Normalize();
            Position += direction * MoveSpeed;
        }

        public void ReplaceToTargetPosition(Vector2? targetPosition = null)
        {
            if (targetPosition is not null)
                TargetPosition = targetPosition.Value;
            OnUpdate += MoveToTargetPosition;
        }
        public void LowerToBelowLayer() => OnLayerChange?.Invoke(-1);
        public void RaiseToHigherLayer() => OnLayerChange?.Invoke(+1);

        public void DoOnDiscard()
        {
            LowerToBelowLayer();
            DestroyRectangle();
            ShadowModel.DestroyRectangle();
            OnDiscard.Invoke();
            _power = _startPower;
        }
        public void TakeByCursor(CursorModel cursorModel)
        {
            cursorModel.MovableCard = this;
            IsTaken = true;
            OnShow?.Invoke();
        }

        public override void CreateRectangle(Vector2? position = null)
        {
            base.CreateRectangle(position);
            ShadowModel.CreateRectangle(Position);  
        }

        public class CardShadowModel : GameObjectModel
        {
            private const byte ShadowSize = 8;

            public CardShadowModel() : base((uint)(CardModel.Width + ShadowSize),
            (uint)(CardModel.Height + ShadowSize)){}
            
        }
    }
}