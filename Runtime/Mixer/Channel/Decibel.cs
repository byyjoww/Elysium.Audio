using UnityEngine;

namespace Elysium.Audio
{
    public class Decibel
    {
        private float value = default;

        public float Value => value;        

        public Decibel()
        {
            this.value = 0f;
        }

        // Creates a decibel
        public Decibel(float _value)
        {
            this.value = _value;
        }

        // Creates a decibel from a value between 0 and 1
        // Decibels are on a log scale. If we consider a scale from 0db ~ -80db, -50% volume should be -6db (not -40db).
        public static Decibel FromNormalized(float _normalized)
        {
            float decibels = Mathf.Log10(_normalized) * 20f;
            var value = (_normalized == 0) ? -80f : decibels;
            return new Decibel(value);
        }

        public float Normalized
        {
            get
            {
                if (value == -80f) { return 0; }
                float normalized = Mathf.Pow(10, value / 20);
                return normalized;
            }
        }
    }
}
