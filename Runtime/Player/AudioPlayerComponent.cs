using Elysium.Core.Attributes;
using UnityEngine;

namespace Elysium.Audio
{
    public class AudioPlayerComponent : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private bool playOnStart = false;
        [RequireInterface(typeof(IAudioCue))]
        [SerializeField] private UnityEngine.Object cue = default;        
        [RequireInterface(typeof(IAudioConfig))]
        [SerializeField] private UnityEngine.Object config = default;
        [RequireInterface(typeof(IPositionalAudioEvent))]
        [SerializeField] private UnityEngine.Object audioEvent = default;

        private IAudioCue Cue => cue as IAudioCue;
        private IAudioConfig Config => config as IAudioConfig;
        private IPositionalAudioEvent AudioEvent => audioEvent as IPositionalAudioEvent;        

        private void Start()
        {
            if (playOnStart) { Play(); }                
        }

        [ContextMenu("Play Audio Cue")]
        public void Play()
        {
            AudioEvent.Raise(Cue, Config, transform.position);
        }
    }
}
