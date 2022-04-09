using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable
{
    public enum NetworkState
    {
        Handshake,
        Status,
        Login,
        Play,
    }
}
