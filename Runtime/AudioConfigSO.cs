using UnityEngine;
using UnityEngine.Audio;

namespace Elysium.Audio
{
    //TODO: Check which settings we really need at this level
    [CreateAssetMenu(fileName = "AudioConfigSO_", menuName = "Scriptable Objects/Audio/Audio Configuration")]
    public class AudioConfigSO : ScriptableObject
    {
        public AudioMixerGroup OutputAudioMixerGroup = null;

        // Simplified management of priority levels (values are counterintuitive, see enum below)
        [SerializeField] private PriorityLevel _priorityLevel = PriorityLevel.Standard;
        
        public int Priority
        {
            get { return (int)_priorityLevel; }
            set { _priorityLevel = (PriorityLevel)value; }
        }

        [Header("Sound properties")]
        public bool Mute = false;
        public bool IsExclusive = false;
        [Range(0f, 1f)] public float Volume = 1f;
        [Range(-3f, 3f)] public float Pitch = 1f;
        [Range(-1f, 1f)] public float PanStereo = 0f;
        [Range(0f, 1.1f)] public float ReverbZoneMix = 1f;

        [Header("Spatialisation")]
        [Range(0f, 1f)] public float SpatialBlend = 0f;
        public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
        [Range(0.1f, 5f)] public float MinDistance = 0.1f;
        [Range(5f, 100f)] public float MaxDistance = 50f;
        [Range(0, 360)] public int Spread = 0;
        [Range(0f, 5f)] public float DopplerLevel = 1f;

        [Header("Ignores")]
        public bool BypassEffects = false;
        public bool BypassListenerEffects = false;
        public bool BypassReverbZones = false;
        public bool IgnoreListenerVolume = false;
        public bool IgnoreListenerPause = false;

        private enum PriorityLevel
        {
            Highest = 0,
            High = 64,
            Standard = 128,
            Low = 194,
            VeryLow = 256,
        }

        public void ApplyTo(AudioSource _audioSource)
        {
            _audioSource.outputAudioMixerGroup = OutputAudioMixerGroup;
            _audioSource.mute = Mute;
            _audioSource.bypassEffects = BypassEffects;
            _audioSource.bypassListenerEffects = BypassListenerEffects;
            _audioSource.bypassReverbZones = BypassReverbZones;
            _audioSource.priority = Priority;
            _audioSource.volume = Volume;
            _audioSource.pitch = Pitch;
            _audioSource.panStereo = PanStereo;
            _audioSource.spatialBlend = SpatialBlend;
            _audioSource.reverbZoneMix = ReverbZoneMix;
            _audioSource.dopplerLevel = DopplerLevel;
            _audioSource.spread = Spread;
            _audioSource.rolloffMode = RolloffMode;
            _audioSource.minDistance = MinDistance;
            _audioSource.maxDistance = MaxDistance;
            _audioSource.ignoreListenerVolume = IgnoreListenerVolume;
            _audioSource.ignoreListenerPause = IgnoreListenerPause;
        }
    }
}