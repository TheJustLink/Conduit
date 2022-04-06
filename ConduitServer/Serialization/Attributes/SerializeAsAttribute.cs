using System;

namespace ConduitServer.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    class SerializeAsAttribute : Attribute
    {
        public SerializeAsAttribute()
        {
            
        }
    }
}