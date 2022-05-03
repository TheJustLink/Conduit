using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Conduit.Net.Extensions;

using BinaryWriter = Conduit.Net.IO.Binary.Writer;
using RawPacketWriter = Conduit.Net.IO.RawPacket.Writer;

namespace Conduit.Net.IO.Packet.Serialization
{
    public static class Serializer
    {
        private static readonly JsonSerializerOptions s_jsonOptions;
        private static readonly Type s_defaultIgnoredBaseType = typeof(object);

        private delegate void WriteDelegate(BinaryWriter writer, object @object);

        static Serializer()
        {
            s_jsonOptions = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };


        }
        
        public static void Serialize<T>(T packet, Stream output) where T : Packets.Packet
        {
            var rawPacket = Serialize(packet);

            using var rawPacketWriter = new RawPacketWriter(output, true);
            rawPacketWriter.Write(rawPacket);
        }
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

        private static void SerializeObject(BinaryWriter writer, object @object)
        {
            SerializeObject(writer, @object, s_defaultIgnoredBaseType);
        }
        private static void SerializeObject(BinaryWriter writer, object @object, Type ignoredBaseType)
        {
            SerializeObject(writer, @object.GetType(), @object, ignoredBaseType);
        }
        private static void SerializeObject(BinaryWriter writer, Type type, object @object, Type ignoredBaseType)
        {
            if (type.BaseType != ignoredBaseType)
                SerializeObject(writer, type.BaseType, @object, ignoredBaseType);
            SerializeFields(writer, @object, type.GetDeclaredPublicFields());
        }

        private static void SerializeFields(BinaryWriter writer, object @object, FieldInfo[] fields)
        {
            foreach (var field in fields)
                SerializeField(writer, @object, field);
        }
        private static void SerializeField(BinaryWriter writer, object @object, FieldInfo field)
        {
            var value = field.GetValue(@object);
            if (value is null)
                throw new SerializationException($"Value of field {field.Name} empty");
            
            SerializePrimitiveObject(writer, value, field.FieldType);
        }

        private static void SerializePrimitiveObject(BinaryWriter writer, object @object, Type type)
        {
            if (type.Attributes.HasFlag(TypeAttributes.Serializable))
                SerializeJson(writer, @object, type);
            else if (type.IsEnum)
                SerializeEnum(writer, @object, type);
            else if (type.IsArray)
                SerializeArray(writer, (Array)@object, type.GetElementType());
            
        }

        private static void SerializeJson(BinaryWriter writer, object @object, Type type)
        {
            writer.Write(JsonSerializer.Serialize(@object, type, s_jsonOptions));
        }
        private static void SerializeEnum(BinaryWriter writer, object @object, Type type)
        {
            //writer.WriteObject(Convert.ChangeType(@object, Enum.GetUnderlyingType(type)));
        }
        private static void SerializeArray(BinaryWriter writer, Array array, Type elementType)
        {
            writer.Write7BitEncodedInt(array.Length);

            foreach (var item in array)
                SerializePrimitiveObject(writer, item, elementType);
        }
    }
}