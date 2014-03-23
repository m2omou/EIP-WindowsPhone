using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{

    public class Thumb
    {
        public string url { get; set; }
    }

    public class Avatar
    {
        public string url { get; set; }
        public Thumb thumb { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public object firstname { get; set; }
        public object lastname { get; set; }
        public string email { get; set; }
        public Avatar avatar { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string auth_token { get; set; }
        public string password { get; set; }
    }

    ///// <summary>
    ///// Modele utilisateur
    ///// </summary>
    //public class User
    //{
    //    /// <summary>
    //    /// User Id
    //    /// </summary>
    //    public int id { get; set; }

    //    /// <summary>
    //    /// Username
    //    /// </summary>
    //    public string username { get; set; }

    //    /// <summary>
    //    /// Email
    //    /// </summary>
    //    public string email { get; set; }

    //    /// <summary>
    //    /// Password
    //    /// </summary>
    //    public string password { get; set; }

    //    /// <summary>
    //    /// Avatar
    //    /// </summary>
    //    [JsonIgnore]
    //    public string avatar { get; set; }

    //    /// <summary>
    //    /// Creation Date
    //    /// </summary>
    //    [JsonIgnore]
    //    public string created_at { get; set; }

    //    /// <summary>
    //    /// Updated Date
    //    /// </summary>
    //    [JsonIgnore]
    //    public string updated_at { get; set; }

    //    /// <summary>
    //    /// Authentication Token
    //    /// </summary>
    //    [JsonIgnore]
    //    public object authentication_token { get; set; }
        
    //    /// <summary>
    //    /// Called to know if the object should be serialized
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool ShouldSerializeid()
    //    {
    //        return (id != -1);
    //    }
    //}
}
