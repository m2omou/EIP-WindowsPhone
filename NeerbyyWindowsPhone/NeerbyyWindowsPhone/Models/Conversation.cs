using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// A Conversation Object
    /// </summary>
    public class Conversation
    {
        /// <summary>
        /// A Conversation ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// The other User in the Conversation
        /// </summary>
        public User recipient { get; set; }

        /// <summary>
        /// A list of Messages in the Conversation
        /// </summary>
        public List<Message> messages { get; set; }
    }
}
