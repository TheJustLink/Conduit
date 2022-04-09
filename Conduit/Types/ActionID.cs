using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Types
{
    public enum ActionID : int
    {
        PerformRespawn, /* Sent when the client is ready to complete login and when
                         * the client is ready to respawn after death. 
                         */
        RequestStats, // Sent when the client opens the Statistics menu. 
    }
}
