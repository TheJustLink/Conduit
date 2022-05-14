namespace Conduit.Net.Packets.Play
{
    public sealed class PlayerAbilities : Packet
    {
        public sbyte Flags;
        public float FlyingSpeed;
        public float FieldOfViewModifier;

        public PlayerAbilities() => Id = 0x32;
    }
}