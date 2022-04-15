using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using Conduit.Net.Attributes;
using Conduit.Net.Extensions;

using BinaryReader = Conduit.Net.IO.Binary.Reader;
using RawPacketReader = Conduit.Net.IO.RawPacket.Reader;

namespace Conduit.Net.IO.Packet
{
    public static class Deserializer
    {
        private static readonly JsonSerializerOptions s_jsonOptions;

        static Deserializer()
        {
            s_jsonOptions = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }
        
        public static T Deserialize<T>(Stream input) where T : Packets.Packet, new()
        {
            using var rawPacketReader = new RawPacketReader(input, true);

            return Deserialize<T>(rawPacketReader.Read());
        }
        public static T Deserialize<T>(Packets.RawPacket rawPacket) where T : Packets.Packet, new()
        {
            var type = typeof(T);
            var @object = new T
            {
                Length = rawPacket.Length,
                Id = rawPacket.Id
            };
            
            using var binaryReader = new BinaryReader(rawPacket.Data);
            PopulateObject(binaryReader, type, @object);

            return @object;
        }

        private static void PopulateObject(BinaryReader reader, Type type, object @object)
        {
            if (type.BaseType != typeof(Packets.Packet))
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
                : DeserializeJson(input.ReadString(), field.FieldType);
        }
        private static object DeserializeJson(string json, Type type)
        {
            return JsonSerializer.Deserialize(json, type, s_jsonOptions);
        }
    }
}