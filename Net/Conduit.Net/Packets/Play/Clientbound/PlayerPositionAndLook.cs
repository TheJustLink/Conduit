using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play.Clientbound
{
    public sealed class PlayerPositionAndLook : Packet
    {
        public double X;
        public double Y;
        public double Z;
        public float Yaw;
        public float Pitch;
        public byte Flags;

        [VarInt] public int TeleportId;
        
        public bool DismountVehicle;
    }
}