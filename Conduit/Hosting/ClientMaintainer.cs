using Conduit.Hosting.ClientWorkers;
using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Protocol.Serializable.Status;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Hosting
{
    public sealed class ClientMaintainer
    {
        public VClient VClient { get; private set; }
        public Protocol Protocol { get; private set; }

        public Handshaker Handshaker { get; private set; }
        public Loginer Loginer { get; private set; }
        public Player Player { get; private set; }
        public Stater Stater { get; private set; }
        public ClientWorker CurrentWorker { get; private set; }
        public ClientState State { get; private set; }
        public ClientMaintainer(VClient vClient)
        {
            VClient = vClient;
            Protocol = VClient.ServerInstance.Protocol;
            State = ClientState.Handshake;

            Handshaker = new Handshaker(this);
            Stater = new Stater(this);
            Loginer = new Loginer(this);
            Player = new Player(this);

            CurrentWorker = Handshaker;
        }
        public void ChangeState(ClientState state)
        {
            if (State == state || // if current state equals requested state
                !Enum.IsDefined(typeof(ClientState), state)) // if not defined in enum
                return; // break;

            State = state;
            switch (State)
            {
                case ClientState.Status:
                    {
                        CurrentWorker = Stater;
                        break;
                    }
                case ClientState.Login:
                    {
                        CurrentWorker = Loginer;
                        break;
                    }
                case ClientState.Play:
                    {
                        CurrentWorker = Player;
                        break;
                    }
            }
        }

        public void MaintaintClient()
        {
            CurrentWorker.Maintain();
        }
    }
}
