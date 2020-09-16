using System.Collections.Generic;

namespace Jukebox.Shared.Player
{
    public class RoomInfo
    {
        public string Name { get; set; }
        public PlaylistInfo Playlist { get; set; }
        public IList<UserInfo> Users { get; set; }
    }
}
