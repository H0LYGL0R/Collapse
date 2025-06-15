using Collapse.Code.Model;
using Collapse.Code.Model.BothAdversariesObjectsModels;
using Collapse.Code.Model.BothAdversariesObjectsModels.Counters;
using Collapse.Code.View.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Collapse.Code.View.BothAdversariesObjectsViews.Counters
{
    public class MoneyCounterView : CounterView
    {
        private struct ValueColorComponents
        {
            public const byte Red = 140;
            public const byte Green = 140;
            public const byte Blue = 140;

            public const byte ColorShift = 10;
        }

        private readonly Vector2 _valuePositionShift = new Vector2(-15, 0);
        private readonly Vector2 _valuePositionShadowShift = new Vector2(-1, 0);
        private readonly Vector2 _intendationForText = new Vector2(15, 20);

        private static readonly Color _valueShadowColor = new Color(40, 40, 40);

        public MoneyCounterView(MoneyCounterModel model, GraphicsDevice graphicsDevice) :
            base(model, graphicsDevice, Image.MoneyCounter,
            GetValueAndTextColor(),
            Text.Font.QuantAntiquaBig)
        { }

        protected override void DrawElements()
        {
            GameView.SpriteBatch.Draw(_image, Vector2.Zero, Color.White);

            var counterModel = Model as CounterModel;
            var moneyQuantity = counterModel.Quantity;
            var stringMoneyQuantity = moneyQuantity.ToString();
            var textForMoneyQuantity = PluralizeRubles(moneyQuantity);
            var valueAndTextColor = GetValueAndTextColor();

            Text.PrintCentered(
                                _font,
                                stringMoneyQuantity,
                                _valuePosition + _valuePositionShift + _valuePositionShadowShift,
                                _valueShadowColor);
            Text.PrintCentered(
                                _font,
                                stringMoneyQuantity,
                                _valuePosition + _valuePositionShift,
                                valueAndTextColor);

            Text.PrintCentered(
                                Text.Font.QuantAntiquaSmall,
                                textForMoneyQuantity,
                                _valuePosition + _valuePositionShift +
                                _valuePositionShadowShift + _intendationForText,
                                _valueShadowColor);
            Text.PrintCentered(
                                Text.Font.QuantAntiquaSmall,
                                textForMoneyQuantity,
                                _valuePosition + _valuePositionShift + _intendationForText,
                                valueAndTextColor);

        }

        private static string PluralizeRubles(int count)
	    {
            var lastTwoDigits = count % 100;
            var lastDigit = count % 10;

                if (count > 10 && lastTwoDigits % 100 >= 10 && lastTwoDigits <= 14
                    || lastDigit > 4 || lastDigit == 0) return "рублей";
                else if (count%10 > 1 && count%10 < 5) return "рубля";
                return "рубль";
	    }

        private static Color GetValueAndTextColor() =>
            new Color(ValueColorComponents.Red +
                GameModel.Random.Next(2 * ValueColorComponents.ColorShift) - ValueColorComponents.ColorShift,
                ValueColorComponents.Green +
                GameModel.Random.Next(2 * ValueColorComponents.ColorShift) - ValueColorComponents.ColorShift,
                ValueColorComponents.Blue +
                GameModel.Random.Next(2 * ValueColorComponents.ColorShift) - ValueColorComponents.ColorShift);  
    }
    
}

