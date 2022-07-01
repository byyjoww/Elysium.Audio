using UnityEngine;

namespace Elysium.Audio
{
    public interface IAudioConfig
    {
        void ApplyTo(AudioSource source);
    }
}