using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConduitServer
{
    internal class App
    {
        private ILogger _logger;
        private bool _isRunning;
        private Server _server;

        internal App()
        {
            _isRunning = true;
            _server = new Server(25565);

            var fileStream = new FileStream(@"C:\2\123.txt", FileMode.OpenOrCreate);
            var writer = new StreamWriter(fileStream);
            _logger = new Logger(new ILog[] {_server}, writer);
        }

        public void Start()
        {
            var updateThread = new Thread(Update);
            updateThread.Start();

            _server.Start();
        }

        private async void Update()
        {
            while (_isRunning)
            {
                _logger.Update();
                await Task.Delay(100);
            }
        }
    }
}