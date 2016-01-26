using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfMap
{
    public class GeoCar
    {
        public string Id { get; set; }
        public GeoData GeoData { get; set; }

       
    }
    public struct GeoData
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

}
