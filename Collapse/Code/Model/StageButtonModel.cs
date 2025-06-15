using Collapse.Code.Model.GameObjects;
using System;
using Microsoft.Xna.Framework;
using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.View;
using Collapse.Code.Model.Battle;

namespace Collapse.Code.Model
{
    public class StageButtonModel : ClickableGameObjectModel
    {

        public static new byte Width { get; private set; } = 150;
        public static new byte Height { get; private set; } = 150;
        public static byte HorizontalIndentation { get; private set; } = 1;

        public static ushort XPosition { get; private set; } = (ushort)(GameView.WindowWidth - Width - HorizontalIndentation);
        public static ushort YPosition { get; private set; } = (ushort)(GameView.WindowHeight / 2 - Height / 2);

        private const byte PressedStateFramesQuantity = 30;

        public PlayerModel PlayerModel { get; private set; }
        public EnemyModel EnemyModel { get; private set; }

        private readonly CursorModel _playersCursorModel;
        private readonly CursorModel _enemiesCursorModel;

        private BattleView _battleView;

        private readonly GameView.GameObjectsManager _objectsManager;

        public event Action<bool> OnPressedChange;

        private bool _pressed;
        public bool Pressed { 
            get
            {
                return _pressed;
            }

            private set
            {
                _pressed = value;
                OnPressedChange?.Invoke(value);
            }
        }

        private static GameStage _gameStage;
        public static GameStage Stage
        {
            get
            {
                return _gameStage;
            }

            private set
            {
                _gameStage = value <= GameStage.Battle ?
                    value : GameStage.PlayerTurn;
            }
        }


        public StageButtonModel(int x, int y, PlayerModel playerModel, EnemyModel enemyModel, 
            GameView.GameObjectsManager objectsManager) : base(Width, Height)
        {
            PlayerModel = playerModel;
            EnemyModel = enemyModel;
            _playersCursorModel = playerModel.CursorModel;
            _enemiesCursorModel = enemyModel.CursorModel;
            _objectsManager = objectsManager;
            _battleView = new BattleView(new BattleModel(playerModel, enemyModel, _objectsManager));
            _battleView.OnBattleEnds += EndBattle;

            OnClick += NextStage;
            _playersCursorModel.OnClick += ClickIfCollides;

            _pressed = false;
            CreateRectangle(new Vector2(x, y));

            Stage = GameStage.PlayerTurn;
        }

        public void NextStage()
        {   
            Stage++;

            switch (Stage)
            {
                case GameStage.PlayerTurn:
                    _playersCursorModel.OnClick += ClickIfCollides;
                    PlayerModel.StartTurn();
                    CreateTimer();
                    break;

                case GameStage.EnemyTurn:
                    _playersCursorModel.OnClick -= ClickIfCollides;
                    _enemiesCursorModel.OnClick += ClickIfCollides;
                    EnemyModel.StartTurn();
                    CreateTimer();
                    Pressed = true;
                    break;

                case GameStage.Battle:
                    _objectsManager.AddObjectView(_battleView);
                    (_battleView.Model as BattleModel).MoveNext();
                    _enemiesCursorModel.OnClick -= ClickIfCollides;
                    Pressed = true;
                    break;

            }
        } 
        
        private void EndBattle()
        {
            Pressed = false;
            _objectsManager.RemoveObjectView(_battleView);
            NextStage();
        }

        private void CreateTimer() => _ = new Timer(PressedStateFramesQuantity, _objectsManager, () => Pressed = false);

    }
}
