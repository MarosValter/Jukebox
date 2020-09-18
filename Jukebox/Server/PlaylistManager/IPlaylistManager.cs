using System.Threading.Tasks;
using Jukebox.Player;
using Jukebox.Shared.Player;

namespace Jukebox.Server.PlaylistManager
{
    public interface IPlaylistManager
    {
        Task<SongInfo> NextSong(PlaylistInfo playlist);
        Task<SongInfo> PreviousSong(PlaylistInfo playlist);

        Task<PlayerState> ToggleSong(PlaylistInfo playlistInfo, bool play);
    }
}
