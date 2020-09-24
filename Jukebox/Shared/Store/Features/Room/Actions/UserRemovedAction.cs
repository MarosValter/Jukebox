namespace Jukebox.Shared.Store.Features.Room.Actions
{
    public class UserRemovedAction
    {
        public string ConnectionId { get; }

        public UserRemovedAction(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}
