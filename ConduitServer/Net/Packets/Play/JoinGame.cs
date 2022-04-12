using ConduitServer.Serialization.Attributes;

namespace ConduitServer.Net.Packets.Play
{
    class JoinGame : Packet
    {
        public int EntityId;
        public bool IsHardcore;
        public byte Gamemode;
        public sbyte PreviousGamemode;
        [VarInt]
        public int WorldCount;
        public ushort[] DimensionNames;
        public string DimensionCodec;
        public string Dimension;
        public ushort DimensionName;
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
