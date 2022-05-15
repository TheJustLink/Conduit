using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Conduit.Net.Reflection;
using Conduit.Net.Attributes;

namespace Conduit.Net.Packets
{
    public static class PacketMap
    {
        private static readonly Dictionary<byte, int> s_idTypeHashTable;
        private static readonly Dictionary<int, byte> s_typeIdHashTable;

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool Has(byte id) => s_idTypeHashTable.ContainsKey(id);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>() where T : Packet => Has(Object<T>.HashCode);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool Has(Type packetType) => Has(packetType.GetHashCode());
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static bool Has(int packetTypeHash) => s_typeIdHashTable.ContainsKey(packetTypeHash);

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static int GetTypeHash(byte id) => s_idTypeHashTable[id];
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static byte GetId<T>() where T : Packet => GetId(Object<T>.HashCode);
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static byte GetId(Type type) => GetId(type.GetHashCode());
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static byte GetId(int typeHash) => s_typeIdHashTable[typeHash];

        static PacketMap()
        {
            s_idTypeHashTable = new Dictionary<byte, int>();

            var implementations = Object<Packet>.GetImplementations();
            foreach (var implementation in implementations)
            {
                var attribute = implementation.GetCustomAttribute<PacketAttribute>();
                if (attribute == null)
                    throw new MissingFieldException($"Attribute {typeof(PacketAttribute)} not set in {implementation}");

                s_idTypeHashTable.Add(attribute.Id, implementation.ReflectedType.GetHashCode());
            }

            s_idTypeHashTable.TrimExcess();
            s_typeIdHashTable = s_idTypeHashTable.ToDictionary(keyPair => keyPair.Value, keyPair => keyPair.Key);
        }
    }
}