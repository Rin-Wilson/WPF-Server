using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WPF_Server
{
    public class UI
    {
        
        public static void Log(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        (window as MainWindow).TextDisplay.Content += message;
                    }
                }
            });
        }

        public static void LogLine(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if ( window.GetType() == typeof(MainWindow))
                    {
                        
                        
                        (window as MainWindow).TextDisplay.Content += "\n" + message;

                    }
                }
            });
        }

        public static void ClearLog()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        (window as MainWindow).TextDisplay.Content = "";
                    }
                }
            });
        }

        public static void GloveValue(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        //(window as MainWindow).GloveValue.Content = message;
                    }
                }
            });
        }

        public static void AddDisplay(string ip, float battery)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        (window as MainWindow).AddDisplay(ip, battery);
                    }
                }
            });
        }

        public static void DisplayConnection()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        MainWindow mainWindow = (window as MainWindow);

                        for(int i = 0; i < Device.deviceList.Count; i++)
                        {
                            DeviceDisplay d = (DeviceDisplay)mainWindow.deviceList.Items.GetItemAt(i);
                            d.ConnectionIdicator.Visibility = Device.deviceList[i].isConnected ? Visibility.Visible : Visibility.Hidden;
                        }
                    }
                }
            });
        }

        public static void DisplayBattery() 
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        MainWindow mainWindow = (window as MainWindow);

                        for (int i = 0; i < Device.deviceList.Count; i++)
                        {
                            DeviceDisplay d = (DeviceDisplay)mainWindow.deviceList.Items.GetItemAt(i);
                            d.Battery.Value = Device.deviceList[i].battery;
                        }
                    }
                }
            });
        }

        public static void DisplayType()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        MainWindow mainWindow = (window as MainWindow);

                        for (int i = 0; i < Device.deviceList.Count; i++)
                        {
                            DeviceDisplay d = (DeviceDisplay)mainWindow.deviceList.Items.GetItemAt(i);
                            Device.deviceList[i].type = d.type;
                        }
                    }
                }
            });
        }
    }
}
