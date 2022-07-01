using UnityEngine;
using UnityEngine.Audio;

namespace Elysium.Audio
{
    public struct AudioConfig : IAudioConfig
    {
        private AudioMixerGroup group { get; }

        public AudioConfig(AudioMixerGroup _group)
        {
            this.group = _group;
        }

        public void ApplyTo(AudioSource _source)
        {
            _source.outputAudioMixerGroup = group;
        }
    }
}