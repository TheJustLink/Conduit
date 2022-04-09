using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Network.Protocol.Serializable
{
    public enum NextState : int
    {
        Unknown, // 0
        Status, // 1
        Login, // 2
    }
}
