using fNbt;

namespace ConduitServer.Nbt
{
    class DimensionType
    {
        public NbtByte PiglinSafe;
        public NbtByte Natural;
        public NbtFloat AmbientLight = new NbtFloat(0.0f);
        public NbtLong FixedTime;
        public NbtString Infiniburn = new NbtString("minecraft:infiniburn_overworld");
        public NbtByte RespawnAnchorWorks;
        public NbtByte HasSkylight;
        public NbtByte BedWorks;
        public NbtString Effects = new NbtString("minecraft:overworld");
        public NbtByte HasRaids;
        public NbtInt MinY = new NbtInt(-64);
        public NbtInt Height = new NbtInt(384);
        public NbtInt LogicalHeight = new NbtInt(384);
        public NbtDouble CoordinateScale = new NbtDouble(1.0f);
        public NbtByte UltraWarm;
        public NbtByte HasCailing;
    }
}
