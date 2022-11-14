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
        public List<DeviceDisplay> displays = new List<DeviceDisplay>();
        public MainWindow()
        {
            InitializeComponent();
            
            for (int i = 0; i < 3; i++)
            {
                DeviceDisplay display = new DeviceDisplay();
                display.Margin = new Thickness(20, 10, 0, 0);
                display.HorizontalAlignment = HorizontalAlignment.Left;
                display.VerticalAlignment = VerticalAlignment.Top;

                displays.Add(display);
            }
            deviceList.ItemsSource = displays;
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
    }
}
