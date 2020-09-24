using System.Threading.Tasks;
using Jukebox.Player.Base;
using Jukebox.Shared.Player;

namespace Jukebox.Server.PlaylistManager
{
    public interface IPlaylistManager
    {
        Task<bool> AddSong(PlaylistInfo playlist, SongInfo song);
        Task<bool> RemoveSong(PlaylistInfo playlist, SongInfo song);

        Task<SongInfo> NextSong(PlaylistInfo playlist);
        Task<SongInfo> PreviousSong(PlaylistInfo playlist);

        Task<PlayerState> ToggleSong(PlaylistInfo playlistInfo);
        Task ChangeSong(PlaylistInfo playlist, SongInfo song);
    }
}
