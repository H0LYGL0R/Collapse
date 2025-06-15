using Collapse.Code.Model;
using Collapse.Code.View.Assets;

namespace Collapse.Code.View.Cursors
{
    public class EnemiesCursorView : GameObjectView, ICursorView
    {
        public EnemiesCursorView(CursorModel model) : base(RenderLayer.TopLayer)
        {
            Model = model;
            (this as ICursorView).Initialize(model);
            model.Position = CursorModel.EnemiesCursorBasicPosition;
        }

        void ICursorView.Squeeze()
        {
            _texture = Image.EnemiesCursorSqueezed;
        }

        void ICursorView.Unsqueeze()
        {
            _texture = Image.EnemiesCursor;
        }

    }
}