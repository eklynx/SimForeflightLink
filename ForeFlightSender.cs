using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Configuration;

namespace SimForeflightLink
{
    class ForeFlightSender
    {
        private const String DeviceName = "MSFS 2020";

        private UdpClient udpClient;
        private IPEndPoint endpoint;

        public ForeFlightSender()
        {
            udpClient = new UdpClient(49002, AddressFamily.InterNetwork);
        }

        private void send(String message)
        {
            Byte[] msg = Encoding.ASCII.GetBytes(message);
            udpClient.Send(msg, msg.Length, endpoint);
        }
    }
}
