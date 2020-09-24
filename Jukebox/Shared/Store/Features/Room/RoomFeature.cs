using Fluxor;
using Jukebox.Shared.Store.States;

namespace Jukebox.Shared.Store.Features.Room
{
    public class RoomFeature : Feature<RoomState>
    {
        public override string GetName() => "Room";

        protected override RoomState GetInitialState() => new RoomState(null, null);
    }
}
