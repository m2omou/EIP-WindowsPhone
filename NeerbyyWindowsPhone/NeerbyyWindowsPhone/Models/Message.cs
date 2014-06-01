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

        public int sender_id { get; set; }

        public int recipient_id { get; set; }
        public string content { get; set; }
    }
}
