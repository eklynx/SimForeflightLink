using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimForeflightLink.Foreflight
{
    public class ForeFlightNetworkOption : IComparable<ForeFlightNetworkOption>, IEquatable<ForeFlightNetworkOption>
    {
        public const string DIRECT_STRING = "Direct to IPv4 Address";
        public const string IPV6_STRING = "IPv6 Link Local";
        public const string IPV4_BROADCAST_STRING_PREFIX = "Network Broadcast to ";

        public enum NetworkTypes
        {
            DirectIPv4,
            IPv4NetworkBroadcast,
            IPv6LinkLocal
        }

        public ForeFlightNetworkOption(NetworkTypes networkType, IPAddress address = null)
        {
            NetworkType = networkType;
            if (networkType == NetworkTypes.IPv6LinkLocal)
            {
                Address = IPAddress.IPv6Any;
            }
            else if (networkType == NetworkTypes.DirectIPv4)
            {
                Address = IPAddress.Loopback;
            }
            else
            {
                if (null == address)
                    throw new ArgumentException("Broadcast address must be specified for IPv4 Netowrk Broadcasts");
                Address = address;
            }
        }

        public IPAddress Address { get; set; }
        public NetworkTypes NetworkType { get; set; }
        public string Description { get { return ToString(); } }

        public override string ToString()
        {
            if (NetworkType == NetworkTypes.DirectIPv4)
                return DIRECT_STRING;
            if (NetworkType == NetworkTypes.IPv6LinkLocal)
                return IPV6_STRING;
            return IPV4_BROADCAST_STRING_PREFIX + Address.ToString();
        }

        public int CompareTo(ForeFlightNetworkOption other)
        {
            if (ReferenceEquals(this, other))
                return 0;
            if (NetworkType == other.NetworkType && NetworkType != NetworkTypes.IPv4NetworkBroadcast)
                throw new ArgumentException("You can only have more than one IPv4 Broadcast option");

            if (NetworkType == NetworkTypes.DirectIPv4)
                return 1;
            if (other.NetworkType == NetworkTypes.DirectIPv4)
                return -1;
            if (other.NetworkType == NetworkTypes.IPv6LinkLocal)
                return -1;

            uint intAddr = BitConverter.ToUInt32(Address.GetAddressBytes(), 0);
            uint intOtherAddr = BitConverter.ToUInt32(other.Address.GetAddressBytes(), 0);
            return intAddr.CompareTo(intOtherAddr);
        }

        public bool Equals(ForeFlightNetworkOption other)
        {
            return NetworkType == other.NetworkType && Address.Equals(other.Address);
        }
    }
}
