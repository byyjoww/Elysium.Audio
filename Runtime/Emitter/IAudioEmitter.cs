using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public interface IAudioEmitter
    {
        bool IsPlaying { get; }
        bool IsLooping { get; }

        event UnityAction<AudioClip, IAudioConfig, bool> OnPlay;
        event UnityAction OnStop;
        event UnityAction OnPause;
        event UnityAction OnResume;
        event UnityAction OnFinish;

        void Play(AudioClip _clip, IAudioConfig _settings, bool _loop);
        void Stop();
        void Pause();
        void Resume();
    }
}