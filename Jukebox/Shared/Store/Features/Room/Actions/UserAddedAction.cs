using Jukebox.Shared.Player;

namespace Jukebox.Shared.Store.Features.Room.Actions
{
    public class UserAddedAction
    {
        public UserInfo User { get; }

        public UserAddedAction(UserInfo user)
        {
            User = user;
        }
    }
}
