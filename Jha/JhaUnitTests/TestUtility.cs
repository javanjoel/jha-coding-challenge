using JhaModels.Tweets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaUnitTests
{
	public static class TestUtility
	{
		public static Tweet CreateTweet(string[] hashtags = null, string[] mentions = null)
		{
			var tweet = new Tweet()
			{
				Entities = new TweetEntity()
			};

			if (hashtags?.Length > 0)
				tweet.Entities.Hashtags = hashtags.Select(t => new TweetEntityHashtag() { Tag = t }).ToList();

			if (mentions?.Length > 0)
				tweet.Entities.Mentions = mentions.Select(u => new TweetEntityMention() { Username = u }).ToList();

			return tweet;
		}//end CreateTweet
	}
}
