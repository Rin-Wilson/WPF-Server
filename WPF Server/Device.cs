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
        public static List<Device> deviceList = new List<Device>();

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
        }

        public static void AddDevice(Device newDevice)
        {
            foreach (Device d in deviceList)
            {
                if (newDevice.iP == d.iP)
                {
                    Trace.WriteLine("Device: " + newDevice.iP.ToString() + "is already connected");
                    return;
                }
            }
            deviceList.Add(newDevice);
            UI.Log("Added " + newDevice.iP.ToString());
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
                    GloveInputLink link = new GloveInputLink(GloveInputLink.Handness.Right);
                    link.Write(data);
                }
                else if (type == Type.Left)
                {
                    GloveInputLink link = new GloveInputLink(GloveInputLink.Handness.Left);
                    link.Write(data);
                }
            }
        }
    }
}
