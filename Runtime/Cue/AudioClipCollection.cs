using UnityEngine;

namespace Elysium.Audio
{
    [System.Serializable]
    public class AudioClipCollection
    {
        [SerializeField] private SequenceMode mode = SequenceMode.RandomNoImmediateRepeat;
        [SerializeField] private AudioClip[] clips = new AudioClip[0];

        private int nextClipToPlay = -1;
        private int lastClipPlayed = -1;

        public enum SequenceMode
        {
            Random,
            RandomNoImmediateRepeat,
            Sequential,
        }

        /// <summary>
        /// Chooses the next clip in the sequence, either following the order or randomly.
        /// </summary>
        public AudioClip GetNextClip()
        {
            if (clips.Length == 1) { return clips[0]; }

            if (nextClipToPlay == -1)
            {
                // Index needs to be initialised: 0 if Sequential, random if otherwise
                nextClipToPlay = (mode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, clips.Length);
            }
            else
            {
                if (mode == SequenceMode.Random)
                {
                    nextClipToPlay = UnityEngine.Random.Range(0, clips.Length);
                }
                else if (mode == SequenceMode.RandomNoImmediateRepeat)
                {
                    do
                    {
                        nextClipToPlay = UnityEngine.Random.Range(0, clips.Length);
                    }
                    while (nextClipToPlay == lastClipPlayed);
                }
                else if (mode == SequenceMode.Sequential)
                {
                    nextClipToPlay = (int)Mathf.Repeat(++nextClipToPlay, clips.Length);
                }
            }

            lastClipPlayed = nextClipToPlay;
            return clips[nextClipToPlay];
        }        
    }
}
