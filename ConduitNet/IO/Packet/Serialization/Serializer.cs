using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Conduit.Net.Attributes;
using Conduit.Net.Data;
using Conduit.Net.Extensions;

using BinaryWriter = Conduit.Net.IO.Binary.Writer;
using RawPacketWriter = Conduit.Net.IO.RawPacket.Writer;

namespace Conduit.Net.IO.Packet.Serialization
{
    public static class Serializer
    {
        private static readonly JsonSerializerOptions s_jsonOptions;
        private static readonly int s_defaultIgnoredBaseTypeHash = typeof(object).GetHashCode();
        
        static Serializer()
        {
            s_jsonOptions = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static void Serialize<T>(T packet, Stream output) where T : Packets.Packet
        {
            var rawPacket = Serialize(packet);

            using var rawPacketWriter = new RawPacketWriter(output, true);
            rawPacketWriter.Write(rawPacket);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static Packets.RawPacket Serialize<T>(T packet) where T : Packets.Packet
        {
            using var dataStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(dataStream, Encoding.UTF8);
            
            SerializeObject(binaryWriter, packet, typeof(Packets.Packet));
            var data = dataStream.ToArray();
            
            return new Packets.RawPacket
            {
                Id = packet.Id,
                Data = data,
                Length = data.Length + 1
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void SerializeObject(BinaryWriter writer, Type type, object @object)
        {
            SerializeObject(writer, type, @object, s_defaultIgnoredBaseTypeHash);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void SerializeObject(BinaryWriter writer, object @object, Type ignoredBaseType)
        {
            SerializeObject(writer, @object.GetType(), @object, ignoredBaseType.GetHashCode());
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static void SerializeObject(BinaryWriter writer, Type type, object @object, int ignoredBaseTypeHash)
        {
            if (type.BaseType.GetHashCode() != ignoredBaseTypeHash)
                SerializeObject(writer, type.BaseType, @object, ignoredBaseTypeHash);
            SerializeFields(writer, @object, type.GetDeclaredPublicFields());
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void SerializeFields(BinaryWriter writer, object @object, FieldInfo[] fields)
        {
            var countFields = fields.Length;

            for (var i = 0; i < countFields; i++)
                SerializeField(writer, @object, fields[i]);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void SerializeField(BinaryWriter writer, object @object, FieldInfo field)
        {
            var value = field.GetValue(@object);
            if (value is null)
                throw new SerializationException($"Value of field {field.Name} empty in {@object}");

            var asAttribute = field.GetCustomAttribute<AsAttribute>();
            var targetTypeHash = asAttribute?.TypeHashCode ?? field.FieldType.GetHashCode();
            
            if (targetTypeHash == Json.TypeHash)
                SerializeJson(writer, value, field.FieldType);
            else SerializePrimitiveObject(writer, value, field.FieldType, targetTypeHash);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static void SerializePrimitiveObject(BinaryWriter writer, object @object, Type type, int targetTypeHash)
        {
            if (BinaryWriter.CanWriteType(targetTypeHash))
                BinaryWriter.WriteObject(writer, @object, targetTypeHash);
            else if (type.IsEnum)
                SerializeEnum(writer, @object, type, targetTypeHash);
            else if (type.IsArray)
                SerializeArray(writer, (Array)@object, type, targetTypeHash);
            else if (type.IsClass)
                SerializeObject(writer, type, @object);
            else throw new ArgumentException($"Can't serialize object {@object} of type {type}");
        }

        private static void SerializeJson(BinaryWriter writer, object @object, Type type)
        {
            writer.Write(JsonSerializer.Serialize(@object, type, s_jsonOptions));
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void SerializeEnum(BinaryWriter writer, object @object, Type type, int targetTypeHash)
        {
            var underlyingType = Enum.GetUnderlyingType(type);
            var underlyingObject = Convert.ChangeType(@object, underlyingType);

            if (type.GetHashCode() == targetTypeHash)
                targetTypeHash = underlyingType.GetHashCode();

            BinaryWriter.WriteObject(writer, underlyingObject, targetTypeHash);
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static void SerializeArray(BinaryWriter writer, Array array, Type type, int targetTypeHash)
        {
            var elementType = type.GetElementType();

            if (type.GetHashCode() == targetTypeHash)
                targetTypeHash = elementType.GetHashCode();

            writer.Write7BitEncodedInt(array.Length);
            
            foreach (var item in array)
                SerializePrimitiveObject(writer, item, elementType, targetTypeHash);
        }
    }
}