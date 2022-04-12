using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Client
{
    public sealed class SetDifficulty : Packet
    {
        public byte NewDifficulty; // 0: peaceful, 1: easy, 2: normal, 3: hard

        public SetDifficulty() => Id = 0x02;

        protected override void OnClear()
        {
            NewDifficulty = 0;
        }
    }
}
