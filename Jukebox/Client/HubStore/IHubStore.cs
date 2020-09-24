using Fluxor;
using Jukebox.Shared.Player;
using System;
using System.Threading.Tasks;

namespace Jukebox.Client.HubStore
{
    public interface IHubStore : IAsyncDisposable
    {
        Task Initialize(IDispatcher dispatcher, Uri hubUrl, string roomName, string userName);
        Task AddSong(SongInfo song);
        Task RemoveSong(SongInfo song);
        Task NextSong();
        Task PreviousSong();
        Task ToggleSong();
        Task ChangeSong(SongInfo song);
        Task ChangeSongPosition(int seconds);
    }
}