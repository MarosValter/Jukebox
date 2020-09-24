using System;

namespace Jukebox.Shared.Store.Features.Playlist.Actions
{
    public class ElapsedChangedAction
    {
        public TimeSpan Elapsed { get; }

        public ElapsedChangedAction(TimeSpan elapsed)
        {
            Elapsed = elapsed;
        }
    }
}
