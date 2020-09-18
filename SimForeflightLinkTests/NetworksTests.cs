using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimForeflightLink;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SimForeflightLink.Tests
{
    [TestClass]
    public class NetworksTests
    {
        [TestMethod]
        public void TestClassCBroadcastAddress()
        {
            Mock<UnicastIPAddressInformation> unicastInfoMock = new Mock<UnicastIPAddressInformation>();

            IPAddress ipAddr = IPAddress.Parse("192.168.1.5");
            IPAddress ipMask = IPAddress.Parse("255.255.255.0");

            // UnicastIPInfo
            unicastInfoMock.SetupGet(n => n.Address).Returns(ipAddr);
            unicastInfoMock.SetupGet(n => n.IPv4Mask).Returns(ipMask);
            UnicastIPAddressInformation unicastInfo = unicastInfoMock.Object;

            IPAddress result = Networks.GetIPv4BroadcastAddress(unicastInfo);
            Assert.AreEqual(IPAddress.Parse("192.168.1.255"), result);
        }

        [TestMethod]
        public void TestClassBBroadcastAddress()
        {
            Mock<UnicastIPAddressInformation> unicastInfoMock = new Mock<UnicastIPAddressInformation>();

            IPAddress ipAddr = IPAddress.Parse("172.17.1.5");
            IPAddress ipMask = IPAddress.Parse("255.240.0.0");

            // UnicastIPInfo
            unicastInfoMock.SetupGet(n => n.Address).Returns(ipAddr);
            unicastInfoMock.SetupGet(n => n.IPv4Mask).Returns(ipMask);
            UnicastIPAddressInformation unicastInfo = unicastInfoMock.Object;

            IPAddress result = Networks.GetIPv4BroadcastAddress(unicastInfo);
            Assert.AreEqual(IPAddress.Parse("172.31.255.255"), result);
        }

        [TestMethod]
        public void TestClassABroadcastAddress()
        {
            Mock<UnicastIPAddressInformation> unicastInfoMock = new Mock<UnicastIPAddressInformation>();

            IPAddress ipAddr = IPAddress.Parse("10.1.2.5");
            IPAddress ipMask = IPAddress.Parse("255.0.0.0");

            // UnicastIPInfo
            unicastInfoMock.SetupGet(n => n.Address).Returns(ipAddr);
            unicastInfoMock.SetupGet(n => n.IPv4Mask).Returns(ipMask);
            UnicastIPAddressInformation unicastInfo = unicastInfoMock.Object;

            IPAddress result = Networks.GetIPv4BroadcastAddress(unicastInfo);
            Assert.AreEqual(IPAddress.Parse("10.255.255.255"), result);
        }

    }
}
