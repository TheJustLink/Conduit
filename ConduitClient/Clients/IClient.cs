namespace Conduit.Client.Clients
{
    interface IClient
    {
        public string Host { get; }
        public int Port { get; }
        public bool IsConnected { get; }

        public void CheckServerState();
        public void JoinGame(string username);
        public void Disconnect();
    }
}