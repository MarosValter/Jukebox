using Jukebox.Player.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jukebox.Player.Base
{
    public abstract class PlayerBase : IPlayer
    {
        private readonly SpinLock _spinLock = new SpinLock();

        public event EventHandler<PlayerState> StateChanged;

        public abstract PlayerType Type { get; }
        public PlayerState State { get; set; }

        public async Task Play()
        {
            await _spinLock.ExecuteLockedAsync(async () =>
            {
                await PlayInternal();
                if (State != PlayerState.Playing)
                {
                    State = PlayerState.Playing;
                    StateChanged?.Invoke(this, State);
                }
            });
        }

        public async Task Pause()
        {
            await _spinLock.ExecuteLockedAsync(async () =>
            {
                await PauseInternal();
                if (State != PlayerState.Paused)
                {
                    State = PlayerState.Paused;
                    StateChanged?.Invoke(this, State);
                }
            });     
        }

        public abstract Task Initialize(string element);
        public abstract Task<bool> IsReady();
        public abstract Task PlayInternal();
        public abstract Task PauseInternal();
        public abstract Task Next();
        public abstract Task Previous();
        public abstract Task SeekTo(double seconds);
        public abstract Task SetVolume(int value);
        public abstract Task<int> GetVolume();
        public abstract Task Mute();
        public abstract Task UnMute();
        public abstract Task<bool> IsMuted();
        public abstract Task QueueMediaById(string id);
        public abstract Task QueueMediaByUrl(string url);
        public abstract Task<TimeSpan> GetDuration();
        public abstract Task<TimeSpan> GetElapsedTime();
    }
}
