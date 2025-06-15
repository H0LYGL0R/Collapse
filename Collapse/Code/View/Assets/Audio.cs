using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace Collapse.Code.View.Assets
{
    public static class Audio
    {
        public static SoundEffect CardTouch1 { get; private set; }
        public static SoundEffect CardTouch2 { get; private set; }
        public static SoundEffect CardTouch3 { get; private set; }
        public static SoundEffect CardSlotTakesCard { get; private set; }
        public static SoundEffect StageButtonClick { get; private set; }
        public static SoundEffect CardPowerChanged { get; private set; }
        public static SoundEffect CardDiscarded { get; private set; }
        public static SoundEffect AdversaryDead { get; private set; }
        public static SoundEffect UnableToPlaceCardNotification { get; private set; }
        public static SoundEffect EndBattle { get; private set; }
        public static SoundEffect BattleMoveNext { get; private set; }
        public static SoundEffect Shuffle { get; private set; }

        private static Song _soundtrack;

        public static void Load(ContentManager content)
        {
            CardTouch1 = LoadSound(content, "CardTouch1");
            CardTouch2 = LoadSound(content, "CardTouch2");
            CardTouch3 = LoadSound(content, "CardTouch3");
            CardSlotTakesCard = LoadSound(content, "CardSlotTakesCard");
            StageButtonClick = LoadSound(content, "StageButtonClick");
            CardPowerChanged = LoadSound(content, "CardPowerChanged");
            CardDiscarded = LoadSound(content, "CardDiscarded");
            AdversaryDead = LoadSound(content, "AdversaryDead");
            UnableToPlaceCardNotification = LoadSound(content, "UnableToPlaceCardNotification");
            EndBattle = LoadSound(content, "EndBattle");
            BattleMoveNext = LoadSound(content, "BattleMoveNext");
            Shuffle = LoadSound(content, "Shuffle");

            _soundtrack = content.Load<Song>("Sounds/Soundtrack");
        }

        public static void StartSoundtrack() => MediaPlayer.Play(_soundtrack);
        public static void StopSoundtrack() => MediaPlayer.Stop();

        private static SoundEffect LoadSound(ContentManager content, string textureName) =>
    content.Load<SoundEffect>(String.Format("Sounds/{0}", textureName));
    }
}
