using Elysium.Core.Utils;

namespace Elysium.Audio
{
    public class Pitch
    {
        private float value = default;

        public float Value => value;
        public Percentage PercentValue => Percentage.Decimal((decimal)value);

        public Pitch(float _value)
        {
            this.value = _value;
        }

        public static Pitch FromPercentage(Percentage _percentage)
        {
            return new Pitch((float)_percentage.Value);
        }
    }
}