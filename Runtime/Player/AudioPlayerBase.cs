using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public abstract class AudioPlayerBase : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] protected bool playOnStart = default;
        [SerializeField] protected bool loop = default;        
        [SerializeField] protected AudioClip clip = default;        

        protected abstract IAudioPlayer Player { get; }
        protected abstract IAudioConfig Config { get; }
        public bool IsPlaying => Player != null && Player.IsPlaying;
        public bool IsLooping => Player != null && Player.IsLooping;

        public event UnityAction<AudioClip, IAudioConfig, bool> OnPlay = delegate { };
        public event UnityAction OnStop = delegate { };
        public event UnityAction OnPause = delegate { };
        public event UnityAction OnResume = delegate { };
        public event UnityAction OnFinish = delegate { };

        private void Start()
        {
            OnStarted();            
            if (playOnStart) { Play(); }
        }

        [ContextMenu("Play")]
        public void Play()
        {
            Play(clip, Config, loop);
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            OnClipStoppedPlaying();
            TriggerOnStop();
        }

        [ContextMenu("Pause")]
        public void Pause()
        {
            OnClipPaused();
            TriggerOnPause();
        }

        [ContextMenu("Resume")]
        public void Resume()
        {
            OnClipResumed();
            TriggerOnResume();
        }

        public void Play(AudioClip _clip, IAudioConfig _settings, bool _loop)
        {
            if (IsPlaying) { Stop(); }
            OnClipStartedPlaying(_clip, _settings, _loop);
            TriggerOnPlay(_clip, _settings, _loop);
        }

        protected void Finish()
        {
            OnClipFinished();
            TriggerOnFinish();
        }        

        protected virtual void OnClipStartedPlaying(AudioClip _clip, IAudioConfig _settings, bool _loop)
        {
            Player.Play(_clip, _settings, _loop);
        }

        protected virtual void OnClipStoppedPlaying()
        {
            Player.Stop();
        }

        protected virtual void OnClipPaused()
        {
            Player.Pause();
        }

        protected virtual void OnClipResumed()
        {
            Player.Resume();
        }

        protected virtual void OnClipFinished()
        {

        }

        protected virtual void OnStarted()
        {

        }

        protected virtual void OnDestroyed()
        {

        }

        protected void TriggerOnPlay(AudioClip _clip, IAudioConfig _settings, bool _loop)
        {
            OnPlay.Invoke(_clip, _settings, _loop);
        }

        protected void TriggerOnStop()
        {
            OnStop.Invoke();
        }

        protected void TriggerOnPause()
        {
            OnPause.Invoke();
        }

        protected void TriggerOnResume()
        {
            OnResume.Invoke();
        }

        protected void TriggerOnFinish()
        {
            OnFinish.Invoke();
        }

        private void OnDestroy()
        {
            OnDestroyed();
            OnPlay = null;
            OnStop = null;
            OnPause = null;
            OnResume = null;
            OnFinish = null;
        }
    }
}