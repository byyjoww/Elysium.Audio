using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Audio
{
    public class SimpleAudioTrack : IAudioTrack
    {
        public IEnumerable<AudioClip> clips = default;

        public SimpleAudioTrack(params AudioClip[] _clips)
        {
            this.clips = _clips;
        }

        public IEnumerable<AudioClip> GetAudioClips()
        {
            return clips;
        }
    }
}