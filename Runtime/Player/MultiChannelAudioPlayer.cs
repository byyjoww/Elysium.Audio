using Elysium.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Audio
{
    public class MultiChannelAudioPlayer : MonoBehaviour
    {
        [SerializeField] private bool enableLogging = default;
        [SerializeField] private OpenChannelEventSO openChannelEvent = default;
        [SerializeField] private SoundEmitterPoolSO pool = default;
        private UnityLogger logger = new UnityLogger();
        private Dictionary<IAudioChannelInternal, List<IAudioPlayer>> openAudioChannels = default;
        private Dictionary<IAudioPlayer, UnityAction> playerStopEvents = default;
        private Dictionary<IAudioPlayer, UnityAction> playerFinishEvents = default;

        public void Awake()
        {
            logger.logEnabled = enableLogging;
            openAudioChannels = new Dictionary<IAudioChannelInternal, List<IAudioPlayer>>();
            playerStopEvents = new Dictionary<IAudioPlayer, UnityAction>();
            playerFinishEvents = new Dictionary<IAudioPlayer, UnityAction>();
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
            openAudioChannels.Add(_channel, new List<IAudioPlayer>());
            _channel.OnPlay += (_clip, _settings, _loop) => Play(_channel, _clip, _settings, _loop);
            _channel.OnClose += delegate { OnChannelClosed(_channel); };
        }

        private void OnChannelClosed(IAudioChannelInternal _channel)
        {
            List<IAudioPlayer> players = openAudioChannels[_channel];
            ReleaseAudioPlayersForChannel(_channel, players.ToArray());
            openAudioChannels.Remove(_channel);
        }

        private void Play(IAudioChannelInternal _channel, AudioClip _clip, IAudioConfig _settings, bool _loop)
        {
            IAudioPlayer player = RequestAudioPlayerForChannel(_channel);
            player.Play(_clip, _settings, _loop);
        }

        private void Stop(IAudioChannelInternal _channel, IAudioPlayer _player)
        {
            _player.Stop();
            ReleaseAudioPlayersForChannel(_channel, _player);
        }

        private void Finish(IAudioChannelInternal _channel, IAudioPlayer _player)
        {
            _channel.NotifyTrackFinished();
            if (!_player.IsLooping) { ReleaseAudioPlayersForChannel(_channel, _player); }
        }

        private IAudioPlayer RequestAudioPlayerForChannel(IAudioChannelInternal _channel)
        {
            IAudioPlayer player = pool.Request();
            openAudioChannels[_channel].Add(player);

            _channel.OnPause += player.Pause;
            _channel.OnResume += player.Resume;

            // TODO: Figure out some better way to handle this
            UnityAction stopFunc = () => Stop(_channel, player);
            UnityAction finishFunc = () => Finish(_channel, player);
            playerStopEvents[player] = stopFunc;
            playerFinishEvents[player] = finishFunc;
            _channel.OnStop += stopFunc;
            player.OnFinish += finishFunc;

            _channel.LogEvents();
            return player;
        }

        private void ReleaseAudioPlayersForChannel(IAudioChannelInternal _channel, params IAudioPlayer[] _players)
        {
            for (int i = _players.Length; i-- > 0;)
            {
                IAudioPlayer player = _players[i];
                _channel.OnPause -= player.Pause;
                _channel.OnResume -= player.Resume;

                // TODO: Figure out some better way to handle this
                UnityAction stopFunc = playerStopEvents[player];
                UnityAction finishFunc = playerFinishEvents[player];
                _channel.OnStop -= stopFunc;
                player.OnFinish -= finishFunc;
              
                _channel.LogEvents();
                openAudioChannels[_channel].Remove(player);
                pool.Return(_players);
            }
        }
    }
}