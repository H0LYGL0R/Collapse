using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Collapse.Code.View.Assets;
using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.Model;
using Collapse.Model;


namespace Collapse.Code.View.BothAdversariesObjectsViews.Card
{
    public class CardView : GameObjectView
    {
        private readonly RenderTarget2D _renderTarget;
        private readonly GraphicsDevice _graphicsDevice;

        public readonly Texture2D _picture;
        public string Title { get; private set; }

        private bool _isHidden;
        private readonly bool _needToHide;
        private readonly bool _needToReverseImage;

        private readonly GameView.GameObjectsManager _objectsManager;

        private static readonly Vector2 _titlePosition = new Vector2(CardModel.Width / 2, CardModel.Height / 2);
        private static readonly Vector2 _cardValuesPositionShift = new Vector2(0, 29);
        private readonly Vector2 _costPosition;
        private readonly Vector2 _powerPosition;

        private readonly Color _powerColor = new Color(163, 73, 164);
        private readonly Color _costColor = new Color(32, 32, 32);
        private readonly Color _darkColor = new Color(160, 160, 160);

        private readonly Rectangle _notebookSheetFragment;
        private readonly CardShadowView _shadowView;

        public CardView(
                      GraphicsDevice graphicsDevice,
                      GameView.GameObjectsManager objectsManager,
                      CardModel model,
                      Texture2D picture,
                      string title)
            : base(RenderLayer.Midground)
        {
            Model = model;
            _picture = picture;
            Title = title;
            _graphicsDevice = graphicsDevice;
            _renderTarget = new RenderTarget2D(_graphicsDevice, (int)Model.Width, (int)Model.Height);
            _objectsManager = objectsManager;

            model.OnClick += PlayRandomCardTouchSound;
            model.OnDrawFromPile += DrawFromPile;
            model.OnDiscard += Discard;
            model.OnLayerChange += ChangeSelfAndShadowsLayer;
            model.OnPowerChange += ConstructImage;
            model.OnMoveToField += MoveToField;

            _shadowView = new CardShadowView(model.ShadowModel);

            _notebookSheetFragment = new Rectangle(GameModel.Random.Next((int)(Image.PaperSheet.Width - Model.Width)),
                GameModel.Random.Next((int)(Image.PaperSheet.Height - Model.Height)),
                (int)Model.Width, (int)Model.Height);

            var _isEnemies = model.OwnerModel is EnemyModel;

            _costPosition = _cardValuesPositionShift * (_isEnemies ? -1 : 1) + _titlePosition;
            _powerPosition = _cardValuesPositionShift * (_isEnemies ? 1 : -1) + _titlePosition;
            _needToHide = _isEnemies;
            _isHidden = _needToHide;
            _needToReverseImage = _isEnemies;

            if (_isHidden) model.OnShow += Show;

        }

        private static readonly SoundEffect[] _cardTouchSounds = new SoundEffect[]
        {Audio.CardTouch1, Audio.CardTouch2, Audio.CardTouch3};

        private static void PlayRandomCardTouchSound() =>
            _cardTouchSounds[GameModel.Random.Next(0, _cardTouchSounds.Length - 1)].Play();

        public void ConstructImage()
        {
            var cardModel = Model as CardModel;

            var isDark = !cardModel.CanBePlayed && cardModel.Location == Location.Hand && !_needToHide;

            GameView.SpriteBatch.Begin();
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _graphicsDevice.Clear(Color.Transparent);

            if (_isHidden)
            {
                GameView.SpriteBatch.Draw(Image.CardReverse, Vector2.Zero, Color.White);
                GameView.SpriteBatch.End();

                _graphicsDevice.SetRenderTarget(null);
                _texture = _renderTarget;
                return;
            }

            GameView.SpriteBatch.Draw(Image.PaperSheet, Vector2.Zero, _notebookSheetFragment,
                isDark ? _darkColor : Color.White);

            GameView.SpriteBatch.Draw(_picture, Vector2.Zero, Color.White);

            GameView.SpriteBatch.Draw(Image.CardPattern, new Rectangle(0, 0, (int)Model.Width, (int)Model.Height), null, Color.White, 0f, Vector2.Zero, 
                _needToReverseImage ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);


            Text.PrintCentered(
               Text.Font.RoundsBlack,
                                      cardModel.Power.ToString(),
                                      _powerPosition,
                                      isDark ? Color.DarkRed : _powerColor);

            Text.PrintCentered(
                           Text.Font.PTSans,
                           Title,
                           _titlePosition,
                           isDark ? Color.DarkRed : Color.Black);

            Text.PrintCentered(
                 Text.Font.PTSans,
                 cardModel.Cost.ToString(),
                 _costPosition,
                 isDark ? Color.DarkRed : _costColor);

            GameView.SpriteBatch.End();

            _graphicsDevice.SetRenderTarget(null);
            _texture = _renderTarget;
        }

        private void DrawFromPile()
        {
            _isHidden = _needToHide;
            if (_isHidden) (Model as CardModel).OnShow += Show;
            ConstructImage();
            _objectsManager.AddObjectView(this);
            _objectsManager.AddObjectView(_shadowView);
        }

        private void Discard()
        {
            _objectsManager.RemoveObjectView(this);
            _objectsManager.RemoveObjectView(_shadowView);
            Audio.CardDiscarded.Play();
            (Model as CardModel).OwnerModel.DiscardPileModel.AddCard(this);
        }

        private void Show()
        {
            _isHidden = false;
            ConstructImage();
            (Model as CardModel).OnShow -= Show;
        }

        private void MoveToField()
        {
            ConstructImage();
            Audio.CardPowerChanged.Play();
            _objectsManager.RemoveObjectView(_shadowView);
        }

        private void ChangeSelfAndShadowsLayer(sbyte layerChange)
        {
            var newLayer = (RenderLayer)((sbyte)Layer + layerChange);
            ChangeLayer(_objectsManager, newLayer);
            _shadowView.ChangeLayer(_objectsManager, newLayer);
        }

        private class CardShadowView : GameObjectView
        {
            public CardShadowView(CardModel.CardShadowModel model) : base(RenderLayer.Background)
            {
                _texture = Image.CardShadow;
                Model = model;
            }
        }

    }
}
