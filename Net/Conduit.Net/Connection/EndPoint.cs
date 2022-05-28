namespace Conduit.Net.Connection
{
    public class EndPoint
    {
        public string Host { get; }
        public ushort Port { get; }

        public EndPoint(System.Net.EndPoint endPoint) : this((System.Net.IPEndPoint)endPoint) { }
        public EndPoint(System.Net.IPEndPoint endPoint) : this(endPoint.Address.ToString(), (ushort)endPoint.Port) { }
        public EndPoint(string host, ushort port)
        {
            Host = host;
            Port = port;
        }

        public override string ToString() => Host + ":" + Port;

        public static implicit operator EndPoint(System.Net.EndPoint endPoint) => new(endPoint);
        public static implicit operator string(EndPoint endPoint) => endPoint.ToString();
    }
}