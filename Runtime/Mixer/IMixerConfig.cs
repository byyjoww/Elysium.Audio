using Elysium.Core;
using System.Collections.Generic;

namespace Elysium.Audio
{
    public interface IMixerConfig
    {
        IEnumerable<IFactory<IChannel>> ChannelConfigs { get; }
    }
}
