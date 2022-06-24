using Elysium.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Audio
{
    public class MixerConfigSO : IMixerConfig
    {
        public const string MASTER_CHANNEL = "MasterVolume";
        public const string BGM_CHANNEL = "BGMVolume";
        public const string SFX_CHANNEL = "SFXVolume";

        [SerializeField]
        private ChannelConfig[] channels = new ChannelConfig[]
        {
            // DEFAULT MIXER CHANNELS FOR NEW SCRIPTS
            new ChannelConfig(MASTER_CHANNEL),
            new ChannelConfig(BGM_CHANNEL),
            new ChannelConfig(SFX_CHANNEL),
        };

        public IEnumerable<IFactory<IChannel>> ChannelConfigs => channels;
    }
}
