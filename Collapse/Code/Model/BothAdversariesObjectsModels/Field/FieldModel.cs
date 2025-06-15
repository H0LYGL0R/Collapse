using Collapse.Code.Model.AdversariesModels;
using Collapse.Code.Model.BothAdversariesObjects.Piles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Collapse.Code.Model.BothAdversariesObjectsModels.Field
{
    public class FieldModel
    {
        public const byte Size = 5;

        public static ushort PlayersY { get; private set; } = 
            (ushort)(CardModel.HiddenModHeight + CardSlotModel.VerticalIndentation + CardModel.Height + CardSlotModel.VerticalIndentation);
        public static ushort EnemiesY { get; private set; } =
            (ushort)(CardModel.HiddenModHeight + CardSlotModel.VerticalIndentation);

        public AdversaryModel Owner { get; private set; }

        public List<CardSlotModel> Slots { get; } = new List<CardSlotModel>();
        public HashSet<byte> EmptySlotsNumbers { get; private set; }

        public List<CardPaymentPatternModel> PaymentPatterns { get; } = new List<CardPaymentPatternModel>();

        public FieldModel(AdversaryModel owner, ushort y)
        {
            Owner = owner;
            ushort currentSlotX;
            CardSlotModel cardSlot;
            EmptySlotsNumbers = new HashSet<byte>();
            for (byte number = 0; number < Size; number++)
            {
                currentSlotX = (ushort)(PileModel.PlayersX + PileModel.Width + PileModel.HorizontalIndentation + 
                    (CardSlotModel.Width + CardSlotModel.HorizontalIndentation) * number);
                cardSlot = new CardSlotModel(Owner, currentSlotX, y, number);
                cardSlot.OnCardTaken += slotNumber => EmptySlotsNumbers.Remove(slotNumber);
                cardSlot.OnCardDeleted += slotNumber => EmptySlotsNumbers.Add(slotNumber);
                Slots.Add(cardSlot);

                EmptySlotsNumbers.Add(number);
            }
        }
    }

    public class CardPaymentPatternModel
    {
        public CardModel Card { get; }
        public int FoodCounter { get; set; }
        public bool IsMinusFood { get; set; }
        public List<Vector2> Squares { get; } = new List<Vector2>();

        public CardPaymentPatternModel(CardModel card)
        {
            Card = card;
        }
    }
}
