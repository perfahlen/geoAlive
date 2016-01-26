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
            var dc = new DataClient(this.map, this);
            //CreatePushpins();
        }
 

        public void UpdatePushpin(string id, Microsoft.Maps.MapControl.WPF.Location location = null)
        {
            this.Dispatcher.Invoke(delegate ()
            {

                var pins = map.Children;
                bool found = false;
                foreach (var item in pins)
                {
                    if (item.GetType() != typeof(IdPushpin))
                        continue;
                    var idPin = (IdPushpin)item;
                    if (idPin.Id == id)
                    {
                        found = true;
                        idPin.Location = location;
                        map.Children.Remove(idPin);
                        map.Children.Add(idPin);
                        break;
                    }
                }
                if (!found)
                {
                    ControlTemplate template = (ControlTemplate)this.FindResource("CutomPushpinTemplate");
                    var pin = new IdPushpin() { Location = location, Id = id };
                    pin.Template = template;
                    map.Children.Add(pin);
                }
            });
        }
    }
}
