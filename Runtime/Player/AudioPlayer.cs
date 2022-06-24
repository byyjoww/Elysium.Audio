namespace Elysium.Audio
{
    public class AudioPlayer : IAudioPlayer
    {
        private IAudioCue cue = default;        
        private IAudioEvent audioEvent = default;
        private IAudioConfig config = default;

        public AudioPlayer(IAudioCue _cue, IAudioConfig _config, IAudioEvent _audioEvent)
        {
            this.cue = _cue;
            this.config = _config;
            this.audioEvent = _audioEvent;
        }

        public void Play()
        {
            audioEvent.Raise(cue, config);
        }
    }
}
