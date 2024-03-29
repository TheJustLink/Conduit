﻿using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play.Serverbound
{
    public sealed class ClientSettings : Packet
    {
        public string Locale;
        public sbyte ViewDistance;

        [VarInt] public int ChatMode;

        public bool ChatColors;
        public byte DisplayedSkinParts;

        [VarInt] public int MainHand;

        public bool EnableTextFiltering;
        public bool AllowServerListings;
    }
}