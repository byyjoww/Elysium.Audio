using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    internal interface IAudioChannelInternal : IAudioChannel
    {
        AudioClip Clip { get; }
        IAudioConfig Config { get; }

        event UnityAction OnClose;
        void NotifyTrackFinished();
        void LogEvents();
    }
}