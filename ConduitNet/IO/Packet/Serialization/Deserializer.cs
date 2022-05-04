using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using Conduit.Net.Attributes;
using Conduit.Net.Data;
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
        private static readonly int s_defaultIgnoredBaseTypeHash = typeof(object).GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static T Deserialize<T>(Stream input) where T : Packets.Packet, new()
        {
            using var rawPacketReader = new RawPacketReader(input, true);

            return Deserialize<T>(rawPacketReader.Read());
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static object DeserializeObject(BinaryReader input, Type type)
        {
            var @object = RuntimeHelpers.GetUninitializedObject(type);
            
            PopulateObject(input, type, @object);

            return @object;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void PopulateObject(BinaryReader input, Type type, object @object)
        {
            PopulateObject(input, type, @object, s_defaultIgnoredBaseTypeHash);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void PopulateObject(BinaryReader input, Type type, object @object, Type ignoredBaseType)
        {
            PopulateObject(input, type, @object, ignoredBaseType.GetHashCode());
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static void PopulateObject(BinaryReader input, Type type, object @object, int ignoredBaseTypeHash)
        {
            if (type.BaseType.GetHashCode() != ignoredBaseTypeHash)
                PopulateObject(input, type.BaseType, @object, ignoredBaseTypeHash);
            PopulateFields(input, type.GetDeclaredPublicFields(), @object);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void PopulateFields(BinaryReader input, FieldInfo[] fields, object @object)
        {
            var countFields = fields.Length;

            for (var i = 0; i < countFields; i++)
                PopulateField(input, fields[i], @object);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void PopulateField(BinaryReader input, FieldInfo field, object @object)
        {
            var asAttribute = field.GetCustomAttribute<AsAttribute>();
            var readTypeHash = asAttribute?.TypeHashCode ?? field.FieldType.GetHashCode();

            var value = readTypeHash == Json.TypeHash
                ? DeserializeJson(input, field.FieldType)
                : DeserializePrimitiveObject(input, field.FieldType, readTypeHash);
            
            field.SetValue(@object, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static object DeserializePrimitiveObject(BinaryReader input, Type type, int readTypeHash)
        {;
            if (BinaryReader.CanReadType(readTypeHash))
                return input.ReadObject(readTypeHash);

            if (type.IsEnum)
                return DeserializeEnum(input, type, readTypeHash);

            if (type.IsArray)
                return DeserializeArray(input, type, readTypeHash);

            if (type.IsClass)
                return DeserializeObject(input, type);

            throw new ArgumentException($"Can't deserialize {type}");
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static object DeserializeJson(BinaryReader input, Type type)
        {
            return JsonSerializer.Deserialize(input.ReadString(), type, s_jsonOptions);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static object DeserializeEnum(BinaryReader input, Type type, int readTypeHash)
        {
            var underlyingType = Enum.GetUnderlyingType(type);

            if (type.GetHashCode() == readTypeHash)
                readTypeHash = underlyingType.GetHashCode();

            return BinaryReader.ReadObject(input, readTypeHash);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static object DeserializeArray(BinaryReader input, Type type, int readTypeHash)
        {
            var elementType = type.GetElementType();

            if (type.GetHashCode() == readTypeHash)
                readTypeHash = elementType.GetHashCode();

            var count = input.Read7BitEncodedInt();
            var array = Array.CreateInstance(elementType, count);

            for (var i = 0; i < count; i++)
                array.SetValue(DeserializePrimitiveObject(input, elementType, readTypeHash), i);

            return array;
        }
    }
}