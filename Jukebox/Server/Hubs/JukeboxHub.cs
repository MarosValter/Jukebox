using System.Threading.Tasks;
using Jukebox.Server.Storage;
using Jukebox.Shared.Client;
using Jukebox.Shared.Player;
using Microsoft.AspNetCore.SignalR;

namespace Jukebox.Server.Hubs
{
    public class JukeboxHub : Hub<IJukeboxClient>
    {
        private readonly IRoomStorage _roomStorage;

        public JukeboxHub(IRoomStorage roomStorage)
        {
            _roomStorage = roomStorage;
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
    }
}
