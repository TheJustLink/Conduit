using System.IO;

namespace ConduitServer
{
    internal class Logger : ILogger
    {
        private readonly StreamWriter _writer;
        private readonly ILog[] _logs;

        public Logger(ILog[] logs, StreamWriter writer)
        {
            _logs = logs;
            _writer = writer;
        }

        public void Update()
        {
            for (var i = 0; i < _logs.Length; i++)
            {
                var currentLog = _logs[i];
                if (currentLog.HasMessages())
                    Write(currentLog.Messages());
            }

            _writer.Flush();
        }

        private void Write(string[] messages)
        {
            for (var i = 0; i < messages.Length; i++)
            {
                var message = messages[i];
                _writer.Write(message);
            }
        }
    }
}