using System;
using UnityEngine;

namespace Elysium.Audio
{
    public interface IAudioChannel : IAudioEmitter
    {        
        void Close();
    }
}