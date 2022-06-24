using UnityEngine;

namespace Elysium.Audio
{
    public interface IAudioCue
    {
        bool Looping { get; }

        AudioClip[] Clips();
        bool IsEmpty { get; }
    }
}
