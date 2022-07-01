using UnityEngine;

namespace Elysium.Audio
{
    public static class AudioSourceExtensions
    {
        public static bool IsReversePitch(this AudioSource _source)
        {
            return _source.pitch < 0f;
        }

        public static float GetClipRemainingTime(this AudioSource _source)
        {
            // Calculate the remainingTime of the given AudioSource, if we keep playing with the same pitch.
            float remainingTime = (_source.clip.length - _source.time) / _source.pitch;
            return _source.IsReversePitch() ? (_source.clip.length + remainingTime) : remainingTime;
        }
    }
}