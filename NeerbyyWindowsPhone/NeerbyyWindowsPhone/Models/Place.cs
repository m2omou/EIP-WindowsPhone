using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    public class Place
    {
        public string id { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string name { get; set; }
        public object postcode { get; set; }
        public string city { get; set; }
        public object address { get; set; }
        public string country { get; set; }
        public string icon { get; set; }
    }
}
