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
        public int id { get; set; }
        public int publication_id { get; set; }
        public int user_id { get; set; }
        public bool value { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
