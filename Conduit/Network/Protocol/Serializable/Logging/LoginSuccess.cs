using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Logging
{
    public class LoginSuccess : Packet
    {
        public GuidUnsafe UUID;
        public string Username;

        public LoginSuccess() => Id = 2;

        protected override void OnClear()
        {
            UUID = default;
            Username = null;
        }
    }
}
