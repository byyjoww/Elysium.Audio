using UnityEngine.Events;

namespace Elysium.Audio
{
    public interface IAudioEvent
    {
        event UnityAction<IAudioCue, IAudioConfig> OnRaise;
        void Raise(IAudioCue _cue, IAudioConfig _config);
    }
}
