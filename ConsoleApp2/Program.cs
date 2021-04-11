using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();
            Console.WriteLine($"Server has started at {server.Address}:{server.Port}");

            server.Listen(100);

            Console.ReadLine();
        }
    }
}
