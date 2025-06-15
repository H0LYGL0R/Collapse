using Collapse.Code.Model.GameObjects;
using Collapse.Code.View.Assets;
using Collapse.Code.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Collapse.Code.View.Cursors;
using Collapse.Code.View.BothAdversariesObjectsViews.Card;


namespace Collapse.Code.View
{
    public class GameView: Game
    {
        public static SpriteBatch SpriteBatch { get; set; }
        public GraphicsDeviceManager Graphics { get; private set; }
        private readonly GameModel _model;

        private readonly Rectangle _backgroundRectangle;

        public static ushort WindowWidth { get; private set; } = 1536;
        public static ushort WindowHeight { get; private set; } = 960;

        public AdversaryView PlayerView { get; set; }
        public AdversaryView EnemyView { get; set; }
        public StageButtonView StageButtonView { get; set; }

        private  GameObjectsManager ObjectsManager { get; set; }

        public GameView()
        {

            ObjectsManager = new GameObjectsManager(this);
            Graphics = new GraphicsDeviceManager(this);
            _model = new GameModel(Content, ObjectsManager);

            Graphics.PreferredBackBufferWidth = WindowWidth;
            Graphics.PreferredBackBufferHeight = WindowHeight;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();
            Window.AllowUserResizing = false;
            IsMouseVisible = true;

            _backgroundRectangle = new Rectangle(0, 0, WindowWidth, WindowHeight);
        }

        private void StartGame()
        {
            _model.StartGame(out var playerModel, out var enemyModel, out var stageButtonModel);

            PlayerView = new AdversaryView(playerModel, ObjectsManager, GraphicsDevice);
            EnemyView = new AdversaryView(enemyModel, ObjectsManager, GraphicsDevice);

            PlayerView.CursorView = new PlayersCursorView(PlayerView.Model.CursorModel);
            EnemyView.CursorView = new EnemiesCursorView(EnemyView.Model.CursorModel);
            ObjectsManager.AddObjectView(EnemyView.CursorView as GameObjectView);

            StageButtonView = new StageButtonView(stageButtonModel);
            ObjectsManager.AddObjectView(StageButtonView);

            PlayerView.CreateOwnViews();
            EnemyView.CreateOwnViews();

            PlayerView.DrawPileView.CreateTitle(GraphicsDevice, ObjectsManager);
            PlayerView.DiscardPileView.CreateTitle(GraphicsDevice, ObjectsManager);

            playerModel.OnDead += RestartGame;
            enemyModel.OnDead += RestartGame;

            GameBalancer.GenerateDecks(playerModel, enemyModel, GraphicsDevice);
            PlayerView.Model.Power = 1;

            PlayerView.Model.DealStartingHand();
            EnemyView.Model.DealStartingHand();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        
            SpriteBatch.Draw(
                Image.BackgroundTable,
                _backgroundRectangle,
                Color.White);
            
            foreach (var layer in Enum.GetValues<RenderLayer>())
            {
                foreach (var renderable in ObjectsManager.Layers[layer])
                {
                    renderable.Draw();
                }
            }
            SpriteBatch.End();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            _model.Load();
            CardPictureGenerator.Load(GraphicsDevice);
            StartGame();
        }

        protected override void Update(GameTime gameTime)
        {
            _model.Update(gameTime);
            base.Update(gameTime);
        }

        private void RestartGame()
        {
            
            Exit();

            using (var game = new GameView())
            {
                game.Run();
            }
        }

        public class GameObjectsManager
        {
            private readonly GameView _gameView;
            public Dictionary<RenderLayer, List<IRenderable>> Layers { get; private set; }
            public GameObjectsManager(GameView gameView)
            {
                _gameView = gameView;
                Layers = new()
            {
                { RenderLayer.Background, new List<IRenderable>() },
                { RenderLayer.Midground, new List<IRenderable>() },
                { RenderLayer.Foreground, new List<IRenderable>() },
                { RenderLayer.TopLayer, new List<IRenderable>() }
            };
            }

            public void AddObjectView(GameObjectView gameObjectView)
            {
                Layers[gameObjectView.Layer].Add(gameObjectView);
                if (gameObjectView.Model is IUpdatable updatableObject)
                    AddUpdatable(updatableObject);
            }

            public void RemoveObjectView(GameObjectView gameObjectView)
            {
                Layers[gameObjectView.Layer].Remove(gameObjectView);
                if (gameObjectView.Model is IUpdatable updatableObject)
                    RemoveUpdatable(updatableObject);
            }

            public void AddUpdatable(IUpdatable updatableObject) =>
                _gameView._model.UpdatableObjectModels.Add(updatableObject);

            public void RemoveUpdatable(IUpdatable updatableObject) =>
                _gameView._model.UpdatableObjectModels.Remove(updatableObject);

            public void ClearAll()
            {
                foreach (var layer in Layers.Values)
                    layer.Clear();
                _gameView._model.UpdatableObjectModels.Clear();
            }

        }
    }
}
