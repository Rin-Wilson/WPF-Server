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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void OnDetectL(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Detect Left");
        } 

        void OnDetectR(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Detect Right");
        }

        void OnServerStart(object sender, RoutedEventArgs e)
        {
            Network_Manager.StartServer();
        }

        public void AddDisplay(string ip, float battery)
        {
            DeviceDisplay newDevice = new()
            {
                Margin = new Thickness(0, 10, 0, 0)
            };

            newDevice.IP.Text = "IP: " + ip;
            newDevice.Battery.Value = battery;

            deviceList.Items.Add(newDevice);
        }
    }
}
