﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Status
{
    public class Ping : Packet
    {
        public long Payload { get; set; }

        public Ping()
        {
            Id = 0x01;
        }

        protected override void OnClear()
        {
            Payload = 0;
        }
    }
}
