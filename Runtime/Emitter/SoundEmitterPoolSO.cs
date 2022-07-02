using Elysium.Core;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "SoundEmitterPoolSO", menuName = "Scriptable Objects/Audio/Sound Emitter Pool")]
    public class SoundEmitterPoolSO : ComponentPoolSO<IAudioEmitter>, IFactory<IAudioEmitter>
    {
        [SerializeField] private SoundEmitter emitter = default;

        protected override IFactory<IAudioEmitter> factory => this;

        IAudioEmitter IFactory<IAudioEmitter>.Create()
        {
            return GameObject.Instantiate(emitter);
        }
    }
}