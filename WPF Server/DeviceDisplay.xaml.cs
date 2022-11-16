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
using System.Net.Sockets;
using System.Net;

namespace WPF_Server
{
    /// <summary>
    /// Interaction logic for DeviceDisplay.xaml
    /// </summary>
    public partial class DeviceDisplay : UserControl
    {
        public Device.Type type = Device.Type.None;
        public DeviceDisplay()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = TypeSelection.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    type = Device.Type.None;
                    break;
                case 1:
                    type = Device.Type.Left;
                    break;
                case 2:
                    type = Device.Type.Right;
                    break;
            }

            UI.DisplayType();
        }
    }
}
