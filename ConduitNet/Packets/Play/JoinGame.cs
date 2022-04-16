using Conduit.Net.Attributes;
using fNbt;
using Conduit.Net.Data;
using fNbt.Tags;

namespace Conduit.Net.Packets.Play
{
    public class JoinGame : Packet
    {
        public int EntityId;
        public bool IsHardcore;
        public Gamemode Gamemode;
        public sbyte PreviousGamemode;
        //[VarInt]
        //public int WorldCount;
        public string[] DimensionNames;
        public NbtCompound DimensionCodec;
        public NbtCompound Dimension;
        public string DimensionName;
        public long HashedSeed;
        [VarInt]
        public int MaxPlayers;
        [VarInt]
        public int ViewDistance;
        [VarInt]
        public int SimulationDistance;
        public bool ReducedDebugInfo;
        public bool EnableRespawnScreen;
        public bool IsDebug;
        public bool IsFlat;

        public JoinGame() => Id = 0x26;
    }
}