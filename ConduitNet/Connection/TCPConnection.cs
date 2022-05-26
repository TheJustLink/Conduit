using System;
using System.Net.Sockets;
using System.IO.Compression;
using System.Runtime.CompilerServices;

using Conduit.Net.Packets;
using Conduit.Net.IO.Packet;
using Conduit.Net.Extensions;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Net.Connection
{
    public class TCPConnection : IConnection, IDisposable
    {
        public EndPoint RemotePoint { get; }
        public EndPoint LocalPoint { get; }

        private readonly TcpClient _tcpClient;

        private readonly ReaderFactory _readerFactory;
        private readonly WriterFactory _writerFactory;

        private readonly IReader _reader;
        private readonly IWriter _writer;

        private TCPConnection() { }
        public TCPConnection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;

            LocalPoint = tcpClient.Client.LocalEndPoint;
            RemotePoint = tcpClient.Client.RemoteEndPoint;

            var stream = _tcpClient.GetStream();
            _readerFactory = new ReaderFactory(stream);
            _writerFactory = new WriterFactory(stream);

            _reader = _readerFactory.Create();
            _writer = _writerFactory.Create();
        }

        public void Dispose() => _tcpClient?.Dispose();

        public bool Connected => _tcpClient.IsConnected();
        public bool HasData => _tcpClient.Available > 0;

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void AddCompression(int treshold, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            _readerFactory.AddCompression(_reader);
            _writerFactory.AddCompression(_writer, treshold, compressionLevel);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void AddEncryption(byte[] key)
        {
            _readerFactory.AddEncryption(_reader, key);
            _writerFactory.AddEncryption(_writer, key);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void ChangePacketFlow(PacketFlow packetFlow)
        {
            _reader.ChangePacketMap(packetFlow.InboundMap);
            _writer.ChangePacketMap(packetFlow.OutboundMap);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Send(Packet packet) => _writer.Write(packet);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Packet Receive() => _reader.Read();

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void Disconnect() => _tcpClient?.Close();
    }
}