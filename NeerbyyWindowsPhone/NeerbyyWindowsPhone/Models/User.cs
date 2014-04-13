using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Thumb Object for User's Avatar
    /// </summary>
    public class Thumb
    {
        /// <summary>
        /// Url of the Thumb
        /// </summary>
        public string url { get; set; }
    }

    /// <summary>
    /// Avatar Object for User
    /// </summary>
    public class Avatar
    {
        /// <summary>
        /// URL for Avater
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Thumb object
        /// </summary>
        public Thumb thumb { get; set; }
    }

    /// <summary>
    /// User Object
    /// </summary>
    public class User
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// Firstname
        /// </summary>
        public object firstname { get; set; }

        /// <summary>
        /// Lastname
        /// </summary>
        public object lastname { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Avatar
        /// </summary>
        public Avatar avatar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string created_at { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string updated_at { get; set; }

        /// <summary>
        /// Authentication Token
        /// </summary>
        public string auth_token { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// Called to know if the object should be serialized
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeid()
        {
            return (id != -1);
        }
    }
}
