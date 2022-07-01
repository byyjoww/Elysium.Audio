using UnityEngine.Events;

namespace Elysium.Audio
{
    internal interface IAudioChannelInternal : IAudioChannel
    {
        event UnityAction OnClose;
        void NotifyTrackFinished();
        void LogEvents();
    }
}