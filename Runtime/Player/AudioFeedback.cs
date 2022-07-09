using Elysium.Core.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Audio
{
    [System.Serializable]
    public class AudioFeedback : IFeedback
    {
        [SerializeField] private AudioClip audioClip = default;
        [RequireInterface(typeof(IAudioConfig))]
        [SerializeField] private UnityEngine.Object audioConfig = default;

        private IAudioConfig config { get; set; }
        private OpenChannelEventSO openChannelEvent = default;

        public AudioFeedback()
        {
            
        }

        public AudioFeedback(AudioClip _clip, IAudioConfig _config)
        {
            this.audioClip = _clip;
            this.config = _config;
        }

        public void Execute()
        {
            if (config == null && audioConfig != null) { config = audioConfig as IAudioConfig; }
            if (openChannelEvent == null) { openChannelEvent = OpenChannelEventSO.DefaultOpenChannelEvent; }
            IAudioChannel channel = openChannelEvent.Create();
            channel.OnFinish += channel.Close;
            channel.Play(audioClip, config, false);
        }
    }

    public interface IFeedback
    {
        void Execute();
    }
}