﻿namespace Conduit.Net.Packets.Play
{
    public sealed class HeldItemChange : Packet
    {
        /// <summary>
        /// Slot number between 0 and 8
        /// </summary>
        public sbyte Slot;
        // Or short?? Or byte???
    }
}