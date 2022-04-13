using fNbt;

namespace ConduitServer.Nbt
{
    class BiomeEffect
    {
        public NbtInt SkyColor;
        public NbtInt WaterFogColor;
        public NbtInt FogColor;
        public NbtInt WaterColor;
        public NbtInt FoliageColor;
        public NbtInt GrassColor;
        public NbtString GrassColorModifier;
        public NbtCompound Music;
        public NbtString AmbientSound;
        public NbtCompound AdditionsSound;
        public NbtCompound MoodSound;
    }
}