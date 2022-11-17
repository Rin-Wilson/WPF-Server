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

        public static IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

        public static void StartServer()
        {
            Thread serverThread = new Thread( new ThreadStart(ServerLoop) );
            serverThread.Start();
            GloveInputLink.Right_Glove.Relax();
            GloveInputLink.Right_Glove.Relax();

            System.Timers.Timer heartBeatTimer = new()
            {
                Interval = 3000,
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
                new Task(() =>
                {
                    byte[] rcvData = client.Receive(ref remoteEP);
                    RoutePacket(rcvData);
                }).Start();
                foreach (Device d in Device.deviceList)
                {
                        new Task(() =>
                        {
                            d.Listen(remoteEP);
                        }).Start();
                }
            } 
        }

        public static void RoutePacket(byte[] packet)
        {
            //Trace.WriteLine("Routing Packet" + new Random().Next());
            short packetType = Convert.ToInt16(packet[0]);
            Device sender = null;
            foreach (Device d in Device.deviceList)
            {
                if(remoteEP.Address.Equals(d.iP))
                {
                    d.heartBeatRecieved = true;
                    sender = d;
                    //UI.UpdateBox();
                }
            }

            if (packetType == Handshake_Packet)
            {
                Device.AddDevice(new Device(remoteEP.Address, Device.Type.None));
            }
            else if (packetType == Data_Packet && sender != null)
            {
                if (packet.Length == Data_Packet_Length + 1 || packet.Length == Data_Packet_Length + 5)
                {
                    InputData gloveData = ParseData(packet);
                    sender.SendInput(gloveData);
                    if (packet.Length == Data_Packet_Length + 5)
                    {
                        sender.battery = BitConverter.ToSingle(packet, 121);
                        UI.DisplayBattery();
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
                if (!Device.deviceList[i].heartBeatRecieved)
                {
                    //Device.RemoveDevice(i);
                    Device.deviceList[i].isConnected = false;
                }
                else
                {
                    Device.deviceList[i].heartBeatRecieved = false;
                    Device.deviceList[i].isConnected = true;
                }
            }
            new Task( UI.DisplayConnection ).Start();
        }
    }
}
