using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjects.Piles;
using Collapse.Code.View.Assets;
using Collapse.Code.View.BothAdversariesObjectsViews;
using Collapse.Code.View.BothAdversariesObjectsViews.Counters;
using Collapse.Code.View.BothAdversariesObjectsViews.Field;
using Collapse.Code.View.Cursors;
using Microsoft.Xna.Framework.Graphics;

namespace Collapse.Code.View
{
    public class AdversaryView
    {
        public PileView DrawPileView { get; set; }
        public PileView DiscardPileView { get; set; }
        public FieldView FieldView { get; set; }
        public ICursorView CursorView { get; set; }
        public MoneyCounterView MoneyCounterView { get; set; }
        public PowerCounterView PowerCounterView { get; set; }
        public AdversaryModel Model { get; private set; }
        public PileView.AnimationMovingPileView AnimationMovingPileView { get; protected set; }

        private readonly GameView.GameObjectsManager _objectsManager;
        private readonly GraphicsDevice _graphicsDevice;

        public AdversaryView(AdversaryModel model, GameView.GameObjectsManager objectsManager, GraphicsDevice graphicsDevic)
        {
            Model = model;
            model.OnDead += PlayDeadSound;
            _objectsManager = objectsManager;
            _graphicsDevice = graphicsDevic;
        }

        public void CreateOwnViews()
        {
            FieldView = new FieldView(Model.FieldModel, _objectsManager);

            DrawPileView = new PileView(Model.DrawPileModel, this, "Стопка Добора");
            DiscardPileView = new PileView(Model.DiscardPileModel, this, "Стопка Сброса");
            AnimationMovingPileView = new PileView.AnimationMovingPileView(Model.AnimationMovingPileModel);

            MoneyCounterView = new MoneyCounterView(Model.MoneyCounterModel, _graphicsDevice);
            PowerCounterView = new PowerCounterView(Model.PowerCounterModel, _graphicsDevice);


            _objectsManager.AddObjectView(DrawPileView);
            _objectsManager.AddObjectView(DiscardPileView);
            _objectsManager.AddObjectView(MoneyCounterView);
            _objectsManager.AddObjectView(PowerCounterView);

            FieldView.CreateField();
        }

        private void PlayDeadSound() => Audio.AdversaryDead.Play();
    }
}