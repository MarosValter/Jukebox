using System;
using Jukebox.Player;

namespace Jukebox.Shared.Player
{
    public class SongInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public TimeSpan Duration { get; set; }
        public string AddedBy { get; set; }
        public bool IsPlaying { get; set; }
        public PlayerType Type { get; set; }
    }
}
