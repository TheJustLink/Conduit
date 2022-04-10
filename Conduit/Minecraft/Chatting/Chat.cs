using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft.Chatting
{
    public abstract class Chat
    {
        public ulong ID { get; private set; }
        public Chat(ulong id)
        {
            ID = id;
        }
    }
}
