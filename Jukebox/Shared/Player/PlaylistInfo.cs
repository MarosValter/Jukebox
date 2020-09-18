using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jukebox.Shared.Player
{
    public class PlaylistInfo
    {

        public SongInfo CurrentSong { get; set; }
        public IReadOnlyCollection<SongInfo> PreviousSongs => CurrentSong == null ? new ReadOnlyCollection<SongInfo>(new List<SongInfo>()) : AllSongs.Take(AllSongs.FindIndex(x => x.Id == CurrentSong.Id)).ToList().AsReadOnly();
        public IReadOnlyCollection<SongInfo> NextSongs => CurrentSong == null ? AllSongs.AsReadOnly() : AllSongs.Skip(AllSongs.FindIndex(x => x.Id == CurrentSong.Id) + 1).ToList().AsReadOnly();
        public List<SongInfo> AllSongs { get; set; } = new List<SongInfo>();
    }
}
