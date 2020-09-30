using Fluxor;
using Jukebox.Player.Base;
using Jukebox.Player.Manager;
using Jukebox.Shared.Player;
using Jukebox.Shared.Serializer;
using Jukebox.Shared.Store.Features.Playlist.Actions;
using Jukebox.Shared.Store.Features.Room.Actions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Jukebox.Client.HubStore
{
    public class HubStore : IHubStore
    {
        private int _reconnectInterval;
        private bool _reconnecting;
        private readonly Timer _reconnectTimer;
        private readonly ILogger<HubStore> _logger;

        protected string RoomName { get; private set; }
        protected string UserName { get; private set; }

        protected IDispatcher Dispatcher { get; private set; }
        protected HubConnection HubConnection { get; private set; }
        protected IPlayerManager PlayerManager { get; private set; }

        public event Func<HubConnectionState, Task> StateChanged;

        public HubConnectionState State => HubConnection.State;

        public HubStore(IPlayerManager playerManager, ILogger<HubStore> logger)
        {
            PlayerManager = playerManager;
            _logger = logger;
            _reconnectTimer = new Timer();
            
        }

        public async Task Initialize(IDispatcher dispatcher, HubStoreConfig config)
        {
            Dispatcher = dispatcher;
            RoomName = config.RoomName;
            UserName = config.UserName;

            _reconnectInterval = config.ReconnectInterval;
            _reconnectTimer.Interval = config.ReconnectInterval;
            _reconnectTimer.Elapsed += TryReconnect;

            PlayerManager.SongElapsedChanged += OnSongElapsedChanged;

            HubConnection = new HubConnectionBuilder()
            .WithUrl(config.HubUrl)
            .WithAutomaticReconnect(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(10),
                //TimeSpan.FromSeconds(30),
                //TimeSpan.FromSeconds(60),
            })
            .AddJsonProtocol(options => SerializerOptions.ConfigureOptions(options.PayloadSerializerOptions))
            .Build();

            HubConnection.On<UserInfo>("UserAdded", UserAdded);
            HubConnection.On<string>("Connected", Connected);
            HubConnection.On<string>("UserRemoved", UserRemoved);
            HubConnection.On<ChatMessageInfo>("MessageAdded", MessageAdded);
            HubConnection.On<SongInfo>("SongAdded", SongAdded);
            HubConnection.On<SongInfo>("SongRemoved", SongRemoved);
            HubConnection.On<SongInfo>("SongChanged", SongChanged);
            HubConnection.On<PlayerState, PlayerType>("PlayerStateChanged", PlayerStateChanged);
            HubConnection.On<TimeSpan, PlayerType>("SongPositionChanged", SongPositionChanged);

            HubConnection.Closed += OnConnectionClosed;
            HubConnection.Reconnected += OnReconnected;
            HubConnection.Reconnecting += OnReconnecting;

            await HubConnection.StartAsync();
        }

        public async Task AddMessage(ChatMessageInfo message)
        {
            if (State != HubConnectionState.Connected)
            {
                return;
            }

            try
            {
                await HubConnection.SendAsync("AddMessage", RoomName, message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "AddMessage");
            }
        }

        public async Task AddSong(SongInfo song)
        {
            if (State != HubConnectionState.Connected)
            {
                return;
            }

            try
            {
                await HubConnection.SendAsync("AddSong", RoomName, song);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "AddSong");
            }
        }

        public async Task RemoveSong(SongInfo song)
        {
            if (State != HubConnectionState.Connected)
            {
                return;
            }

            try
            {
                await HubConnection.SendAsync("RemoveSong", RoomName, song);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "RemoveSong");
            }
        }

        public async Task PreviousSong()
        {
            if (State != HubConnectionState.Connected)
            {
                return;
            }

            try
            {
                await HubConnection.SendAsync("PreviousSong", RoomName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PreviousSong");
            }
        }

        public async Task NextSong()
        {
            if (State != HubConnectionState.Connected)
            {
                return;
            }

            try
            {
                await HubConnection.SendAsync("NextSong", RoomName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "NextSong");
            }
        }

        public async Task ToggleSong()
        {
            if (State != HubConnectionState.Connected)
            {
                return;
            }

            try
            {
                await HubConnection.SendAsync("ToggleSong", RoomName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ToggleSong");
            }
        }

        public async Task ChangeSong(SongInfo song)
        {
            if (State != HubConnectionState.Connected)
            {
                return;
            }

            try
            {
                await HubConnection.SendAsync("ChangeSong", RoomName, song);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ChangeSong");
            }  
        }

        public async Task ChangeSongElapsed(TimeSpan elapsed)
        {
            if (State != HubConnectionState.Connected)
            {
                return;
            }

            try
            {
                await HubConnection.SendAsync("ChangeSongPosition", RoomName, elapsed);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ChangeSongPosition");
            } 
        }

        public Task ToggleMute(bool muted, PlayerType type)
        {
            Dispatcher.Dispatch(new MuteChangedAction(muted, type));
            return Task.CompletedTask;
        }

        public Task ChangeVolume(int volume, PlayerType type)
        {
            Dispatcher.Dispatch(new VolumeChangedAction(volume, type));
            return Task.CompletedTask;
        }


        private async Task Connected(string connectionId)
        {
            await EnterRoom();

            if (_reconnecting || _reconnectTimer.Enabled)
            {
                _reconnectTimer.Stop();
                _reconnecting = false;

                StateChanged?.Invoke(HubConnection.State);
            }
        }

        private async Task EnterRoom()
        {
            if (State != HubConnectionState.Connected)
            {
                return;
            }

            try
            {
                var result = await HubConnection.InvokeAsync<RoomEnteredResult>("EnterRoom", RoomName, UserName);
                if (result != null)
                {
                    Dispatcher.Dispatch(new RoomEnteredAction(result.ConnectionId, result.Room));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void UserAdded(UserInfo user)
        {
            Dispatcher.Dispatch(new UserAddedAction(user));
        }

        private void UserRemoved(string connectionId)
        {
            Dispatcher.Dispatch(new UserRemovedAction(connectionId));
        }

        private void SongAdded(SongInfo song)
        {
            Dispatcher.Dispatch(new SongAddedAction(song));
        }

        private void MessageAdded(ChatMessageInfo message)
        {
            Dispatcher.Dispatch(new ChatMessageAddedAction(message));
        }

        private void SongRemoved(SongInfo song)
        {
            Dispatcher.Dispatch(new SongRemovedAction(song));
        }

        private void SongChanged(SongInfo song)
        {
            Dispatcher.Dispatch(new SongChangedAction(song));
        }

        private void PlayerStateChanged(PlayerState state, PlayerType type)
        {
            Dispatcher.Dispatch(new StateChangedAction(state, type));
        }

        private void SongPositionChanged(TimeSpan elapsed, PlayerType type)
        {
            Dispatcher.Dispatch(new SeekToAction(elapsed, type));
        }

        private void OnSongElapsedChanged(object sender, TimeSpan elapsed)
        {
            Dispatcher.Dispatch(new ElapsedChangedAction(elapsed));
        }

        private Task OnReconnected(string e)
        {
            _reconnectTimer.Stop();
            _reconnecting = false;

            StateChanged?.Invoke(HubConnection.State);

            _logger.LogInformation("Succesfully reconnected");

            return Task.CompletedTask;
        }

        private Task OnReconnecting(Exception e)
        {
            _reconnecting = true;
            StateChanged?.Invoke(HubConnection.State);           

            if (e != null)
            {
                _logger.LogError(e, "Reconnecting error");
            }
            else
            {
                _logger.LogInformation("Reconnecting..");
            }

            return Task.CompletedTask;
        }

        private Task OnConnectionClosed(Exception e)
        {
            if (_reconnecting || e != null)
            {
                _reconnectTimer.Start();
                _reconnecting = false;
                StateChanged?.Invoke(HubConnection.State);
            }

            if (e != null)
            {
                _logger.LogError(e, "Connection lost");
            }
            else
            {
                _logger.LogInformation("Connection lost");
            }

            return Task.CompletedTask;
        }

        private void TryReconnect(object sender, ElapsedEventArgs e)
        {
            Task.Run(async () =>
            {
                Console.WriteLine("State: {0}", HubConnection.State);

                if (HubConnection.State != HubConnectionState.Disconnected)
                {
                    _reconnectTimer.Stop();
                    return;
                }

                var source = new CancellationTokenSource(_reconnectInterval / 2);
                try
                {
                    await HubConnection.StartAsync(source.Token);
                }
                catch (TaskCanceledException) when (source.IsCancellationRequested)
                {
                    _logger.LogInformation("Reestablish connection timeout");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Reestablish connection error: {0}", e.Message);
                }
                finally
                {
                    source.Dispose();
                }
            });
        }

        public async ValueTask DisposeAsync()
        {
            _reconnectTimer.Elapsed += TryReconnect;
            _reconnectTimer.Stop();
            _reconnectTimer.Dispose();

            PlayerManager.SongElapsedChanged -= OnSongElapsedChanged;

            HubConnection.Closed -= OnConnectionClosed;
            HubConnection.Reconnected -= OnReconnected;
            HubConnection.Reconnecting -= OnReconnecting;
            await HubConnection.DisposeAsync();

            _logger.LogDebug("Disposed");
        }
    }
}
