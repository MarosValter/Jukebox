using System;

namespace Jukebox.Shared.Player
{
    public class SongInfo
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public TimeSpan Duration { get; set; }
        public string AddedBy { get; set; }
        public bool IsPlaying { get; set; }
    }
}
