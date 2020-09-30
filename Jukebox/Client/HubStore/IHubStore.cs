using Fluxor;
using Jukebox.Player.Base;
using Jukebox.Shared.Player;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Jukebox.Client.HubStore
{
    public interface IHubStore : IAsyncDisposable
    {
        event Func<HubConnectionState, Task> StateChanged;
        HubConnectionState State { get; }

        Task Initialize(IDispatcher dispatcher, HubStoreConfig config);
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