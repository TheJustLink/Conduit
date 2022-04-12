using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

using ConduitServer.Extensions;
using ConduitServer.Net.Packets;
using ConduitServer.Serialization.Attributes;

using BinaryWriter = ConduitServer.Net.BinaryWriter;

namespace ConduitServer.Serialization.Packets
{
    class PacketSerializer : IPacketSerializer
    {
        private static readonly JsonSerializerOptions s_jsonOptions;

        static PacketSerializer()
        {
            s_jsonOptions = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public void Serialize<T>(Stream output, T packet) where T : Packet
        {
            using var writer = new BinaryWriter(output, Encoding.UTF8, true);
            using var bufferStream = new MemoryStream();
            using var bufferWriter = new BinaryWriter(bufferStream, Encoding.UTF8);

            var type = typeof(T);
            SerializeObject(bufferWriter, type, packet);
            
            var buffer = bufferStream.ToArray();
            packet.Length = buffer.Length + 1;
            
            SerializeObject(writer, typeof(Packet), packet);
            writer.Write(buffer);
        }

        private static void SerializeObject(BinaryWriter writer, Type type, dynamic @object)
        {
            if (type.BaseType != typeof(Packet) && type.BaseType != typeof(object))
                SerializeObject(writer, type.BaseType, @object);
            SerializeFields(writer, @object, type.GetDeclaredPublicFields());
        }
        private static void SerializeFields(BinaryWriter writer, dynamic @object, FieldInfo[] fields)
        {
            foreach (var field in fields)
                SerializeField(writer, @object, field);
        }
        private static void SerializeField(BinaryWriter writer, dynamic @object, FieldInfo field)
        {
            var value = field.GetValue(@object);
            if (value is null)
                throw new SerializationException($"Value of field {field.Name} empty");
            
            if (field.GetCustomAttribute<VarIntAttribute>(true) is not null)
                writer.Write7BitEncodedInt(value);
            else if (field.GetCustomAttribute<VarLongAttribute>(true) is not null)
                writer.Write7BitEncodedInt64(value);
            else SerializeValue(writer, value, field.FieldType);
        }
        private static void SerializeValue(BinaryWriter writer, dynamic value, Type type)
        {
            if (type.IsArray)
                SerializeArray(writer, value, type.GetElementType());
            else if (type.IsEnum)
                writer.Write(Convert.ChangeType(value, Enum.GetUnderlyingType(type)));
            else if (type.IsStandartValueType())
                writer.Write(value);
            else writer.Write(JsonSerializer.Serialize(value, s_jsonOptions));
        }
        private static void SerializeArray(BinaryWriter writer, dynamic array, Type valueType)
        {
            if (valueType.IsStandartValueType())
            {
                foreach (var item in array)
                    writer.Write(item);
            }
            else
            {
                foreach (var item in array)
                    writer.Write(JsonSerializer.Serialize(item, s_jsonOptions));
            }
        }
    }
}