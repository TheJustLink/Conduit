namespace Conduit.Net.Packets.Play
{
    public class HeldItemChange : Packet
    {
        // Value between 0 and 8
        public sbyte Slot;

        public HeldItemChange() => Id = 0x48;
    }
}