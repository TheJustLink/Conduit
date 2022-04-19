using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using Conduit.Net.Attributes;
using Conduit.Net.Extensions;
using fNbt;
using fNbt.Tags;
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
                : GetStandartObjectValue(input, field.FieldType);
        }
        private static object GetStandartObjectValue(BinaryReader input, Type type)
        {
            return type.IsEnum
                ? input.ReadObject(Enum.GetUnderlyingType(type))
                : type.IsArray
                ? DeserializeArray(input, type.GetElementType())
                : type.IsStandartValueType()
                ? input.ReadObject(type)
                : type == typeof(NbtCompound)
                ? DeserializeNbt(input)
                : DeserializeJson(input.ReadString(), type);
        }
        private static object DeserializeNbt(BinaryReader input)
        {
            var nbtReader = new NbtReader(input.BaseStream);
            return nbtReader.ReadAsTag();
        }
        private static object DeserializeJson(string json, Type type)
        {
            return JsonSerializer.Deserialize(json, type, s_jsonOptions);
        }
        private static Array DeserializeArray(BinaryReader input, Type elementType)
        {
            var count = input.Read7BitEncodedInt();
            var array = Array.CreateInstance(elementType, count);

            for (var i = 0; i < count; i++)
                array.SetValue(input.ReadObject(elementType), i);

            return array;
        }
    }
}