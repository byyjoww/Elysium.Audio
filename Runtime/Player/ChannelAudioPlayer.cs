using UnityEngine;

namespace Elysium.Audio
{
    public class ChannelAudioPlayer : AudioPlayerBase
    {
        [SerializeField] private OpenChannelEventSO openChannelEvent = default;
        private IAudioChannel channel = default;

        protected override IAudioPlayer Player => channel;

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