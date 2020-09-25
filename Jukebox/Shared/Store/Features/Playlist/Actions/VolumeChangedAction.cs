using Jukebox.Player.Base;

namespace Jukebox.Shared.Store.Features.Playlist.Actions
{
    public class VolumeChangedAction
    {
        public int Volume { get; }
        public PlayerType Type { get; }

        public VolumeChangedAction(int volume, PlayerType type)
        {
            Volume = volume;
            Type = type;
        }
    }
}
