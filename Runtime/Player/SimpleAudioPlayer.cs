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
    public class SimpleAudioPlayer : AudioPlayerBase, IFactory<IAudioPlayer>
    {
        [SerializeField] protected AudioMixerGroup group = default;
        [SerializeField] protected bool useSharedPool = default;
        [ConditionalField("useSharedPool")]
        [SerializeField] protected SoundEmitterPoolSO sharedPool = default;
        [ConditionalField("useSharedPool", true)]
        [SerializeField] protected int prewarmAmount = 1;

        private IAudioPlayer player = default;
        private IAudioConfig config = default;

        protected override IAudioPlayer Player => player;
        protected override IAudioConfig Config => config;
        private IPool<IAudioPlayer> Pool { get; set; }

        protected override void OnStarted()
        {
            config = config = new AudioConfig(group);
            Pool = useSharedPool
                ? this.sharedPool
                : CreateDedicatedPool();
        }

        protected override void OnDestroyed()
        {
            if (player != null && player.IsPlaying) { Stop(); }
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
            player = Pool.Request();
            player.OnFinish += Finish;            
        }

        private void ReleaseAudioPlayer()
        {
            player.OnFinish -= Finish;
            Pool.Return(player);
            player = null;
        }

        public IAudioPlayer Create()
        {
            GameObject newObj = new GameObject(nameof(SoundEmitter));
            newObj.transform.SetParent(transform);
            return newObj.AddComponent<SoundEmitter>();
        }

        private IPool<IAudioPlayer> CreateDedicatedPool()
        {
            var pool = new ComponentPool<IAudioPlayer>(this, transform);
            if (prewarmAmount > 0) { pool.Prewarm(prewarmAmount); }
            return pool;
        }
    }
}