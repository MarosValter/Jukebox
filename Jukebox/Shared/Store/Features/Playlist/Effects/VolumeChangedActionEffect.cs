using Fluxor;
using Jukebox.Player.Manager;
using Jukebox.Shared.Store.Features.Playlist.Actions;
using System.Threading.Tasks;

namespace Jukebox.Shared.Store.Features.Playlist.Effects
{
    public class VolumeChangedActionEffect : Effect<VolumeChangedAction>
    {
        private readonly IPlayerManager _playerManager;

        public VolumeChangedActionEffect(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        protected override async Task HandleAsync(VolumeChangedAction action, IDispatcher dispatcher)
        {
            var player = _playerManager.GetPlayer(action.Type);
            while (!(await player.IsReady()))
            {
                await Task.Delay(100);
            }

            await player.SetVolume(action.Volume);
            if (await player.IsMuted())
            {
                await player.UnMute();
            }
        }
    }
}
