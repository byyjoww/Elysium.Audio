using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public struct AudioConfig : IAudioConfig
    {
        private AudioMixerGroup group { get; }

        public event UnityAction OnValueChanged;

        public AudioConfig(AudioMixerGroup _group)
        {
            this.group = _group;
            OnValueChanged = null;
        }        

        public void ApplyTo(AudioSource _source)
        {
            _source.outputAudioMixerGroup = group;
        }
    }
}