using System;
using UnityEngine;

namespace Elysium.Audio
{
    public interface IAudioChannel : IAudioPlayer
    {        
        void Close();
    }
}