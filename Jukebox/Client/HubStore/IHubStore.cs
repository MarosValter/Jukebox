using Fluxor;
using Jukebox.Player.Base;
using Jukebox.Shared.Player;
using System;
using System.Threading.Tasks;

namespace Jukebox.Client.HubStore
{
    public interface IHubStore : IAsyncDisposable
    {
        Task Initialize(IDispatcher dispatcher, Uri hubUrl, string roomName, string userName);
        Task AddMessage(ChatMessageInfo message);
        Task AddSong(SongInfo song);
        Task RemoveSong(SongInfo song);
        Task NextSong();
        Task PreviousSong();
        Task ToggleSong();
        Task ChangeSong(SongInfo song);
        Task ChangeSongElapsed(TimeSpan elapsed);
        Task ToggleMute(bool muted, PlayerType type);
        Task ChangeVolume(int volume, PlayerType type);
    }
}