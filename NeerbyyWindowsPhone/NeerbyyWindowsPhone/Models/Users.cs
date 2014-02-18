using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neerbyy
{

    /*
     * Model User
    */
    class Users
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string avatar { get; set; }
        [JsonIgnore]
        public string created_at { get; set; }
        [JsonIgnore]
        public string updated_at { get; set; }
        [JsonIgnore]
        public object authentication_token { get; set; }
        // Pour que l'id ne soit pas serialise il suffit de le mettre a -1
        public bool ShouldSerializeid()
        {
            return (id != -1);
        }
    }
}
