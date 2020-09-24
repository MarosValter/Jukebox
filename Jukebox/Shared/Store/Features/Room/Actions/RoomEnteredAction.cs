using Jukebox.Shared.Player;

namespace Jukebox.Shared.Store.Features.Room.Actions
{
    public class RoomEnteredAction
    {
        public string ConnectionId { get; }
        public RoomInfo Room { get; }

        public RoomEnteredAction(string connectionId, RoomInfo room)
        {
            ConnectionId = connectionId;
            Room = room;
        }
    }
}
