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
        public StatusBase LastStatusInfo;
        public string CachedJSON;
        public string GetInfo()
        {
            return CachedJSON;
        }

        public void Invoke()
        {
            LastStatusInfo = Maintain();
            CachedJSON = LastStatusInfo.Serialize();
        }

        /// <summary>
        /// Created for generating status about server by events.
        /// </summary>
        /// <returns></returns>
        protected abstract StatusBase Maintain();
    }
}
