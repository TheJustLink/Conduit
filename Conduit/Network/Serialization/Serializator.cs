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
        private Dictionary<ulong, (FieldInfo, BigDataOffsetAttribute)> BigDataFields;

        private static Type VIA = typeof(VarIntAttribute);
        private static Type VLA = typeof(VarLongAttribute);
        private static Type BD = typeof(BigDataAttribute);
        private static Type BDO = typeof(BigDataOffsetAttribute);
        private static Type BDL = typeof(BigDataLengthAttribute);

        public Serializator(bool withbigdata = false)
        {
            TType = typeof(T);
            DeclaredFields = new List<FieldInfo>();
            DeclaredLessFields = new List<FieldInfo>();
            Resolve(TType);
            ResolveLess(TType);
            if (withbigdata)
            {
                BigDataFields = new Dictionary<ulong, (FieldInfo, BigDataOffsetAttribute)>();
                ResolveBigData(TType);
            }
        }
        private void ResolveLess(Type type)
        { // resolving with out baseclass "Packet"
            var bt = type.BaseType;
            if (bt != typeof(object) && bt != typeof(Packet))
                ResolveLess(bt);

            var df = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            DeclaredLessFields.AddRange(df);
        }
        private void Resolve(Type type)
        { // full resolve
            var bt = type.BaseType;
            if (bt != typeof(object))
                Resolve(bt);

            var df = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            DeclaredFields.AddRange(df);
        }
        private void ResolveBigData(Type type)
        { // resolving fields with bigDataLength
            var bt = type.BaseType;
            if (bt != typeof(object))
                ResolveBigData(bt);

            var df = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            For.ForFixedArrayIncrease(df, (finfo) =>
            {
                var abdl = finfo.GetCustomAttribute(BDL);
                var abdo = finfo.GetCustomAttribute(BDO);
                if (abdl is BigDataLengthAttribute bdl && (abdo is BigDataOffsetAttribute bdo))
                {
                    BigDataFields.Add(bdl.Id, (finfo, bdo));
                }
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
        public void DeserializeBigDataOffset(Stream stream, T obj)
        {
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredFields, (finfo) =>
            {
                if (finfo.GetCustomAttribute(BD) is BigDataAttribute bd)
                {
                    if (!BigDataFields.TryGetValue(bd.Id, out (FieldInfo, BigDataOffsetAttribute) _out))
                        throw new Exception("ты сначало атрибут с длиной поставь, ок?");

                    var len = (int)_out.Item1.GetValue(obj) + _out.Item2.Offset;
                    finfo.SetValue(obj, bread.ReadBytes(len));
                    return;
                }
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
        public void DeserializeBigData(Stream stream, T obj)
        {
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredFields, (finfo) =>
            {
                if (finfo.GetCustomAttribute(BD) is BigDataAttribute bd)
                {
                    if (!BigDataFields.TryGetValue(bd.Id, out (FieldInfo, BigDataOffsetAttribute) _out))
                        throw new Exception("ты сначало атрибут с длиной поставь, ок?");

                    finfo.SetValue(obj, bread.ReadBytes((int)_out.Item1.GetValue(obj)));
                    return;
                }
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
        public void DeserializeLessBigDataOffset(Stream stream, T obj)
        {
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredLessFields, (finfo) =>
            {
                if (finfo.GetCustomAttribute(BD) is BigDataAttribute bd)
                {
                    if (!BigDataFields.TryGetValue(bd.Id, out (FieldInfo, BigDataOffsetAttribute) _out))
                        throw new Exception("ты сначало атрибут с длиной поставь, ок?");

                    var len = (int)_out.Item1.GetValue(obj) + _out.Item2.Offset;
                    finfo.SetValue(obj, bread.ReadBytes(len));
                    return;
                }
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
        public void DeserializeLessBigData(Stream stream, T obj)
        {
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredLessFields, (finfo) =>
            {
                if (finfo.GetCustomAttribute(BD) is BigDataAttribute bd)
                {
                    if (!BigDataFields.TryGetValue(bd.Id, out (FieldInfo, BigDataOffsetAttribute) _out))
                        throw new Exception("ты сначало атрибут с длиной поставь, ок?");

                    finfo.SetValue(obj, bread.ReadBytes((int)_out.Item1.GetValue(obj)));
                    return;
                }
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
