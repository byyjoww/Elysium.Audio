using Elysium.Core;
using UnityEngine;

namespace Elysium.Audio
{
    [System.Serializable]
    public class ChannelConfig : IChannelConfig, IFactory<IChannel>
    {
        [SerializeField] private string channel = default;
        [SerializeField] private FloatValueSO volume = default;

        public string Channel => channel;
        public float Volume => volume.Value;

        public ChannelConfig() { }

        public ChannelConfig(string _channel)
        {
            this.channel = _channel;
        }

        public IChannel Create()
        {
            IChannel channel = new Channel(this);
            void UpdateChannelVolume() => channel.VolumeNormalized = volume.Value;
            volume.OnValueChanged += UpdateChannelVolume;
            channel.OnDispose -= UpdateChannelVolume;
            return channel;
        }
    }
}
