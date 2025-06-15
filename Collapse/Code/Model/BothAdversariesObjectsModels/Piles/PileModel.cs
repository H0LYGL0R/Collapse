using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjectsModels;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.View;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Collapse.Code.Model.GameObjects;
using System.Numerics;
using Collapse.Code.View.BothAdversariesObjectsViews.Card;

namespace Collapse.Code.Model.BothAdversariesObjects.Piles
{
    public abstract class PileModel : BothAdversariesGameObjectModel
    {
        private readonly GameView.GameObjectsManager _objectsManager;
        public List<CardView> Cards;

        public event Action OnShuffled;
        public event Action<byte> OnCardsQuantityChange;

        public void NotifyShuffle() => OnShuffled.Invoke();
        public void NotifyCardsQuantityChange() => 
            OnCardsQuantityChange?.Invoke((byte)Cards.Count);

        public static ushort PlayersX { get; private set; } = 1;
        public static ushort HorizontalIndentation { get; set; } = 30;
        public static ushort EnemiesX { get; private set; } =
            (ushort)(PlayersX + Width + HorizontalIndentation +
            (CardSlotModel.Width + CardSlotModel.HorizontalIndentation) * (FieldModel.Size-1) +
            +CardSlotModel.Width + HorizontalIndentation);

        public static ushort DiscardPilesY { get; private set; } = 
            (ushort)((GameView.WindowHeight - CardModel.Height * 2) / 3);
        public static ushort DrawPilesY { get; private set; } = 
            (ushort)(GameView.WindowHeight - DiscardPilesY - CardModel.Height);

        public static new byte Width  => CardModel.Width;
        public static new ushort Height => CardModel.Height;

        public PileModel(AdversaryModel owner, int x, int y, GameView.GameObjectsManager objectsManager) : 
            base(owner, x, y, CardModel.Width, CardModel.Height)
        {
            Cards = new List<CardView>();
            _objectsManager = objectsManager;
        }

        public void CreateNewCard(GraphicsDevice graphicsDevice,
            byte cost, byte power, Texture2D picture, string title) =>
            Cards.Add(new CardView(graphicsDevice, _objectsManager, 
                new CardModel(cost, power, OwnerModel), picture, title));


        public class PileTitleModel : GameObjectModel
        {
            public PileTitleModel() : base(CardModel.Width, (uint)CardModel.Height/5)
            {
             
            }
        }

        public class AnimationMovingPileModel : BothAdversariesGameObjectModel, IUpdatable
        {
            private readonly Vector2 unitAnimationVector = new Vector2(0, 1);
            private sbyte _acceleration;
            private readonly Vector2 _basicPosition;
            private readonly ushort _targetY;

            public GameView.GameObjectsManager ObjectsManager { get; private set; }

            public Action OnMovingEnded;
            public Action OnMovingStarted;

            public AnimationMovingPileModel(AdversaryModel ownerModel, ushort positionX, 
                ushort basicY, ushort targetY, GameView.GameObjectsManager objectsManager) : 
                base(ownerModel, positionX, basicY, PileModel.Width, PileModel.Height)

            {
                _acceleration = 0;
                ObjectsManager = objectsManager;
                _basicPosition = new Vector2(positionX, basicY);
                _targetY = targetY;
                OnMovingEnded += OwnerModel.DiscardPileModel.NotifyCardsQuantityChange;
            }

            public void StartMoving()
            {
                OnUpdate += Move;
                OnMovingStarted.Invoke();
            }

            private void Move()
            {
                if (Position.Y >= _targetY)
                {
                    Position = _basicPosition;
                    OnUpdate -= Move;
                    OnMovingEnded.Invoke();
                    return;
                }

                Position += unitAnimationVector * _acceleration;
                _acceleration++;
            }

            public event Action OnUpdate;

            public void Update() => OnUpdate.Invoke();
        }

    }
}