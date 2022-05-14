﻿namespace Conduit.Net.Packets.Play
{
    // Forget chunk in Mojang edition
    public class UnloadChunk : Packet
    {
        public int ChunkX;
        public int ChunkZ;

        public UnloadChunk() => Id = 0x1D;
    }
}