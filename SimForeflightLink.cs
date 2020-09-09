using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimForeflightLink
{
    public partial class SimForeflightLink : Form
    {
        SimConnectLink simConnectLink;
        FlightData flightData;
        ForeFlightSender foreFlightSender;

        public SimForeflightLink()
        {
            InitializeComponent();
            this.flightData = new FlightData();
        }

        private void buttonSimConnect_Click(object sender, EventArgs e)
        {
            simConnectLink = new SimConnectLink(ref flightData);
            simConnectLink.Connect(Handle);

        }


        private void buttonForeflight_Click(object sender, EventArgs e)
        {
            foreFlightSender = new ForeFlightSender(ref flightData, new UdpClient());
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("192.168.1.255"), ForeFlightSender.DEFAULT_PORT);
            foreFlightSender.EndPoint = endpoint;
            foreFlightSender.Start();
        }
    }
}
