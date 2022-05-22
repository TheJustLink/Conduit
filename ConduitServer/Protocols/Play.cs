using fNbt.Tags;

using Conduit.Net;
using Conduit.Net.Data;
using Conduit.Net.Packets.Play.Clientbound;
using Conduit.Net.Protocols;
using Conduit.Net.Protocols.Flow;

namespace Conduit.Server.Protocols
{
    public class Play : ServerAutoProtocol<Play, PlayFlow>, IStarteable
    {
        public void Start()
        {
            var dimensionCodec = new NbtCompound(string.Empty, new NbtCompound[]
            {
                new("minecraft:dimension_type", new NbtTag[]
                {
                    new NbtString("type", "minecraft:dimension_type"),
                    new NbtList("value", new NbtTag[]
                    {
                        new NbtCompound(new NbtTag[]
                        {
                            new NbtString("name", "minecraft:overworld"),
                            new NbtInt("id", 0),
                            new NbtCompound("element", new NbtTag[]
                            {
                                new NbtByte("piglin_safe", 0),
                                new NbtByte("natural", 1),
                                new NbtFloat("ambient_light", 0),
                                //new NbtLong("fixed_time", 1000),
                                new NbtString("infiniburn", "minecraft:infiniburn_overworld"),
                                new NbtByte("respawn_anchor_works", 0),
                                new NbtByte("has_skylight", 1),
                                new NbtByte("bed_works", 1),
                                new NbtString("effects", "minecraft:overworld"),
                                new NbtByte("has_raids", 1),
                                new NbtInt("min_y", 0),
                                new NbtInt("height", 256),
                                new NbtInt("logical_height", 256),
                                new NbtDouble("coordinate_scale", 1),
                                new NbtByte("shrunk", 0),
                                new NbtByte("ultrawarm", 0),
                                new NbtByte("has_ceiling", 0)
                            })
                        })
                    })
                }),
                new("minecraft:worldgen/biome", new NbtTag[]
                {
                    new NbtString("type", "minecraft:worldgen/biome"),
                    new NbtList("value", new NbtTag[]
                    {
                        new NbtCompound(new NbtTag[]
                        {
                            new NbtString("name", "minecraft:ocean"),
                            new NbtInt("id", 0),
                            new NbtCompound("element", new NbtTag[]
                            {
                                new NbtString("precipitation", "rain"),
                                new NbtFloat("depth", 1.5f),
                                new NbtFloat("temperature", 1f),
                                new NbtFloat("scale", 0.1f),
                                new NbtFloat("downfall", 1f),
                                new NbtString("category", "ocean"),
                                // new NbtString("temperature_modifier", "frozen"),
                                new NbtCompound("effects", new NbtTag[]
                                {
                                    new NbtInt("sky_color", 8364543),
                                    new NbtInt("water_fog_color", 8364543),
                                    new NbtInt("fog_color", 8364543),
                                    new NbtInt("water_color", 8364543),
                                    // new NbtInt("foliage_color", 8364543),
                                    // new NbtInt("grass_color", 8364543),
                                    // new NbtString("grass_color_modifier", "swamp"),
                                    //new NbtCompound("music", new NbtTag[]
                                    //{
                                    //    new NbtByte("replace_current_music", 0),
                                    //    new NbtString("sound", ""),
                                    //    new NbtInt("max_delay", 10),
                                    //    new NbtInt("min_delay", 1)
                                    //})
                                    //new NbtString("ambient_sound", "minecraft:ambient.basalt_deltas.loop"),
                                    //new NbtCompound("additions_sound", new NbtTag[]
                                    //{
                                    //    new NbtString("sound", ""),
                                    //    new NbtDouble("tick_chance", 1)
                                    //})
                                    new NbtCompound("mood_sound", new NbtTag[]
                                    {
                                        new NbtString("sound", "minecraft:ambient.cave"),
                                        new NbtInt("tick_delay", 6000),
                                        new NbtDouble("offset", 2),
                                        new NbtInt("block_search_extent", 8)
                                    })
                                    //new NbtCompound("particle", new NbtTag[]
                                    //{
                                    //    new NbtFloat("probability", 1),
                                    //    new NbtCompound("options", new NbtTag[]
                                    //    {
                                    //        new NbtString("type", "")
                                    //    })
                                    //})
                                })
                            })
                        })
                    })
                })
            });
            var dimension = new NbtCompound(string.Empty, new NbtTag[]
            {
                new NbtByte("piglin_safe", 0),
                new NbtByte("natural", 1),
                new NbtFloat("ambient_light", 1),
                //new NbtLong("fixed_time", 1000),
                new NbtString("infiniburn", "minecraft:infiniburn_overworld"),
                new NbtByte("respawn_anchor_works", 0),
                new NbtByte("has_skylight", 1),
                new NbtByte("bed_works", 1),
                new NbtString("effects", "minecraft:overworld"),
                new NbtByte("has_raids", 1),
                new NbtInt("min_y", 0),
                new NbtInt("height", 256),
                new NbtInt("logical_height", 256),
                new NbtDouble("coordinate_scale", 1),
                new NbtByte("shrunk", 0),
                new NbtByte("ultrawarm", 0),
                new NbtByte("has_ceiling", 0)
            });

            var joinGame = new JoinGame
            {
                EntityId = 0,
                IsHardcore = false,
                Gamemode = Gamemode.Survival,
                PreviousGamemode = -1,
                DimensionNames = new[] { "minecraft:overworld" },
                DimensionCodec = dimensionCodec,
                Dimension = dimension,
                DimensionName = "minecraft:overworld",
                HashedSeed = 0,
                MaxPlayers = 100,
                ViewDistance = 7,
                SimulationDistance = 7,
                ReducedDebugInfo = false,
                EnableRespawnScreen = true,
                IsDebug = false,
                IsFlat = true
            };

            Connection.Send(joinGame);

            // Console.WriteLine($"{UserAgent} Joined");
        }
    }
}