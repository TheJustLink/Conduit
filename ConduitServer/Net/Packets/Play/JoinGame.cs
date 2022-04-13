using ConduitServer.Serialization.Attributes;
using ConduitServer.Nbt;

namespace ConduitServer.Net.Packets.Play
{
    class JoinGame : Packet
    {
        public int EntityId;
        public bool IsHardcore;
        public Gamemode Gamemode;
        public sbyte PreviousGamemode;
        [VarInt]
        public int WorldCount;
        public long[] DimensionNames;
        public MixedCodec DimensionCodec;
        public DimensionCodec Dimension;
        public string DimensionName;
        public long HashedSeed;
        [VarInt]
        public int MaxPlayers;
        [VarInt]
        public int ViewDistance;
        [VarInt]
        public int SimulationDistance;
        public bool EnableRespawnScreen;
        public bool IsDebug;
        public bool IsFlat;

        public JoinGame() => Id = 0x26;
    }
}