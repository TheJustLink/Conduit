﻿namespace Conduit.Net.Packets.Play.Clientbound
{
    // Forget chunk in Mojang edition
    public class UnloadChunk : Packet
    {
        public int ChunkX;
        public int ChunkZ;
    }
}