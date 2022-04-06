using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using ConduitServer.Net.Packets;
using ConduitServer.Serialization.Attributes;

namespace ConduitServer.Serialization.Packets
{
    //class PacketSerializer : IPacketSerializer
    //{
    //    public byte[] Serialize<T>(T packet) where T : Packet
    //    {
    //        var memoryStream = new MemoryStream();
    //        var writer = new BinaryWriter(memoryStream);

    //        var type = typeof(T);
    //        var fields = type.GetFields();

    //        foreach (var field in fields)
    //            SerializeField(BinaryWriter writer, packet, field);
    //    }

    //    private void SerializeField(BinaryWriter writer, object @object, FieldInfo field)
    //    {
    //        var value = field.GetValue(@object);
    //        if (value == null)
    //            throw new SerializationException($"Value of field {field.Name} empty");

    //        if (field.GetCustomAttribute(typeof(VarIntAttribute), true) != null)
    //            return SerializeVarInt((int)value);
    //        if (field.GetCustomAttribute(typeof(VarLongAttribute), true) != null)
    //            return SerializeVarLong((long)value);

    //        writer.Write();
    //        writer.Write();
    //    }
    //}
}