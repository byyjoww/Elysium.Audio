using Elysium.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

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

        private List<AudioCueWrapper> activeSoundEmitters = default;
        private List<AudioCueWrapper> exclusiveAudioCues = default;

        // ------------------------------------ PUBLIC ------------------------------------ //

        /// <summary>
        /// Plays an AudioCue by requesting the appropriate number of SoundEmitters from the pool.
        /// </summary>
        public void PlayAudioCue(AudioCueSO _audioCue, AudioConfigSO _settings, Vector3 _position = default)
        {
            if (_audioCue.IsEmpty()) { return; }
            if (_settings.IsExclusive)
            {
                var sameGroup = exclusiveAudioCues.Where(x => x.AudioConfig.OutputAudioMixerGroup == _settings.OutputAudioMixerGroup).ToList();
                if (sameGroup.Count > 0)
                {
                    var removalList = new List<AudioCueWrapper>();
                    for (int i = 0; i < sameGroup.Count; i++)
                    {
                        OnSoundEmitterFinishedPlaying(sameGroup[i].Emitter);
                        removalList.Add(sameGroup[i]);
                    }
                    sameGroup.RemoveAll(x => removalList.Contains(x));
                }
            }

            AudioClip[] clipsToPlay = _audioCue.GetClips();
            int nOfClips = clipsToPlay.Length;

            for (int i = 0; i < nOfClips; i++)
            {
                SoundEmitter soundEmitter = _pool.Request();
                var wrapper = new AudioCueWrapper(_audioCue, _settings, soundEmitter);
                activeSoundEmitters.Add(wrapper);
                if (_settings.IsExclusive)
                {
                    exclusiveAudioCues.Add(wrapper);
                }
                if (soundEmitter != null)
                {
                    soundEmitter.PlayAudioClip(clipsToPlay[i], _settings, _audioCue.looping, _position);
                    if (!_audioCue.looping) { soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying; }
                }
            }
        }

        // ------------------------------------ PRIVATE ------------------------------------ //

        private void Awake()
        {
            activeSoundEmitters = new List<AudioCueWrapper>();
            exclusiveAudioCues = new List<AudioCueWrapper>();

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

    public class AudioCueWrapper
    {
        public AudioCueSO AudioCue;
        public AudioConfigSO AudioConfig;
        public SoundEmitter Emitter;

        public AudioCueWrapper(AudioCueSO cue, AudioConfigSO config, SoundEmitter emitter)
        {
            this.AudioCue = cue;
            this.AudioConfig = config;
            this.Emitter = emitter;
        }
    }
}