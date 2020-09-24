using System.Linq;
using System.Threading.Tasks;
using Jukebox.Player.Base;
using Jukebox.Shared.Player;

namespace Jukebox.Server.PlaylistManager
{
    public class PlaylistManager : IPlaylistManager
    {
        public Task<bool> AddSong(PlaylistInfo playlist, SongInfo song)
        {
            bool result = false;
            lock (playlist)
            {
                playlist.AllSongs.Add(song);
                if (playlist.CurrentSong == null)
                {
                    playlist.CurrentSong = song;
                    result = true;
                }
            }

            return Task.FromResult(result);
        }

        public Task<bool> RemoveSong(PlaylistInfo playlist, SongInfo song)
        {
            bool result;
            if (playlist.CurrentSong.Equals(song))
            {
                result = false;
            }
            else
            {
                playlist.AllSongs.RemoveAt(playlist.AllSongs.FindIndex(s => s.Equals(song)));
                result = true;
            }

            return Task.FromResult(result);
        }

        public Task<SongInfo> NextSong(PlaylistInfo playlist)
        {
            SongInfo result = null;
            lock (playlist)
            {
                var next = playlist.NextSongs.FirstOrDefault();
                if (next != null)
                {
                    playlist.CurrentSong = next;
                    result = next;
                }
            }

            return Task.FromResult(result);
        }

        public Task<SongInfo> PreviousSong(PlaylistInfo playlist)
        {
            SongInfo result = null;
            lock (playlist)
            {
                var prev = playlist.PreviousSongs.LastOrDefault();
                if (prev != null)
                {
                    playlist.CurrentSong = prev;
                    result = prev;
                }
            }

            return Task.FromResult(result);
        }

        public Task<PlayerState> ToggleSong(PlaylistInfo playlistInfo)
        {
            PlayerState result;
            if (!playlistInfo.IsPlaying)
            {
                playlistInfo.IsPlaying = true;
                result = PlayerState.Playing;
            }
            else
            {
                playlistInfo.IsPlaying = false;
                result = PlayerState.Paused;
            }

            return Task.FromResult(result);
        }

        public Task ChangeSong(PlaylistInfo playlist, SongInfo song)
        {
            lock (playlist)
            {
                var s = playlist.AllSongs.FirstOrDefault(s => s.Equals(song));
                if (s != null)
                {
                    playlist.CurrentSong = s;
                }
            }

            return Task.CompletedTask;
        }
    }
}