using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public interface IPositionalAudioEvent
    {
        event UnityAction<IAudioCue, IAudioConfig, Vector3> OnRaise;

        void Raise(IAudioCue _cue, IAudioConfig _config, Vector3 _position);
    }
}
