using Conduit.Network.JSON.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Intergration.Chat
{
    public sealed class Messages
    {
        public ChatBase ServerNotAvailable;

        public Messages()
        {
            ServerNotAvailable = "Server is not available now.";
        }
    }
}
