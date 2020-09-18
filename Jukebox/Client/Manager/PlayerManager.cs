using System;
using Jukebox.Player;
using Jukebox.Player.Manager;
using Jukebox.Player.YouTube.Player;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Jukebox.Client.Manager
{
    public class PlayerManager : IPlayerManager
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILoggerFactory _loggerFactory;

        public PlayerManager(IJSRuntime jsRuntime, ILoggerFactory loggerFactory)
        {
            _jsRuntime = jsRuntime;
            _loggerFactory = loggerFactory;
        }

        public IPlayer GetPlayer(PlayerType type)
        {
            return type switch
            {
                PlayerType.YouTube => new YouTubePlayer(_jsRuntime, _loggerFactory.CreateLogger<YouTubePlayer>()),
                PlayerType.Spotify => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "")
            };
        }
    }
}
