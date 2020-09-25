using Fluxor;
using Jukebox.Shared.Store.Features.Room.Actions;
using Jukebox.Shared.Store.States;
using System.Linq;

namespace Jukebox.Shared.Store.Features.Room.Reducers
{
    public static class RoomReducer
    {
        [ReducerMethod]
        public static RoomState ReduceRoomEnteredAction(RoomState state, RoomEnteredAction action)
            => new RoomState(action.Room.Name, action.ConnectionId, action.Room.CurrentUser, action.Room.Users);

        [ReducerMethod]
        public static RoomState ReduceUserAddedAction(RoomState state, UserAddedAction action)
            => new RoomState(state.Name, state.ConnectionId, state.CurrentUser, state.Users.Append(action.User).ToList());

        [ReducerMethod]
        public static RoomState ReduceUserRemovedAction(RoomState state, UserRemovedAction action)
            => new RoomState(state.Name, state.ConnectionId, state.CurrentUser, state.Users.Where(u => u.ConnectionId != action.ConnectionId).ToList());
    }
}
