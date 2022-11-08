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
    internal class UI
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
                        (window as MainWindow).GloveValue.Content = message;
                    }
                }
            });
        }

        public static void UpdateBox()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        MainWindow mainWindow = (window as MainWindow);
                        if (Device.deviceList.Count > 0)
                        {
                            mainWindow.glove1IP.Text = "IP: " + Device.deviceList[0].iP.ToString();
                            if (Device.deviceList.Count > 1)
                            {
                                mainWindow.glove2IP.Text = "IP: " + Device.deviceList[1].iP.ToString();
                            }
                        }
                    }
                }
            });
        }
    }
}
