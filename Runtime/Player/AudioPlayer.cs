using UnityEngine;

namespace Elysium.Audio
{
    public class AudioPlayer : AudioPlayerBase
    {
        [SerializeField] private OpenChannelEventSO openChannelEvent = default;
        [SerializeField] private AudioConfigSO config = default;
        private IAudioChannel channel = default;

        protected override IAudioPlayer Player => channel;
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
    }
}