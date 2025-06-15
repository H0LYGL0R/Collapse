using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Collapse.Code.Model.GameObjects;

namespace Collapse.Code.View
{
    public abstract class GameObjectView : IRenderable
    {
        public GameObjectModel Model { get; protected set; }
        protected Texture2D _texture;

        public RenderLayer Layer { get; set; }

        public GameObjectView(RenderLayer layer)
        {
            Layer = layer;
        }

        public void ChangeLayer(GameView.GameObjectsManager objectsManager, RenderLayer newLayer)
        {
            objectsManager.Layers[Layer].Remove(this);
            objectsManager.Layers[newLayer].Add(this);
            Layer = newLayer;
        }

        public virtual void Draw() =>
            GameView.SpriteBatch.Draw(_texture, Model.Position, Color.White);
    }
}