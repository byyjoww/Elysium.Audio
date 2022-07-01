using UnityEngine;

namespace Elysium.Audio
{
    public interface IAudioCue
    {
        bool Loop { get; }

        AudioClip[] Clips();
        bool IsEmpty { get; }
    }
}
