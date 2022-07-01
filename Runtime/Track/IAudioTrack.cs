using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Audio
{
    public interface IAudioTrack
    {
        IEnumerable<AudioClip> GetAudioClips();
    }
}