using Elysium.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour, IAudioEmitter
    {
        private AudioSource source = default;
        private IAudioConfig settings = default;
        private Coroutine active = default;
        private IUnityLogger logger = new UnityLogger();

        public bool IsPlaying => source && source.isPlaying;
        public bool IsLooping => source && source.loop;

        public event UnityAction<AudioClip, IAudioConfig, bool> OnPlay;
        public event UnityAction OnStop;
        public event UnityAction OnPause;
        public event UnityAction OnResume;
        public event UnityAction OnFinish;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void Play(AudioClip _clip, IAudioConfig _settings, bool _loop)
        {
            if (!IsPlaying) { Stop(); }

            this.settings = _settings;            
            UpdateAudioSourceSettings();
            source.clip = _clip;
            source.loop = _loop;

            if (source.outputAudioMixerGroup == null)
            {
                // TODO: Find a good way to default to master?
                logger.LogError($"Attempting to play audio clip {_clip.name} without assigning an output mixer group.");
                OnPlay?.Invoke(_clip, _settings, _loop);
                OnStop?.Invoke();
                return;
            }

            this.settings.OnValueChanged += UpdateAudioSourceSettings;
            source.Play();
            active = StartCoroutine(NotifyOnEnd());
            OnPlay?.Invoke(_clip, _settings, _loop);
        }        

        public void Pause()
        {
            if (!IsPlaying) { return; }
            source.Pause();
            CancelNotifyOnEnd();
            OnPause?.Invoke();            
        }        

        public void Resume()
        {
            if (IsPlaying) { return; }
            source.UnPause();
            active = StartCoroutine(NotifyOnEnd());
            OnResume?.Invoke();                  
        }

        public void Stop()
        {
            if (!IsPlaying) { return; }
            source.Stop();
            this.settings.OnValueChanged -= UpdateAudioSourceSettings;
            CancelNotifyOnEnd();
            OnStop?.Invoke();
        }

        private void Finish()
        {
            this.settings.OnValueChanged -= UpdateAudioSourceSettings;
            CancelNotifyOnEnd();
            OnFinish?.Invoke();
        }

        private void UpdateAudioSourceSettings()
        {
            this.settings.ApplyTo(source);            
            if (IsPlaying)
            {
                // Recalculate end of clip (in case pitch changes, for example)
                CancelNotifyOnEnd();
                StartCoroutine(NotifyOnEnd());
            }            
        }

        private IEnumerator NotifyOnEnd()
        {
            var wait = new WaitForSecondsRealtime(source.GetClipRemainingTime());
            yield return wait;
            Finish();
            if (IsLooping)
            {
                active = StartCoroutine(NotifyOnEnd());
                OnPlay?.Invoke(source.clip, settings, IsLooping);
            }
        }

        private void CancelNotifyOnEnd()
        {
            if (active == null) { return; }
            StopCoroutine(active);
            active = null;
        }

        private void OnDestroy()
        {
            OnPlay = null;
            OnStop = null;
            OnPause = null;
            OnResume = null;
            OnFinish = null;
        }
    }
}