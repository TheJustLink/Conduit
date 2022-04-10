using Conduit.Network.Protocol.StreamingData.Agents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.StreamingData
{
    public sealed class RemoteStream : Stream
    {
        private NetworkStream _stream;

        public IAgent Agent { get; set; }

        public RemoteStream(NetworkStream stream)
        {
            _stream = stream;
            Agent = new RawAgent(_stream);
        }
        public RemoteStream(IAgent agent, NetworkStream stream)
        {
            _stream = stream;
            Agent = agent;
        }
        public bool DataAvailable => _stream.DataAvailable;
        /// <summary>
        /// Raw remotely length.
        /// </summary>
        public override long Length => _stream.Socket.Available;

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Position { get => 0; set => throw new NotImplementedException(); }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return Agent.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Agent.Write(buffer, offset, count);
        }
    }
}
