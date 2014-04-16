using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// A Post Object represents a publication made by a user
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Post ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// The ID of the user that created the post
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// The ID of the place where the post was created
        /// </summary>
        public int place_id { get; set; }

        /// <summary>
        /// The content of the Post
        /// </summary>
        public object content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string created_at { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string updated_at { get; set; }

        /// <summary>
        /// Longitude of the Post
        /// </summary>
        public object longitude { get; set; }

        /// <summary>
        /// Latitude of the Post
        /// </summary>
        public object latitude { get; set; }

        /// <summary>
        /// The Post's type
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// The Post's URL that points the data
        /// </summary>
        public object url { get; set; }

        /// <summary>
        /// The URL for the thumbnail for the data
        /// </summary>
        public object thumb_url { get; set; }

        /// <summary>
        /// Number of comments
        /// </summary>
        public int comments { get; set; }

        /// <summary>
        /// Number of likes
        /// </summary>
        public int like { get; set; }

        /// <summary>
        /// Number of Dislikes
        /// </summary>
        public int dislike { get; set; }
    }
}
