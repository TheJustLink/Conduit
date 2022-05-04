using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class SetExperience : Packet
    {
        // Value between 0 and 1
        public float ExperienceBar;

        [VarInt] public int Level;
        [VarInt] public int TotalExperience;

        public SetExperience() => Id = 0x51;
    }
}