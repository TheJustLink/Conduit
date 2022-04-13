using Conduit.Utilities.Resourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Minecraft.Resources
{
    public sealed class ResourceManager
    {
        private Dictionary<string, Resource> Resources;

        public ResourceManager()
        {

        }
        
        public string WorkingPath { get; private set; }
        public bool AllocateResources(string path)
        {
            WorkingPath = path;
            return false;
        }
    }
}
