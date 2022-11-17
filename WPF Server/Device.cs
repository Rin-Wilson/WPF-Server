using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GloveInputLib;
using System.Diagnostics;

namespace WPF_Server
{
    public class Device
    {
        public static List<Device> deviceList = new();
        public bool heartBeatRecieved = true;
        public bool isConnected = false;
        public float battery;
        public UdpClient udpClient;

        public enum Type
        {
            None,
            Left,
            Right
        }

        public IPAddress iP;
        public Type type;

        public Device(IPAddress iP)
        {
            this.iP = iP;
            type = Type.None;
            udpClient = new UdpClient();
            udpClient.Connect(iP, 0);
        }

        public Device(IPAddress iP, Type type)
        {
            this.type = type;
            this.iP = iP;
            udpClient = new UdpClient();
            udpClient.Connect(iP, 0);
        }

        public static void AddDevice(Device newDevice)
        {
            foreach (Device d in deviceList)
            {
                if (newDevice.iP.Equals(d.iP))
                {
                    Trace.WriteLine("Device: " + newDevice.iP.ToString() + "is already connected");
                    return;
                }
            }
            UI.AddDisplay(newDevice.iP.ToString(), newDevice.battery);
            deviceList.Add(newDevice);
            
            UI.ClearLog();
        }

        public static void RemoveDevice(Device device)
        {
            deviceList.Remove(device);
            UI.ClearLog();
            foreach (Device d in deviceList)
            {
                UI.Log("\n" + deviceList.IndexOf(d) + ": " + d.iP.ToString());
            }
        }

        public static void RemoveDevice(int deviceIndex)
        {
            deviceList.RemoveAt(deviceIndex);
            UI.ClearLog();
            foreach (Device d in deviceList)
            {
                UI.Log("\n" + deviceList.IndexOf(d) + ": " + d.iP.ToString());
            }
        }

        public void SendInput(InputData data)
        {
            if (type == Type.None)
            {
                return;
            }
            else
            {
                if (type == Type.Right)
                {
                    new Task(() => { GloveInputLink.Right_Glove.Write(data); });
                }
                else if (type == Type.Left)
                {
                    new Task(() => { GloveInputLink.Left_Glove.Write(data); });
                }
            }
            UI.GloveValue(data.ToString(), type);
        }

        public void Listen(IPEndPoint remoteEP)
        {
            byte[] rcvData = udpClient.Receive(ref remoteEP); ;
            short packetType = Convert.ToInt16(rcvData[0]);

            if (packetType == 1)
            Network_Manager.RoutePacket(rcvData);
        }
    }
}
