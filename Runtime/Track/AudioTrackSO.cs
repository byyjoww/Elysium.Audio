using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "AudioTrackSO_",  menuName = "Scriptable Objects/Audio/Track")]
    public class AudioTrackSO : ScriptableObject, IAudioTrack
    {
        [SerializeField] private AudioClip[] clips = new AudioClip[0];

        public IEnumerable<AudioClip> GetAudioClips()
        {
            return clips;
        }
    }
}