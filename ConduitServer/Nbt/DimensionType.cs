namespace ConduitServer.Nbt
{
    class DimensionType
    {
        public byte PiglinSafe;
        public byte Natural;
        public float AmbientLight = 0.0f;
        public long FixedTime;
        public string Infiniburn = "minecraft:infiniburn_overworld";
        public byte RespawnAnchorWorks;
        public byte HasSkylight;
        public byte BedWorks;
        public string Effects = "minecraft:overworld";
        public byte HasRaids;
        public int MinY = -64;
        public int Height = 384;
        public int LogicalHeight = 384;
        public double CoordinateScale = 1.0f;
        public byte UltraWarm;
        public byte HasCailing;
    }
}
