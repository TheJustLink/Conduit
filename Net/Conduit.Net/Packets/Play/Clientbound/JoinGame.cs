﻿using Conduit.Net.Attributes;
using Conduit.Net.Data;

using fNbt.Tags;

namespace Conduit.Net.Packets.Play.Clientbound
{
    public sealed class JoinGame : Packet
    {
        public int EntityId;
        public bool IsHardcore;
        
        public Gamemode Gamemode;
        public sbyte PreviousGamemode;
        
        public string[] DimensionNames;
        public NbtCompound DimensionCodec;
        public NbtCompound Dimension;
        public string DimensionName;

        public long HashedSeed;

        [VarInt] public int MaxPlayers;
        [VarInt] public int ViewDistance;
        [VarInt] public int SimulationDistance;

        public bool ReducedDebugInfo;
        public bool EnableRespawnScreen;
        public bool IsDebug;
        public bool IsFlat;
    }
}