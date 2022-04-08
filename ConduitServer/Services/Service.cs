using System.Threading;

namespace ConduitServer.Services
{
    abstract class Service : IService
    {
        public bool IsRunning { get; private set; }

        private readonly int _tickRate;

        public Service(int tickRate)
        {
            _tickRate = tickRate;
        }

        public virtual void Start()
        {
            IsRunning = true;

            var thread = new Thread(ThreadLoop);
            thread.IsBackground = true;
            thread.Start();
        }
        public virtual void Stop()
        {
            IsRunning = false;
        }

        private void ThreadLoop()
        {
            while(IsRunning)
            {
                Tick();
                Thread.Sleep(_tickRate);
            }
        }
        protected abstract void Tick();
    }
}