using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            string address = Console.ReadLine();
            string port = Console.ReadLine();

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address), Convert.ToInt32(port));


            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            server.Connect(endPoint);

            string message = Console.ReadLine();
            byte[] data = Encoding.Unicode.GetBytes(message);
            server.Send(data);
        }

    }
}
