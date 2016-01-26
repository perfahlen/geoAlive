using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maps.MapControl.WPF;
using XSockets.Client40.Common.Interfaces;

namespace wpfMap
{
    public class DataClient
    {

        Map map;
        MainWindow main;
        public DataClient(Map map, MainWindow window)
        {
            this.map = map;
            this.main = window;
            initConnection();
        }

        void initConnection()
        {
            Task.Delay(2000);
            var client = new XSockets.Client40.XSocketClient("ws://localhost:4502", "http://localhost", "geocars");

            client.Controller("geocars").On<GeoCar>("pos", d =>
            {
                System.Diagnostics.Trace.WriteLine(d);
                main.UpdatePushpin(d.Id, new Location() { Latitude = d.GeoData.Lat, Longitude = d.GeoData.Lng });
            });

            client.Open();
        }
        
    }
}
