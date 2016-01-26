namespace RealTimeModules.Model
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Timers;
    public class RouteInfo
    {
        public IList<GeoData> Routes { get; set; }
        private int routePos;
        private bool _loop;
        private Timer _timer;
        private Action<string, GeoData> _action;
        private string RouteId { get; set; }

        public RouteInfo(FileInfo file, Action<string, GeoData> onTick, bool loop = true)
        {
            this.Routes = new List<GeoData>();
            this.RouteId = file.Name;
            this._action = onTick;
            this._loop = loop;

            JArray jArr = (JArray)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(file.FullName));            
            foreach (var item in jArr.GetEveryNthItem(5))
            {
                this.Routes.Add(new GeoData { Lat = (double)item[0], Lng = (double)item[1] });
            }
        }

        public void Start(int interval)
        {
            this._timer = new Timer(interval);
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
}
