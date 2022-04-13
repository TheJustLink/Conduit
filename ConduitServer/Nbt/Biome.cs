using fNbt;

namespace ConduitServer.Nbt
{
    class Biome
    {
        public NbtString Precipitation;
        public NbtFloat Depth;
        public NbtFloat Temperature;
        public NbtFloat Scale;
        public NbtFloat Downfall;
        public NbtString Category;
        public NbtString TemperatureModifier;
        public BiomeEffect Effects;
    }
}
