using UnityEngine;

namespace Elysium.Audio
{
    [System.Serializable]
    public class MixerGroup
    {
        [SerializeField] private Volume volume = default;

        public Volume Volume => volume;

        public MixerGroup() 
        { 

        }

        public MixerGroup(string _exposedVolumeParameter)
        {
            this.volume = new Volume { ExposedParameter = _exposedVolumeParameter };
        }

        internal void Init()
        {
            volume.Init();
        }

        internal void Terminate()
        {
            volume.Terminate();
        }
    }
}