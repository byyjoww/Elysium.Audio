using Elysium.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Elysium.Audio
{
    [System.Serializable]
    public class VolumeControl
    {
        [SerializeField] private AudioMixer audioMixer = default;
        [SerializeField] private MixerGroup[] mixerGroups = new MixerGroup[]
        {
            // DEFAULT MIXER CHANNELS FOR NEW SCRIPTS
            new MixerGroup(MixerGroup.MASTER_CHANNEL),
            new MixerGroup(MixerGroup.BGM_CHANNEL),
            new MixerGroup(MixerGroup.SFX_CHANNEL),
        };

        public event UnityAction OnStart;
        public event UnityAction OnDestroy;

        // Call this on Start (not AWAKE), to ensure that the mixer runtime components aren't created AFTER setting the initial volume.
        public void Initialize()
        {
            for (int i = 0; i < mixerGroups.Length; i++)
            {
                MixerGroup mg = mixerGroups[i];

                void SetVolumeForGroup() => SetGroupVolume(mg.Mixer, mg.Volume.Value);
                mg.Volume.OnValueChanged += SetVolumeForGroup;

                OnDestroy += () => mg.Volume.OnValueChanged -= SetVolumeForGroup;

                SetVolumeForGroup();
            }

            OnStart?.Invoke();
        }

        public void SetGroupVolume(string _parameterName, float _normalizedVolume)
        {
            Debug.Log($"setting initial volume of {_normalizedVolume} for {_parameterName}");
            bool volumeSet = audioMixer.SetFloat(_parameterName, NormalizedToDecibels(_normalizedVolume));
            if (!volumeSet) { Debug.LogError("The AudioMixer parameter was not found"); }                
        }

        public float GetGroupVolume(string _parameterName)
        {
            if (audioMixer.GetFloat(_parameterName, out float rawVolume))
            {
                return DecibelsToNormalized(rawVolume);
            }
            else
            {
                Debug.LogError("The AudioMixer parameter was not found");
                return 0f;
            }
        }

        // Decibels are on a log scale. If we consider a scale from 0db ~ -80db, -50% volume should be -6db (not -40db).
        private float DecibelsToNormalized(float _decibels)
        {            
            if (_decibels == -80f) { return 0; }
            float normalized = Mathf.Pow(10, _decibels / 20);
            return normalized;
        }

        // Decibels are on a log scale. If we consider a scale from 0db ~ -80db, -50% volume should be -6db (not -40db).
        private float NormalizedToDecibels(float _normalized)
        {            
            float decibels = Mathf.Log10(_normalized) * 20f;
            if (_normalized == 0) { return -80f; }
            return decibels;
        }

        ~VolumeControl()
        {
            OnDestroy?.Invoke();
            OnDestroy = null;
        }
    }

    [System.Serializable]
    public class MixerGroup
    {
        public const string MASTER_CHANNEL = "MasterVolume";
        public const string BGM_CHANNEL = "BGMVolume";
        public const string SFX_CHANNEL = "SFXVolume";

        public string Mixer = default;
        public FloatValueSO Volume = default;

        public MixerGroup() { }

        public MixerGroup(string _mixer)
        {
            this.Mixer = _mixer;
        }
    }
}