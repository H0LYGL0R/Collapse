using Collapse.Code.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Collapse.Code.Model.GameObjects;
using Collapse.Code.View.Assets;
using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjects.Piles;
using Collapse.Code.Model.BothAdversariesObjects;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.Model.BothAdversariesObjectsModels.Counters;

namespace Collapse.Code.Model
{
    public class GameModel
    {
        public static GameStage CurrentState  => StageButtonModel.Stage;

        private readonly ContentManager _content;
        public static Random Random { get; private set; }

        public List<IUpdatable> UpdatableObjectModels { get; set; }

        private readonly GameView.GameObjectsManager _objectsManager;

        public GameModel(ContentManager content, GameView.GameObjectsManager objectsManager) 
        {
            _content = content;
            _content.RootDirectory = "Content";
            Random = new Random();
            UpdatableObjectModels = new List<IUpdatable>();
            _objectsManager = objectsManager;
        }

        public void StartGame(out PlayerModel playerModel, out EnemyModel enemyModel, 
            out StageButtonModel stageButtonModel)
        {

            playerModel = new PlayerModel(
                pilesX: PileModel.PlayersX, fieldY: FieldModel.PlayersY, handY: HandModel.PlayersY,
                moneyCounterY: MoneyCounterModel.PlayersY, powerCounterY: PowerCounterModel.PositionY,
                moneyCounterX: MoneyCounterModel.PositionX, powerCounterX: PowerCounterModel.PlayersX, 
                objectsManager: _objectsManager
                );

            enemyModel = new EnemyModel(
                pilesX: PileModel.EnemiesX, fieldY: FieldModel.EnemiesY, handY: HandModel.EnemiesY,
                moneyCounterY: MoneyCounterModel.EnemiesY, powerCounterY: PowerCounterModel.PositionY,
                moneyCounterX: MoneyCounterModel.PositionX, powerCounterX: PowerCounterModel.EnemiesX, 
                objectsManager: _objectsManager);

            playerModel.CreateOwnModels();
            enemyModel.CreateOwnModels();

            UpdatableObjectModels.Add(playerModel.CursorController);
            UpdatableObjectModels.Add(enemyModel.CursorController);

            stageButtonModel = new StageButtonModel(StageButtonModel.XPosition, StageButtonModel.YPosition,
                playerModel, enemyModel, _objectsManager);

            Audio.StartSoundtrack();
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < UpdatableObjectModels.Count; i++)
            {
                UpdatableObjectModels[i].Update();
            }
        }

        public void Load()
        {
            Image.Load(_content);
            Text.Font.Load(_content);
            Audio.Load(_content);
        }

    }
}
