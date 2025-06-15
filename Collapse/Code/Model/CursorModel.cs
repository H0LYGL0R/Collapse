using System;
using Microsoft.Xna.Framework;
using Collapse.Code.Model.GameObjects;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.View;

namespace Collapse.Code.Model
{
    public class CursorModel : GameObjectModel
    {

        public static ushort StartEnemiesCursorHorizontalIndentation { get; private set; } = 50;
        public static ushort StartEnemiesCursorVertiacalIndentation { get; private set; } = 50;

        public static Vector2 EnemiesCursorBasicPosition { get; set; } = new Vector2(
    GameView.WindowWidth - StartEnemiesCursorHorizontalIndentation - Width,
    StartEnemiesCursorVertiacalIndentation);

        public static new byte Width { get; private set; } = 40;
        public static new byte Height { get; private set; } = 40;

        protected bool _clicked = false;

        public event Action<int, int> OnClick;
        public event Action OnSqueeze;
        public event Action OnUnsqueeze;
        public event Action OnCardPlacedUnscuccessfully;
        public event Func<bool> IsStageCorrect;

        protected CardModel _movableCard;
        public CardModel MovableCard
        {
            get => _movableCard;

            set
            {
                if (value is not null)
                {
                    TakenCardDistance = new Vector2(value.Rectangle.X - Position.X,
                        value.Rectangle.Y - Position.Y);
                    OnSqueeze.Invoke();
                    value.RaiseToHigherLayer();
                }
                else
                {
                    MovableCard.IsTaken = false;
                    OnUnsqueeze.Invoke();
                }
                _movableCard = value;
            }
        }

        private Vector2 TakenCardDistance { get; set; }

        public CursorModel() : base(Width, Height) { }

        protected void RaiseOnClick(int x, int y) => OnClick?.Invoke(x, y);

        public void Update(bool isClicked, int xPosition, int yPosition)
        {
            Position = new Vector2(xPosition, yPosition);
            if (isClicked)
            {
                if (_clicked == false)
                {
                    _clicked = true;
                    RaiseOnClick(xPosition, yPosition);

                }
                MovableCard?.DragTo(TakenCardDistance.X + Position.X,
                            TakenCardDistance.Y + Position.Y);

            }
            else
            {
                _clicked = false;
                if (MovableCard is not null)
                {
                    var wasTaken = false;
                    if (IsStageCorrect.Invoke())
                    {
                        foreach (var slotNumber in MovableCard.OwnerModel.FieldModel.EmptySlotsNumbers)
                            if (MovableCard.DoesCollidePoint(MovableCard.OwnerModel.FieldModel.Slots[slotNumber].Rectangle.Center.X,
                                MovableCard.OwnerModel.FieldModel.Slots[slotNumber].Rectangle.Center.Y))
                            {
                                if (!MovableCard.CanBePlayed)
                                {
                                    OnCardPlacedUnscuccessfully.Invoke();
                                    break;
                                }
                                MovableCard.OwnerModel.FieldModel.Slots[slotNumber].TakeCard(MovableCard);
                                wasTaken = true;
                            }

                    }

                    if (!wasTaken)
                    {
                        MovableCard.ReplaceToTargetPosition();
                        MovableCard.LowerToBelowLayer();
                    }
                    MovableCard = null;
                }
            }

        }

    }
}