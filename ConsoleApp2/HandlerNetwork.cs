using System;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    static class HandlerNetwork
    {

        public static IPAddress GetIPAddress()
        {
            foreach (var address in GetWorkingInterface().GetIPProperties().UnicastAddresses)
            {
                if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address.Address;
                }
            }

            throw new InvalidOperationException("Cannot get ip address");
        }

        public static int GetOpenPort(int startingPort)
        {
            int port = startingPort;

            while (!IsPortAvailable(port))
            {
                port++;
            }

            return port;
        }

        private static NetworkInterface GetWorkingInterface()
        {
            var netInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface netInterface in netInterfaces)
            {
                if (netInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    netInterface.OperationalStatus == OperationalStatus.Up)
                {
                    return netInterface;
                }
            }

            throw new InvalidOperationException("No working network interface");
        }

        private static bool IsPortAvailable(int port)
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var endPoints = ipProperties.GetActiveTcpListeners();

            foreach (var endPoint in endPoints)
            {
                if (endPoint.Port == port)
                {
                    return false;
                }
            }

            return true;
        }

    }

}
