using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace MyLightbulb.Plus3
{
    public class LightbulbInterface
    {
        const byte ReadLightSwitchState = (byte)'R';
        const byte WriteLightSwitchState = (byte)'W';
        const byte ByteTrue = (byte)'T';
        const byte ByteFalse = (byte)'F';

        public delegate void BooleanInteraction(object sender, BooleanEventArgs args);

        public BooleanInteraction OnLampStatusChangeRequest;

        public BooleanInteraction OnLampStateRequest;


        private Thread serverThread;

        private Socket s;

        public int Port { get; private set; }

        public bool IsRunning { get; private set; }

        /// <summary>
        /// The port in which the server will be listening to
        /// </summary>
        /// <param name="port"></param>
        public LightbulbInterface(int port)
        {
            Port = port;
            IsRunning = false;
            serverThread = new Thread(StartServer);
        }

        private void StartServer()
        {

            using (Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                server.Bind(new IPEndPoint(IPAddress.Any, Port));
                server.Listen(1);
                byte[] data = new byte[2];
                while (true)
                {
                    using (Socket client = server.Accept())
                    {
                        client.Receive(data, 0, 2, SocketFlags.None);
                        if (((char)data[0]) == WriteLightSwitchState)
                        {
                            if (OnLampStatusChangeRequest != null)
                            {
                                OnLampStatusChangeRequest(this, new BooleanEventArgs(((char)data[1]) == ByteTrue));
                            }
                        }
                        else if (((char)data[0]) == ReadLightSwitchState)
                        {
                            var args = new BooleanEventArgs(false);
                            if (OnLampStateRequest != null)
                            {
                                // Using the delegate we should tell the app whether the switch is on or off
                                OnLampStateRequest(this, args);
                            }
                            data[0] = args.Data ? ByteTrue : ByteFalse;
                            client.Send(data, 0, 1, SocketFlags.None);
                        }
                    }
                }
            }
        }

        public bool Start()
        {
            // Start server           
            try
            {
                serverThread.Start();
                IsRunning = true;
            }
            catch
            {
                IsRunning = true;
            }
            return IsRunning;
        }
    }

    /// <summary>
    /// Simple event args class to handle the outgoing/incoming data from the client
    /// </summary>
    public class BooleanEventArgs : EventArgs
    {
        public bool Data { get; set; }
        public BooleanEventArgs(bool data)
        {
            Data = data;
        }

    }
}
