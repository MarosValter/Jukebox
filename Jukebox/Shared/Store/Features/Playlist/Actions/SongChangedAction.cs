using Jukebox.Shared.Player;

namespace Jukebox.Shared.Store.Features.Playlist.Actions
{
    public class SongChangedAction
    {
        public SongInfo Song { get; }

        public SongChangedAction(SongInfo song)
        {
            Song = song;
        }
    }
}
