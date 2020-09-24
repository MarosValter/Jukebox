using Jukebox.Player.Base;
using System;

namespace Jukebox.Player.Manager
{
    public interface IPlayerManager
    {
        event EventHandler<TimeSpan> SongElapsedChanged;
        IPlayer GetPlayer(PlayerType type);
    }
}
