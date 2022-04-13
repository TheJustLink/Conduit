using Conduit.Utilities;
using GrEmit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
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

        public PropertyInfo Property;

        private Type BaseClass;

        public Action<object, object> Setter;
        //public Func<object, object> Getter;
        public Func<object, object> Getter;
        public Action<object, object> Writer;
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
            Property = pinfo;
            HasVIA = hvia;
            HasVLA = hvla;
            HasBD = hbd;
            IsArray = isarray;

            CompileGet();
            CompileSet();
            CompileWrite();
        }

        private void CompileWrite()
        {
            var method = new DynamicMethod(Guid.NewGuid().ToString(), // имя метода
                                  typeof(void), // возвращаемый тип
                                  new[] { typeof(object), typeof(object) }, // принимаемые параметры
                                  BaseClass, // к какому типу привязать метод, можно указывать, например, string
                                  true); // просим доступ к приватным полям

            using (var il = new GroboIL(method))
            {
                il.Ldarg(0);
                il.Castclass(typeof(PBinaryWriter));

                bool isnum = MathUtils.IsNumber(Property.PropertyType);
                bool istruenumber = (isnum || !Property.PropertyType.IsClass) && !Property.PropertyType.IsArray;
                
                il.Ldarg(1);
                if (istruenumber)
                    il.Unbox_Any(Property.PropertyType);
                else
                    il.Castclass(Property.PropertyType);

                if (HasVIA)
                    il.Call(typeof(PBinaryWriter).GetMethod("Write7BitEncodedInt"));
                else if (HasVLA)
                    il.Call(typeof(PBinaryWriter).GetMethod("Write7BitEncodedInt64"));
                else
                    il.Call(typeof(PBinaryWriter).GetMethod("Write", new Type[] { Property.PropertyType }));
                il.Ret();
            }

            Writer = method.CreateDelegate<Action<object, object>>();
        }

        private void CompileSet()
        {
            var method = new DynamicMethod(Guid.NewGuid().ToString(), // имя метода
                                  typeof(void), // возвращаемый тип
                                  new[] { typeof(object), typeof(object) }, // принимаемые параметры
                                  BaseClass, // к какому типу привязать метод, можно указывать, например, string
                                  true); // просим доступ к приватным полям

            using (var il = new GroboIL(method))
            {
                bool istruenumber = (MathUtils.IsNumber(Property.PropertyType) || !Property.PropertyType.IsClass) && !Property.PropertyType.IsArray;

                il.Ldarg(0);
                il.Castclass(BaseClass);
                il.Ldarg(1);

                if (istruenumber)
                    il.Unbox_Any(Property.PropertyType);
                else
                    il.Castclass(Property.PropertyType);

                il.Call(Property.GetSetMethod());
                il.Ret();
            }

            Setter = method.CreateDelegate<Action<object, object>>();
        }
        private void CompileGet()
        {
            var method = new DynamicMethod(Guid.NewGuid().ToString(), // имя метода
                                  typeof(object), // возвращаемый тип
                                  new[] { typeof(object) }, // принимаемые параметры
                                  BaseClass, // к какому типу привязать метод, можно указывать, например, string
                                  true); // просим доступ к приватным полям

            using (var il = new GroboIL(method))
            {
                il.Ldarg(0);
                il.Castclass(BaseClass);
                il.Call(Property.GetGetMethod());

                bool istruenumber = (MathUtils.IsNumber(Property.PropertyType) || !Property.PropertyType.IsClass) && !Property.PropertyType.IsArray;

                if (istruenumber)
                    il.Box(Property.PropertyType);
                //il.Castclass(Property.PropertyType);

                il.Ret();
            }

            Getter = method.CreateDelegate<Func<object, object>>();
        }
    }
}
