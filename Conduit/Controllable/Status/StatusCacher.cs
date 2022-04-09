using Conduit.Network.JSON.Status;
using Conduit.Utilities.SpecialAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Controllable.Status
{
    public abstract class StatusCacher : IStatus, IMoreToOne
    {
        public StatusInfo LastStatusInfo;
        public StatusInfo GetInfo()
        {
            return LastStatusInfo;
        }

        public void Invoke()
        {
            LastStatusInfo = Maintain();
        }

        /// <summary>
        /// Created for generating status about server by events.
        /// </summary>
        /// <returns></returns>
        protected abstract StatusInfo Maintain();
    }
}
