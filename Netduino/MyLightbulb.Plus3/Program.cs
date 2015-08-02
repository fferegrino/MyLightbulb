using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace MyLightbulb.Plus3
{
    public class Program
    {
        static OutputPort led;

        public static void Main()
        {
            led = new OutputPort(Pins.GPIO_PIN_D8, false);

            Microsoft.SPOT.Net.NetworkInformation.NetworkInterface NI = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0];

            NI.EnableStaticIP("192.168.0.75", "255.255.255.0", "192.168.0.1");
            Debug.Print(NI.IPAddress.ToString());

            LightbulbInterface l = new LightbulbInterface(5436);
            l.OnLampStatusChangeRequest += (s, a) => { led.Write(a.Data); };
            l.OnLampStateRequest += (s, a) => { a.Data = led.Read(); };
            l.Start();
            Thread.Sleep(Timeout.Infinite);

        }

    }
}
