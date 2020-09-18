﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Jukebox.Player.YouTube.Player
{
    public class YouTubePlayer : IPlayer
    {
        public const string PlayerName = "ytPlayer";

        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<YouTubePlayer> _logger;

        public YouTubePlayer(IJSRuntime jsRuntime, ILogger<YouTubePlayer> logger)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        public PlayerType Type { get; } = PlayerType.YouTube;

        public async Task Initialize(string element)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.createPlayer", element);
        }

        public async Task<bool> IsReady()
        {
            return await _jsRuntime.InvokeAsync<bool>($"{PlayerName}.isReady");
        }

        public async Task Play()
        {
            _logger.LogInformation("Play");
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.playVideo");
        }

        public async Task Pause()
        {
            _logger.LogInformation("Pause");
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.pauseVideo");
        }

        public async Task Next()
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.nextVideo");
        }

        public async Task Previous()
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.previousVideo");
        }

        public async Task SeekTo(double seconds)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.seekTo", seconds);
        }

        public async Task SetVolume(int value)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.setVolume", value);
        }

        public async Task<int> GetVolume()
        {
            return await _jsRuntime.InvokeAsync<int>($"{PlayerName}.player.getVolume");
        }

        public async Task Mute()
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.mute");
        }

        public async Task UnMute()
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.unMute");
        }

        public async Task<bool> IsMuted()
        {
            return await _jsRuntime.InvokeAsync<bool>($"{PlayerName}.player.isMuted");
        }

        public async Task<double> GetElapsedTime()
        {
            return await _jsRuntime.InvokeAsync<double>($"{PlayerName}.player.getCurrentTime");
        }

        public async Task<double> GetDuration()
        {
            return await _jsRuntime.InvokeAsync<double>($"{PlayerName}.player.getDuration");
        }

        public async Task QueueMediaById(string id)
        {
            _logger.LogInformation("Queue new video: {0}", id);
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.cueVideoById", id);
        }

        public async Task QueueMediaByUrl(string url)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.cueVideoByUrl", url);
        }
    }
}
