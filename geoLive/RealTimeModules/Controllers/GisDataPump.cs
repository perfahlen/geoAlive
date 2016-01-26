
namespace RealTimeModules.Controllers
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using XSockets.Core.XSocket;
    using XSockets.Core.XSocket.Helpers;
    using XSockets.Plugin.Framework;
    using XSockets.Plugin.Framework.Attributes;

    /// <summary>
    /// There will only be one instance of this class
    /// A singleton...
    /// </summary>
    [XSocketMetadata("GisDataPump", PluginRange.Internal)]
    public class GisDataPump : XSocketController
    {
        private string routeDictionary = Path.Combine(GetAssemblyDirectory(), "routefiles");

        List<RouteInfo> ril;

        public GisDataPump()
        {
            Task.Factory.StartNew(() =>
            {
                ril = new List<RouteInfo>();
                //read all route-files into RouteInfoObjects...
                foreach (var routeFile in Directory.GetFiles(routeDictionary, "*.json"))
                {
                    var file = new FileInfo(routeFile);
                    //Send data to client when movement is detected (or faked in this case)
                    var ri = new RouteInfo(file, (id, geodata) =>
                    {
                        this.InvokeToAll<GeoCars>(new { id, geodata },"pos");
                    });
                    ril.Add(ri);
                    var r = new Random(42);
                    ri.Start(r.Next(1000,5000));
                }

            });
        }

        public static string GetAssemblyDirectory()
        {            
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);         
        }
    }
}
