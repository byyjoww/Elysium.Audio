using Elysium.Core;
using Elysium.Core.Attributes;
using Elysium.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Elysium.Audio
{
    public class SimpleAudioPlayer : AudioPlayerBase
    {
        [SerializeField] protected AudioMixerGroup group = default;
        [SerializeField] protected bool useSharedPool = default;
        [ConditionalField("useSharedPool")]
        [SerializeField] protected SoundEmitterPoolSO pool = default;

        private IAudioPlayer player = default;
        private IAudioConfig config = default;

        protected override IAudioPlayer Player => player;
        protected override IAudioConfig Config => config;

        protected override void OnStarted()
        {
            config = config = new AudioConfig(group);
            if (!useSharedPool)
            {
                player = TryGetComponent(out SoundEmitter _emitter) 
                    ? _emitter
                    : gameObject.AddComponent<SoundEmitter>();
            }
        }

        protected override void OnDestroyed()
        {
            if (player != null && player.IsPlaying) { Stop(); }
        }

        protected override void OnClipStartedPlaying(AudioClip _clip, IAudioConfig _settings, bool _loop)
        {
            if (useSharedPool) { RequestAudioPlayer(); }
            base.OnClipStartedPlaying(_clip, _settings, _loop);
        }

        protected override void OnClipStoppedPlaying()
        {
            base.OnClipStoppedPlaying();
            if (useSharedPool) { ReleaseAudioPlayer(); }            
        }

        protected override void OnClipFinished()
        {
            base.OnClipFinished();
            if (useSharedPool) { ReleaseAudioPlayer(); }            
        }

        private void RequestAudioPlayer()
        {
            player = pool.Request();
            player.OnFinish += Finish;            
        }

        private void ReleaseAudioPlayer()
        {
            player.OnFinish -= Finish;
            pool.Return(player);
            player = null;
        }
    }
}