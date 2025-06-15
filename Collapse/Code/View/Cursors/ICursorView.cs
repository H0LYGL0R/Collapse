using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Collapse.Code.Model;
using Collapse.Code.Model.GameObjects;

namespace Collapse.Code.View.Cursors
{
    public interface ICursorView
    {
        void Squeeze();

        void Unsqueeze();

        void Initialize(CursorModel model)
        { 
            model.Position = CursorModel.EnemiesCursorBasicPosition;
            model.CreateRectangle();
            Unsqueeze();
            model.OnSqueeze += Squeeze;
            model.OnUnsqueeze += Unsqueeze;
        }

}
}