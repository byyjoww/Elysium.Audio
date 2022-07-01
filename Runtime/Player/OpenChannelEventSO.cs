using Elysium.Core;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "OpenChannelEventSO", menuName = "Scriptable Objects/Audio/Open Channel Event")]
    internal class OpenChannelEventSO : GenericEventSO<IAudioChannelInternal>, IFactory<IAudioChannel>
    {
        public IAudioChannel Create()
        {
            AudioChannel channel = new AudioChannel();
            Raise(channel);
            return channel;
        }
    }
}