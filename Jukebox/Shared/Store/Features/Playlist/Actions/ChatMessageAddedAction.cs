using Jukebox.Shared.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jukebox.Shared.Store.Features.Playlist.Actions
{
    public class ChatMessageAddedAction
    {
        public ChatMessageInfo ChatMessage { get; }
        public ChatMessageAddedAction(ChatMessageInfo message)
        {
            ChatMessage = message;
        }
    }
}
