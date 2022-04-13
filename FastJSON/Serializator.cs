using System;
using System.Collections.Generic;
using System.Reflection;

namespace FastJSON
{
    public class Serializator<TObj>
    {
        private Type TType;
        private PropertyInfo[] PropertyInfos;

        public Serializator()
        {
            TType = typeof(TObj);
            var pf = TType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            var clen = pf.LongLength;

            for (long i = 0; i < clen; i++)
            {
                pf[i].ge
            }
        }

        public TObj DeserializeReflect(string str)
        {

        }
    }
}
