using System.Threading.Tasks;

namespace Jukebox.Player
{
    public interface IPlayer
    {
        PlayerType Type { get; }

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

        Task<double> GetElapsedTime();
        Task<double> GetDuration();

        Task QueueMediaById(string id);
        Task QueueMediaByUrl(string url);
    }
}
