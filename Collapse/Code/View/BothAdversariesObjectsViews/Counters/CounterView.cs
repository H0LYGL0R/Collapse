using Collapse.Code.View.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Collapse.Code.Model.BothAdversariesObjectsModels;


namespace Collapse.Code.View.BothAdversariesObjectsViews.Counters
{
    public abstract class CounterView : GameObjectView
    {
        private readonly RenderTarget2D _renderTarget;
        private readonly GraphicsDevice _graphicsDevice;
        protected readonly Vector2 _valuePosition;
        protected readonly Color _valueTextColor;
        protected readonly SpriteFont _font;
        protected readonly Texture2D _image;

        public CounterView(CounterModel model, GraphicsDevice graphicsDevice,
            Texture2D image, Color valueTextColor, SpriteFont font) : base(RenderLayer.Background)
        {
            Model = model;
            _renderTarget = new RenderTarget2D(graphicsDevice, (int)Model.Width, (int)Model.Height);
            _graphicsDevice = graphicsDevice;
            model.OnQuantityChanged += ConstructImage;
            _valuePosition = new Vector2(Model.Width / 2, Model.Height / 2);
            _image = image;
            _texture = _image;
            _valueTextColor = valueTextColor;
            _font = font;

            ConstructImage();
        }

        private void ConstructImage()
        {
            GameView.SpriteBatch.Begin();
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _graphicsDevice.Clear(Color.Transparent);

            DrawElements();

            GameView.SpriteBatch.End();
            _graphicsDevice.SetRenderTarget(null);
            _texture = _renderTarget;
        }

        protected virtual void DrawElements()
        {
            GameView.SpriteBatch.Draw(_image, Vector2.Zero, Color.White);

            Text.PrintCentered(
                                _font,
                                (Model as CounterModel).Quantity.ToString(),
                                _valuePosition,
                                _valueTextColor);
        }
    }
}
