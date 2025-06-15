using System;
using Collapse.Code.View.Assets;
using Collapse.Code.Model.AdversariesModels;

namespace Collapse.Code.Model.BothAdversariesObjectsModels.Field
{
    public class CardSlotModel : BothAdversariesGameObjectModel
    {
        public CardModel KeptCard { get; private set; }

        public static byte HorizontalIndentation { get; private set; } = 15;
        public static byte VerticalIndentation { get; private set; } = 36;

        public static new byte Width => CardModel.Width;
        public static new ushort Height => CardModel.Height;

        private readonly byte _number;

        public event Action<byte> OnCardTaken;
        public event Action<byte> OnCardDeleted;

        public CardSlotModel(AdversaryModel owner, int x, int y, byte number) : base(owner, x, y, CardModel.Width, CardModel.Height)
        { _number = number; }

        public void TakeCard(CardModel cardModel)
        {
            KeptCard = cardModel;
            cardModel.MoveToField(Position);
            cardModel.OnDiscard += DeleteCard;
            Audio.CardSlotTakesCard.Play();
            OnCardTaken?.Invoke(_number);               
        }

        private void DeleteCard()
        {
            KeptCard = null;
            OnCardDeleted?.Invoke(_number);
        }
    }
}
