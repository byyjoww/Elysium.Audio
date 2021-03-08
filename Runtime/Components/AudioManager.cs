using Elysium.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Elysium.Audio
{
    // REMEMBER TO EXPOSE THE VARIABLES FOR EACH MIXER CHANNEL IN THE INSPECTOR
    public class AudioManager : MonoBehaviour
    {
        [Header("SoundEmitters pool")]
        [SerializeField] private SoundEmitterFactorySO _factory = default;
        [SerializeField] private SoundEmitterPoolSO _pool = default;
        [SerializeField] private int _initialSize = 10;

        [Header("Listening on channels")]
        [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play sounds")]
        [SerializeField] private AudioChannelSO _AudioEventChannel = default;

        [Header("Audio control")]
        [SerializeField] private VolumeControl volumeControl = default;

        private Dictionary<AudioCueSO, SoundEmitter> activeSoundEmitters = default;

        // ------------------------------------ PUBLIC ------------------------------------ //

        /// <summary>
        /// Plays an AudioCue by requesting the appropriate number of SoundEmitters from the pool.
        /// </summary>
        public void PlayAudioCue(AudioCueSO _audioCue, AudioConfigSO _settings, Vector3 _position = default)
        {
            AudioClip[] clipsToPlay = _audioCue.GetClips();
            int nOfClips = clipsToPlay.Length;

            for (int i = 0; i < nOfClips; i++)
            {
                SoundEmitter soundEmitter = _pool.Request();
                activeSoundEmitters.Add(_audioCue, soundEmitter);
                if (soundEmitter != null)
                {
                    soundEmitter.PlayAudioClip(clipsToPlay[i], _settings, _audioCue.looping, _position);
                    if (!_audioCue.looping) { soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying; }                        
                }
            }
        }

        /// <summary>
        /// Stop an AudioCue and release any held resources.
        /// </summary>
        public void StopAudioCue(AudioCueSO _audioCue)
        {
            if (activeSoundEmitters.ContainsKey(_audioCue)) { return; }
            activeSoundEmitters[_audioCue].Stop();            
        }

        /// <summary>
        /// Pause an AudioCue, while still witholding any resources.
        /// </summary>
        public void PauseAudioCue(AudioCueSO _audioCue)
        {
            if (activeSoundEmitters.ContainsKey(_audioCue)) { return; }
            activeSoundEmitters[_audioCue].Pause();
        }

        /// <summary>
        /// Resume a paused AudioCue.
        /// </summary>
        public void ResumeAudioCue(AudioCueSO _audioCue)
        {
            if (activeSoundEmitters.ContainsKey(_audioCue)) { return; }
            activeSoundEmitters[_audioCue].Resume();
        }

        // ------------------------------------ PRIVATE ------------------------------------ //

        private void Awake()
        {
            activeSoundEmitters = new Dictionary<AudioCueSO, SoundEmitter>();

            _AudioEventChannel.OnAudioCueRequested += PlayAudioCue;

            _pool.Prewarm(_initialSize);
            _pool.SetParent(transform);
        }

        private void Start()
        {
            volumeControl.Initialize();
        }

        private void OnSoundEmitterFinishedPlaying(SoundEmitter _soundEmitter)
        {
            _soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
            _soundEmitter.Stop();
            _pool.Return(_soundEmitter);
        }
    }
}