using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public interface IAudioConfig
    {
        AudioMixerGroup Group { get; }

        event UnityAction OnValueChanged;

        void ApplyTo(AudioSource source);
    }
}