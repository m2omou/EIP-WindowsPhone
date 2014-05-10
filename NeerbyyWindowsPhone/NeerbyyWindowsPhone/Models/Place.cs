using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Place Object
    /// </summary>
    public class Place
    {
        /// <summary>
        /// Place ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Longitude of the Place
        /// </summary>
        public double longitude { get; set; }

        /// <summary>
        /// Latitude of the Plae
        /// </summary>
        public double latitude { get; set; }

        /// <summary>
        /// Name of the Place
        /// </summary>
        public string name { get; set; }
        
        /// <summary>
        /// Postcode of the Place
        /// </summary>
        public object postcode { get; set; }

        /// <summary>
        /// City of Place
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Address of the Place
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// Country of the Place
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Icon to be used with the Place
        /// </summary>
        public string icon { get; set; }
    }
}
