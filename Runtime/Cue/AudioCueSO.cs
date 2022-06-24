using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "AudioCueSO_", menuName = "Scriptable Objects/Audio/Cue")]
    public class AudioCueSO : IAudioCue
    {
        [SerializeField] private bool looping = false;
        [SerializeField] private AudioClipCollection[] clipCollections = new AudioClipCollection[0];

        public bool Looping => looping;
        public bool IsEmpty => clipCollections.Length == 0;

        public AudioClip[] Clips()
        {
            AudioClip[] resultingClips = new AudioClip[clipCollections.Length];

            for (int i = 0; i < clipCollections.Length; i++)
            {
                resultingClips[i] = clipCollections[i].GetNextClip();
            }

            return resultingClips;
        }
    }
}
