namespace Conduit.Net.Packets.Play
{
    public class PlayerAbilities : Packet
    {
        public sbyte Flags;
        public float FlyingSpeed;//0.05 default value
        public float FieldOfViewModifier;

        public PlayerAbilities() => Id = 0x32;
    }
}
