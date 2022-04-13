using System;
using System.IO;
using System.Reflection;
using System.Text;

using ConduitServer.Extensions;
using ConduitServer.Net.Packets;
using ConduitServer.Serialization.Attributes;

using BinaryReader = ConduitServer.Net.BinaryReader;

namespace ConduitServer.Serialization.Packets
{
    class PacketDeserializer : IPacketDeserializer
    {
        public T Deserialize<T>(Stream input) where T : Packet, new()
        {
            using var reader = new BinaryReader(input, Encoding.UTF8, true);

            var type = typeof(T);
            var @object = new T();
            
            PopulateObject(reader, type, @object);

            return @object;
        }

        private static void PopulateObject(BinaryReader input, Type type, object @object)
        {
            if (type.BaseType != typeof(object))
                PopulateObject(input, type.BaseType, @object);
            PopulateFields(input, type.GetDeclaredPublicFields(), @object);
        }
        private static void PopulateFields(BinaryReader input, FieldInfo[] fields, object @object)
        {
            foreach (var field in fields)
                PopulateField(input, field, @object);
        }
        private static void PopulateField(BinaryReader input, FieldInfo field, object @object)
        {
            field.SetValue(@object, GetObjectValue(input, field));
        }
        private static object GetObjectValue(BinaryReader input, FieldInfo field)
        {
            return field.GetCustomAttribute(typeof(VarIntAttribute)) is not null
                ? input.Read7BitEncodedInt()
                : field.GetCustomAttribute(typeof(VarLongAttribute)) is not null
                ? input.Read7BitEncodedInt64()
                : input.ReadObject(field.FieldType);
        }
    }
}