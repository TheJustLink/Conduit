using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.StreamingData
{
    public interface IAgent
    {
        public int Read(byte[] buffer, int offset, int count);
        public void Write(byte[] buffer, int offset, int count);
    }
}
