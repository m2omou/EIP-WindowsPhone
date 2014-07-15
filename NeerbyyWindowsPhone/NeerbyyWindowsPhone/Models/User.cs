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
        /// The date of the Creation of the User
        /// </summary>
        public string created_at { get; set; }

        /// <summary>
        /// The date of the Update of the User
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
        /// Avatar URL
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// Avatar Thumbnail URL
        /// </summary>
        public string avatar_thumb { get; set; }

        /// <summary>
        /// The User's settings
        /// </summary>
        public Settings settings { get; set; }
    }
}
