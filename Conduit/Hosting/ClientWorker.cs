using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Hosting
{
    public abstract class ClientWorker
    {
        protected ClientHandler ClientHandler { get; private set; }
        public ClientWorker(ClientHandler cm)
        {
            ClientHandler = cm;
        }
        public abstract void Handling();
        /*
        protected MemoryStream ReadToStream(int length)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(ClientMaintainer.VClient.RemoteStream.ReadData(length));
            ms.Position = 0;
            return ms;
        }
        */

        protected void ShutdownClient()
        {
            ClientHandler.VClient.Shutdown();
        }
    }
}
