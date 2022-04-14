using Conduit.Net.Serialization.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class ClientSettings : Packet
    {
        public string Locale;
        public sbyte ViewDistance;
        [VarInt]
        public int ChatMode;
        public bool ChatColors;
        public byte DisplayedSkinParts;
        [VarInt]
        public int MainHand;
        public bool EnableTextFiltering;
        public bool AllowServerListings;

        public ClientSettings() => Id = 0x05;
    }
}