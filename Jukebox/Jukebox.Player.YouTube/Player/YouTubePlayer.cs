using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Jukebox.Player.YouTube.Player
{
    public class YouTubePlayer : IPlayer
    {
        public const string PlayerName = "ytPlayer";
        private readonly IJSRuntime _jsRuntime;

        public YouTubePlayer(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public PlayerType Type { get; } = PlayerType.YouTube;

        public async Task Initialize(string element)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.createPlayer", element);
        }

        public async Task Play()
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.playVideo");
        }

        public async Task Pause()
        {
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
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.cueVideoById", id);
        }

        public async Task QueueMediaByUrl(string url)
        {
            await _jsRuntime.InvokeVoidAsync($"{PlayerName}.player.cueVideoByUrl", url);
        }
    }
}
