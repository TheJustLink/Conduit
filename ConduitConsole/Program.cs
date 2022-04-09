// See https://aka.ms/new-console-template for more information
using Conduit;
using Conduit.Configurable;

var entry = new Entry();
entry.Server.Setup(new ServerOptions() { Port = 25565 });
entry.Server.Start();
