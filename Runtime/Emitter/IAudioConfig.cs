using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public interface IAudioConfig
    {
        event UnityAction OnValueChanged;

        void ApplyTo(AudioSource source);
    }
}