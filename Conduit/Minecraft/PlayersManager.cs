using Conduit.Hosting;
using Conduit.Minecraft.Game.Objects;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Minecraft
{
    public sealed class PlayersManager
    {
        private Dictionary<GuidUnsafe, Player> _Players;

        private Stack<(GuidUnsafe, Player)> AddStack;
        private Stack<GuidUnsafe> RemoveStack;

        private Thread NativeThread;
        public bool IsWorking { get; private set; }

        public PlayersManager()
        {
            _Players = new Dictionary<GuidUnsafe, Player>();

            AddStack = new Stack<(GuidUnsafe, Player)>();
            RemoveStack = new Stack<GuidUnsafe>();
        }
        public void RegisterPlayer(VClient vcl, GuidUnsafe uuid)
        {
            Add(uuid, new Player(vcl) { UUID = uuid });
        }
        public bool Add(GuidUnsafe uuid, Player pl)
        {
            if (_Players.ContainsKey(uuid))
                return false;

            AddStack.Push((uuid, pl));
            return true;
        }
        public bool Remove(GuidUnsafe uuid)
        {
            if (!_Players.ContainsKey(uuid))
                return false;

            RemoveStack.Push(uuid);
            return true;
        }

        public void Start()
        {
            if (!IsWorking)
            {
                NativeThread = new Thread(Work);
                IsWorking = true;
                NativeThread.Start();
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
                            _Players.Remove(RemoveStack.Pop());
                        }
                    }
                    if (asc > 0)
                    {
                        for (int i = 0; i < asc; i++)
                        {
                            var acl = AddStack.Pop();
                            _Players.Add(acl.Item1, acl.Item2);
                        }
                    }
                    await Task.Delay(5); // low priority
                }
            }
        }
    }
}
