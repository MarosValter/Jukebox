using System.Threading.Tasks;
using Jukebox.Player;
using Jukebox.Shared.Player;

namespace Jukebox.Shared.Client
{
    public interface IJukeboxClient// : IPlayer
    {
        Task RoomEntered(string connectionId, RoomInfo room);

        Task UserAdded(UserInfo user);
        Task UserRemoved(string connectionId);

        Task SongAdded(SongInfo song);
        Task SongRemoved(SongInfo song);
    }
}
