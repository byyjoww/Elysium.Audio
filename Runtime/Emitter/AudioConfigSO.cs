using Elysium.Core;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "AudioConfigSO_", menuName = "Scriptable Objects/Audio/Configuration")]
    public class AudioConfigSO : ScriptableObject, IAudioConfig
    {
        [Header("General")]
        [SerializeField] private AudioMixerGroup output = null;
        [SerializeField] private PriorityLevel priorityLevel = PriorityLevel.Standard;        

        [Header("Sound properties")]
        [SerializeField] private bool mute = false;
        [SerializeField] private bool isExclusive = false;
        [SerializeField][Range(0f, 1f)] private float volume = 1f;
        [SerializeField][Range(-3f, 3f)] private float pitch = 1f;
        [SerializeField][Range(-1f, 1f)] private float panStereo = 0f;
        [SerializeField][Range(0f, 1.1f)] private float reverbZoneMix = 1f;

        [Header("Spatialisation")]
        [SerializeField][Range(0f, 1f)] private float spatialBlend = 0f;
        [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        [SerializeField][Range(0.1f, 5f)] private float minDistance = 0.1f;
        [SerializeField][Range(5f, 100f)] private float maxDistance = 50f;
        [SerializeField][Range(0, 360)] private int spread = 0;
        [SerializeField][Range(0f, 5f)] private float dopplerLevel = 1f;

        [Header("Ignores")]
        [SerializeField] private bool bypassEffects = false;
        [SerializeField] private bool bypassListenerEffects = false;
        [SerializeField] private bool bypassReverbZones = false;
        [SerializeField] private bool ignoreListenerVolume = false;
        [SerializeField] private bool ignoreListenerPause = false;

        private IUnityLogger logger = new UnityLogger();

        public event UnityAction OnValueChanged;

        private int Priority
        {
            get { return (int)priorityLevel; }
            set { priorityLevel = (PriorityLevel)value; }
        }

        private enum PriorityLevel
        {
            Highest = 0,
            High = 64,
            Standard = 128,
            Low = 194,
            VeryLow = 256,
        }

        public AudioMixerGroup Group => output;

        public void ApplyTo(AudioSource _audioSource)
        {
            _audioSource.outputAudioMixerGroup = output;
            _audioSource.mute = mute;
            _audioSource.bypassEffects = bypassEffects;
            _audioSource.bypassListenerEffects = bypassListenerEffects;
            _audioSource.bypassReverbZones = bypassReverbZones;
            _audioSource.priority = Priority;
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;
            _audioSource.panStereo = panStereo;
            _audioSource.spatialBlend = spatialBlend;
            _audioSource.reverbZoneMix = reverbZoneMix;
            _audioSource.dopplerLevel = dopplerLevel;
            _audioSource.spread = spread;
            _audioSource.rolloffMode = rolloffMode;
            _audioSource.minDistance = minDistance;
            _audioSource.maxDistance = maxDistance;
            _audioSource.ignoreListenerVolume = ignoreListenerVolume;
            _audioSource.ignoreListenerPause = ignoreListenerPause;
        }

        [ContextMenu("Apply Changes")]
        private void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        private void OnValidate()
        {
            if (output == null) { logger.LogError($"Audio Config {name} doesn't have an output mixer group assigned."); }
        }
    }
}
