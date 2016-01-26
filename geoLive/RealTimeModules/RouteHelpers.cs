namespace RealTimeModules
{
    using Newtonsoft.Json.Linq;    
    using System.Collections.Generic;
    using System.Linq;

    public static class RouteHelpers
    {
        public static IEnumerable<JToken> GetEveryNthItem(this JArray list, int nth)
        {            
            return list.Where((x, i) => i % nth == 0);            
        }
    }
}
