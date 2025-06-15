using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Collapse.Code.View.Assets
{
    public static class Text
    {
        public static class Font
        {
            public static SpriteFont RoundsBlack { get; private set; }
            public static SpriteFont PTSans { get; private set; }
            public static SpriteFont UbuntuMono { get; private set; }
            public static SpriteFont QuantAntiquaBig { get; private set; }
            public static SpriteFont QuantAntiquaSmall { get; private set; }

            public static void Load(ContentManager content)
            {
                RoundsBlack = content.Load<SpriteFont>("Fonts/RoundsBlack_18pt_Bold");
                PTSans = content.Load<SpriteFont>("Fonts/PT Sans_18pt");
                UbuntuMono = content.Load<SpriteFont>("Fonts/Ubuntu Mono_18pt");
                QuantAntiquaBig = content.Load<SpriteFont>("Fonts/Quant Antiqua_25pt");
                QuantAntiquaSmall = content.Load<SpriteFont>("Fonts/Quant Antiqua_13pt");
            }
        }


        public static void Print(SpriteFont font, string text, Vector2 position, Color color)
        {
            GameView.SpriteBatch.DrawString(
                font,
                text,
                position,
                color,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f);
        }

        public static void PrintCentered(SpriteFont font, string text, Vector2 position, Color color)
        {
            var textSize = font.MeasureString(text);
            var textOrigin = textSize / 2f;

            GameView.SpriteBatch.DrawString(
                font,
                text,
                position,
                color,
                0f,
                textOrigin, 
                1f,
                SpriteEffects.None,
                0f);
        }
    }
}