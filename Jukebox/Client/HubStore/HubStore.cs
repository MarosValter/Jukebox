using Fluxor;
using Jukebox.Player.Base;
using Jukebox.Player.Manager;
using Jukebox.Shared.Player;
using Jukebox.Shared.Store.Features.Playlist.Actions;
using Jukebox.Shared.Store.Features.Room.Actions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Jukebox.Client.HubStore
{
    public class HubStore : IHubStore
    {
        protected string RoomName { get; private set; }
        protected IDispatcher Dispatcher { get; private set; }
        protected HubConnection HubConnection { get; private set; }
        protected IPlayerManager PlayerManager { get; private set; }

        public HubStore(IPlayerManager playerManager)
        {
            PlayerManager = playerManager;
        }

        public async Task Initialize(IDispatcher dispatcher, Uri hubUrl, string roomName, string userName)
        {
            RoomName = roomName;
            Dispatcher = dispatcher;

            PlayerManager.SongElapsedChanged += OnSongElapsedChanged;

            HubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .AddJsonProtocol(options => options.PayloadSerializerOptions.Converters.Add(new TimeSpanConverter()))
            .Build();

            HubConnection.On<UserInfo>("UserAdded", UserAdded);
            HubConnection.On<string>("UserRemoved", UserRemoved);
            HubConnection.On<SongInfo>("SongAdded", SongAdded);
            HubConnection.On<SongInfo>("SongRemoved", SongRemoved);
            HubConnection.On<SongInfo>("SongChanged", SongChanged);
            HubConnection.On<PlayerState, PlayerType>("PlayerStateChanged", PlayerStateChanged);

            HubConnection.Closed += OnConnectionClosed;

            await HubConnection.StartAsync();

            await EneterRoom(userName);
        }

        public async Task AddSong(SongInfo song)
        {
            await HubConnection.SendAsync("AddSong", RoomName, song);
        }

        public async Task RemoveSong(SongInfo song)
        {
            await HubConnection.SendAsync("RemoveSong", RoomName, song);
        }

        public async Task PreviousSong()
        {
            await HubConnection.SendAsync("PreviousSong", RoomName);
        }

        public async Task NextSong()
        {
            await HubConnection.SendAsync("NextSong", RoomName);
        }

        public async Task ToggleSong()
        {
            await HubConnection.SendAsync("ToggleSong", RoomName);
        }

        public async Task ChangeSong(SongInfo song)
        {
            await HubConnection.SendAsync("ChangeSong", RoomName, song);
        }

        public async Task ChangeSongElapsed(TimeSpan elapsed)
        {
            //await HubConnection.SendAsync("")
        }

        public Task ToggleMute(bool muted, PlayerType type)
        {
            Dispatcher.Dispatch(new MuteChangedAction(muted, type));
            return Task.CompletedTask;
        }


        private async Task EneterRoom(string userName)
        {
            try
            {
                var result = await HubConnection.InvokeAsync<RoomEnteredResult>("EnterRoom", RoomName, userName);
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

        private void SongRemoved(SongInfo song)
        {
            Dispatcher.Dispatch(new SongRemovedAction(song));
        }

        private void SongChanged(SongInfo song)
        {
            Dispatcher.Dispatch(new SongChangedAction(song));
            //Dispatcher.Dispatch(new StateChangedAction(PlayerState.Playing, song.Type));
        }

        private void PlayerStateChanged(PlayerState state, PlayerType type)
        {
            Dispatcher.Dispatch(new StateChangedAction(state, type));
        }

        private async Task OnConnectionClosed(Exception e)
        {

        }

        private void OnSongElapsedChanged(object sender, TimeSpan elapsed)
        {
            Dispatcher.Dispatch(new ElapsedChangedAction(elapsed));
        }

        public async ValueTask DisposeAsync()
        {
            PlayerManager.SongElapsedChanged -= OnSongElapsedChanged;
            HubConnection.Closed -= OnConnectionClosed;
            await HubConnection.DisposeAsync();
        }
    }
}
