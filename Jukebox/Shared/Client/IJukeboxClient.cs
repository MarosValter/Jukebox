using System;
using System.Threading.Tasks;
using Jukebox.Player.Base;
using Jukebox.Shared.Player;

namespace Jukebox.Shared.Client
{
    public interface IJukeboxClient
    {
        Task Connected(string connectionId);
        Task UserAdded(UserInfo user);
        Task UserRemoved(string connectionId);
        Task MessageAdded(ChatMessageInfo message);
        Task SongAdded(SongInfo song);
        Task SongRemoved(SongInfo song);
        Task SongChanged(SongInfo song);
        Task SongPositionChanged(TimeSpan duration, PlayerType type);
        Task PlayerStateChanged(PlayerState state, PlayerType type);
    }
}
