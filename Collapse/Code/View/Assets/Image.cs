using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Collapse.Code.View.Assets
{
    public static class Image
    {
        public static Texture2D CardPattern { get; private set; }
        public static Texture2D Prototype { get; private set; }
        public static Texture2D Cursor { get; private set; }
        public static Texture2D CursorSqueezed { get; private set; }
        public static Texture2D CardReverse { get; private set; }
        public static Texture2D EnemiesCursor { get; private set; }
        public static Texture2D EnemiesCursorSqueezed { get; private set; }
        public static Texture2D CardSlot { get; private set; }
        public static Texture2D UnpressedButton { get; private set; }
        public static Texture2D PressedButton { get; private set; }
        public static Texture2D MoneyCounter { get; private set; }
        public static Texture2D PowerCounter { get; private set; }
        public static Texture2D BattlePattern { get; private set; }
        public static Texture2D PaperSheet { get; private set; }
        public static Texture2D BackgroundTable { get; private set; }
        public static Texture2D CardShadow { get; private set; }
        public static Texture2D EmptyPile { get; private set; }

        public static void Load(ContentManager content)
        {
            CardPattern = LoadTexture(content, "CardPattern");
            Prototype = LoadTexture(content, "Prototype");
            Cursor = LoadTexture(content, "Cursor");
            CursorSqueezed = LoadTexture(content, "CursorSqueezed");
            EnemiesCursor = LoadTexture(content, "EnemiesCursor");
            EnemiesCursorSqueezed = LoadTexture(content, "EnemiesCursorSqueezed");
            CardSlot = LoadTexture(content, "CardSlot");
            CardReverse = LoadTexture(content, "CardReverse");
            PressedButton = LoadTexture(content, "PressedButton");
            UnpressedButton = LoadTexture(content, "UnpressedButton");
            MoneyCounter = LoadTexture(content, "MoneyCounter");
            PowerCounter = LoadTexture(content, "PowerCounter");
            BattlePattern = LoadTexture(content, "BattlePattern");
            PaperSheet = LoadTexture(content, "PaperSheet");
            BackgroundTable = LoadTexture(content, "BackgroundTable");
            CardShadow = LoadTexture(content, "CardShadow");
            EmptyPile = LoadTexture(content, "EmptyPile");
        }

        private static Texture2D LoadTexture(ContentManager content, string textureName) =>
            content.Load<Texture2D>(String.Format("Images/{0}", textureName));
    }
}
