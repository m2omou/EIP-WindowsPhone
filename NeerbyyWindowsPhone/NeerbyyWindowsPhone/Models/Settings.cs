using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// The User Settings Object
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// The ID of the Settings for the User
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// Setting for allowing Users to contact the User
        /// </summary>
        public bool? allow_messages { get; set; }

        /// <summary>
        /// Setting to request Notifications for Comments on a User's Post
        /// </summary>
        public bool? send_notification_for_comments { get; set; }

        /// <summary>
        /// Setting to request Notifications for private Messages
        /// </summary>
        public bool? send_notification_for_messages { get; set; }
    }
}
