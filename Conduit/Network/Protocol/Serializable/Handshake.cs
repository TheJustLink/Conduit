using Conduit.Network.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Conduit.Network.Protocol.Serializable
{
    public sealed class Handshake : Packet
    {
        [VarInt]
        public int ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        [VarInt]
        public int NextState { get; set; }

        [Ignore]
        public NextState NextStateEnum => (NextState)NextState;

        public override string ToString()
        {
            return 
                $"Protocol Version:{ProtocolVersion}\n" +
                $"ServerAddress:{ServerAddress}\n" +
                $"ServerPort:{ServerPort}\n" +
                $"NextState:{NextStateEnum}\n";
        }

        protected override void OnClear()
        {
            ProtocolVersion = 0;
            ServerAddress = null;
            ServerPort = 0;
        }
    }
}
