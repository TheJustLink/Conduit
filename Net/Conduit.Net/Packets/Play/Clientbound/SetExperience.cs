using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play.Clientbound
{
    public sealed class SetExperience : Packet
    {
        /// <summary>
        /// Value between 0 and 1
        /// </summary>
        public float ExperienceBar;

        [VarInt] public int Level;
        [VarInt] public int TotalExperience;
    }
}