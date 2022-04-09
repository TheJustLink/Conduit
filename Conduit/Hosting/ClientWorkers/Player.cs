using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Hosting.ClientWorkers
{
    public sealed class Player : ClientWorker
    {
        public Player (ClientMaintainer cm) : base(cm)
        {
        }

        public override void Maintain()
        {
            throw new NotImplementedException();
        }
    }
}
