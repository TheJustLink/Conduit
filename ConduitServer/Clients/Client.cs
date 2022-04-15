using System;
using System.Threading;

using fNbt;

using Conduit.Net.Data;
using Conduit.Net.Data.Status;
using Conduit.Net.Packets.Handshake;
using Conduit.Net.Packets.Login;
using Conduit.Net.Packets.Play;
using Conduit.Net.Packets.Status;

using IPacketReader = Conduit.Net.IO.Packet.IReader;
using IPacketWriter = Conduit.Net.IO.Packet.IWriter;
using Version = Conduit.Net.Data.Status.Version;

namespace Conduit.Server.Clients
{
    abstract class Client : IClient
    {
        private ClientState _state;

        private readonly IPacketReader _packetReader;
        private readonly IPacketWriter _packetWriter;

        private int _protocolVersion;

        protected Client(IPacketReader packetReader, IPacketWriter packetWriter)
        {
            _packetReader = packetReader;
            _packetWriter = packetWriter;
        }
        
        public void Tick()
        {
            while (true)
            {
                switch (_state)
                {
                    case ClientState.Handshaking: HandshakingState(); break;
                    case ClientState.Status: StatusState(); break;
                    case ClientState.Login: LoginState(); break;
                    case ClientState.Play: PlayState(); break;
                    default:
                    case ClientState.Disconnected: Disconnect(); return;
                }
                Thread.Sleep(1);
            }

            try
            {
                
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void HandshakingState()
        {
            var handshake = _packetReader.Read<Handshake>();

            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{handshake.Id}](length={handshake.Length})");
            Console.WriteLine("HandShake");
            Console.WriteLine("ProtocolVerision=" + handshake.ProtocolVersion);
            Console.WriteLine("ServerAddress=" + handshake.ServerAddress);
            Console.WriteLine("ServerPort=" + handshake.ServerPort);
            Console.WriteLine("NextState=" + handshake.NextState);

            _protocolVersion = handshake.ProtocolVersion;
            _state = handshake.NextState;
        }
        private void StatusState()
        {
            _packetReader.Read<Request>();

            Console.WriteLine();
            Console.WriteLine("Get packet:");
            Console.WriteLine("Request");

            // var statusText = @"{""version"": {""name"": ""Hell server 1.18"",""protocol"": " + _protocolVersion + @"},""players"": {""max"": 666,""online"": 99}}";
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

            Console.WriteLine();
            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{ping.Id}](length={ping.Length})");
            Console.WriteLine("Ping");
            Console.WriteLine("Payload=" + ping.Payload);

            _state = ClientState.Disconnected;
        }
        private void LoginState()
        {
            var loginStart = _packetReader.Read<Start>();

            Console.WriteLine("Get packet:");
            Console.WriteLine($"[{loginStart.Id}](length={loginStart.Length})");
            Console.WriteLine("Username=" + loginStart.Username);

            var loginSuccess = new Success
            {
                Guid = Guid.NewGuid(),
                Username = loginStart.Username
            };
            _packetWriter.Write(loginSuccess);

            // If login failed - disconnect
            //var disconnect = new LoginDisconnect
            //{
            //    Reason = new Message { Text  = "ABOBUS!" }
            //};
            //_packetWriter.Send(disconnect);

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
                                new NbtFloat("ambient_light", 1),
                                new NbtLong("fixed_time", 1000),
                                new NbtString("infiniburn", ""),
                                new NbtByte("respawn_anchor_works", 0),
                                new NbtByte("has_skylight", 1),
                                new NbtByte("bed_works", 1),
                                new NbtString("effects", "minecraft:overworld"),
                                new NbtByte("has_raids", 1),
                                new NbtInt("min_y", 0),
                                new NbtInt("height", 255),
                                new NbtInt("logical_height", 255),
                                new NbtDouble("coordinate_scale", 1),
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
                            new NbtString("name", "minecraft:the_void"),
                            new NbtInt("id", 0),
                            new NbtCompound("element", new NbtTag[]
                            {
                                new NbtString("precipitation", "none"),
                                new NbtFloat("depth", 1.5f),
                                new NbtFloat("temperature", 1f),
                                new NbtFloat("scale", 1f),
                                new NbtFloat("downfall", 1f),
                                new NbtString("category", "none"),
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
                                    //new NbtCompound("mood_sound", new NbtTag[]
                                    //{
                                    //    new NbtString("sound", ""),
                                    //    new NbtInt("tick_delay", 1),
                                    //    new NbtDouble("offset", 0),
                                    //    new NbtInt("block_search_extent", 0)
                                    //})
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
                new NbtLong("fixed_time", 1000),
                new NbtString("infiniburn", ""),
                new NbtByte("respawn_anchor_works", 0),
                new NbtByte("has_skylight", 1),
                new NbtByte("bed_works", 1),
                new NbtString("effects", "minecraft:overworld"),
                new NbtByte("has_raids", 1),
                new NbtInt("min_y", 0),
                new NbtInt("height", 255),
                new NbtInt("logical_height", 255),
                new NbtDouble("coordinate_scale", 1),
                new NbtByte("ultrawarm", 0),
                new NbtByte("has_ceiling", 0)
            });

            var joinGame = new JoinGame
            {
                EntityId = 0,
                IsHardcore = false,
                Gamemode = Gamemode.Adventure,
                PreviousGamemode = -1,
                WorldCount = 1,
                DimensionNames = new[] { "minecraft:overworld" },
                DimensionCodec = dimensionCodec,
                Dimension = dimension,
                DimensionName = "minecraft:overworld",
                HashedSeed = 0,
                MaxPlayers = 100,
                ViewDistance = 2,
                SimulationDistance = 2,
                ReducedDebugInfo = false,
                EnableRespawnScreen = true,
                IsDebug = true,
                IsFlat = true
            };
            _packetWriter.Write(joinGame);
            Console.WriteLine("Join game sended");

            _state = ClientState.Disconnected;
        }

        protected abstract void Disconnect();
    }
}