using Conduit.Utilities.SpecialAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.JSON
{
    public abstract class JSONCacherComponent : IMoreToOne
    {
        public string LastJSON { get; private set; }
        public void Invoke()
        {
            AInvoke();
            LastJSON = Cache();
        }
        protected abstract void AInvoke();
        protected abstract string Cache();
    }
}
