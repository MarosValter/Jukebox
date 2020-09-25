using Jukebox.Player.Base;
using System;

namespace Jukebox.Shared.Store.Features.Playlist.Actions
{
    public class SeekToAction
    {
        public TimeSpan Elapsed { get; }
        public PlayerType Type { get; }

        public SeekToAction(TimeSpan elapsed, PlayerType type)
        {
            Elapsed = elapsed;
            Type = type;
        }
    }
}
