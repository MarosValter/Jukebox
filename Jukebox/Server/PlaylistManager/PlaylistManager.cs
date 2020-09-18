using System.Linq;
using System.Threading.Tasks;
using Jukebox.Player;
using Jukebox.Shared.Player;

namespace Jukebox.Server.PlaylistManager
{
    public class PlaylistManager : IPlaylistManager
    {
        public Task<SongInfo> NextSong(PlaylistInfo playlist)
        {
            SongInfo result = null;
            lock (playlist)
            {
                var next = playlist.NextSongs.FirstOrDefault();
                if (next != null)
                {
                    playlist.CurrentSong.IsPlaying = false;
                    playlist.CurrentSong = next;
                    playlist.CurrentSong.IsPlaying = true;
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
                    playlist.CurrentSong.IsPlaying = false;
                    playlist.CurrentSong = prev;
                    playlist.CurrentSong.IsPlaying = true;
                    result = prev;
                }
            }

            return Task.FromResult(result);
        }

        public Task<PlayerState> ToggleSong(PlaylistInfo playlistInfo, bool play)
        {
            PlayerState result;
            if (play)
            {
                playlistInfo.CurrentSong.IsPlaying = true;
                result = PlayerState.Playing;
            }
            else
            {
                playlistInfo.CurrentSong.IsPlaying = false;
                result = PlayerState.Paused;
            }

            return Task.FromResult(result);
        }
    }
}