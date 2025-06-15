using Collapse.Code.View.Assets;
using Collapse.Code.View;
using System;
using Collapse.Code.Model.Battle;

namespace Collapse.Code.View
{
    public class BattleView : GameObjectView
    {
        public event Action OnBattleEnds;
        public BattleView(BattleModel model) : base(RenderLayer.Background)
        {
            Model = model;
            model.OnBattleEnds += EndBattle;
            model.OnForcedToPlayMoveSound += () => Audio.BattleMoveNext.Play();
            _texture =  Image.BattlePattern;
        }

        private void EndBattle()
        {
            OnBattleEnds?.Invoke();
            Audio.EndBattle.Play();
        }

    }
}
