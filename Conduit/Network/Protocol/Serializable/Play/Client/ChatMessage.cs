using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Client
{
    public sealed class ChatMessage : Packet
    {
        public string Message;

        public ChatMessage() => Id = 0x03;

        protected override void OnClear()
        {
            Message = null;
        }
    }
}
