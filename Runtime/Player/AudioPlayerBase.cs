using Elysium.Core;
using Elysium.Core.Attributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public abstract class AudioPlayerBase : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] protected bool playOnStart = default;
        [ConditionalField("playOnStart")]
        [SerializeField] protected bool loop = default;
        [ConditionalField("playOnStart")]
        [SerializeField] protected AudioClip clip = default;

        protected IUnityLogger logger = new UnityLogger();

        protected abstract IAudioEmitter Emitter { get; }
        protected abstract IAudioConfig Config { get; }
        public bool IsPlaying => Emitter != null && Emitter.IsPlaying;
        public bool IsLooping => Emitter != null && Emitter.IsLooping;

        public event UnityAction<AudioClip, IAudioConfig, bool> OnPlay;
        public event UnityAction OnStop;
        public event UnityAction OnPause;
        public event UnityAction OnResume;
        public event UnityAction OnFinish;

        private void Start()
        {
            OnStarted();            
            if (playOnStart) { Play(); }
        }

        [ContextMenu("Play One Shot")]
        private void PlayOneShot()
        {
            if (clip is null)
            {
                logger.Log($"No audio clip was loaded for player {name}.");
                return;
            }
            PlayOneShot(clip, Config);
        }

        [ContextMenu("Play")]
        private void Play()
        {
            if (clip is null)
            {
                logger.Log($"No audio clip was loaded for player {name}.");
                return;
            }
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

        public abstract void PlayOneShot(AudioClip _clip, IAudioConfig _settings);

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
            Emitter.Play(_clip, _settings, _loop);
        }

        protected virtual void OnClipStoppedPlaying()
        {
            Emitter.Stop();
        }

        protected virtual void OnClipPaused()
        {
            Emitter.Pause();
        }

        protected virtual void OnClipResumed()
        {
            Emitter.Resume();
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
            OnPlay?.Invoke(_clip, _settings, _loop);
        }

        protected void TriggerOnStop()
        {
            OnStop?.Invoke();
        }

        protected void TriggerOnPause()
        {
            OnPause?.Invoke();
        }

        protected void TriggerOnResume()
        {
            OnResume?.Invoke();
        }

        protected void TriggerOnFinish()
        {
            OnFinish?.Invoke();
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