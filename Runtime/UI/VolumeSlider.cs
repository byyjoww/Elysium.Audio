using Elysium.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Elysium.Audio
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private FloatValueSO normalizedVolume = default;
        [SerializeField] private Slider slider = default;

        private void Start()
        {
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = normalizedVolume.Value;
            slider.onValueChanged.AddListener(OnSliderChanged);            
        }

        private void OnSliderChanged(float _new)
        {
            normalizedVolume.Value = _new;
        }

        private void OnValidate()
        {
            if (slider is null) { slider = GetComponent<Slider>(); }
        }
    }
}
