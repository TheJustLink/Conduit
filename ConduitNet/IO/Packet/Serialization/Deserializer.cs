using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using Conduit.Net.Data;
using Conduit.Net.Attributes;
using Conduit.Net.Extensions;
using Conduit.Net.Reflection;

using BinaryReader = Conduit.Net.IO.Binary.Reader;

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

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static Packets.Packet Deserialize(Packets.RawPacket rawPacket, Type type)
        {
            var packet = Unsafe.As<Packets.Packet>(RuntimeHelpers.GetUninitializedObject(type));

            using var reader = new BinaryReader(rawPacket.Data);
            PopulateObject(reader, type, packet, Object<Packets.Packet>.HashCode);

            return packet;
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
            PopulateObject(input, type, @object, Object<object>.HashCode);
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
        {
            if (type.IsEnum)
                return DeserializeEnum(input, type, readTypeHash);

            if (type.IsArray)
                return DeserializeArray(input, type, readTypeHash);

            if (BinaryReader.CanReadType(readTypeHash))
                return input.ReadObject(readTypeHash);

            if (type.IsClass || type.IsValueType)
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
            if (type.GetHashCode() == readTypeHash)
                readTypeHash = Enum.GetUnderlyingType(type).GetHashCode();

            var value = BinaryReader.ReadObject(input, readTypeHash);

            return Converters.Enum.ConvertObject(type, value);
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