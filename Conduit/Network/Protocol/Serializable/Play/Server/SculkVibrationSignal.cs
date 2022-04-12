using Conduit.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Protocol.Serializable.Play.Server
{
    public sealed class SculkVibrationSignal : Packet
    {
        public Position SourcePosition;
        public string DestinationIdentifier;
        // я нихуя не понял поэтому не закончил

        protected override void OnClear()
        {
            SourcePosition = default;
            DestinationIdentifier = null;
        }
    }
}
