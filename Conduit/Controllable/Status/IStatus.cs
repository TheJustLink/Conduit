using Conduit.Network.JSON.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Controllable.Status
{
    public interface IStatus
    {
        public string GetInfo();
    }
}
