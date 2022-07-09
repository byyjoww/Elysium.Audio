using Elysium.Core;
using Elysium.Core.Utils.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "OpenChannelEventSO", menuName = "Scriptable Objects/Audio/Open Channel Event")]
    internal class OpenChannelEventSO : GenericEventSO<IAudioChannelInternal>, IFactory<IAudioChannel>
    {
        [SerializeField] private bool isDefaultOpenChannelEvent = false;

        private IUnityLogger logger = new UnityLogger();
        public static OpenChannelEventSO DefaultOpenChannelEvent = default;

        public void Awake()
        {
            if (GetAllOpenChannelEvents().Count() == 0)
            {
                isDefaultOpenChannelEvent = true;
            }
        }        

        public IAudioChannel Create()
        {
            WarnIfNoSubscribedAudioListeners();
            AudioChannel channel = new AudioChannel();
            Raise(channel);
            return channel;
        }

        private void OnEnable()
        {
            if (isDefaultOpenChannelEvent)
            {
                DefaultOpenChannelEvent = this;
            }
        }

        private void OnDisable()
        {
            if (isDefaultOpenChannelEvent)
            {
                DefaultOpenChannelEvent = null;
            }
        }

        private void WarnIfNoSubscribedAudioListeners()
        {
            if (HasNoSubscribedEvents)
            {
                logger.LogError($"Attempting to open new audio channel using an open channel event ({name}) with no subscribed event listeners. " +
                    $"Make sure you have an active AudioChannelListener in your current or persistent scene.");
            }
        }

        private static IEnumerable<OpenChannelEventSO> GetAllOpenChannelEvents()
        {
            return AssemblyTools.GetAllScriptableObjects<OpenChannelEventSO>().Where(x => x.isDefaultOpenChannelEvent);
        }

        private void OnValidate()
        {
            if (isDefaultOpenChannelEvent)
            {
                var chans = GetAllOpenChannelEvents();
                if (chans.Count() > 1)
                {
                    logger.LogError($"There is more than one default open channel event in this project. " +
                        $"Default Channels: [{string.Join(", ", chans.Select(x => x.name))}]");
                }
            }
        }
    }
}