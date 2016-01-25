using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XSockets.Core.Common.Utility.Logging;
using XSockets.Core.Common.Utility.Serialization;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework;
using XSockets.Plugin.Framework.Attributes;

namespace RealTimeModules
{
    [XSocketMetadata("geocars")]
    public class GeoCars : XSocketController
    {

    }

    /// <summary>
    /// There will only be one instance of this class
    /// A singleton...
    /// </summary>
    [XSocketMetadata("GisDataPump", PluginRange.Internal)]
    public class GisDataPump : XSocketController
    {
        private string routeDictionary = @"C:\Users\Uffe\Documents\GitHub\geoAlive\geoLive\geoLiveWeb\routeFiles";

        List<RouteInfo> ril;

        public GisDataPump()
        {
            Task.Factory.StartNew(() =>
            {
                ril = new List<RouteInfo>();
                //read all route-files into RouteInfoObjects...
                foreach (var f in System.IO.Directory.GetFiles(routeDictionary, "*.json"))
                {
                    var file = new FileInfo(f);
                    var ri = new RouteInfo(file, (id, geodata) =>
                    {
                        this.InvokeToAll<GeoCars>(new { id, geodata },"pos");
                    });
                    //Composable.GetExport<IXLogger>().Warning("Starting timer");
                    //ri.Start();
                    ril.Add(ri);
                    var r = new Random(42);
                    ri.Start(r.Next(1000,5000));
                }


                //foreach(var r in ril)
                //{
                //    r.Start();
                //}
            });
        }
    }

    public class RouteInfo
    {
        public IList<GeoData> Routes { get; set; }
        private int routePos;
        private bool _loop;
        private System.Timers.Timer _timer;
        private Action<string, GeoData> _action;
        private string RouteId { get; set; }
        private IXSocketJsonSerializer jsonSerializer;

        public RouteInfo(FileInfo file, Action<string, GeoData> onTick, bool loop = true)
        {
            this.Routes = new List<GeoData>();
            this.RouteId = file.Name;

            this.jsonSerializer = Composable.GetExport<IXSocketJsonSerializer>();

            JArray jArr = (JArray)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(file.FullName));
            foreach (var item in jArr)
            {
                //Console.WriteLine("Item {0}, {1}", item[0], item[1]);
                this.Routes.Add(new GeoData { Lat = (double)item[0], Lng = (double)item[1] });
            }
            
            //this.Routes = routes;
            this._action = onTick;
            this._loop = loop;
        }

        public void Start(int interval)
        {
            this._timer = new System.Timers.Timer(interval);
            this._timer.Elapsed += (s, e) =>
            {
                //get geodata
                if (routePos >= this.Routes.Count)
                {
                    routePos = 0;
                    if (!this._loop)
                    {
                        this.Stop();
                        return;
                    }
                }
                var pos = this.Routes[routePos++];
                //call action
                _action(this.RouteId, pos);
            };
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }

    public struct GeoData
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
