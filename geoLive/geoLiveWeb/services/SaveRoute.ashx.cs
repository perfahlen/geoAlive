using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geoLiveWeb
{
    /// <summary>
    /// Summary description for SaveRoute
    /// </summary>
    public class SaveRoute : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var data = context.Request.Form[0];
            var routeName = (context.Request["routeName"] as string) + ".json";
            SaveCurrentRoute(context, routeName, data);
            context.Response.ContentType = context.Request.ContentType;
            context.Response.Write("ok");
        }

        void SaveCurrentRoute(HttpContext ctx, string routeName, string data)
        {

            try
            {
                string routeDir = ctx.Request.PhysicalApplicationPath;
                var routeDirs = routeDir + "routeFiles\\";
                using (var streamWriter = new System.IO.StreamWriter(routeDirs + routeName))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception x)
            {
                System.Diagnostics.Debug.WriteLine(x.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}