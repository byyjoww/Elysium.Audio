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
    public class SimpleAudioPlayer : AudioPlayerBase, IFactory<IAudioEmitter>
    {
        [SerializeField] protected AudioMixerGroup group = default;
        [SerializeField] protected bool useSharedPool = default;
        [ConditionalField("useSharedPool")]
        [SerializeField] protected SoundEmitterPoolSO sharedPool = default;
        [ConditionalField("useSharedPool", true)]
        [SerializeField] protected int prewarmAmount = 1;

        private IAudioEmitter emitter = default;
        private IAudioConfig config = default;

        protected override IAudioEmitter Emitter => emitter;
        protected override IAudioConfig Config => config;
        private IPool<IAudioEmitter> Pool { get; set; }

        protected override void OnStarted()
        {
            config = config = new AudioConfig(group);
            Pool = useSharedPool
                ? this.sharedPool
                : CreateDedicatedPool();
        }

        protected override void OnDestroyed()
        {
            if (emitter != null && emitter.IsPlaying) { Stop(); }
        }

        protected override void OnClipStartedPlaying(AudioClip _clip, IAudioConfig _settings, bool _loop)
        {
            RequestAudioPlayer();
            base.OnClipStartedPlaying(_clip, _settings, _loop);
        }

        protected override void OnClipStoppedPlaying()
        {
            base.OnClipStoppedPlaying();
            ReleaseAudioPlayer();
        }

        protected override void OnClipFinished()
        {
            base.OnClipFinished();
            ReleaseAudioPlayer();
        }

        private void RequestAudioPlayer()
        {
            emitter = Pool.Request();
            emitter.OnFinish += Finish;            
        }

        private void ReleaseAudioPlayer()
        {
            emitter.OnFinish -= Finish;
            Pool.Return(emitter);
            emitter = null;
        }

        public IAudioEmitter Create()
        {
            GameObject newObj = new GameObject(nameof(SoundEmitter));
            newObj.transform.SetParent(transform);
            return newObj.AddComponent<SoundEmitter>();
        }

        private IPool<IAudioEmitter> CreateDedicatedPool()
        {
            var pool = new ComponentPool<IAudioEmitter>(this, transform);
            if (prewarmAmount > 0) { pool.Prewarm(prewarmAmount); }
            return pool;
        }

        public override void PlayOneShot(AudioClip _clip, IAudioConfig _settings)
        {
            IAudioEmitter emitter = Pool.Request();
            void OnOneShotFinish()
            {
                emitter.OnFinish -= OnOneShotFinish;
                emitter.OnStop -= OnOneShotFinish;
                Pool.Return(emitter);
            }
            emitter.OnFinish += OnOneShotFinish;
            emitter.OnStop += OnOneShotFinish;
            emitter.Play(_clip, _settings, false);
        }
    }
}