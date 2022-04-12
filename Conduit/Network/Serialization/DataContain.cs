using FastMember;
using Sigil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Network.Serialization
{
    public sealed class DataContain
    {
        public bool HasVIA;
        public bool HasVLA;
        public bool HasBD;
        public bool IsArray;

        public PropertyInfo PropertyInfo;

        private Type BaseClass;

        public Action<object, object> Setter;
        //public Func<object, object> Getter;
        public Func<object, object> Getter;
        //public MethodInfo Setter;

        public DataContain(
            Type baseclass,
            PropertyInfo pinfo,
            bool hvia,
            bool hvla,
            bool hbd,
            bool isarray)
        {
            BaseClass = baseclass;
            PropertyInfo = pinfo;
            HasVIA = hvia;
            HasVLA = hvla;
            HasBD = hbd;
            IsArray = isarray;

            Emit<Func<object, object>> gem = Emit<Func<object, object>>.NewDynamicMethod("GetTestClassDataProperty");
            gem = gem.LoadArgument(0);
            gem = gem.CastClass(BaseClass);
            gem = gem.Call(pinfo.GetGetMethod());
            if (pinfo.PropertyType != typeof(string) && !pinfo.PropertyType.IsArray)
                gem = gem.Box(pinfo.PropertyType);
            gem = gem.Return();

            Getter = gem.CreateDelegate();
        }
    }
}
