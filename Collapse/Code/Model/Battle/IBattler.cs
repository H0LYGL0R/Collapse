namespace Collapse.Code.Model.Battle
{
    public interface IBattler
    {
        public short Power { get; set; }

        public bool NeedToStopPowerGet(ref byte previousPower, short newPower)
        {
            if (newPower == previousPower) return true;
            CheckPowerBounds(ref previousPower, newPower);
            return false;
        }

        private static void CheckPowerBounds(ref byte previousPower, short newPower)
        {  
            if (newPower < byte.MinValue) newPower = byte.MinValue;
            else if (newPower > byte.MaxValue) newPower = byte.MaxValue;

            previousPower = (byte)newPower;
        }
    }
}
