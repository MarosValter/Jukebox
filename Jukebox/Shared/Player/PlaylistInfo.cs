using System;
using System.Collections.Generic;
using System.Text;

namespace Jukebox.Shared.Player
{
    public class PlaylistInfo
    {
        public IList<SongInfo> Songs { get; set; } = new List<SongInfo>();
    }
}
