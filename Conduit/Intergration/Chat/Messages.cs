using Conduit.Network.JSON;
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
        public JSONCacher<ChatBase> ServerNotAvailable;

        public Messages()
        {
            SetupDefault();
        }

        private void SetupDefault()
        {
            InitAll();
            CacheAll();
        }

        private void InitAll()
        {
            ServerNotAvailable = new JSONCacher<ChatBase>("Server is not available now.");
        }
        private void CacheAll()
        {
            ServerNotAvailable.Cache();
        }
    }
}
