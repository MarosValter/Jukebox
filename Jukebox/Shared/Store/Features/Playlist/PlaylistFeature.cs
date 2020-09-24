using Fluxor;
using Jukebox.Shared.Store.States;

namespace Jukebox.Shared.Store.Features.Playlist
{
    public class PlaylistFeature : Feature<PlaylistState>
    {
        public override string GetName() => "Playlist";

        protected override PlaylistState GetInitialState() => new PlaylistState(null);
    }
}
