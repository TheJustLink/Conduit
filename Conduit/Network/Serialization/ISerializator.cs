using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Conduit.Network.Serialization
{
    public interface ISerializator
    {
        public void Deserialize(Stream stream, object data);
        public void Serialize(Stream stream, object data);
    }
}
