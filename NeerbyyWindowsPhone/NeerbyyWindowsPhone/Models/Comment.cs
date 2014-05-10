using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Comment for a Post
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Comment ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// The content of the Comment
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// The ID of the Publication that the Comment is linked to
        /// </summary>
        public int publication_id { get; set; }

        /// <summary>
        /// The ID of the user having posted the comment
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string created_at { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string updated_at { get; set; }
    }
}
