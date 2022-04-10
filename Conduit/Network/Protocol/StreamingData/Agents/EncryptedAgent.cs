using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.StreamingData.Agents
{
    internal class EncryptedAgent : IAgent
    {
        public NetworkStream NetworkStream;

        public EncryptedAgent(NetworkStream ns)
        {
            NetworkStream = ns;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException("а здесь дешифровка");
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException("здесь зашифровка");
        }
    }
}
