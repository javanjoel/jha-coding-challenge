using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaModels.Tweets
{
    public class TweetEntity
    {
        public List<TweetEntityHashtag> Hashtags { get; set; }
        public List<TweetEntityMention> Mentions { get; set; }
    }
}
