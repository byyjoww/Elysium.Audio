using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public class Emitter : MonoBehaviour, IEmitter
    {
        private AudioSource source = default;
        private Coroutine active = default;

        public bool IsInUse => source.isPlaying;
        public bool IsLooping => source.loop;

        public event UnityAction<IEmitter> OnSoundFinishedPlaying = delegate { };        

        public void Play(AudioClip _clip, IAudioConfig _settings, bool _loop, Vector3 _position = default)
        {
            source.clip = _clip;
            _settings.ApplyTo(source);
            source.transform.position = _position;
            source.loop = _loop;
            source.Play();

            if (!_loop) { active = StartCoroutine(FinishedPlaying(_clip.length)); }
        }

        public void Resume()
        {
            if (source.isPlaying) { return; }
            source.UnPause();
            ResumeActiveCoroutine();
        }

        public void Pause()
        {
            source.Pause();
            StopActiveCoroutine();
        }

        public void Stop()
        {
            source.Stop();
            StopActiveCoroutine();
            OnSoundFinishedPlaying?.Invoke(this);
        }

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
        }

        private void StopActiveCoroutine()
        {
            if (IsLooping) { return; }
            if (active == null) { return; }

            StopCoroutine(active);
            active = null;
        }

        private void ResumeActiveCoroutine()
        {
            if (IsLooping) { return; }

            float timeLeft = source.clip.length - source.time;
            if (timeLeft < 0) { throw new System.Exception("attempting to resume a coroutine with time lower than 0 for non-looping sound emitter"); }

            active = StartCoroutine(FinishedPlaying(timeLeft));
        }

        IEnumerator FinishedPlaying(float _length)
        {
            yield return new WaitForSeconds(_length);

            active = null;
            OnSoundFinishedPlaying?.Invoke(this); // The AudioManager will pick this up
        }
    }
}
