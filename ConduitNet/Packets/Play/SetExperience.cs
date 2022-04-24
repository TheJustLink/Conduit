using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class SetExperience : Packet
    {
        public float ExperienceBar; //value between 0 and 1
        [VarInt]
        public int Level;
        [VarInt]
        public int TotalExperience;

        public SetExperience() => Id = 0x51;
    }
}
