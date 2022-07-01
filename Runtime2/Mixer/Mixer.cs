using Elysium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Audio
{
    //// REMEMBER TO EXPOSE THE VARIABLES FOR EACH MIXER CHANNEL IN THE INSPECTOR
    //public class Mixer
    //{
    //    public class Emission
    //    {
    //        public IAudioCue AudioCue;
    //        public IAudioConfig AudioConfig;
    //        public IEmitter Emitter;

    //        public Emission(IAudioCue _cue, IAudioConfig _config, IEmitter _emitter)
    //        {
    //            this.AudioCue = _cue;
    //            this.AudioConfig = _config;
    //            this.Emitter = _emitter;
    //        }
    //    }

    //    private IPool<IEmitter> pool = default;
    //    private int initialSize = 10;
    //    private IMixerConfig config = default;
    //    private List<IAudioEvent> audioEvents = default;
    //    private List<IPositionalAudioEvent> positionalAudioEvents = default;
    //    private IEnumerable<IChannel> channels = default;

    //    private List<Emission> activeEmitters = new List<Emission>();
    //    private List<Emission> exclusiveAudioCues = new List<Emission>();

    //    public IEnumerable<IChannel> Channels => channels;

    //    public Mixer(IMixerConfig _config, IPool<IEmitter> _pool, int _initialSize)
    //    {
    //        this.config = _config;
    //        this.pool = _pool;
    //        this.initialSize = _initialSize;
    //        this.audioEvents = new List<IAudioEvent>();
    //        this.positionalAudioEvents = new List<IPositionalAudioEvent>();

    //        pool.Prewarm(initialSize);
    //        channels = this.config.CreateChannels();
    //    }

    //    public void BindEvents(params IAudioEvent[] _events)
    //    {
    //        this.audioEvents.AddRange(_events);
    //        foreach (var audioEvent in _events)
    //        {
    //            audioEvent.OnRaise += Play;
    //        }
    //    }

    //    public void BindPositionalEvents(params IPositionalAudioEvent[] _events)
    //    {
    //        this.positionalAudioEvents.AddRange(_events);
    //        foreach (var audioEvent in _events)
    //        {
    //            audioEvent.OnRaise += Play;
    //        }
    //    }

    //    private void Play(IAudioCue _cue, IAudioConfig _config)
    //    {
    //        Play(_cue, _config, default);
    //    }

    //    private void Play(IAudioCue _cue, IAudioConfig _config, Vector3 _position)
    //    {
    //        if (_cue.IsEmpty) { return; }
    //        if (_config.Output == null) { return; }
    //        if (_config.IsExclusive)
    //        {
    //            var sameGroup = exclusiveAudioCues.Where(x => x.AudioConfig.Output == _config.Output).ToList();
    //            if (sameGroup.Count > 0)
    //            {
    //                var removalList = new List<Emission>();
    //                for (int i = 0; i < sameGroup.Count; i++)
    //                {
    //                    OnSoundEmitterFinishedPlaying(sameGroup[i].Emitter);
    //                    removalList.Add(sameGroup[i]);
    //                }
    //                sameGroup.RemoveAll(x => removalList.Contains(x));
    //            }
    //        }

    //        AudioClip[] clipsToPlay = _cue.Clips();
    //        int nOfClips = clipsToPlay.Length;

    //        for (int i = 0; i < nOfClips; i++)
    //        {
    //            IEmitter soundEmitter = pool.Request();
    //            var wrapper = new Emission(_cue, _config, soundEmitter);
    //            activeEmitters.Add(wrapper);
    //            if (_config.IsExclusive)
    //            {
    //                exclusiveAudioCues.Add(wrapper);
    //            }
    //            if (soundEmitter != null)
    //            {
    //                soundEmitter.Play(clipsToPlay[i], _config, _cue.Loop);
    //                soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
    //            }
    //        }
    //    }

    //    private void OnSoundEmitterFinishedPlaying(IEmitter _emitter)
    //    {
    //        _emitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
    //        _emitter.Stop();
    //        pool.Return(_emitter);
    //    }

    //    private void UnbindAudioEvents()
    //    {
    //        foreach (var audioEvent in audioEvents)
    //        {
    //            audioEvent.OnRaise -= Play;
    //        }

    //        foreach (var audioEvent in positionalAudioEvents)
    //        {
    //            audioEvent.OnRaise -= Play;
    //        }
    //    }

    //    ~Mixer()
    //    {
    //        UnbindAudioEvents();
    //    }
    //}
}
