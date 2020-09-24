using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Timers;
using Fluxor.Extensions;
using Jukebox.Player.Base;
using Jukebox.Player.Manager;
using Jukebox.Player.YouTube.Player;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Timer = System.Timers.Timer;

namespace Jukebox.Client.Manager
{
    public class PlayerManager : IPlayerManager, IDisposable
    {
        private readonly SpinLock _spinLock = new SpinLock();

        private readonly TimeSpan _elapsedRefreshRate = TimeSpan.FromMilliseconds(500);
        private readonly Timer _elapsedTimer;

        private readonly IJSRuntime _jsRuntime;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<PlayerManager> _logger;

        private readonly ConcurrentDictionary<PlayerType, IPlayer> _players = new ConcurrentDictionary<PlayerType, IPlayer>();

        private IPlayer _activePlayer;

        public event EventHandler<TimeSpan> SongElapsedChanged;

        public IPlayer ActivePlayer
        {
            get => _activePlayer;
            set
            {
                if (value == _activePlayer)
                {
                    return;
                }

                if (_activePlayer != null)
                {
                    _activePlayer.StateChanged -= PlayerStateChanged;
                }

                if (value != null)
                {
                    value.StateChanged += PlayerStateChanged;
                }

                var playing = _activePlayer?.State == PlayerState.Playing;
                _activePlayer?.Pause();
                _activePlayer = value;
                if (playing && _activePlayer != null)
                {
                    _activePlayer.Play();
                }

                _logger.LogInformation("Active player changed to {0}.", value.Type);
            }
        }

        public PlayerManager(IJSRuntime jsRuntime, ILoggerFactory loggerFactory)
        {
            _jsRuntime = jsRuntime;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<PlayerManager>();

            _elapsedTimer = new Timer(_elapsedRefreshRate.Milliseconds);
            _elapsedTimer.AutoReset = true;
            _elapsedTimer.Elapsed += OnElapsedTimer;
        }

        public IPlayer GetPlayer(PlayerType type)
        {
            _spinLock.ExecuteLocked(() =>
            {
                ActivePlayer = _players.GetOrAdd(type, CreatePlayer);
            });
            return ActivePlayer;
        }

        public IPlayer CreatePlayer(PlayerType type)
        {
            var player = type switch
            {
                PlayerType.YouTube => new YouTubePlayer(_jsRuntime, _loggerFactory.CreateLogger<YouTubePlayer>()),
                PlayerType.Spotify => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "")
            };

            return player;
        }

        private async void OnElapsedTimer(object sender, ElapsedEventArgs args)
        {
            try
            {
                await _spinLock.ExecuteLockedAsync(async () => {
                    if (!(await ActivePlayer.IsReady()))
                    {
                        return;
                    }
                    var elapsed = await ActivePlayer.GetElapsedTime();
                    SongElapsedChanged?.Invoke(ActivePlayer, elapsed);
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PlayerManager elapsed timer fail.");
            }
        }

        private void PlayerStateChanged(object sender, PlayerState state)
        {
            _elapsedTimer.Enabled = state == PlayerState.Playing;
            _logger.LogInformation("Player state changed to {0}.", state);
        }

        public void Dispose()
        {
            _spinLock.ExecuteLocked(() =>
            {
                ActivePlayer = null;
            });

            _elapsedTimer.Enabled = false;
            _elapsedTimer.Elapsed -= OnElapsedTimer;
            _elapsedTimer.Dispose();
        }
    }
}
