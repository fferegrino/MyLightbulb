using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLightbulb
{
    public class LightbulbInterface
    {
        const byte ReadLampState = (byte)'R';
        const byte WriteLampState = (byte)'W';
        const byte ByteTrue = (byte)'T';
        const byte ByteFalse = (byte)'F';

        public string NetduinoIp { get; private set; }

        public int Port { get; set; }


        public LightbulbInterface(string netduinoIp, int port)
        {
            NetduinoIp = netduinoIp;
            Port = port;
        }

        public async Task SetLampStatus(bool on)
        {
            using (var s = new Sockets.Plugin.TcpSocketClient())
            {
                await s.ConnectAsync(NetduinoIp, Port);
                byte[] data = new byte[2];
                data[0] = WriteLampState;
                data[1] = (byte)(on ? ByteTrue : ByteFalse);
                s.WriteStream.Write(data, 0, 2);
            }
        }

        public async Task<bool> GetLampstatus()
        {
            using (var s = new Sockets.Plugin.TcpSocketClient())
            {
                await s.ConnectAsync(NetduinoIp, Port);
                byte[] data = new byte[2];
                data[0] = ReadLampState;
                data[1] = ReadLampState;
                s.WriteStream.Write(data, 0, 2);
                s.ReadStream.Read(data, 0, 1);
                return data[0] == ByteTrue;
            }
        }
    }
}
