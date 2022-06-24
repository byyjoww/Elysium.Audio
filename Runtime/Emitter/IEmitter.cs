using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public interface IEmitter
    {
        bool IsInUse { get; }
        bool IsLooping { get; }

        event UnityAction<IEmitter> OnSoundFinishedPlaying;
        
        void Play(AudioClip _clip, IAudioConfig _settings, bool _loop, Vector3 _position = default);
        void Stop();
        void Pause();
        void Resume();        
    }
}
