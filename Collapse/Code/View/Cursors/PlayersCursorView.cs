using Collapse.Code.Model;
using Collapse.Code.View.Assets;
using Collapse.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Collapse.Code.View.Cursors
{
    public class PlayersCursorView : ICursorView
    {
        public CursorModel Model { get; set; }

        public PlayersCursorView(CursorModel model)
        {
            Model = model;
            Model.Position = Vector2.Zero;
            (this as ICursorView).Initialize(model);
            Model.OnCardPlacedUnscuccessfully += () => Audio.UnableToPlaceCardNotification.Play();

        }

        void ICursorView.Squeeze()
        {
            Mouse.SetCursor(MouseCursor.FromTexture2D(Image.CursorSqueezed, 10, 5));
        }

        void ICursorView.Unsqueeze()
        {
            Mouse.SetCursor(MouseCursor.FromTexture2D(Image.Cursor, 10, 0));
        }

    }
}