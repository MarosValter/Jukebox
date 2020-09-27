using System;
using System.Threading.Tasks;
using Jukebox.Server.PlaylistManager;
using Jukebox.Server.Storage;
using Jukebox.Shared.Client;
using Jukebox.Shared.Player;
using Microsoft.AspNetCore.SignalR;

namespace Jukebox.Server.Hubs
{
    public class JukeboxHub : Hub<IJukeboxClient>
    {
        private readonly IRoomStorage _roomStorage;
        private readonly IPlaylistManager _playlistManager;

        public JukeboxHub(IRoomStorage roomStorage, IPlaylistManager playlistManager)
        {
            _roomStorage = roomStorage;
            _playlistManager = playlistManager;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var rooms = await _roomStorage.GetUserRooms(Context.ConnectionId);
            foreach(var room in rooms)
            {
                await LeaveRoom(room.Name);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<RoomEnteredResult> EnterRoom(string roomName, string userName)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            var user = new UserInfo {Name = userName, ConnectionId = Context.ConnectionId};
            var result = await _roomStorage.AddUserAsync(roomName, user);

            if (result)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Clients.OthersInGroup(roomName).UserAdded(user);

                room.CurrentUser = user;
                return new RoomEnteredResult
                {
                    ConnectionId = Context.ConnectionId,
                    Room = room
                };
            }

            return null;
        }

        public async Task LeaveRoom(string roomName)
        {
            var result = await _roomStorage.RemoveUserAsync(roomName, Context.ConnectionId);

            if (result)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                await Clients.OthersInGroup(roomName).UserRemoved(Context.ConnectionId);
            }
        }

        public async Task AddMessage(string roomName, ChatMessageInfo message)
        {
            var result = await _roomStorage.AddMessageAsync(roomName, message);
            if (result)
            {
                await Clients.Group(roomName).MessageAdded(message);
            }
        }

        public async Task AddSong(string roomName, SongInfo song)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            var firstSong = await _playlistManager.AddSong(room.Playlist, song);
            await Clients.Group(roomName).SongAdded(song);
            if (firstSong)
            {
                await _playlistManager.ToggleSong(room.Playlist);
                await Clients.Group(roomName).SongChanged(song);
            }
        }

        public async Task RemoveSong(string roomName, SongInfo song)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            if (await _playlistManager.RemoveSong(room.Playlist, song))
            {
                await Clients.Group(roomName).SongRemoved(song);
            }
        }

        public async Task NextSong(string roomName)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            var result = await _playlistManager.NextSong(room.Playlist);
            if (result != null)
            {
                await Clients.Group(roomName).SongChanged(result);
            }
        }

        public async Task PreviousSong(string roomName)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            var result = await _playlistManager.PreviousSong(room.Playlist);
            if (result != null)
            {
                await Clients.Group(roomName).SongChanged(result);
            }
        }

        public async Task ToggleSong(string roomName)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            var result = await _playlistManager.ToggleSong(room.Playlist);
            await Clients.Group(roomName).PlayerStateChanged(result, room.Playlist.CurrentSong.Type);
        }

        public async Task ChangeSong(string roomName, SongInfo song)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            await _playlistManager.ChangeSong(room.Playlist, song);
            await Clients.Group(roomName).SongChanged(song);
        }

        public async Task ChangeSongPosition(string roomName, TimeSpan elapsed)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            //TODO
            await Clients.Group(roomName).SongPositionChanged(elapsed, room.Playlist.CurrentSong.Type);
        }
    }
}
