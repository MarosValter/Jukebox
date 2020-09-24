using Fluxor;
using Jukebox.Player.Base;
using Jukebox.Player.Manager;
using Jukebox.Shared.Store.Features.Playlist.Actions;
using Jukebox.Shared.Store.Features.Room.Actions;
using System.Threading.Tasks;

namespace Jukebox.Shared.Store.Features.Room.Effects
{
    public class RoomEnteredActionEffect : Effect<RoomEnteredAction>
    {
        private readonly IPlayerManager _playerManager;

        public RoomEnteredActionEffect(IPlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        protected override async Task HandleAsync(RoomEnteredAction action, IDispatcher dispatcher)
        {
            if (action.Room.Playlist.CurrentSong != null)
            {
                var player = _playerManager.GetPlayer(action.Room.Playlist.CurrentSong.Type);
                while (!(await player.IsReady()))
                {
                    await Task.Delay(100);
                }
                await player.QueueMediaById(action.Room.Playlist.CurrentSong.Id);

                dispatcher.Dispatch(new StateChangedAction(PlayerState.Playing, action.Room.Playlist.CurrentSong.Type));
            }
        }
    }
}
