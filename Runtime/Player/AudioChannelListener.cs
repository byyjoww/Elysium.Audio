using Elysium.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public class AudioChannelListener : MonoBehaviour
    {
        [SerializeField] private bool enableLogging = default;
        [SerializeField] private OpenChannelEventSO openChannelEvent = default;
        [SerializeField] private SoundEmitterPoolSO pool = default;

        private IUnityLogger logger = new UnityLogger();
        private Dictionary<IAudioChannelInternal, List<IAudioEmitter>> openAudioChannels = default;
        private Dictionary<IAudioEmitter, UnityAction> emitterStopEvents = default;
        private Dictionary<IAudioEmitter, UnityAction> emitterFinishEvents = default;

        public void Awake()
        {
            logger.logEnabled = enableLogging;
            openAudioChannels = new Dictionary<IAudioChannelInternal, List<IAudioEmitter>>();
            emitterStopEvents = new Dictionary<IAudioEmitter, UnityAction>();
            emitterFinishEvents = new Dictionary<IAudioEmitter, UnityAction>();
            openChannelEvent.OnRaise += OnChannelOpened;
        }

        public void OnDestroy()
        {
            openChannelEvent.OnRaise -= OnChannelOpened;
            for (int i = openAudioChannels.Count; i-- > 0;)
            {
                var kvp = openAudioChannels.ElementAt(i);
                kvp.Key.Close();
            }
            openAudioChannels.Clear();
        }

        internal void OnChannelOpened(IAudioChannelInternal _channel)
        {
            openAudioChannels.Add(_channel, new List<IAudioEmitter>());
            _channel.OnPlay += (_clip, _settings, _loop) => Play(_channel, _clip, _settings, _loop);
            _channel.OnClose += delegate { OnChannelClosed(_channel); };
        }

        private void OnChannelClosed(IAudioChannelInternal _channel)
        {
            List<IAudioEmitter> emitters = openAudioChannels[_channel];
            ReleaseAudioEmittersForChannel(_channel, emitters.ToArray());
            openAudioChannels.Remove(_channel);
        }

        private void Play(IAudioChannelInternal _channel, AudioClip _clip, IAudioConfig _settings, bool _loop)
        {
            IAudioEmitter emitter = RequestAudioEmitterForChannel(_channel);
            emitter.Play(_clip, _settings, _loop);
        }

        private void Stop(IAudioChannelInternal _channel, IAudioEmitter _emitter)
        {
            _emitter.Stop();
            ReleaseAudioEmittersForChannel(_channel, _emitter);
        }

        private void Finish(IAudioChannelInternal _channel, IAudioEmitter _emitter)
        {
            _channel.NotifyTrackFinished();
            if (!_emitter.IsLooping) { ReleaseAudioEmittersForChannel(_channel, _emitter); }
        }

        private IAudioEmitter RequestAudioEmitterForChannel(IAudioChannelInternal _channel)
        {
            IAudioEmitter emitter = pool.Request();
            openAudioChannels[_channel].Add(emitter);

            _channel.OnPause += emitter.Pause;
            _channel.OnResume += emitter.Resume;

            // TODO: Figure out some better way to handle this
            UnityAction stopFunc = () => Stop(_channel, emitter);
            UnityAction finishFunc = () => Finish(_channel, emitter);
            emitterStopEvents[emitter] = stopFunc;
            emitterFinishEvents[emitter] = finishFunc;
            _channel.OnStop += stopFunc;
            emitter.OnFinish += finishFunc;

            return emitter;
        }

        private void ReleaseAudioEmittersForChannel(IAudioChannelInternal _channel, params IAudioEmitter[] _emitters)
        {
            for (int i = _emitters.Length; i-- > 0;)
            {
                IAudioEmitter emitter = _emitters[i];
                _channel.OnPause -= emitter.Pause;
                _channel.OnResume -= emitter.Resume;

                // TODO: Figure out some better way to handle this
                UnityAction stopFunc = emitterStopEvents[emitter];
                UnityAction finishFunc = emitterFinishEvents[emitter];
                _channel.OnStop -= stopFunc;
                emitter.OnFinish -= finishFunc;
              
                openAudioChannels[_channel].Remove(emitter);
                pool.Return(_emitters);
            }
        }
    }
}