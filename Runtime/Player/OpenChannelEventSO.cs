using Elysium.Core;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "OpenChannelEventSO", menuName = "Scriptable Objects/Audio/Open Channel Event")]
    internal class OpenChannelEventSO : GenericEventSO<IAudioChannelInternal>, IFactory<IAudioChannel>
    {
        private UnityLogger logger = new UnityLogger();

        public IAudioChannel Create()
        {
            WarnIfNoSubscribedAudioListeners();
            AudioChannel channel = new AudioChannel();
            Raise(channel);
            return channel;
        }

        private void WarnIfNoSubscribedAudioListeners()
        {
            if (HasNoSubscribedEvents)
            {
                logger.LogError($"Attempting to open new audio channel using an open channel event ({name}) with no subscribed event listeners. " +
                    $"Make sure you have an active AudioChannelListener in your current or persistent scene.");
            }
        }
    }
}