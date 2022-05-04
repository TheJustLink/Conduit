using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Conduit.Net.Extensions
{
    public static class TcpClientExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static IPEndPoint GetEndPoint(this TcpClient tcpClient)
        {
            return tcpClient.Client.RemoteEndPoint as IPEndPoint;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static string GetFormatEndPoint(this TcpClient tcpClient)
        {
            var endPoint = GetEndPoint(tcpClient);

            return endPoint.Address + ":" + endPoint.Port;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static int GetPort(this TcpClient tcpClient)
        {
            return GetEndPoint(tcpClient).Port;
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static string GetAddress(this TcpClient tcpClient)
        {
            return GetEndPoint(tcpClient).Address.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static bool IsConnected(this TcpClient client)
        {
            try
            {
                if (client is null || client.Connected == false)
                    return false;

                var socket = client.Client;
                if (socket.Connected == false)
                    return false;
                
                if (socket.Poll(0, SelectMode.SelectRead))
                    return socket.Receive(new byte[1], SocketFlags.Peek) != 0;

                return true;
            }
            catch { return false; }
        }
    }
}