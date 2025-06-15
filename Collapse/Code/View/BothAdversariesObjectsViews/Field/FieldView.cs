using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.View;

namespace Collapse.Code.View.BothAdversariesObjectsViews.Field
{
    public class FieldView
    {
        public readonly FieldModel _model;
        private readonly GameView.GameObjectsManager _objectsManager;
        public FieldView(FieldModel fieldModel, GameView.GameObjectsManager objectsManager)
        {
            _model = fieldModel;
            _objectsManager = objectsManager;
        }
        public void CreateField()
        {
            foreach (var slot in _model.Slots)
                _objectsManager.AddObjectView(new CardSlotView(slot));
        }

    }
}
