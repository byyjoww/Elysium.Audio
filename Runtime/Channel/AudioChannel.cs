using Elysium.Core;
using Elysium.Core.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public class AudioChannel : IAudioChannel, IAudioChannelInternal
    {
        private IUnityLogger logger = new UnityLogger();

        public bool IsPlaying { get; private set; }
        public bool IsLooping { get; private set; }

        public event UnityAction<AudioClip, IAudioConfig, bool> OnPlay;
        public event UnityAction OnStop;
        public event UnityAction OnPause;
        public event UnityAction OnResume;
        public event UnityAction OnFinish;
        public event UnityAction OnClose;

        internal AudioChannel()
        {
            logger.logEnabled = true;
        }

        public void Play(AudioClip _clip, IAudioConfig _settings, bool _loop)
        {
            if (IsPlaying) { Stop(); }
            IsPlaying = true;
            IsLooping = _loop;
            OnPlay?.Invoke(_clip, _settings, _loop);
        }

        public void Stop()
        {
            if (!IsPlaying) { return; }
            IsPlaying = false;
            OnStop?.Invoke();
        }

        public void Pause()
        {
            if (!IsPlaying) { return; }
            IsPlaying = false;
            OnPause?.Invoke();
        }

        public void Resume()
        {
            if (IsPlaying) { return; }
            IsPlaying = true;
            OnResume?.Invoke();
        }        

        void IAudioChannel.Close()
        {
            Stop();
            OnClose?.Invoke();
            OnPlay = null;
            OnStop = null;
            OnPause = null;
            OnResume = null;
            OnFinish = null;
            OnClose = null;
        }

        void IAudioChannelInternal.NotifyTrackFinished()
        {
            IsPlaying = IsLooping;
        }

        public void LogEvents()
        {
            logger.Log($"OnPlay: {(OnPlay != null ? OnPlay.GetInvocationList().Length : 0)}");
            logger.Log($"OnStop: {(OnStop != null ? OnStop.GetInvocationList().Length : 0)}");
            logger.Log($"OnPause: {(OnPause != null ? OnPause.GetInvocationList().Length : 0)}");
            logger.Log($"OnResume: {(OnResume != null ? OnResume.GetInvocationList().Length : 0)}");
            logger.Log($"OnFinish: {(OnFinish != null ? OnFinish.GetInvocationList().Length : 0)}");
            logger.Log($"OnClose: {(OnClose != null ? OnClose.GetInvocationList().Length : 0)}");
        }
    }
}