using Jukebox.Player.Base;

namespace Jukebox.Shared.Store.Features.Playlist.Actions
{
    public class StateChangedAction
    {
        public PlayerState State { get; }

        public PlayerType Type { get; set; }

        public StateChangedAction(PlayerState state, PlayerType type)
        {
            State = state;
            Type = type;
        }
    }
}
