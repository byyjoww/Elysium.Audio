using Elysium.Core;
using Elysium.Core.Attributes;
using UnityEngine.Events;

namespace Elysium.Audio
{
    [System.Serializable]
    public class Volume
    {
        public const string MASTER = "Master_Volume";
        public const string BGM = "BGM_Volume";
        public const string SFX = "SFX_Volume";
        public const string UI = "UI_Volume";

        public string ExposedParameter = default;
        [InlineEditor] public FloatValueSO Value = default;

        public event UnityAction<Volume> OnValueChanged;

        internal void Init()
        {
            Value.OnValueChanged += TriggerOnValueChanged;
        }

        internal void Terminate()
        {
            Value.OnValueChanged -= TriggerOnValueChanged;
            OnValueChanged = null;
        }

        private void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke(this);
        }
    }
}