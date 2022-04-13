using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Hosting
{
    public sealed class ClientsManager
    {
        private Dictionary<GuidUnsafe, VClient> Clients;
        public int Count => Clients.Count;
        private Stack<GuidUnsafe> RemoveStack;
        private Stack<(GuidUnsafe, VClient)> AddStack;

        private Thread WorkThread;
        public bool IsWorking { get; private set; }

        public ClientsManager()
        {
            Clients = new Dictionary<GuidUnsafe, VClient>();
            RemoveStack = new Stack<GuidUnsafe>();
            AddStack = new Stack<(GuidUnsafe, VClient)>();
        }

        public void Start()
        {
            if (!IsWorking)
            {
                WorkThread = new Thread(Work);
                WorkThread.Start();
            }
        }
        public void Stop()
        {
            IsWorking = false;
        }
        private async void Work()
        {
            while (IsWorking)
            {
                var rsc = RemoveStack.Count;
                var asc = AddStack.Count;
                if (rsc == 0 && asc == 0)
                    await Task.Delay(100); // low priority
                else
                {
                    if (rsc > 0)
                    {
                        for (int i = 0; i < rsc; i++)
                        {
                            Clients.Remove(RemoveStack.Pop());
                        }
                    }
                    if (asc > 0)
                    {
                        for (int i = 0; i < asc; i++)
                        {
                            var acl = AddStack.Pop();
                            Clients.Add(acl.Item1, acl.Item2);
                        }
                    }
                    await Task.Delay(5); // low priority
                }
            }
        }

        public bool TryGetClient(GuidUnsafe id, out VClient client)
        {
            return Clients.TryGetValue(id, out client);
        }
        public bool ContainsClient(GuidUnsafe id)
        {
            return Clients.ContainsKey(id);
        }

        public bool Remove(GuidUnsafe id)
        {
            if (!Clients.ContainsKey(id))
                return false;

            RemoveStack.Push(id);

            return true;
        }
        public bool Add(GuidUnsafe id, VClient client)
        {
            if (Clients.ContainsKey(id))
                return false;

            AddStack.Push((id, client));

            return true;
        }
    }
}
