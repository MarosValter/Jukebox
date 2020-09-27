using System.Collections.Generic;

namespace Jukebox.Shared.Player
{
    public class RoomInfo
    {
        public string Name { get; set; }
        public PlaylistInfo Playlist { get; set; }
        public UserInfo CurrentUser { get; set; }
        public IList<UserInfo> Users { get; set; }
        public IList<ChatMessageInfo> ChatMessages { get; set; }

    }
}
