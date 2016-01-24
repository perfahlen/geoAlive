using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Script.Serialization;


namespace geoLiveWeb.services
{
    /// <summary>
    /// Summary description for AvailableRoutesashx
    /// </summary>
    public class AvailableRoutesashx : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var calcRoutes = GetRoutes(context);

            JavaScriptSerializer jsonSerialiazer = new JavaScriptSerializer();
            var json = jsonSerialiazer.Serialize(calcRoutes);


            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }

        private IEnumerable<string> GetRoutes(HttpContext context)
        {
            var routeDirs = System.Configuration.ConfigurationManager.AppSettings["filePath"] as string;
            var dirInfo = new System.IO.DirectoryInfo(routeDirs);
            var files = dirInfo.GetFiles();
            var fileNames = new List<string>(files.Count());
            
            foreach (var item in files)
            {
                fileNames.Add(item.Name);
            }
            return fileNames;
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