using UnityEngine;
using UnityEngine.Audio;

namespace Elysium.Audio
{
    public interface IAudioConfig
    {
        bool IsExclusive { get; }
        AudioMixerGroup Output { get; }

        void ApplyTo(AudioSource source);
    }
}
