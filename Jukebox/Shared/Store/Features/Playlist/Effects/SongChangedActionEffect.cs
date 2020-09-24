using Fluxor;
using Jukebox.Player.Base;
using Jukebox.Player.Manager;
using Jukebox.Shared.Store.Features.Playlist.Actions;
using System.Threading.Tasks;

namespace Jukebox.Shared.Store.Features.Playlist.Effects
{
    public class SongChangedActionEffect : Effect<SongChangedAction>
    {
        private readonly IPlayerManager _playerManager;

        public SongChangedActionEffect(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        protected override async Task HandleAsync(SongChangedAction action, IDispatcher dispatcher)
        {
            if (action.Song != null)
            {
                var player = _playerManager.GetPlayer(action.Song.Type);
                while (!(await player.IsReady()))
                {
                    await Task.Delay(100);
                }
                await player.QueueMediaById(action.Song.Id);
                dispatcher.Dispatch(new StateChangedAction(PlayerState.Playing, action.Song.Type));
            }
        }
    }
}
