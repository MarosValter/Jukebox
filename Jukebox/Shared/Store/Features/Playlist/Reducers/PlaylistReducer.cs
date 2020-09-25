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
        // Enter room
        [ReducerMethod]
        public static PlaylistState ReduceRoomEnteredAction(PlaylistState state, RoomEnteredAction action)
            => new PlaylistState(action.Room.Playlist);

        // Add song
        [ReducerMethod]
        public static PlaylistState ReduceSongAddedAction(PlaylistState state, SongAddedAction action)
            => new PlaylistState(state.AllSongs.Append(action.Song).ToList(), state.CurrentSong ?? action.Song, state.IsPlaying, state.Started, state.Elapsed, state.IsMuted, state.Volume);

        // Remove song
        [ReducerMethod]
        public static PlaylistState ReduceSongRemovedAction(PlaylistState state, SongRemovedAction action)
            => new PlaylistState(state.AllSongs.Where(s => !s.Equals(action.Song)).ToList(), state.CurrentSong, state.IsPlaying, state.Started, state.Elapsed, state.IsMuted, state.Volume);

        // Change current song
        [ReducerMethod]
        public static PlaylistState ReduceSongChangedAction(PlaylistState state, SongChangedAction action)
            => new PlaylistState(state.AllSongs, state.AllSongs.FirstOrDefault(s => s.Equals(action.Song)), state.IsPlaying, state.Started, state.Elapsed, state.IsMuted, state.Volume);

        // Play / Pause
        [ReducerMethod]
        public static PlaylistState ReduceStateChangedAction(PlaylistState state, StateChangedAction action)
            => new PlaylistState(state.AllSongs, state.CurrentSong, action.State == PlayerState.Playing, state.Started, state.Elapsed, state.IsMuted, state.Volume);

        // Elapsed duration
        [ReducerMethod]
        public static PlaylistState ReduceElapsedChangedAction(PlaylistState state, ElapsedChangedAction action)
            => new PlaylistState(state.AllSongs, state.CurrentSong, state.IsPlaying, state.Started, action.Elapsed, state.IsMuted, state.Volume);

        // Mute / UnMute
        [ReducerMethod]
        public static PlaylistState ReduceMuteChangedAction(PlaylistState state, MuteChangedAction action)
            => new PlaylistState(state.AllSongs, state.CurrentSong, state.IsPlaying, state.Started, state.Elapsed, action.IsMuted, action.IsMuted ? 0 : state.Volume);

        // Volume
        [ReducerMethod]
        public static PlaylistState ReduceVolumeChangedAction(PlaylistState state, VolumeChangedAction action)
            => new PlaylistState(state.AllSongs, state.CurrentSong, state.IsPlaying, state.Started, state.Elapsed, action.Volume > 0 ? false : state.IsMuted, action.Volume);
    }
}
