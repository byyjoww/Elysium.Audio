using Elysium.Core;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "PositionalAudioEventSO_", menuName = "Scriptable Objects/Event/Positional Audio Event")]
    public class PositionalAudioEventSO : GenericEventSO<IAudioCue, IAudioConfig, Vector3>, IPositionalAudioEvent
    {

    }
}
