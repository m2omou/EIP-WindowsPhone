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

        /// <summary>
        /// The ID of to reference following this place or null
        /// </summary>
        public int? followed_place_id { get; set; }

        /// <summary>
        /// The Distance of the user to that place
        /// </summary>
        public int? distance { get; set; }

        /// <summary>
        /// The Distance from the User to the Boundary around the Place
        /// </summary>
        public int? distance_boundary { get; set; }

        /// <summary>
        /// A Boolean to know if the User can publish in a Place
        /// </summary>
        public Boolean? can_publish { get; set; }

        /// <summary>
        /// The number of posts at this place
        /// </summary>
        public int? publications { get; set; }
    }
}
