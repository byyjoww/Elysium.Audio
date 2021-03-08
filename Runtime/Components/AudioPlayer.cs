using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Audio
{

    /// <summary>
    /// Simple implementation of a MonoBehaviour that is able to request a sound being played by the <c>AudioManager</c>.
    /// It fires an event on an <c>AudioCueEventSO</c> which acts as a channel, that the <c>AudioManager</c> will pick up and play.
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        [Header("Sound definition")]
        [SerializeField] private AudioCueSO _audioCue = default;
        [SerializeField] private bool _playOnStart = false;

        [Header("Configuration")]
        [SerializeField] private AudioChannelSO _audioCueEventChannel = default;
        [SerializeField] private AudioConfigSO _audioConfiguration = default;

        private void Start()
        {
            if (_playOnStart)
                PlayAudioCue();
        }

        [ContextMenu("Play Audio Cue")]
        public void PlayAudioCue()
        {
            _audioCueEventChannel.RaiseEvent(_audioCue, _audioConfiguration, transform.position);
        }
    }
}