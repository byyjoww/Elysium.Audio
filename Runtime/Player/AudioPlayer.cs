using UnityEngine;

namespace Elysium.Audio
{
    public class AudioPlayer : AudioPlayerBase
    {
        [SerializeField] private OpenChannelEventSO openChannelEvent = default;
        [SerializeField] private AudioConfigSO config = default;
        private IAudioChannel channel = default;

        protected override IAudioEmitter Emitter => channel;
        protected override IAudioConfig Config => config;

        protected override void OnStarted()
        {
            channel = openChannelEvent.Create();
            channel.OnFinish += Finish;
        }

        protected override void OnDestroyed()
        {
            if (channel != null)
            {
                channel.OnFinish -= Finish;
                channel.Close();
            }
        }

        public override void PlayOneShot(AudioClip _clip, IAudioConfig _settings)
        {
            IAudioChannel channel = openChannelEvent.Create();
            void OnOneShotFinish()
            {
                channel.OnFinish -= OnOneShotFinish;
                channel.OnStop -= OnOneShotFinish;
                channel.Close();
            }
            channel.OnFinish += OnOneShotFinish;
            channel.OnStop += OnOneShotFinish;
            channel.Play(_clip, _settings, false);
        }
    }
}