using Jukebox.Shared.Player;

namespace Jukebox.Shared.Store.Features.Playlist.Actions
{
    public class SongRemovedAction
    {
        public SongInfo Song { get; }

        public SongRemovedAction(SongInfo song)
        {
            Song = song;
        }
    }
}
