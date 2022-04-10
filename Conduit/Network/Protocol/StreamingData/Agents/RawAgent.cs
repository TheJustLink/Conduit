using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.StreamingData.Agents
{
    public sealed class RawAgent : IAgent
    {
        public NetworkStream NetworkStream;

        public RawAgent(NetworkStream ns)
        {
            NetworkStream = ns;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return NetworkStream.Read(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            NetworkStream.Write(buffer, offset, count);
        }
    }
}
