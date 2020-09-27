using System.Collections.Generic;
using System.Threading.Tasks;
using Jukebox.Shared.Player;

namespace Jukebox.Server.Storage
{
    public interface IRoomStorage
    {
        Task<IList<RoomInfo>> GetUserRooms(string connectionId);
        Task<RoomInfo> GetOrCreateRoomAsync(string roomName);

        Task<bool> AddUserAsync(string roomName, UserInfo user);
        Task<bool> RemoveUserAsync(string roomName, string connectionId);
        Task<bool> AddMessageAsync(string roomName, ChatMessageInfo message);
    }
}
