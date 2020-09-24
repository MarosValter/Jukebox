using Jukebox.Shared.Player;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jukebox.Shared.Store.States
{
    public class PlaylistState
    {
        public bool IsPlaying { get; }
        public DateTime? Started { get; }
        public TimeSpan Elapsed { get; }

        public SongInfo CurrentSong { get; }
        public List<SongInfo> AllSongs { get; }

        public IReadOnlyCollection<SongInfo> PreviousSongs => CurrentSong == null ? new ReadOnlyCollection<SongInfo>(new List<SongInfo>()) : AllSongs.Take(AllSongs.FindIndex(x => x.Id == CurrentSong.Id)).ToList().AsReadOnly();
        public IReadOnlyCollection<SongInfo> NextSongs => CurrentSong == null ? AllSongs.AsReadOnly() : AllSongs.Skip(AllSongs.FindIndex(x => x.Id == CurrentSong.Id) + 1).ToList().AsReadOnly();

        public PlaylistState(List<SongInfo> allSongs, SongInfo currentSong = null, bool isPlaying = false, DateTime? started = default, TimeSpan elapsed = default)
        {
            AllSongs = allSongs ?? new List<SongInfo>();
            CurrentSong = currentSong;
            IsPlaying = currentSong != null && isPlaying;
            Started = started;
            Elapsed = elapsed;
        }

        public PlaylistState(PlaylistInfo playlist) : this(playlist?.AllSongs, playlist?.CurrentSong, playlist?.IsPlaying ?? false, playlist?.Started, playlist?.Elapsed ?? default) { }
    }
}
