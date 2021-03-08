using Elysium.Core;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "NewSoundEmitterFactory", menuName = "Scriptable Objects/Audio/SoundEmitter Factory")]
    public class SoundEmitterFactorySO : FactorySO<SoundEmitter>
    {
        public SoundEmitter prefab = default;

        public override SoundEmitter Create()
        {
            return Instantiate(prefab);
        }
    }
}