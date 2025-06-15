using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.GameObjects;
using Microsoft.Xna.Framework;

namespace Collapse.Code.Model.BothAdversariesObjectsModels
{
    public abstract class BothAdversariesGameObjectModel : GameObjectModel
    {
        public AdversaryModel OwnerModel { get; private set; }
        public BothAdversariesGameObjectModel(AdversaryModel owner, int x, int y, uint width, uint height):
            base(width, height)
        {
            OwnerModel = owner;
            CreateRectangle(new Vector2(x, y));
        }

    }
}
