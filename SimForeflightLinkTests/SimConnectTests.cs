using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using SimForeflightLink;
using System.Net.Sockets;
using System.Diagnostics;

namespace SimForeflightLinkTests
{
    [TestClass]
    public class SimConnectTests
    {
 //       [TestMethod]
        public void Test1()
        {
            FlightData data = new FlightData();
            SimConnectLink link = new SimConnectLink(ref data);
            link.Connect(Process.GetCurrentProcess().Handle);

            System.Threading.Thread.Sleep(20000);

        }
    }
}
