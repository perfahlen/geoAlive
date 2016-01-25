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

namespace wpfMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ControlTemplate template = (ControlTemplate)this.FindResource("CutomPushpinTemplate");
            var pin = new IdPushpin();
            pin.Location = new Microsoft.Maps.MapControl.WPF.Location(63, 15);
            pin.Id = "kalle";
            pin.Template = template;
            map.Children.Add(pin);

            var pin2 = new IdPushpin();
            pin2.Location = new Microsoft.Maps.MapControl.WPF.Location(60, 13);
            pin2.Id = "apa";
            pin2.Template = template;
            map.Children.Add(pin2);
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            UpdatePushpin("apa");
        }

        public void UpdatePushpin(string id, Microsoft.Maps.MapControl.WPF.Location location = null)
        {
            var pins = map.Children;

            foreach (var item in pins)
            {
                if (item.GetType() != typeof(IdPushpin))
                    continue;
                var idPin = (IdPushpin)item;
                if (idPin.Id == id)
                {
                    var _location = idPin.Location;
                    _location.Latitude += 1;
                    _location.Longitude += 1;
                    idPin.Location = _location;
                    map.Children.Remove(idPin);
                    map.Children.Add(idPin);
                    break;
                }
            }
        }
    }
}
