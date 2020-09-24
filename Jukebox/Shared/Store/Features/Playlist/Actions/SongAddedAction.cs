using Jukebox.Shared.Player;

namespace Jukebox.Shared.Store.Features.Playlist.Actions
{
    public class SongAddedAction
    {
        public SongInfo Song { get; }

        public SongAddedAction(SongInfo song)
        {
            Song = song;
        }
    }
}
