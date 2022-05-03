using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using Conduit.Net.Extensions;

using BinaryReader = Conduit.Net.IO.Binary.Reader;
using RawPacketReader = Conduit.Net.IO.RawPacket.Reader;

namespace Conduit.Net.IO.Packet.Serialization
{
    public static class Deserializer
    {
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            IncludeFields = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        private static readonly Type s_defaultIgnoredBaseType = typeof(object);

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
            PopulateObject(binaryReader, type, @object, typeof(Packets.Packet));

            return @object;
        }

        private static object DeserializeObject(BinaryReader input, Type type)
        {
            var @object = RuntimeHelpers.GetUninitializedObject(type);
            
            PopulateObject(input, type, @object);

            return @object;
        }

        private static void PopulateObject(BinaryReader input, Type type, object @object)
        {
            PopulateObject(input, type, @object, s_defaultIgnoredBaseType);
        }
        private static void PopulateObject(BinaryReader input, Type type, object @object, Type ignoredBaseType)
        {
            if (type.BaseType != ignoredBaseType)
                PopulateObject(input, type.BaseType, @object, ignoredBaseType);
            PopulateFields(input, type.GetDeclaredPublicFields(), @object);
        }

        private static void PopulateFields(BinaryReader input, FieldInfo[] fields, object @object)
        {
            foreach (var field in fields)
                PopulateField(input, field, @object);
        }
        private static void PopulateField(BinaryReader input, FieldInfo field, object @object)
        {
            field.SetValue(@object, DeserializePrimitiveObject(input, field.FieldType));
        }
        
        private static object DeserializePrimitiveObject(BinaryReader input, Type type)
        {;
            if (type.Attributes.HasFlag(TypeAttributes.Serializable))
                return DeserializeJson(input, type);

            if (type.IsEnum)
                return input.ReadObject(Enum.GetUnderlyingType(type));

            if (type.IsArray)
                return DeserializeArray(input, type.GetElementType());

            if (BinaryReader.CanReadType(type))
                return input.ReadObject(type);
            
            return DeserializeObject(input, type);
        }
        private static object DeserializeJson(BinaryReader input, Type type)
        {
            return JsonSerializer.Deserialize(input.ReadString(), type, s_jsonOptions);
        }
        private static object DeserializeArray(BinaryReader input, Type elementType)
        {
            var count = input.Read7BitEncodedInt();
            var array = Array.CreateInstance(elementType, count);

            for (var i = 0; i < count; i++)
                array.SetValue(DeserializePrimitiveObject(input, elementType), i);

            return array;
        }
    }
}