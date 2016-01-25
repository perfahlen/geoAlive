using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfMap
{
    public class IdPushpin : Microsoft.Maps.MapControl.WPF.Pushpin
    {
        public string Id { get; set; }
    }
}
