using System;
using System.Collections.Generic;
using System.Text;

namespace Jukebox.Shared.Player
{
    public class ChatMessageInfo
    {
        public UserInfo Sender { get; set; }
        public UserInfo Reciever { get; set; }
        public string Message { get; set; }
        public DateTime SentWhen { get; set; }
    }
}
