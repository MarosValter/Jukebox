using Jukebox.Player.Base;

namespace Jukebox.Shared.Store.Features.Playlist.Actions
{
    public class MuteChangedAction
    {
        public bool IsMuted { get; }
        public PlayerType Type { get; }

        public MuteChangedAction(bool isMuted, PlayerType type)
        {
            IsMuted = isMuted;
            Type = type;
        }
    }
}
