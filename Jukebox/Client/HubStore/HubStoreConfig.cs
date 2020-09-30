using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Jukebox.Client.HubStore
{
    public class HubStoreConfig
    {
        public Uri HubUrl { get; set; }
        public string RoomName { get; set; }
        public string UserName { get; set; }
        public int ReconnectInterval { get; set; }
    }
}
