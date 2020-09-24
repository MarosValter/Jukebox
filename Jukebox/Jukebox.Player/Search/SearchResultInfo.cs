using Jukebox.Player.Base;
using System;

namespace Jukebox.Player.Search
{
    public class SearchResultInfo
    {
        public PlayerType Type { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public int ViewCount { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
