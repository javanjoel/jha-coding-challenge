using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaModels.Tweets
{
    public class TweetEntityMention
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string Username { get; set; }
    }
}
