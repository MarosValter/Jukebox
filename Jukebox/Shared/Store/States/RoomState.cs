using Jukebox.Shared.Player;
using System.Collections.Generic;

namespace Jukebox.Shared.Store.States
{
    public class RoomState
    {
        public string Name { get; }
        public string ConnectionId { get; }

        public UserInfo CurrentUser { get; }
        public IList<UserInfo> Users { get; }

        public RoomState(string name, string connectionId, UserInfo user, IList<UserInfo> users)
        {
            Name = name;
            ConnectionId = connectionId;
            CurrentUser = user;
            Users = users ?? new List<UserInfo>();
        }

        public RoomState(string connectionId, RoomInfo room) : this(room?.Name, connectionId, room?.CurrentUser, room?.Users) { }
    }
}
