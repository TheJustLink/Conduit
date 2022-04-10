using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Intergration.Chat
{
    public sealed class ChatIntegrate
    {
        public Messages Messages;

        public ChatIntegrate(Messages mes)
        {
            Messages = mes;
        }
        public ChatIntegrate()
        {
            Messages = new Messages();
        }
    }
}
