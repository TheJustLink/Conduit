﻿using Conduit.Network.JSON;
using Conduit.Network.JSON.Status;
using Conduit.Utilities.SpecialAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Controllable.Status
{
    public abstract class StatusCacher : JSONCacherComponent, IMoreToOne
    {
        public StatusBase LastStatusInfo { get; private set; }

        public string LastStatus => LastJSON;

        protected override void AInvoke()
        {
            LastStatusInfo = Maintain();
        }

        protected override string Cache()
        {
            return LastStatusInfo.Serialize();
        }

        /// <summary>
        /// Created for generating status about server by events.
        /// </summary>
        /// <returns></returns>
        protected abstract StatusBase Maintain();
    }
}
