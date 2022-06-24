using System;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public interface IChannel : IDisposable
    {
        float VolumeNormalized { get; set; }
        Decibel Volume { get; set; }

        event UnityAction OnDispose;
    }
}
