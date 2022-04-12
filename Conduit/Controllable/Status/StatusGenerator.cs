using Conduit.Network.JSON.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = Conduit.Network.JSON.Status.Version;

namespace Conduit.Controllable.Status
{
    /// <summary>
    /// For abstract tests.
    /// </summary>
    public sealed class StatusGenerator : StatusCacher
    {
        protected override StatusBase Maintain()
        {
            return new StatusBase()
            {
                Version = new Version()
                {
                    Name = "Conduit # 1.18.2",
                    ProtocolVersion = 757,
                },
                Players = new Players()
                {
                    Max = 10,
                    Online = 5,
                },
                Description = new Description()
                {
                    Text = "MOTD | ALPHA TEST"
                }
            };
        }
    }
}
