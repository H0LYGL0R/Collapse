using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjectsModels.Field;
using Collapse.Code.Model.GameObjects;
using Microsoft.Xna.Framework;
using Collapse.Code.View;
using System;

namespace Collapse.Code.Model.Battle
{
    public class BattleModel : GameObjectModel
    {
        private readonly PlayerModel _player;
        private readonly EnemyModel _enemy;

        private readonly FieldModel _playersField;
        private readonly FieldModel _enemiesField;

        private sbyte _battlersCoupleNumber;
        private readonly GameView.GameObjectsManager _objectsManager;
        private const byte AnimationFramesQuantity = 60;

        public event Action OnBattleEnds;
        public event Action OnForcedToPlayMoveSound;

        public BattleModel(PlayerModel player, EnemyModel enemy,
                         GameView.GameObjectsManager objectsManager) :
            base(CardSlotModel.Width + CardSlotModel.HorizontalIndentation * 2u,
                GameView.WindowHeight)
        {
            _player = player;
            _enemy = enemy;

            _playersField = player.FieldModel;
            _enemiesField = enemy.FieldModel;

            _battlersCoupleNumber = 0;
            _objectsManager = objectsManager;

            CreateRectangle(GetPosition());
        }


        public void MoveNext()
        {
            if (_battlersCoupleNumber >= _playersField.Slots.Count)
            {
                _battlersCoupleNumber = 0;
                OnBattleEnds.Invoke();
                return;

            }
            else if (_battlersCoupleNumber != 0) OnForcedToPlayMoveSound.Invoke();
            var playersCard = _playersField.Slots[_battlersCoupleNumber].KeptCard as IBattler;
            var enemiesCard = _enemiesField.Slots[_battlersCoupleNumber].KeptCard as IBattler;

            Timer timer = null;

            if (playersCard is not null)
            {
                if (enemiesCard is not null)
                    timer = new Timer(AnimationFramesQuantity, _objectsManager, () => Clash(playersCard, enemiesCard));
                else
                    timer = new Timer(AnimationFramesQuantity, _objectsManager, () => Clash(playersCard, _enemy));
            }
            else if (enemiesCard is not null)
                timer = new Timer(AnimationFramesQuantity, _objectsManager, () => Clash(_player, enemiesCard));

            timer ??= new Timer(AnimationFramesQuantity, _objectsManager, MoveNext);
            Position = GetPosition();
            _battlersCoupleNumber++;
        }


        private void Clash(IBattler playersBattler, IBattler enemiesBattler)
        {
            var playersPreviousPower = playersBattler.Power;
            if (enemiesBattler is CardModel)
                playersBattler.Power -= enemiesBattler.Power;
            if (playersBattler is CardModel)
                enemiesBattler.Power -= playersPreviousPower;
            MoveNext();
        }

        private Vector2 GetPosition() => 
            new Vector2(_enemiesField.Slots[_battlersCoupleNumber].Position.X - CardSlotModel.HorizontalIndentation, 0);

    }
}