using System;
using System.Threading;

using fNbt.Tags;

using Conduit.Net.Data;
using Conduit.Net.Data.Status;
using Conduit.Net.Extensions;
using Conduit.Net.IO.Packet;
using Conduit.Net.Packets.Handshake;
using Conduit.Net.Packets.Login;
using Conduit.Net.Packets.Play;
using Conduit.Net.Packets.Status;

using Version = Conduit.Net.Data.Status.Version;

namespace Conduit.Server.Clients
{
    abstract class Client : IClient
    {
        private ClientState _state;

        private readonly IReader _packetReader;
        private readonly IWriter _packetWriter;

        private readonly ReaderFactory _packetReaderFactory;
        private readonly WriterFactory _packetWriterFactory;

        private int _protocolVersion;
        private string _username;

        protected Client(ReaderFactory packetReaderFactory, WriterFactory packetWriterFactory)
        {
            _packetReaderFactory = packetReaderFactory;
            _packetWriterFactory = packetWriterFactory;

            _packetReader = packetReaderFactory.Create();
            _packetWriter = packetWriterFactory.Create();
        }

        public string UserAgent => _username is not null
            ? $"[{GetInternalUserAgent()}:{_username}]"
            : $"[{GetInternalUserAgent()}]";
        public abstract bool Connected { get; }

        public void Tick()
        {
            while (Connected)
            {
                switch (_state)
                {
                    case ClientState.Handshaking: HandshakingState(); break;
                    case ClientState.Status: StatusState(); break;
                    case ClientState.Login: LoginState(); break;
                    case ClientState.Play: PlayState(); break;
                    default:
                    case ClientState.Disconnected: Disconnect(); break;
                }

                Thread.Sleep(1);
            }

            Console.WriteLine($"{UserAgent} Disconnected");
        }

        protected abstract string GetInternalUserAgent();
        protected abstract void Disconnect();

        private void HandshakingState()
        {
            var handshake = _packetReader.Read<Handshake>();
            _protocolVersion = handshake.ProtocolVersion;

            _state = handshake.NextState;
        }
        private void StatusState()
        {
            _packetReader.Read<Request>();
            
            var server = new Net.Data.Status.Server
            {
                Version = new Version { Name = "1.18", Protocol = _protocolVersion },
                Description = new Message { Text = "Minecraft hell server" },
                Players = new Players { Max = 666, Online = 66 }
            };
            var response = new Response { Server = server };
            _packetWriter.Write(response);

            var ping = _packetReader.Read<Ping>();
            _packetWriter.Write(ping);

            _state = ClientState.Disconnected;
        }
        private void LoginState()
        {
            var loginStart = _packetReader.Read<Start>();

            //var publicKey = Random.Shared.NextBytes(1);
            //var verifyToken = Random.Shared.NextBytes(4);

            //var encryptionRequest = new EncryptionRequest
            //{
            //    ServerId = "",
            //    PublicKey = null,
            //    VerifyToken = verifyToken
            //};

            var treshold = 256;
            var compression = new SetCompression { Treshold = treshold };
            _packetWriter.Write(compression);
            
            _packetReaderFactory.AddCompression(_packetReader);
            _packetWriterFactory.AddCompression(_packetWriter, treshold);

            var loginSuccess = new Success
            {
                Guid = Guid.NewGuid(),
                Username = loginStart.Username
            };
            _packetWriter.Write(loginSuccess);

            _username = loginStart.Username;
            Console.WriteLine($"{UserAgent} Logined");

            _state = ClientState.Play;
        }

        private void PlayState()
        {   
            var dimensionCodec = new NbtCompound("", new NbtCompound[]
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
            var dimension = new NbtCompound("", new NbtTag[]
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
            _packetWriter.Write(joinGame);

            Console.WriteLine($"{UserAgent} Joined");

            _state = ClientState.Disconnected;
        }

    }
}