using System.Threading.Tasks;
using Jukebox.Player;
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

        public async Task EnterRoom(string roomName, string userName)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            var user = new UserInfo {Name = userName, ConnectionId = Context.ConnectionId};
            var result = await _roomStorage.AddUserAsync(roomName, user);

            if (result)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                await Clients.Caller.RoomEntered(Context.ConnectionId, room);
                await Clients.OthersInGroup(roomName).UserAdded(user);
            }
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

        public async Task AddSong(string roomName, SongInfo song)
        {
            await _roomStorage.AddSongAsync(roomName, song);
            await Clients.Group(roomName).SongAdded(song);
        }

        public async Task RemoveSong(string roomName, SongInfo song)
        {
            if (await _roomStorage.RemoveSongAsync(roomName, song))
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
                await Clients.Groups(roomName).PlayerStateChanged(PlayerState.Playing);
            }
        }

        public async Task PreviousSong(string roomName)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            var result = await _playlistManager.PreviousSong(room.Playlist);
            if (result != null)
            {
                await Clients.Group(roomName).SongChanged(result);
                await Clients.Groups(roomName).PlayerStateChanged(PlayerState.Playing);
            }
        }

        public async Task ToggleSong(string roomName, bool play)
        {
            var room = await _roomStorage.GetOrCreateRoomAsync(roomName);
            var result = await _playlistManager.ToggleSong(room.Playlist, play);
            await Clients.Group(roomName).PlayerStateChanged(result);
        }
    }
}
