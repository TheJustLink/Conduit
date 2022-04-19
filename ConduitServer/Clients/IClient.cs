namespace Conduit.Server.Clients
{
    interface IClient
    {
        bool Connected { get; }
        string UserAgent { get; }

        void Tick();
    }
}