using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public class Channel : IChannel
    {
        private AudioMixer mixer = default;
        private IChannelConfig config = default;

        public event UnityAction OnDispose = delegate { };

        public Channel(IChannelConfig _config)
        {
            this.config = _config;
            this.VolumeNormalized = config.Volume;
        }

        public float VolumeNormalized
        {
            get
            {
                if (mixer.GetFloat(config.Channel, out float _raw))
                {
                    Decibel decibel = new Decibel(_raw);
                    return decibel.Normalized;
                }

                Debug.LogError($"No mixer channels with name '{config.Channel}' were found");
                return 0f;
            }
            set
            {
                Decibel decibel = Decibel.FromNormalized(value);
                if (!mixer.SetFloat(config.Channel, decibel.Value)) 
                {
                    Debug.LogError($"No mixer channels with name '{config.Channel}' were found");
                }
            }
        }

        public Decibel Volume
        {
            get
            {
                if (mixer.GetFloat(config.Channel, out float _raw))
                {
                    return new Decibel(_raw);
                }

                Debug.LogError($"No mixer channels with name '{config.Channel}' were found");
                return new Decibel();
            }
            set
            {
                if (!mixer.SetFloat(config.Channel, value.Value))
                {
                    Debug.LogError($"No mixer channels with name '{config.Channel}' were found");
                }
            }
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}
