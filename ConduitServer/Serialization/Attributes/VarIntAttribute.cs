using System;

namespace ConduitServer.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    class VarIntAttribute : Attribute { }
}