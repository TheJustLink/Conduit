﻿using Conduit.Net.Data;

namespace Conduit.Net.Attributes
{
    public class VarIntAttribute : AsAttribute
    {
        public VarIntAttribute() : base(VarInt.TypeHash) { }
    }
}