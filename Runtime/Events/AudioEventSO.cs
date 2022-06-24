using Elysium.Core;
using UnityEngine;

namespace Elysium.Audio
{
    [CreateAssetMenu(fileName = "AudioEventSO_",menuName = "Scriptable Objects/Event/Audio Event")]
    public class AudioEventSO : GenericEventSO<IAudioCue, IAudioConfig>, IAudioEvent
    {

    }
}
