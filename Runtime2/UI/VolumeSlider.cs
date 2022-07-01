using Elysium.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Elysium.Audio
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private FloatValueSO normalizedVolume = default;

        private Slider slider = default;

        private void Start()
        {
            SetupSlider();
            UpdateSliderValue();

            void OnSliderChanged(float _new) => normalizedVolume.Value = _new;
            slider.onValueChanged.AddListener(OnSliderChanged);
        }

        private void SetupSlider()
        {
            slider = GetComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 1;
        }

        private void UpdateSliderValue()
        {
            slider.value = normalizedVolume.Value;
        }
    }
}
