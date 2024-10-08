﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jukebox.Shared.Player;

namespace Jukebox.Server.Storage
{
    public class InMemoryRoomStorage : IRoomStorage
    {
        private readonly ConcurrentDictionary<string, RoomInfo> _rooms = new ConcurrentDictionary<string, RoomInfo>();

        public Task<IList<RoomInfo>> GetUserRooms(string connectionId)
        {
            return Task.FromResult((IList<RoomInfo>)_rooms.Values.Where(x => x.Users.Any(y => y.ConnectionId == connectionId)).ToList());
        }

        public Task<RoomInfo> GetOrCreateRoomAsync(string roomName)
        {
            var room = _rooms.GetOrAdd(roomName, name => new RoomInfo
            {
                Name = name,
                Users = new List<UserInfo>(),
                Playlist = new PlaylistInfo(),
                ChatMessages = new List<ChatMessageInfo>()
            });
            return Task.FromResult(room);
        }

        public Task<bool> AddMessageAsync(string roomName, ChatMessageInfo message)
        {
            if (!_rooms.ContainsKey(roomName))
            {
                throw new InvalidOperationException("Room not found.");
            }

            bool result;
            var room = _rooms[roomName];
            lock (room)
            {
                room.ChatMessages.Add(message);
                result = true;
            }

            return Task.FromResult(result);
        }

        public Task<bool> AddUserAsync(string roomName, UserInfo user)
        {
            if (!_rooms.ContainsKey(roomName))
            {
                throw new InvalidOperationException("Room not found.");
            }

            bool result;
            var room = _rooms[roomName];
            lock (room)
            {
                if (room.Users.Any(x => x.ConnectionId == user.ConnectionId))
                {
                    result = false;
                }
                else
                {
                    room.Users.Add(user);
                    result = true;
                }
            }

            return Task.FromResult(result);
        }

        public Task<bool> RemoveUserAsync(string roomName, string connectionId)
        {
            if (!_rooms.ContainsKey(roomName))
            {
                throw new InvalidOperationException("Room not found.");
            }

            bool result;
            var room = _rooms[roomName];
            lock (room)
            {
                var user = room.Users.FirstOrDefault(x => x.ConnectionId == connectionId);
                if (user == null)
                {
                    result = false;
                }
                else
                {
                    room.Users.Remove(user);
                    result = true;
                }
            }

            return Task.FromResult(result);
        }
    }
}