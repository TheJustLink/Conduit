using Conduit.Hosting;
using Conduit.Network.Protocol.Serializable;
using Conduit.Network.Serialization.Attributes;
using Conduit.Utilities;
using FastMember;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Conduit.Network.Serialization
{
    public sealed class Serializator<TPacket> : ISerializator where TPacket : Packet, new()
    {
        private Type TType;
        private List<DataContain> DeclaredFields;
        private List<DataContain> DeclaredLessFields; // without packet fields
        private Dictionary<ulong, (PropertyInfo, BigDataOffsetAttribute)> BigDataFields;

        private TypeAccessor TypeA;

        private static Type VIA = typeof(VarIntAttribute);
        private static Type VLA = typeof(VarLongAttribute);
        private static Type BD = typeof(BigDataAttribute);
        private static Type BDO = typeof(BigDataOffsetAttribute);
        private static Type BDL = typeof(BigDataLengthAttribute);
        private static Type IGN = typeof(IgnoreAttribute);

        public Serializator(bool withbigdata = false)
        {
            TType = typeof(TPacket);
            TypeA = TypeAccessor.Create(TType);
            DeclaredFields = new List<DataContain>();
            DeclaredLessFields = new List<DataContain>();

            Resolve(TType);
            ResolveLess(TType);

            if (withbigdata)
            {
                BigDataFields = new Dictionary<ulong, (PropertyInfo, BigDataOffsetAttribute)>();
                ResolveBigData(TType);
            }
        }
        private void ResolveLess(Type type)
        { // resolving with out baseclass "Packet"
            var bt = type.BaseType;
            if (bt != typeof(object) && bt != typeof(Packet))
                ResolveLess(bt);

            var df = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            For.ForFixedArrayIncrease(df, (pinfo) =>
            {
                if (pinfo.GetCustomAttribute(IGN) is null)
                    DeclaredLessFields.Add(new DataContain(TType, pinfo,
                        pinfo.GetCustomAttribute(VIA) is not null,
                        pinfo.GetCustomAttribute(VLA) is not null,
                        pinfo.GetCustomAttribute(BD) is not null,
                        pinfo.PropertyType.IsArray));
            });
        }
        private void Resolve(Type type)
        { // full resolve
            var bt = type.BaseType;
            if (bt != typeof(object))
                Resolve(bt);

            var df = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            For.ForFixedArrayIncrease(df, (pinfo) =>
            {
                if (pinfo.GetCustomAttribute(IGN) is null)
                    DeclaredFields.Add(new DataContain(TType, pinfo,
                        pinfo.GetCustomAttribute(VIA) is not null,
                        pinfo.GetCustomAttribute(VLA) is not null,
                        pinfo.GetCustomAttribute(BD) is not null,
                        pinfo.PropertyType.IsArray));
            });
        }
        private void ResolveBigData(Type type)
        { // resolving fields with bigDataLength
            var bt = type.BaseType;
            if (bt != typeof(object))
                ResolveBigData(bt);

            var df = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            For.ForFixedArrayIncrease(df, (pinfo) =>
            {
                if (pinfo.GetCustomAttribute(IGN) is null)
                {
                    var abdl = pinfo.GetCustomAttribute(BDL);
                    var abdo = pinfo.GetCustomAttribute(BDO);
                    if (abdl is BigDataLengthAttribute bdl && (abdo is BigDataOffsetAttribute bdo))
                    {
                        BigDataFields.Add(bdl.Id, (pinfo, bdo));
                    }
                }
            });
        }

        public void Deserialize(Stream stream, TPacket obj)
        {
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredFields, (dc) =>
            {
                if (dc.HasVIA) 
                {
                    dc.PropertyInfo.SetValue(obj, bread.Read7BitEncodedInt());
                    return;
                }
                if (dc.HasVLA)
                {
                    dc.PropertyInfo.SetValue(obj, bread.Read7BitEncodedInt64());
                    return;
                }

                dc.PropertyInfo.SetValue(obj, bread.ReadObject(dc.PropertyInfo.PropertyType));
            });
        }
        public void DeserializeBigDataOffset(Stream stream, TPacket obj)
        {
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredFields, (dc) =>
            {
                if (dc.PropertyInfo.GetCustomAttribute(BD) is BigDataAttribute bd)
                {
                    if (!BigDataFields.TryGetValue(bd.Id, out (PropertyInfo, BigDataOffsetAttribute) _out))
                        throw new Exception("ты сначало атрибут с длиной поставь, ок?");
                    var len = (int)_out.Item1.GetValue(obj) + _out.Item2.Offset;
                    dc.PropertyInfo.SetValue(obj, bread.ReadBytes(len));
                    return;
                }
                if (dc.HasVIA)
                {
                    dc.PropertyInfo.SetValue(obj, bread.Read7BitEncodedInt());
                    return;
                }
                if (dc.HasVLA)
                {
                    dc.PropertyInfo.SetValue(obj, bread.Read7BitEncodedInt64());
                    return;
                }

                dc.PropertyInfo.SetValue(obj, bread.ReadObject(dc.PropertyInfo.PropertyType));
            });
        }

        /* [is not using now but can be in future]
        
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
        */

        public void DeserializeLess(Stream stream, TPacket obj)
        {
            using var bread = new PBinaryReader(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredLessFields, (dc) =>
            {
                if (dc.HasVIA)
                {
                    dc.PropertyInfo.SetValue(obj, bread.Read7BitEncodedInt());
                    return;
                }
                if (dc.HasVLA)
                {
                    dc.PropertyInfo.SetValue(obj, bread.Read7BitEncodedInt64());
                    return;
                }

                dc.PropertyInfo.SetValue(obj, bread.ReadObject(dc.PropertyInfo.PropertyType));
            });
        }
        public void Serialize(Stream stream, TPacket data)
        {
            using var memstream = new MemoryStream();
            SerializeObject(memstream, data);

            Array.Clear(memstream.GetBuffer());

            data.Length = (int)memstream.Length - 1;
            SerializeObject(memstream, data);

            stream.Write(memstream.GetBuffer());
        }
        public void SerializeObject(Stream stream, TPacket data)
        {
            var bwrite = new PBinaryWriter(stream, Encoding.UTF8, true);
            For.ForFixedListIncrease(DeclaredFields, (dc) =>
            {
                dynamic obj = dc.Getter(data); //dc.PropertyInfo.GetValue(data);
                //var obj = dc.PropertyInfo.GetValue(data);
                if (obj is null)
                    throw new Exception($"{dc.PropertyInfo.Name} value null");

                if (dc.HasVIA)
                    bwrite.Write7BitEncodedInt(obj);
                else if (dc.HasVLA)
                    bwrite.Write7BitEncodedInt64(obj);
                else
                    bwrite.Write(obj);
            });
        }

        public void Deserialize(Stream stream, object data) => Deserialize(stream, (TPacket)data);

        public void Serialize(Stream stream, object data) => Serialize(stream, (TPacket)data);
    }
}
