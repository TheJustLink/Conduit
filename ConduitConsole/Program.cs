using Conduit;
using Conduit.Configurable;

namespace ConduitConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            var entry = new Entry();

            entry.Setup(args);
            Console.ReadLine();
        }
    }
}