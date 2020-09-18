using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static SimForeflightLink.Foreflight.ForeFlightNetworkOption;

namespace SimForeflightLink.Foreflight.Tests
{
    [TestClass]
    public class ForeFlightNetworkOptionTests
    {
        [TestMethod]
        public void TestDirectIPValues()
        {
            ForeFlightNetworkOption ffno = new ForeFlightNetworkOption(NetworkTypes.DirectIPv4, IPAddress.Parse("0.0.0.1"));
            Assert.AreEqual(IPAddress.Loopback, ffno.Address);
            Assert.AreEqual(NetworkTypes.DirectIPv4, ffno.NetworkType);
            Assert.AreEqual(ForeFlightNetworkOption.DIRECT_STRING, ffno.ToString());
        }

        [TestMethod]
        public void TestIPv6Values()
        {
            ForeFlightNetworkOption ffno = new ForeFlightNetworkOption(NetworkTypes.IPv6LinkLocal, IPAddress.Parse("0.0.0.1"));
            Assert.AreEqual(IPAddress.IPv6Any, ffno.Address);
            Assert.AreEqual(NetworkTypes.IPv6LinkLocal, ffno.NetworkType);
            Assert.AreEqual(ForeFlightNetworkOption.IPV6_STRING, ffno.ToString());
        }

        [TestMethod]
        public void TestIPv4NetworkValues()
        {
            IPAddress addr = IPAddress.Parse("0.0.0.1");
            ForeFlightNetworkOption ffno = new ForeFlightNetworkOption(NetworkTypes.IPv4NetworkBroadcast, addr);
            Assert.AreEqual(addr, ffno.Address);
            Assert.AreEqual(NetworkTypes.IPv4NetworkBroadcast, ffno.NetworkType);
            Assert.AreEqual(ForeFlightNetworkOption.IPV4_BROADCAST_STRING_PREFIX + "0.0.0.1", ffno.ToString());
        }

        [TestMethod]
        public void TestDirectIPValuesNullAddress()
        {
            ForeFlightNetworkOption ffno = new ForeFlightNetworkOption(NetworkTypes.DirectIPv4, null);
            Assert.AreEqual(IPAddress.Loopback, ffno.Address);
            Assert.AreEqual(NetworkTypes.DirectIPv4, ffno.NetworkType);
        }

        [TestMethod]
        public void TestIPv6ValuesNullAddress()
        {
            ForeFlightNetworkOption ffno = new ForeFlightNetworkOption(NetworkTypes.IPv6LinkLocal, null);
            Assert.AreEqual(IPAddress.IPv6Any, ffno.Address);
            Assert.AreEqual(NetworkTypes.IPv6LinkLocal, ffno.NetworkType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIPv4NetworkValuesNullAddress()
        {
            ForeFlightNetworkOption ffno = new ForeFlightNetworkOption(NetworkTypes.IPv4NetworkBroadcast, null);
        }

    }
}
