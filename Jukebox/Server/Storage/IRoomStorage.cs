using System.Threading.Tasks;
using Jukebox.Shared.Player;

namespace Jukebox.Server.Storage
{
    public interface IRoomStorage
    {
        Task<RoomInfo> GetOrCreateRoomAsync(string roomName);

        Task<bool> AddUserAsync(string roomName, UserInfo user);
        Task<bool> RemoveUserAsync(string roomName, string connectionId);

        Task AddSongAsync(string roomName, SongInfo song);
        Task RemoveSongAsync(string roomName, SongInfo song);
    }
}
