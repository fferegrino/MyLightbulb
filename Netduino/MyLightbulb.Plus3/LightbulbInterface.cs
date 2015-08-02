using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace MyLightbulb.Plus3
{
    public class LightbulbInterface
    {
        public delegate void DataReceived(object sender, DataReceivedEventArgs args);

        public DataReceived OnDataReceived;


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
                while (true)
                {
                    using (Socket newSocket = servidor.Accept())
                    {
                        Debug.Print("Accepted");
                        if (OnDataReceived != null)
                        {
                            OnDataReceived(this, new DataReceivedEventArgs('A'));
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


    public class DataReceivedEventArgs : EventArgs
    {
        public char Data { get; private set; }
        public DataReceivedEventArgs(char data)
        {
            Data = data;
        }

    }
}
