using UnityEngine;
using System.Linq;
using Elysium.Core;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "NewSoundEmitterPool", menuName = "Scriptable Objects/Audio/SoundEmitter Pool")]
    public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
    {
        [SerializeField]
        private SoundEmitterFactorySO _factory;

        public override IFactory<SoundEmitter> Factory
        {
            get
            {
                return _factory;
            }
            set
            {
                _factory = value as SoundEmitterFactorySO;
            }
        }
    }
}