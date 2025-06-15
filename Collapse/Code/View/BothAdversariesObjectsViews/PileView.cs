using Collapse.Code.Model;
using Collapse.Code.Model.BothAdversariesObjects.Piles;
using Collapse.Code.View.Assets;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Collapse.Code.View.BothAdversariesObjectsViews.Card;


namespace Collapse.Code.View.BothAdversariesObjectsViews
{
    public class PileView : GameObjectView
    {
        public List<CardView> Cards;

        private PileTitleView TitleView { get; set; }

        public AdversaryView OwnerView { get; private set; }

        protected readonly string _name;

        public PileView(PileModel model, AdversaryView ownerView, string name) : base(RenderLayer.Midground)
        {
            _texture = Image.EmptyPile;
            OwnerView = ownerView;
            Model = model;
            Cards = new List<CardView>();
            _name = name;
            model.OnCardsQuantityChange += (cardsQuantity) =>
            {   if (cardsQuantity == 0) _texture = Image.EmptyPile;
                else _texture = Image.CardReverse;};
            model.OnShuffled += () => Audio.Shuffle.Play();

        }

        public void CreateTitle(GraphicsDevice graphicsDevice, GameView.GameObjectsManager objectsManager)
        {
            TitleView = new PileTitleView(graphicsDevice, _name, Model.Position + 
                new Vector2(Model.Width, Model.Height)/2, objectsManager);
            TitleView.ConstructImage((byte)Cards.Count);
            (Model as PileModel).OnCardsQuantityChange += TitleView.ConstructImage;
        }


        public class PileTitleView : GameObjectView
        {
            private readonly RenderTarget2D _renderTarget;
            private readonly GraphicsDevice _graphicsDevice;
            private readonly GameView.GameObjectsManager _objectsManager;
            private readonly string _name;

            private readonly Vector2 _namePosition;
            private readonly Vector2 _cardsQuantityPosition;

            private bool _isVisible;

            public PileTitleView(GraphicsDevice graphicsDevice, string name, Vector2 pilePosition,
                GameView.GameObjectsManager objectsManager) : base(RenderLayer.Midground)
            {
                _graphicsDevice = graphicsDevice;
                Model = new PileModel.PileTitleModel();
                _renderTarget = new RenderTarget2D(graphicsDevice, (int)Model.Width, (int)Model.Height);
                _name = name;
                _objectsManager = objectsManager;

                _namePosition = new Vector2(Model.Width/2, Model.Height/5);
                _cardsQuantityPosition = new Vector2(Model.Width / 2, Model.Height - Model.Height / 5);
                Model.CreateRectangle(pilePosition - new Vector2(Model.Width, Model.Height)/2);
            }

            private static string GetCardWordForm(byte cardsQuantity)
            {
                var lastTwoDigits = cardsQuantity % 100;

                if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
                    return "карт";

                switch (cardsQuantity % 10)
                {
                    case 1:
                        return "карта";
                    case 2:
                    case 3:
                    case 4:
                        return "карты";
                    default:
                        return "карт";
                }
            }
            
            public void ConstructImage(byte newCardsQuantity)
            {
                if (newCardsQuantity == 0)
                {
                    if (_isVisible)
                    {
                        _objectsManager.RemoveObjectView(this);
                        _isVisible = false;
                    }

                }
                else
                {
                    if (!_isVisible)
                    {
                        _objectsManager.AddObjectView(this);
                        _isVisible = true;
                    }
                }

                GameView.SpriteBatch.Begin();
                _graphicsDevice.SetRenderTarget(_renderTarget);
                _graphicsDevice.Clear(Color.Transparent);

                Text.PrintCentered(
                                   Text.Font.UbuntuMono,
                                   _name,
                                   _namePosition,
                                   Color.DarkSlateGray);

                Text.PrintCentered(
                                   Text.Font.UbuntuMono,
                                   String.Format("{0} {1}", newCardsQuantity.ToString(),
                                   GetCardWordForm(newCardsQuantity)),
                                   _cardsQuantityPosition,
                                   Color.DarkSlateGray);

                GameView.SpriteBatch.End();

                _graphicsDevice.SetRenderTarget(null);
                _texture = _renderTarget;
            }
        }

        public class AnimationMovingPileView : GameObjectView
        {
            private readonly GameView.GameObjectsManager _gameObjectsManager;

            public AnimationMovingPileView(PileModel.AnimationMovingPileModel model) : base(RenderLayer.Midground)
            {
                Model = model;

                model.OnMovingEnded += MovingEnded;

                model.OnMovingStarted += () => _gameObjectsManager.AddObjectView(this);
                _gameObjectsManager = model.ObjectsManager;
                _texture = Image.CardReverse;
            }
            
            private void MovingEnded()
            {
                _gameObjectsManager.RemoveObjectView(this);
                var animationMovingPile = Model as PileModel.AnimationMovingPileModel;

                animationMovingPile.OwnerModel.DrawPileModel.NotifyCardsQuantityChange();
            }
        }
    }
}