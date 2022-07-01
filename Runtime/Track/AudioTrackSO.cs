using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "AudioTrackSO_",  menuName = "Scriptable Objects/Audio/Track")]
    public class AudioTrackSO : ScriptableObject, IAudioTrack
    {
        [SerializeField] private bool loop = false;
        [SerializeField] private AudioClipCollection[] clipCollections = new AudioClipCollection[0];

        public bool Loop => loop;

        public IEnumerable<AudioClip> GetAudioClips()
        {
            return clipCollections.Select(x => x.GetNextClip()).ToArray();
        }
    }
}