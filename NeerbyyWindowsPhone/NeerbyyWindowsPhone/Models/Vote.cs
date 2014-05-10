using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// 
    /// </summary>
    public class Vote
    {
        /// <summary>
        /// ID of the Vote
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// The ID of the Publication being voted on
        /// </summary>
        public int publication_id { get; set; }

        /// <summary>
        /// The ID of the User having made the vote
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// The Value of the Vote, Upvote or Downvote
        /// </summary>
        public bool value { get; set; }

        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
