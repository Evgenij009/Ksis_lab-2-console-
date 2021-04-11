using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class Server
    {
        private class StateObject
        {
            public const int BufferSize = 1024;
            public byte[] buffer = new byte[BufferSize];

            public StringBuilder sb = new StringBuilder();
            public Socket workSocket = null;
        }

        public IPAddress iPAddress { get; private set; }
        public IPAddress Address { get; private set; }
        public int Port { get; private set; }

        private Socket socket;
        private ManualResetEvent allDone = new ManualResetEvent(false);
        private bool isStarted;

        public Server()
        {
            isStarted = false;
        }

        public void Start()
        {

            Address = HandlerNetwork.GetIPAddress();
            Port = HandlerNetwork.GetOpenPort(10000);

            var endPoint = new IPEndPoint(Address, Port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(endPoint);

            isStarted = true;
        }

        public void Listen(int connections)
        {
            if (!isStarted)
            {
                throw new InvalidOperationException("Server has not been started");
            }

            socket.Listen(connections);
            while (true)
            {
                allDone.Reset();

                socket.BeginAccept(AcceptCallBack, socket);

                allDone.WaitOne();
            }
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, RecieveCallBack, state);
        }

        private void RecieveCallBack(IAsyncResult ar)
        {
            string content = string.Empty;

            StateObject state = (StateObject) ar.AsyncState;
            Socket handler = state.workSocket;
            int bytes = handler.EndReceive(ar);
            if (bytes > 0)
            {
                state.sb.Append(Encoding.Unicode.GetString(state.buffer, 0, bytes));

                content = state.sb.ToString();
                Console.WriteLine($"Read {bytes} bytes from socket.\nData : {content}");
            }
        }
    }
}
