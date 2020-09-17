using System;
using Jukebox.Player;
using Jukebox.Player.Manager;
using Jukebox.Player.YouTube.Player;
using Microsoft.JSInterop;

namespace Jukebox.Client.Manager
{
    public class PlayerManager : IPlayerManager
    {
        private readonly IJSRuntime _jsRuntime;

        public PlayerManager(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public IPlayer GetPlayer(PlayerType type)
        {
            return type switch
            {
                PlayerType.YouTube => new YouTubePlayer(_jsRuntime),
                PlayerType.Spotify => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "")
            };
        }
    }
}
