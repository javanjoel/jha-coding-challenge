using System.Text.Json.Serialization;

namespace JhaModels.Tweets
{
    public class Tweet
    {
        public TweetEntity Entities { get; set; }
    }
}