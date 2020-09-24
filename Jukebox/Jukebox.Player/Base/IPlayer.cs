using System;
using System.Threading.Tasks;

namespace Jukebox.Player.Base
{
    public interface IPlayer
    {
        PlayerType Type { get; }
        PlayerState State { get; set; }

        event EventHandler<PlayerState> StateChanged;

        Task Initialize(string element);
        Task<bool> IsReady();

        Task Play();
        Task Pause();
        Task Next();
        Task Previous();
        Task SeekTo(double seconds);

        Task SetVolume(int value);
        Task<int> GetVolume();

        Task Mute();
        Task UnMute();
        Task<bool> IsMuted();

        Task<TimeSpan> GetElapsedTime();
        Task<TimeSpan> GetDuration();

        Task QueueMediaById(string id);
        Task QueueMediaByUrl(string url);
    }
}
