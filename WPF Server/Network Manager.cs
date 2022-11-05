using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Windows;

namespace WPF_Server
{
    internal class Network_Manager
    {
        private const short Handshake_Packet = 0;


        private static IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        public static void StartServer()
        {
            Thread serverThread = new Thread( new ThreadStart(ServerLoop) );
            serverThread.Start();
        }

        private static void ServerLoop()
        {
            Trace.WriteLine("Serverloop Started");
            UdpClient client = new UdpClient(11000);

            while (true)
            {
                byte[] rcvData = client.Receive(ref remoteEP);
                ParsePacket(rcvData);
            } 
        }

        private static void ParsePacket(byte[] packet)
        {
            Trace.WriteLine("Parsing Packet");
            short packetType = Convert.ToInt16(packet[0]);
            UI.Log("" + packetType);

            if (packetType == Handshake_Packet)
            {
                Device.AddDevice(new Device(remoteEP.Address));
            }
        }
    }
}
