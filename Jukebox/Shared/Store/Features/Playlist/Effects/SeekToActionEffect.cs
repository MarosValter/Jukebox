using Fluxor;
using Jukebox.Player.Manager;
using Jukebox.Shared.Store.Features.Playlist.Actions;
using System.Threading.Tasks;

namespace Jukebox.Shared.Store.Features.Playlist.Effects
{
    public class SeekToActionEffect : Effect<SeekToAction>
    {
        private readonly IPlayerManager _playerManager;

        public SeekToActionEffect(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        protected override async Task HandleAsync(SeekToAction action, IDispatcher dispatcher)
        {
            var player = _playerManager.GetPlayer(action.Type);
            while (!(await player.IsReady()))
            {
                await Task.Delay(100);
            }

            await player.SeekTo(action.Elapsed.TotalSeconds);
        }
    }
}
