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
using System.Timers;
using GloveInputLib;

namespace WPF_Server
{
    internal class Network_Manager
    {
        private const short Handshake_Packet = 0;
        private const short Data_Packet = 1;
        private const short Data_Packet_Length = 120;

        private static IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

        public static void StartServer()
        {
            Thread serverThread = new Thread( new ThreadStart(ServerLoop) );
            serverThread.Start();

            System.Timers.Timer heartBeatTimer = new()
            {
                Interval = 6000,
                AutoReset = true,
                Enabled = true
            };
            heartBeatTimer.Elapsed += HeartBeat;
        }

        private static void ServerLoop()
        {
            Trace.WriteLine("Serverloop Started");
            UdpClient client = new UdpClient(11000);

            while (true)
            {
                byte[] rcvData = client.Receive(ref remoteEP);
                RoutePacket(rcvData);
            } 
        }

        private static void RoutePacket(byte[] packet)
        {
            Trace.WriteLine("Routing Packet");
            short packetType = Convert.ToInt16(packet[0]);
            Device sender = null;
            foreach (Device d in Device.deviceList)
            {
                if(remoteEP.Address.Equals(d.iP))
                {
                    d.isConnected = true;
                    sender = d;
                    UI.UpdateBox();
                }
            }

            if (packetType == Handshake_Packet)
            {
                Device.AddDevice(new Device(remoteEP.Address, Device.Type.Right));
            }
            else if (packetType == Data_Packet && sender != null)
            {
                if (packet.Length == Data_Packet_Length + 1 || packet.Length == Data_Packet_Length + 5)
                {
                    InputData gloveData = ParseData(packet);
                    UI.GloveValue(gloveData.ToString());
                    sender.SendInput(gloveData);
                    if (packet.Length == Data_Packet_Length + 5)
                    {
                        sender.battery = BitConverter.ToSingle(packet, 122);
                    }
                }
            }
        }

        private static InputData ParseData(byte[] data)
        {
            InputData inputData = new InputData();

            for (int i = 0; i < 20; i++)
            {
                inputData.flexion[i] = BitConverter.ToSingle(data, 1 + (i * 4));
            }

            for (int i = 0; i < 5; i++)
            {
                inputData.splay[i] = BitConverter.ToSingle(data, 81 + (i * 4));
            }

            inputData.joyX = BitConverter.ToSingle(data, 101);
            inputData.joyY = BitConverter.ToSingle(data, 105);
            inputData.joyButton = BitConverter.ToBoolean(data, 109);
            inputData.trgButton = BitConverter.ToBoolean(data, 110);
            inputData.aButton = BitConverter.ToBoolean(data, 111);
            inputData.bButton = BitConverter.ToBoolean(data, 112);
            inputData.grab = BitConverter.ToBoolean(data, 113);
            inputData.pinch = BitConverter.ToBoolean(data, 114);
            inputData.menu = BitConverter.ToBoolean(data, 115);
            inputData.calibrate = BitConverter.ToBoolean(data, 116);
            inputData.trgValue = BitConverter.ToSingle(data, 117);

            return inputData;
        }

        private static void HeartBeat(Object source, System.Timers.ElapsedEventArgs e)
        {
            for (int i = 0; i < Device.deviceList.Count; i++)
            {
                if (!Device.deviceList[i].isConnected)
                {
                    Device.RemoveDevice(i);
                }
                else
                {
                    Device.deviceList[i].isConnected = false;
                }
            }
        }
    }
}
