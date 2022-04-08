namespace ConduitServer.Services
{
    interface IService
    {
        bool IsRunning { get; }

        void Start();
        void Stop();
    }
}