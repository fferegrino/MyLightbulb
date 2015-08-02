using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace MyLightbulb.Plus3
{
    public class LightbulbInterface
    {
        public delegate void BooleanInteraction(object sender, BooleanEventArgs args);

        public BooleanInteraction OnLampStatusChangeRequest;

        public BooleanInteraction OnLampStateRequest;

        const byte ReadLampState = (byte)'R';
        const byte WriteLampState = (byte)'W';
        const byte ByteTrue = (byte)'T';
        const byte ByteFalse = (byte)'F';

        private Thread serverThread;

        private Socket s;

        public int Port { get; private set; }

        public bool IsRunning { get; private set; }

        public LightbulbInterface(int port)
        {
            Port = port;
            IsRunning = false;
            serverThread = new Thread(StartServer);
        }

        private void StartServer()
        {

            using (Socket servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                servidor.Bind(new IPEndPoint(IPAddress.Any, Port));
                servidor.Listen(1);
                byte[] data = new byte[2];
                while (true)
                {
                    using (Socket newSocket = servidor.Accept())
                    {
                        newSocket.Receive(data, 0, 2, SocketFlags.None);
                        if (((char)data[0]) == WriteLampState)
                        {
                            if (OnLampStatusChangeRequest != null)
                            {
                                OnLampStatusChangeRequest(this, new BooleanEventArgs(((char)data[1]) == ByteTrue));
                            }
                        }
                        else if (((char)data[0]) == ReadLampState)
                        {
                            var args = new BooleanEventArgs(false);
                            if (OnLampStateRequest != null)
                            {
                                OnLampStateRequest(this, args);
                            }
                            data[0] = args.Data ? ByteTrue : ByteFalse;
                            newSocket.Send(data, 0, 1, SocketFlags.None);
                        }
                    }
                }
            }
        }

        public bool Start()
        {
            // start server           
            try
            {
                serverThread.Start();
                IsRunning = true;
                Debug.Print("Started server");
            }
            catch
            {
                IsRunning = true;
            }
            return IsRunning;
        }
    }


    public class BooleanEventArgs : EventArgs
    {
        public bool Data { get; set; }
        public BooleanEventArgs(bool data)
        {
            Data = data;
        }

    }
}
