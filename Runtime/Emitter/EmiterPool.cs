using Elysium.Core;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "EmitterPoolSO", menuName = "Scriptable Objects/Audio/Emitter Pool")]
    public class EmiterPool : ComponentPoolSO<Emitter>
    {
        [SerializeField] private EmitterFactorySO _factory;

        public override IFactory<Emitter> Factory
        {
            get
            {
                return _factory;
            }
            set
            {
                _factory = value as EmitterFactorySO;
            }
        }
    }
}
