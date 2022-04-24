namespace Conduit.Net.Packets.Play
{
    public class HeldItemChange : Packet
    {
        public sbyte Slot;//value between 0 and 8

        public HeldItemChange() => Id = 0x48;
    }
}
