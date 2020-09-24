using Fluxor;
using Jukebox.Player.Base;
using Jukebox.Shared.Store.Features.Playlist.Actions;
using Jukebox.Shared.Store.Features.Room.Actions;
using Jukebox.Shared.Store.States;
using System.Linq;

namespace Jukebox.Shared.Store.Features.Playlist.Reducers
{
    public static class PlaylistReducer
    {
        [ReducerMethod]
        public static PlaylistState ReduceRoomEnteredAction(PlaylistState state, RoomEnteredAction action)
            => new PlaylistState(action.Room.Playlist);

        [ReducerMethod]
        public static PlaylistState ReduceSongAddedAction(PlaylistState state, SongAddedAction action)
            => new PlaylistState(state.AllSongs.Append(action.Song).ToList(), state.CurrentSong ?? action.Song, state.IsPlaying, state.Started, state.Elapsed);

        [ReducerMethod]
        public static PlaylistState ReduceSongRemovedAction(PlaylistState state, SongRemovedAction action)
            => new PlaylistState(state.AllSongs.Where(s => !s.Equals(action.Song)).ToList(), state.CurrentSong, state.IsPlaying, state.Started, state.Elapsed);

        [ReducerMethod]
        public static PlaylistState ReduceSongChangedAction(PlaylistState state, SongChangedAction action)
            => new PlaylistState(state.AllSongs, state.AllSongs.FirstOrDefault(s => s.Equals(action.Song)), state.IsPlaying, state.Started, state.Elapsed);

        [ReducerMethod]
        public static PlaylistState ReduceStateChangedAction(PlaylistState state, StateChangedAction action)
            => new PlaylistState(state.AllSongs, state.CurrentSong, action.State == PlayerState.Playing, state.Started, state.Elapsed);

        [ReducerMethod]
        public static PlaylistState ReduceElapsedChangedAction(PlaylistState state, ElapsedChangedAction action)
            => new PlaylistState(state.AllSongs, state.CurrentSong, state.IsPlaying, state.Started, action.Elapsed);
    }
}
