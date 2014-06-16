using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// A Message Object
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Message ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// The User ID of th Sender
        /// </summary>
        public int sender_id { get; set; }

        /// <summary>
        /// The content of the Message
        /// </summary>
        public string content { get; set; }
    }
}
