namespace Conduit.Server.Services
{
    interface IService
    {
        bool IsRunning { get; }

        void Start();
        void Stop();
    }
}