using Fluxor;
using Jukebox.Player.Base;
using Jukebox.Player.Manager;
using Jukebox.Shared.Store.Features.Playlist.Actions;
using System.Threading.Tasks;

namespace Jukebox.Shared.Store.Features.Playlist.Effects
{
    public class StateChangedActionEffect : Effect<StateChangedAction>
    {
        private readonly IPlayerManager _playerManager;

        public StateChangedActionEffect(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        protected override async Task HandleAsync(StateChangedAction action, IDispatcher dispatcher)
        {
            var player = _playerManager.GetPlayer(action.Type);
            if (action.State == PlayerState.Playing)
            {
                await player.Play();
            }
            else
            {
                await player.Pause();
            }
        }
    }
}
