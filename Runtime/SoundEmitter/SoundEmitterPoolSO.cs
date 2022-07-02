using Elysium.Core;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "SoundEmitterPoolSO", menuName = "Scriptable Objects/Audio/Sound Emitter Pool")]
    public class SoundEmitterPoolSO : ComponentPoolSO<IAudioPlayer>, IFactory<IAudioPlayer>
    {
        [SerializeField] private SoundEmitter emitter = default;

        protected override IFactory<IAudioPlayer> factory => this;

        IAudioPlayer IFactory<IAudioPlayer>.Create()
        {
            return GameObject.Instantiate(emitter);
        }
    }
}