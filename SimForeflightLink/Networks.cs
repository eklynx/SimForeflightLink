using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SimForeflightLink
{
    static public class Networks
    {

        public static readonly Func<UnicastIPAddressInformation, IPAddress> IS_IPV4 = (UnicastIPAddressInformation addrInfo) =>
        {
            // Only look at Non-loopback IPv4 addresses
            if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork
                && !IPAddress.IsLoopback(addrInfo.Address)
            )
                return GetIPv4BroadcastAddress(addrInfo);
            else
                return null;
        };

        public static readonly Func<UnicastIPAddressInformation, IPAddress> IS_IPV6_Link_Local = (UnicastIPAddressInformation addrInfo) =>
        {
            // Only look at Non-loopback IPv4 addresses
            if (addrInfo.Address.AddressFamily == AddressFamily.InterNetworkV6
                && addrInfo.Address.IsIPv6LinkLocal
            )
                return GetIPv6LocalBroadcastAddress(addrInfo);
            else
                return null;
        };


        static public ISet<IPAddress> GetAllIPv4Addresses()
        {
            return GetAllIPv4Addresses(NetworkInterface.GetAllNetworkInterfaces());
        }
        static public ISet<IPAddress> GetAllIPv4Addresses(NetworkInterface[] netInterfaces)
        {
            return GetAllNetworkAddresses(netInterfaces, IS_IPV4);
        }

        static private ISet<IPAddress> GetAllNetworkAddresses(NetworkInterface[] netInterfaces, 
            Func<UnicastIPAddressInformation, IPAddress> evalFunc)
        {
            ISet<IPAddress> ipAddresses = new HashSet<IPAddress>(10);

            // get All IP Addresses on all networks.
            foreach (NetworkInterface interfaces in netInterfaces)
            {
                // Only look at Ethernet and Wifi.
                if (interfaces.NetworkInterfaceType != NetworkInterfaceType.Ethernet 
                    && interfaces.NetworkInterfaceType != NetworkInterfaceType.Wireless80211
                )
                    continue;
                
                IPInterfaceProperties ipProperties = interfaces.GetIPProperties();
                foreach (UnicastIPAddressInformation addrInfo in ipProperties.UnicastAddresses)
                {
                    {
                        IPAddress addr = evalFunc.Invoke(addrInfo);
                        if (null != addr)
                            ipAddresses.Add(addr);
                    }
                }
            }
            return ipAddresses;
        }


        static public IPAddress GetIPv4BroadcastAddress(UnicastIPAddressInformation address)
        {
            if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                throw new ArgumentException("Address is not an IPv4 address");
            
            uint intAddr = BitConverter.ToUInt32(address.Address.GetAddressBytes(), 0);
            uint intSubnetMask = BitConverter.ToUInt32(address.IPv4Mask.GetAddressBytes(), 0);
            uint intBroadcast = intAddr | ~intSubnetMask;

            return new IPAddress(BitConverter.GetBytes(intBroadcast));
        }

        static public IPAddress GetIPv6LocalBroadcastAddress(UnicastIPAddressInformation address)
        {
            if (address.Address.AddressFamily != AddressFamily.InterNetworkV6 && !address.Address.IsIPv6LinkLocal)
                throw new ArgumentException("Address is not an IPv6 Link Local address");
            throw new NotImplementedException("Not yet implemented");
        }

    }
}
