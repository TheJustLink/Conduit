using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

using Conduit.Net.Extensions;
using Conduit.Net.Packets;
using Conduit.Net.Serialization.Attributes;

using BinaryReader = Conduit.Net.IO.Binary.Reader;
using RawPacketReader = Conduit.Net.IO.RawPacket.Reader;

namespace Conduit.Net.Serialization
{
    public class PacketDeserializer
    {
        private static readonly JsonSerializerOptions s_jsonOptions;

        static PacketDeserializer()
        {
            s_jsonOptions = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }
        
        public static T Deserialize<T>(Stream input) where T : Packet, new()
        {
            using var binaryReader = new BinaryReader(input, Encoding.UTF8, true);
            using var rawPacketReader = new RawPacketReader(binaryReader);

            return Deserialize<T>(rawPacketReader.Read());
        }
        public static T Deserialize<T>(RawPacket rawPacket) where T : Packet, new()
        {
            var type = typeof(T);
            var @object = new T
            {
                Length = rawPacket.Length,
                Id = rawPacket.Id
            };

            using var dataStream = new MemoryStream(rawPacket.Data, false);
            using var binaryReader = new BinaryReader(dataStream, Encoding.UTF8, false);

            PopulateObject(binaryReader, type, @object);

            return @object;
        }

        private static void PopulateObject(BinaryReader reader, Type type, object @object)
        {
            if (type.BaseType != typeof(Packet))
                PopulateObject(reader, type.BaseType, @object);
            PopulateFields(reader, type.GetDeclaredPublicFields(), @object);
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
                : field.FieldType.IsStandartValueType()
                ? input.ReadObject(field.FieldType)
                : JsonSerializer.Deserialize(input.ReadString(), field.FieldType, s_jsonOptions);
        }
    }
}