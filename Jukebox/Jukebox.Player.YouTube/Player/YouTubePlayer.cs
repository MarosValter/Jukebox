using System;
using System.Threading.Tasks;
using Jukebox.Player.Base;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Jukebox.Player.YouTube.Player
{
    public class YouTubePlayer : PlayerBase
    {
        public const string PlayerName = "ytPlayer";

        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<YouTubePlayer> _logger;

        public YouTubePlayer(IJSRuntime jsRuntime, ILogger<YouTubePlayer> logger)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        public override PlayerType Type { get; } = PlayerType.YouTube;

        public override async Task Initialize(string element)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.createPlayer", element);
        }

        public override async Task<bool> IsReady()
        {
            return await _jsRuntime.InvokeAsync<bool>($"{PlayerName}.isReady");
        }

        public override async Task PlayInternal()
        {
            _logger.LogInformation("Play");
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.playVideo");
        }

        public override async Task PauseInternal()
        {
            _logger.LogInformation("Pause");
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.pauseVideo");
        }

        public override async Task Next()
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.nextVideo");
        }

        public override async Task Previous()
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.previousVideo");
        }

        public override async Task SeekTo(double seconds)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.seekTo", seconds);
        }

        public override async Task SetVolume(int value)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.setVolume", value);
        }

        public override async Task<int> GetVolume()
        {
            return await _jsRuntime.InvokeAsync<int>($"{PlayerName}.player.getVolume");
        }

        public override async Task Mute()
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.mute");
        }

        public override async Task UnMute()
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.unMute");
        }

        public override async Task<bool> IsMuted()
        {
            return await _jsRuntime.InvokeAsync<bool>($"{PlayerName}.player.isMuted");
        }

        public override async Task<TimeSpan> GetElapsedTime()
        {
            return TimeSpan.FromSeconds(await _jsRuntime.InvokeAsync<double>($"{PlayerName}.player.getCurrentTime"));
        }

        public override async Task<TimeSpan> GetDuration()
        {
            return TimeSpan.FromSeconds(await _jsRuntime.InvokeAsync<double>($"{PlayerName}.player.getDuration"));
        }

        public override async Task QueueMediaById(string id)
        {
            _logger.LogInformation("Queue new video: {0}", id);
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.cueVideoById", id);
        }

        public override async Task QueueMediaByUrl(string url)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.cueVideoByUrl", url);
        }
    }
}
