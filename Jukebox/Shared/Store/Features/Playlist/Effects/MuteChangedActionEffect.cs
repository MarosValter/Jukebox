using Fluxor;
using Jukebox.Player.Manager;
using Jukebox.Shared.Store.Features.Playlist.Actions;
using System.Threading.Tasks;

namespace Jukebox.Shared.Store.Features.Playlist.Effects
{
    public class MuteChangedActionEffect : Effect<MuteChangedAction>
    {
        private readonly IPlayerManager _playerManager;

        public MuteChangedActionEffect(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        protected override async Task HandleAsync(MuteChangedAction action, IDispatcher dispatcher)
        {
            var player = _playerManager.GetPlayer(action.Type);
            while (!(await player.IsReady()))
            {
                await Task.Delay(100);
            }
            if (action.IsMuted)
            {
                await player.Mute();
            }
            else
            {
                await player.UnMute();
            }
        }
    }
}
