using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Network.Protocol.Serializable
{
    public enum NetworkState : int
    {
        Unknown,   // 0
        Handshake,// 1
        Status,  // 2
        Login,  // 3
        Play,  // 4
    }
}
