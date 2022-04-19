using System.Net;
using System.Net.Sockets;

namespace Conduit.Net.Extensions
{
    public static class TcpClientExtensions
    {
        public static IPEndPoint GetEndPoint(this TcpClient tcpClient)
        {
            return tcpClient.Client.RemoteEndPoint as IPEndPoint;
        }
        public static string GetFormatEndPoint(this TcpClient tcpClient)
        {
            var endPoint = GetEndPoint(tcpClient);

            return endPoint.Address + ":" + endPoint.Port;
        }
        public static int GetPort(this TcpClient tcpClient)
        {
            return GetEndPoint(tcpClient).Port;
        }
        public static string GetAddress(this TcpClient tcpClient)
        {
            return GetEndPoint(tcpClient).Address.ToString();
        }

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