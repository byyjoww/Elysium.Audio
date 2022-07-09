using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public struct AudioConfig : IAudioConfig
    {
        public AudioMixerGroup Group { get; }

        public event UnityAction OnValueChanged;

        public AudioConfig(AudioMixerGroup _group)
        {
            this.Group = _group;
            OnValueChanged = null;
        }        

        public void ApplyTo(AudioSource _source)
        {
            _source.outputAudioMixerGroup = Group;
        }
    }
}