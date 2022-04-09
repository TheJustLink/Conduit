using Conduit.Hosting;
using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Serialization.Attributes;
using Conduit.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Conduit.Network.Serialization
{
    public sealed class Serializator<T> : ISerializator where T : Packet, new()
    {
        private Type TType;
        private List<FieldInfo> DeclaredFields;
        private List<FieldInfo> DeclaredLessFields; // without packet fields

        private static Type VIA = typeof(VarIntAttribute);
        private static Type VLA = typeof(VarLongAttribute);

        public Serializator()
        {
            TType = typeof(T);
            DeclaredFields = new List<FieldInfo>();
            DeclaredLessFields = new List<FieldInfo>();
            Resolve(TType);
            ResolveLess(TType);
        }
        private void ResolveLess(Type type)
        {
            var bt = type.BaseType;
            if (bt != typeof(object) && bt != typeof(Packet))
                ResolveLess(bt);

            var df = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            DeclaredLessFields.AddRange(df);
        }
        private void Resolve(Type type)
        {
            var bt = type.BaseType;
            if (bt != typeof(object))
                Resolve(bt);

            var df = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            DeclaredFields.AddRange(df);
        }

        public void Deserialize(Stream stream, T obj, out MemoryStream readed)
        {
            readed = new MemoryStream();
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            using var bwrite = new PBinaryWriter(readed, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredFields, (finfo) =>
            {
                bool hasvia = finfo.GetCustomAttribute(VIA) is not null;
                if (hasvia)
                {
                    var val = bread.Read7BitEncodedInt();
                    bwrite.Write7BitEncodedInt(val);
                    finfo.SetValue(obj, val);
                    return;
                }
                bool hasvla = finfo.GetCustomAttribute(VLA) is not null;
                if (hasvla)
                {
                    var val = bread.Read7BitEncodedInt64();
                    bwrite.Write7BitEncodedInt64(val);
                    finfo.SetValue(obj, val);
                    return;
                }

                var sval = bread.ReadObject(finfo.FieldType, out MemoryStream r);
                bwrite.Write(r.ReadData(r.Length));
                finfo.SetValue(obj, sval);

            });
        }
        public void DeserializeLess(Stream stream, T obj, out MemoryStream readed)
        {
            readed = new MemoryStream();
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            using var bwrite = new PBinaryWriter(readed, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredLessFields, (finfo) =>
            {
                bool hasvia = finfo.GetCustomAttribute(VIA) is not null;
                if (hasvia)
                {
                    var val = bread.Read7BitEncodedInt();
                    bwrite.Write7BitEncodedInt(val);
                    finfo.SetValue(obj, val);
                    return;
                }
                bool hasvla = finfo.GetCustomAttribute(VLA) is not null;
                if (hasvla)
                {
                    var val = bread.Read7BitEncodedInt64();
                    bwrite.Write7BitEncodedInt64(val);
                    finfo.SetValue(obj, val);
                    return;
                }

                var sval = bread.ReadObject(finfo.FieldType, out MemoryStream r);
                bwrite.Write(r.ReadData(r.Length));
                finfo.SetValue(obj, sval);

            });
        }
        public void Deserialize(Stream stream, T obj)
        {
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredFields, (finfo) =>
            {
                bool hasvia = finfo.GetCustomAttribute(VIA) is not null;
                if (hasvia) 
                {
                    finfo.SetValue(obj, bread.Read7BitEncodedInt());
                    return;
                }
                bool hasvla = finfo.GetCustomAttribute(VLA) is not null;
                if (hasvla)
                {
                    finfo.SetValue(obj, bread.Read7BitEncodedInt64());
                    return;
                }

                finfo.SetValue(obj, bread.ReadObject(finfo.FieldType));
            });
        }
        public void DeserializeLess(Stream stream, T obj)
        {
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredLessFields, (finfo) =>
            {
                bool hasvia = finfo.GetCustomAttribute(VIA) is not null;
                if (hasvia)
                {
                    finfo.SetValue(obj, bread.Read7BitEncodedInt());
                    return;
                }
                bool hasvla = finfo.GetCustomAttribute(VLA) is not null;
                if (hasvla)
                {
                    finfo.SetValue(obj, bread.Read7BitEncodedInt64());
                    return;
                }

                finfo.SetValue(obj, bread.ReadObject(finfo.FieldType));
            });
        }
        public void Serialize(Stream stream, T data)
        {
            using var memstream = new MemoryStream();
            SerializeObject(memstream, data);

            Array.Clear(memstream.GetBuffer());

            data.Length = (int)memstream.Length - 1;
            SerializeObject(memstream, data);

            stream.Write(memstream.GetBuffer());
        }
        public void SerializeObject(Stream stream, T data)
        {
            var bwrite = new PBinaryWriter(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredFields, (finfo) =>
            {
                var obj = finfo.GetValue(data);
                if (obj is null)
                    throw new Exception($"{finfo.Name} value null");

                if (finfo.GetCustomAttribute(VIA, true) is not null)
                    bwrite.Write7BitEncodedInt((int)obj);
                else if (finfo.GetCustomAttribute(VLA, true) is not null)
                    bwrite.Write7BitEncodedInt64((long)obj);
                else
                    bwrite.WriteObject(obj);
            });
        }

        public void Deserialize(Stream stream, object data) => Deserialize(stream, (T)data);

        public void Serialize(Stream stream, object data) => Serialize(stream, (T)data);
    }
}
