using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
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
        public string firstname { get; set; }

        /// <summary>
        /// Lastname
        /// </summary>
        public string lastname { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string email { get; set; }

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
        /// 
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string avatar_thumb { get; set; }

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
