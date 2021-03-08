using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource audioSource = default;
        private Coroutine activeCoroutine = default;

        public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

        public bool IsInUse => audioSource.isPlaying;
        public bool IsLooping => audioSource.loop;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        /// <summary>
        /// Instructs the AudioSource to play a single clip, with optional looping, in a position in 3D space.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="settings"></param>
        /// <param name="hasToLoop"></param>
        /// <param name="position"></param>
        public void PlayAudioClip(AudioClip clip, AudioConfigSO settings, bool hasToLoop, Vector3 position = default)
        {
            audioSource.clip = clip;
            settings.ApplyTo(audioSource);
            audioSource.transform.position = position;
            audioSource.loop = hasToLoop;
            audioSource.Play();

            if (!hasToLoop) { activeCoroutine = StartCoroutine(FinishedPlaying(clip.length)); }
        }

        /// <summary>
        /// Used when the game is unpaused, to pick up SFX from where they left.
        /// </summary>
        public void Resume()
        {
            if (audioSource.isPlaying) { return; }
            audioSource.UnPause();
            ResumeActiveCoroutine();
        }

        /// <summary>
        /// Used when the game is paused.
        /// </summary>
        public void Pause()
        {
            audioSource.Pause();
            StopActiveCoroutine();
        }

        /// <summary>
        /// Used when the SFX finished playing. Called by the <c>AudioManager</c>.
        /// </summary>
        public void Stop()
        {
            audioSource.Stop();
            StopActiveCoroutine();
            OnSoundFinishedPlaying.Invoke(this);
        }

        private void StopActiveCoroutine()
        {
            if (IsLooping) { return; }
            if (activeCoroutine != null) { return; }

            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        private void ResumeActiveCoroutine()
        {
            if (IsLooping) { return; }

            float timeLeft = audioSource.clip.length - audioSource.time;
            if (timeLeft < 0) { throw new System.Exception("attempting to resume a coroutine with time lower than 0 for non-looping sound emitter"); }

            activeCoroutine = StartCoroutine(FinishedPlaying(timeLeft));
        }

        IEnumerator FinishedPlaying(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);

            activeCoroutine = null;
            OnSoundFinishedPlaying.Invoke(this); // The AudioManager will pick this up
        }
    }
}