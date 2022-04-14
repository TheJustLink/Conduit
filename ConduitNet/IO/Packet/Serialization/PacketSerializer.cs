using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using fNbt;

using Conduit.Net.Extensions;
using Conduit.Net.Packets;
using Conduit.Net.Serialization.Attributes;

using BinaryWriter = Conduit.Net.IO.Binary.Writer;

namespace Conduit.Net.Serialization
{
    public class PacketSerializer
    {
        private static readonly JsonSerializerOptions s_jsonOptions;

        static PacketSerializer()
        {
            s_jsonOptions = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }
        
        public static void Serialize<T>(T packet, Stream output) where T : Packet
        {
            var rawPacket = Serialize(packet);

            // TODO!
        }
        public static RawPacket Serialize<T>(T packet) where T : Packet
        {
            using var dataStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(dataStream, Encoding.UTF8, false);

            var rawPacket = new RawPacket { Id = packet.Id };
            var type = typeof(T);
            
            SerializeObject(binaryWriter, type, packet);
            rawPacket.Data = dataStream.GetBuffer();
            rawPacket.Length = rawPacket.Data.Length + 1;

            return rawPacket;
        }

        private static void SerializeObject(BinaryWriter writer, Type type, object @object)
        {
            if (type.BaseType != typeof(Packet))
                SerializeObject(writer, type.BaseType, @object);
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
            
            if (field.GetCustomAttribute<VarIntAttribute>(true) is not null)
                writer.Write7BitEncodedInt((int)value);
            else if (field.GetCustomAttribute<VarLongAttribute>(true) is not null)
                writer.Write7BitEncodedInt64((long)value);
            else SerializeValue(writer, value, field.FieldType);
        }
        private static void SerializeValue(BinaryWriter writer, object value, Type type)
        {
            if (type.IsArray)
                SerializeArray(writer, (Array)value, type.GetElementType());
            else if (type.IsEnum)
                writer.WriteObject(Convert.ChangeType(value, Enum.GetUnderlyingType(type)));
            else if (value is NbtTag tag)
                new NbtWriter(writer.BaseStream, "Data").WriteTag(tag);
            else if (type.IsStandartValueType())
                writer.WriteObject(value);
            else writer.Write(JsonSerializer.Serialize(value, s_jsonOptions));
        }
        private static void SerializeArray(BinaryWriter writer, Array array, Type valueType)
        {
            if (valueType.IsStandartValueType())
            {
                foreach (var item in array)
                    writer.WriteObject(item);
            }
            else
            {
                foreach (var item in array)
                    writer.Write(JsonSerializer.Serialize(item, s_jsonOptions));
            }
        }
    }
}