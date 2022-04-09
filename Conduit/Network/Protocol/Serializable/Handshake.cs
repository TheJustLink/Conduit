using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Network.Protocol.Serializable
{
    public sealed class Handshake : Packet
    {
        [VarInt]
        public int ProtocolVersion;
        public string ServerAddress;
        public ushort ServerPort;
        [VarInt]
        public int NextState;

        public NextState NextStateEnum => (NextState)NextState;

        public override string ToString()
        {
            return 
                $"Protocol Version:{ProtocolVersion}\n" +
                $"ServerAddress:{ServerAddress}\n" +
                $"ServerPort:{ServerPort}\n" +
                $"NextState:{NextStateEnum}\n";
        }
    }
}
