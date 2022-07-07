using Elysium.Core;
using Elysium.Core.Utils;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Elysium.Audio
{
    public class SimpleMixerController : MonoBehaviour
    {
        [SerializeField] private bool enableLogging = default;
        [SerializeField] private AudioMixer mixer = default;
        [SerializeField] private MixerGroup[] groups = new MixerGroup[]
        {
            new MixerGroup(Volume.MASTER),
            new MixerGroup(Volume.BGM),
            new MixerGroup(Volume.SFX),
            new MixerGroup(Volume.UI),
        };

        private IUnityLogger logger = new UnityLogger();

        private void Start()
        {
            logger.logEnabled = enableLogging;
            foreach (var group in groups)
            {
                group.Init();
                group.Volume.OnValueChanged += SetVolume;
                SetVolume(group.Volume);
            }
        }

        private void OnDestroy()
        {
            foreach (var group in groups)
            {
                group.Volume.OnValueChanged -= SetVolume;
                group.Terminate();
            }
        }

        private void SetVolume(Volume _volume)
        {
            string param = _volume.ExposedParameter;
            Decibels decibels = Decibels.FromNormalized(_volume.Value.Value);
            if (!mixer.GetFloat(param, out float _current) && _current != decibels.Value) 
            {
                _volume.Value.Value = _current;
                logger.LogError($"Failed to get mixer group volume with exposed parameter {param}. Make sure this parameter " +
                    $"is exposed in the mixer, and that it matches with the 'Exposed Parameter' value in this script.");
                return;
            }

            if (!mixer.SetFloat(param, decibels.Value))
            {
                // If we couldn't set the mixer volume, set the slider value back to previous value
                // to show there was an issue setting the parameter in the mixer.
                _volume.Value.Value = _current;
                logger.LogError($"Failed to set mixer group volume with exposed parameter {param} to {decibels.Value} decibels.");
                return;
            }

            logger.Log($"Set mixer group volume with exposed parameter {param} to {decibels.Value} decibels", _color: Color.yellow);
        }
    }
}